using WorldFestSolution.XamarinApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorldFestSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProgramView : ContentPage
    {
        public AddProgramView(AddEditProgramViewModel addProgramViewModel)
        {
            InitializeComponent();
            BindingContext = addProgramViewModel;
        }
    }
}