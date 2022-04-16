using WorldFestSolution.XamarinApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorldFestSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommentsView : ContentPage
    {
        private readonly CommentsViewModel _viewModel;

        public CommentsView()
        {
            InitializeComponent();
        }

        public CommentsView(CommentsViewModel commentsViewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = commentsViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}