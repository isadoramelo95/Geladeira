using Domain;

namespace Service.Interface
{
    public interface IGeladeiraService<TEntity> where TEntity : class
    {
        Task<string> AddNaGeladeira(Item item);
        Task<string> AdicionarListaItensGeladeira(List<Item> items);
        Task<string> EditarItemNaGeladeira(Item item);
        Task<string> EsvaziarPorContainer(int numContainer);
        Item? GetItemById(int id);
        Task<List<Item>> ListaDeItens();
        Task<string> RemoverItembyId(int id);
    }
}
