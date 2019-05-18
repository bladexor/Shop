using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data
{
    public class DataContext: IdentityDbContext<User>
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Country> Countries { get; set; }

        //For Orders Functionality
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<OrderDetailTemp> OrderDetailTemps { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

       /* protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2");

            var cascadeFKs = builder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach(var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);
        } */
    }
}
