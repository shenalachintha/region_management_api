using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace myApi.Data
{
    public class MyApiAuthDbContext : IdentityDbContext
    {
        public MyApiAuthDbContext(DbContextOptions <MyApiAuthDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerId = "4e9f585c-7eeb-486c-92d9-85953db3ea0d";
            var WriterId = "7c7ed7ce-d6b3-47d5-9884-21b1f751fcba";
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id=readerId,
                    ConcurrencyStamp=readerId,
                    Name="Reader",
                    NormalizedName="READER".ToUpper()

                },
                new IdentityRole
                {
                    Id=WriterId,
                    ConcurrencyStamp=WriterId,
                    Name="Writer",
                    NormalizedName="WRITER".ToUpper()
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
