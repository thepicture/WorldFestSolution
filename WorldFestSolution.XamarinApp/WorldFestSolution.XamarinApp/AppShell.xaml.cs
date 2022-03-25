using System;
using System.Collections.Generic;
using WorldFestSolution.XamarinApp.ViewModels;
using WorldFestSolution.XamarinApp.Views;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
