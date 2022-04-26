using RatingBarControl;
using System;
using WorldFestSolution.XamarinApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorldFestSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FestivalView : ContentPage
    {
        private readonly FestivalViewModel _viewModel;

        public FestivalView(FestivalViewModel festivalViewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = festivalViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}