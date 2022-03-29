using WorldFestSolution.XamarinApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorldFestSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditFestivalView : ContentPage
    {
        public AddEditFestivalView(AddEditFestivalViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}