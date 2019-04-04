﻿
namespace Shop.Web.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.AspNetCore.Identity;
    using Shop.Web.Helpers;

    public class SeedDb
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;
        private Random random;

        public SeedDb(DataContext context,IUserHelper userHelper)
        {
            this.context = context;
            this.userHelper = userHelper;
            this.random = new Random();
        }

        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();

            var user = await this.userHelper.GetUserByEmailAsync("bladi135@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Bladimir",
                    LastName = "Velásquez",
                    Email = "bladi135@gmail.com",
                    UserName = "bladi135@gmail.com",
                    PhoneNumber="04121907221"
                };

                var result = await this.userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }

            if (!this.context.Products.Any())
            {
                this.AddProduct("Mini Radio FM",user);
                this.AddProduct("Zapatos Apolo",user);
                this.AddProduct("iWatch Series 4",user);
                await this.context.SaveChangesAsync();


            }
        }

        private void AddProduct(string name,User user)
        {
            this.context.Products.Add(new Product
            {
                Name=name,
                Price=this.random.Next(1000),
                IsAvailable=true,
                Stock=this.random.Next(100),
                User=user
            });
        }
    }
}
