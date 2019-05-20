using Shop.Common.Models;
using Shop.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Shop.UIForms.ViewModels
{
    public class ProductsViewModel:BaseViewModel
        
    {
        private ApiService apiService;
        private ObservableCollection<Product> products;
        private bool isRefreshing;

        public ObservableCollection<Product> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public ProductsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            this.IsRefreshing = true;
         //   var response = await this.apiService.GetListAsync<Product>(
         //       "http://192.168.1.201:5000",
         //       "/api",
         //       "/Products");

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<Product>(
                url,
                "/api",
                "/Products",
                "bearer",
                MainViewModel.GetInstance().Token.Token);


            this.IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            var myProducts = (List<Product>)response.Result;
            this.Products = new ObservableCollection<Product>(myProducts.OrderBy(p => p.Name));
        }
    }
}
