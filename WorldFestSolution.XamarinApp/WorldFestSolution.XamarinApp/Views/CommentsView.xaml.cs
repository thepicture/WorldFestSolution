using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldFestSolution.XamarinApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorldFestSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommentsView : ContentPage
    {
        public CommentsView()
        {
            InitializeComponent();
        }

        public CommentsView(CommentsViewModel commentsViewModel)
        {
            InitializeComponent();
            BindingContext = commentsViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as CommentsViewModel).OnAppearing();
        }
    }
}