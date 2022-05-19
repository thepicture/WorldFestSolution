using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorldFestSolution.XamarinApp.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoItemsTemplate : ContentView
    {


        public string Reason
        {
            get { return (string)GetValue(ReasonProperty); }
            set { SetValue(ReasonProperty, value); }
        }

        public static readonly BindableProperty ReasonProperty =
            BindableProperty.Create("Reason",
                                    typeof(string),
                                    typeof(NoItemsTemplate),
                                    default(string));


        public NoItemsTemplate()
        {
            InitializeComponent();
        }
    }
}