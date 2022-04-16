using WorldFestSolution.XamarinApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorldFestSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InviteView : ContentPage
    {
        private readonly InviteViewModel _viewModel;

        public InviteView(InviteViewModel inviteViewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = inviteViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}