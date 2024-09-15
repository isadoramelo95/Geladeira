using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Repository.Interfaces;

namespace Repository.RepositoryClasses
{
    public class GeladeiraRepository : IGeladeiraRepository<Item>
    {
        GeladeiraContext _context;

        public GeladeiraRepository(GeladeiraContext context)
        {
            _context = context;
        }

        public Item? GetItemById(int id)
        {
            try
            {
                var itemPorId = _context.Items.Where(i => i.Id == id).ToList();
                return itemPorId != null ? itemPorId.FirstOrDefault() : null;
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<Item>> ListaDeItens() => await _context.Items.ToListAsync();

        public async Task AddNaGeladeira(Item item)
        {
            await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.Items.AddAsync(item);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();
            }
            catch (SqlException ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Erro ao inserir item na geladeira: {ex.Message}");
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Erro ao inserir item na geladeira: {ex.Message}");
            }
        }


        public async Task RemoverItembyId(int id)
        {
            try
            {
                var item = await _context.Items.FindAsync(id);

                if (item != null)
                {
                    _context.Remove(item);
                    await _context.SaveChangesAsync();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> EditarItemNaGeladeira(Item item)
        {
            try
            {
                var itemExistente = _context.Items.Find(item.Id);

                if (itemExistente == null)
                {
                    throw new Exception("Item não encontrado");
                }

                itemExistente.Alimento = item.Alimento;
                itemExistente.Quantidade = item.Quantidade;
                itemExistente.Unidade = item.Unidade;

                _context.Items.Update(itemExistente);
                await _context.SaveChangesAsync();

                return "Item atualizado com sucesso!";
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ValidarItemExistente(int id) => _context.Items.Any(e => e.Id == id);

    }
}