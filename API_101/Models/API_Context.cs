//programmed by: Himank Bansal
//u3183058
//Reference code1:https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.0&tabs=visual-studio
//Reference Code2:https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-3.0

using System;
using Microsoft.EntityFrameworkCore;

namespace API_101.Models
{
    public class API_Context: DbContext
    {
        public API_Context(DbContextOptions<API_Context> options):base(options)
        {

        }
         public DbSet<APIModels> APIModel { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) // to se the token value of the model as the primary key
        {
            modelBuilder.Entity<APIModels>()
                .HasKey(c => new { c.Token});
        }

          
    }
}
