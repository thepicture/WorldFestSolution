using System.ComponentModel;
using WorldFestSolution.XamarinApp.ViewModels;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}