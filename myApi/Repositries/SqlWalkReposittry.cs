using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using myApi.Data;
using myApi.Models.Domain;

namespace myApi.Repositries
{
    public class SqlWalkReposittry : IWalkRepositry
    {
        private readonly MyApiDbContext dbContext;
        public SqlWalkReposittry(MyApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walks> createAsync(Walks walks)
        {
            await dbContext.Walks.AddAsync(walks);
            await dbContext.SaveChangesAsync();
            return walks;
          
        }

        public async Task<Walks?> deleteByIdAsync(Guid id)
        {
           var existingWalk=await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null) {
                return null;
            }
           dbContext.Walks.Remove(existingWalk);
            await dbContext.SaveChangesAsync();
            return existingWalk;

        }


        public async Task<List<Walks>> getAllAsync(string? filterOn = null, string? filterQuery = null,

            string? sortBy = null, bool? isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();
            //Filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }
            //Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    // Replace the following line in getAllAsync method:
                    // walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);

                    // With this fix:
                    walks = (isAscending ?? true) ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                   
                }
                else if (sortBy.Equals("LengthInKm", StringComparison.OrdinalIgnoreCase))
                {
                    walks = (isAscending ?? true) ? walks.OrderBy(x => x.LenthlnKm) : walks.OrderByDescending(x => x.LenthlnKm);
                }
            }
            //Pagination
            var skipResults=(pageNumber-1)* pageSize;
            // return await dbContext.Walks.Include("Difficulty").Include("Region"). ToListAsync();
            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
            
        }
        public async Task<Walks?> getByIdAsync(Guid id)
        {
           return await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walks?> updateByIdAsync(Guid id, Walks walks)
        {
         var existingWalk= await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
               return null;
            }
            existingWalk.Name = walks.Name;
            existingWalk.Description = walks.Description;
            existingWalk.LenthlnKm = walks.LenthlnKm;
            existingWalk.RegionId = walks.RegionId;
            existingWalk.DifficultyId = walks.DifficultyId;
            existingWalk.WalkImageUrl = walks.WalkImageUrl;
            await dbContext.SaveChangesAsync();
            return existingWalk;

        }
    }
}
 