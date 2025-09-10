using myApi.Models.Domain;

namespace myApi.Repositries
{
    public interface IWalkRepositry
    {
        Task<Walks> createAsync(Walks walks);
        Task<List<Walks>> getAllAsync(string? filterOn=null,string? filterQuery=null,string? sortBy=null, bool? isAscending = true, int pageNumber = 1,int pageSize=1000);
        Task<Walks?> getByIdAsync(Guid id);
        Task<Walks?> updateByIdAsync(Guid id, Walks walks);
        Task<Walks?> deleteByIdAsync(Guid id);

    }
}
