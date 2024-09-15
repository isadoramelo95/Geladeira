using Domain;

namespace Repository.Interfaces
{
    public interface IGeladeiraRepository<TEntity> where TEntity : class
    {
        Task<string> EditarItemNaGeladeira(Item item);
        Task AddNaGeladeira(Item item);
        Task<List<Item>> ListaDeItens();
        Task RemoverItembyId(int id);
        Item? GetItemById(int id);
        bool ValidarItemExistente(int id);
    }
}
