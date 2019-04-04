using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Web.Data;
using Shop.Web.Data.Entities;
using Shop.Web.Helpers;
using Shop.Web.Models;

namespace Shop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IUserHelper userHelper;

        public ProductsController(IProductRepository productRepository,IUserHelper userHelper)
        {
            this.productRepository = productRepository;
            this.userHelper = userHelper;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(this.productRepository.GetAll());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productvm)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if(productvm.ImageFile!=null && productvm.ImageFile.Length > 0)
                {
                    path = Path.Combine(
                        Directory.GetCurrentDirectory(), 
                        "wwwroot\\images\\Products",
                        productvm.ImageFile.FileName);

                    using(var stream=new FileStream(path, FileMode.Create))
                    {
                        await productvm.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Products/{productvm.ImageFile.FileName}";
                }
                var product = this.ToProduct(productvm,path);

                //TODO: Cambiar por el usuario logueado
                productvm.User = await this.userHelper.GetUserByEmailAsync("bladi135@gmail.com");
                await this.productRepository.CreateAsync(product);
                           
                return RedirectToAction(nameof(Index));
            }
            return View(productvm);
        }

        private Product ToProduct(ProductViewModel productvm, string path)
        {
            return new Product
            {
                Id = productvm.Id,
                ImageUrl = path,
                IsAvailable = productvm.IsAvailable,
                LastPurchase = productvm.LastPurchase,
                LastSale = productvm.LastSale,
                Name = productvm.Name,
                Price = productvm.Price,
                Stock = productvm.Stock,
                User = productvm.User
            };
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();  
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    //TODO: Cambiar por el usuario logueado
                    product.User = await this.userHelper.GetUserByEmailAsync("bladi135@gmail.com");
                    await this.productRepository.UpdateAsync(product);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.productRepository.ExistAsync(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await this.productRepository.GetByIdAsync(id);
            await this.productRepository.DeleteAsync(product);
            
            return RedirectToAction(nameof(Index));
        }

    }
}
