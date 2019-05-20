
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
        private readonly Random random;

        public SeedDb(DataContext context,IUserHelper userHelper)
        {
            this.context = context;
            this.userHelper = userHelper;
            this.random = new Random();
        }

        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();

            //Check and Add Roles
            await this.CheckRoles();

            //Add Cities and Country
            if (!this.context.Countries.Any())
            {
                await this.CountriesAndCities();
            }

            //Add User
            await this.CheckUser("brad@gmail.com", "Brad", "Pit", "Customer");
            await this.CheckUser("angelina@gmail.com", "Angelina", "Jolie", "Customer");
            await this.CheckUser("tcanaima447@gmail.com", "Tablet", "Canaima", "Customer");

            await this.CheckUser("miguel135@gmail.com", "Juan", "Zuluaga", "Admin");

            var user= await this.CheckUser("bladi135@gmail.com", "Bladimir", "Velásquez", "Admin");
            
            //Add Products
            if (!this.context.Products.Any())
            {
                this.AddProduct("Athlon II X2 245", 159, user);
                this.AddProduct("Audifonos Razor", 799, user);
                this.AddProduct("Celular Android", 749, user);
                this.AddProduct("Gorra Diamante", 399, user);
                this.AddProduct("iWatch Series 4", 1299, user);
                this.AddProduct("Licuadora Oster", 47, user);
                this.AddProduct("Monitor LG", 67.67M, user);
                this.AddProduct("Mouse Lenovo", 67.67M, user);
                this.AddProduct("Pendrive Sandisk", 67.67M, user);
                this.AddProduct("Zapato Deportivo", 67.67M, user);

                await this.context.SaveChangesAsync();

            }
        }

        private async Task CheckRoles()
        {
            await this.userHelper.CheckRoleAsync("Admin");
            await this.userHelper.CheckRoleAsync("Customer");
        }

        private async Task<User> CheckUser(string userName, string firstName, string lastName, string role)
        {
            // Add user
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                user = await this.AddUser(userName, firstName, lastName, role);
            }

            var isInRole = await this.userHelper.IsUserInRoleAsync(user, role);
            if (!isInRole)
            {
                await this.userHelper.AddUserToRoleAsync(user, role);
            }

            return user;
        }


        private async Task<User> AddUser(string userName, string firstName, string lastName, string role)
        {
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = userName,
                UserName = userName,
                Address = "Calle Luna Calle Sol",
                PhoneNumber = "350 634 2747",
                CityIde = this.context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                City = this.context.Countries.FirstOrDefault().Cities.FirstOrDefault()
            };

            var result = await this.userHelper.AddUserAsync(user, "123456");
            if (result != IdentityResult.Success)
            {
                throw new InvalidOperationException("Could not create the user in seeder");
            }

            await this.userHelper.AddUserToRoleAsync(user, role);
            var token = await this.userHelper.GenerateEmailConfirmationTokenAsync(user);
            await this.userHelper.ConfirmEmailAsync(user, token);
            return user;
        }


        private void AddProduct(string name, decimal price, User user)
        {
            this.context.Products.Add(new Product
            {
                Name = name,
                Price = price,
                IsAvailable = true,
                Stock = this.random.Next(100),
                User = user,
                ImageUrl = $"~/images/Products/{name}.jpg"
            });
        }


        private async Task CountriesAndCities()
        {
            var citiesCol = new List<City>();
            citiesCol.Add(new City { Name = "Medellín" });
            citiesCol.Add(new City { Name = "Bogotá" });
            citiesCol.Add(new City { Name = "Calí" });

            this.context.Countries.Add(new Country
            {
                Cities = citiesCol,
                Name = "Colombia"
            });

            var citiesArg = new List<City>();
            citiesArg.Add(new City { Name = "Córdoba" });
            citiesArg.Add(new City { Name = "Buenos Aires" });
            citiesArg.Add(new City { Name = "Rosario" });

            this.context.Countries.Add(new Country
            {
                Cities = citiesArg,
                Name = "Argentina"
            });

            var citiesVen = new List<City>();
            citiesVen.Add(new City { Name = "Barcelona" });
            citiesVen.Add(new City { Name = "Puerto La Cruz" });
            citiesVen.Add(new City { Name = "Caracas" });
            citiesVen.Add(new City { Name = "Santa Fe" });
            citiesVen.Add(new City { Name = "Valencia" });

            this.context.Countries.Add(new Country
            {
                Cities = citiesVen,
                Name = "Venezuela"
            });

            var citiesUsa = new List<City>();
            citiesUsa.Add(new City { Name = "New York" });
            citiesUsa.Add(new City { Name = "Los Ángeles" });
            citiesUsa.Add(new City { Name = "Chicago" });

            this.context.Countries.Add(new Country
            {
                Cities = citiesUsa,
                Name = "Estados Unidos"
            });
            await this.context.SaveChangesAsync();
        }




        }
    }
