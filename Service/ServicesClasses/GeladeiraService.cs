using Domain;
using Microsoft.Data.SqlClient;
using Repository.Interfaces;
using Service.Interface;
using System.ComponentModel;

namespace Service.ServicesClasses
{
    public class GeladeiraService : IGeladeiraService<Item>
    {
        private readonly IGeladeiraRepository<Item> _repository;
        public GeladeiraService(IGeladeiraRepository<Item> repository) =>
            _repository = repository;

        public async Task<string> AddNaGeladeira(Item item)
        {
            try
            {
                if (item != null)
                {

                    if (await ValidarItemExistente(item))
                        throw new Exception("Posição já preenchida!");

                    var itemExistente = GetItemById(item.Id);

                    if (itemExistente == null)
                    {
                        await _repository.AddNaGeladeira(item);

                        return "Item cadastrado com sucesso!";
                    }
                    else
                        throw new Exception("Item já foi cadastrado anteriormente");
                }
                else
                    return "Item inválido!";
            }
            catch (SqlException)
            {
                return "não foi possível se comunicar com a base de dados!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private async Task<bool> ValidarItemExistente(Item item)
        {
            var listaItensExistentes = await ListaDeItens();

            if (listaItensExistentes == null || !listaItensExistentes.Any())
                return false;

            listaItensExistentes = listaItensExistentes?.Where(i => i.NumeroContainer == item.NumeroContainer
                                                                                        && i.NumeroAndar == item.NumeroAndar
                                                                                        && i.Posicao == item.Posicao
                                                                                        && i.Id == item.Id).ToList();
            var itemValidado = listaItensExistentes?.FirstOrDefault();

            return itemValidado != null;

        }

        public async Task<string> AdicionarListaItensGeladeira(List<Item> items)
        {
            foreach (Item item in items)
            {
                var itensContainer = await ListaDeItens();

                itensContainer = itensContainer?.Where(c => c.NumeroAndar == item.NumeroAndar
                                                                             && c.NumeroContainer == item.NumeroContainer).ToList();

                if (itensContainer is not null)
                    await _repository.AddNaGeladeira(item);

                else
                    throw new Exception("Container já está cheio!");
            }

            return "Itens adicionados com sucesso!";
        }
        public Item? GetItemById(int id)
        {
            Item? item = new Item();
            try
            {
                if (id <= 0)
                    return null;

                item.Id = id;
                return _repository.GetItemById(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Item>> ListaDeItens() =>
           await _repository.ListaDeItens();

        public async Task<string> EditarItemNaGeladeira(Item item)
        {
            try
            {       
                if (item != null)
                {
                    await _repository.EditarItemNaGeladeira(item);

                    return "Item alterado com sucesso!";
                }
                else
                    return "Item inválido!";
            }
            catch (SqlException)
            {
                return "Não foi possível se comunicar com a base de dados!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> RemoverItembyId(int id)
        {
            try
            {
                if (id == 0)
                    throw new Exception("ID do item inválido ou incorreto! Tente novamente.");
                else
                {
                    await _repository.RemoverItembyId(id);

                    return "Item exclído com sucesso!";
                }
            }
            catch (SqlException)
            {
                return "Não foi possível se comunicar com a base de dados!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> EsvaziarPorContainer(int numContainer)
        {
            var container = await ListaDeItens();

            container = container?.Where(c => c.NumeroContainer == numContainer).ToList();

            if (container != null)
            {
                foreach (var item in container)
                {
                    await _repository.RemoverItembyId(item.Id);
                }
            }
            else
                throw new Exception("Andar selecionado está vazio");

            return "Andar esvaziado com sucesso!";

        }

        public bool AvaliarItem(int id) =>
            _repository.ValidarItemExistente(id);
    }
}