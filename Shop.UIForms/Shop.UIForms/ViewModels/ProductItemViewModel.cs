﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.UIForms.ViewModels
{
    using System.Windows.Input;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Views;

    public class ProductItemViewModel : Product
    {
        public ICommand SelectProductCommand => new RelayCommand(this.SelectProduct);

        private async void SelectProduct()
        {
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel((Product)this);
            await App.Navigator.PushAsync(new EditProductPage());
        }
    }

}
