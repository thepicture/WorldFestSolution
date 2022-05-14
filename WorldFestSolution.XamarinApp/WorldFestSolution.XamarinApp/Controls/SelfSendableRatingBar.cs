using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace WorldFestSolution.XamarinApp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [XamlFilePath("SelfSendableRatingBar.xaml")]
    public class SelfSendableRatingBar : StackLayout
    {
        public string emptyStarImage = string.Empty;

        public string fillStarImage = string.Empty;

        public Image star1;

        public Image star2;

        public Image star3;

        public Image star4;

        public Image star5;

        private int commandParameterForStar1 = 1;

        private int commandParameterForStar2 = 2;

        private int commandParameterForStar3 = 3;

        private int commandParameterForStar4 = 4;

        private int commandParameterForStar5 = 5;

        public static readonly BindableProperty SelectedStarCommandProperty = BindableProperty.Create("SelectedStarCommand", typeof(ICommand), typeof(SelfSendableRatingBar));

        public static readonly BindableProperty EmptyStarImageProperty = BindableProperty.Create("EmptyStarImage", typeof(string), typeof(SelfSendableRatingBar), "", BindingMode.TwoWay, null, EmptyStarImagePropertyChanged);

        public static readonly BindableProperty FillStarImageProperty = BindableProperty.Create("FillStarImage", typeof(string), typeof(SelfSendableRatingBar), "", BindingMode.TwoWay, null, FillStarImagePropertyChanged);

        public static readonly BindableProperty StarHeightRequestProperty = BindableProperty.Create("StarHeightRequest", typeof(double), typeof(SelfSendableRatingBar), 30.0, BindingMode.TwoWay, null, StarHeightRequestPropertyChanged);

        public static readonly BindableProperty StarWidthRequestProperty = BindableProperty.Create("StarWidthRequest", typeof(double), typeof(SelfSendableRatingBar), 30.0, BindingMode.TwoWay, null, StarWidthRequestPropertyChanged);

        public static readonly BindableProperty SelectedStarValueProperty = BindableProperty.Create("SelectedStarValue", typeof(double), typeof(SelfSendableRatingBar), default(double), BindingMode.TwoWay, null, SelectedStarValuePropertyChanged);

        public new static readonly BindableProperty FlowDirectionProperty = BindableProperty.Create("FlowDirection", typeof(FlowDirectionEnum), typeof(SelfSendableRatingBar), FlowDirectionEnum.LeftToRight, BindingMode.OneWay, null, FlowDirectionPropertyChanged);

        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private StackLayout stkRattingbar;

        private ICommand ItemTappedCommand => new Command<int>(delegate (int selectedStar)
        {
            fillStar(selectedStar, this);
            SelectedStarCommand.Execute(this);
        });

        public ICommand SelectedStarCommand
        {
            get
            {
                return (ICommand)GetValue(SelectedStarCommandProperty);
            }
            set
            {
                SetValue(SelectedStarCommandProperty, value);
            }
        }

        public string EmptyStarImage
        {
            get
            {
                return (string)GetValue(EmptyStarImageProperty);
            }
            set
            {
                SetValue(EmptyStarImageProperty, value);
            }
        }

        public string FillStarImage
        {
            get
            {
                return (string)GetValue(FillStarImageProperty);
            }
            set
            {
                SetValue(FillStarImageProperty, value);
            }
        }

        public double StarHeightRequest
        {
            get
            {
                return (double)GetValue(StarHeightRequestProperty);
            }
            set
            {
                SetValue(StarHeightRequestProperty, value);
            }
        }

        public double StarWidthRequest
        {
            get
            {
                return (double)GetValue(StarHeightRequestProperty);
            }
            set
            {
                SetValue(StarHeightRequestProperty, value);
            }
        }

        public double SelectedStarValue
        {
            get
            {
                return (double)GetValue(SelectedStarValueProperty);
            }
            set
            {
                SetValue(SelectedStarValueProperty, value);
            }
        }

        public new FlowDirectionEnum FlowDirection
        {
            get
            {
                return (FlowDirectionEnum)GetValue(FlowDirectionProperty);
            }
            set
            {
                SetValue(FlowDirectionProperty, value);
            }
        }

        public event EventHandler ItemTapped = delegate
        {
        };

        public SelfSendableRatingBar()
        {
            InitializeComponent();
            star1 = new Image();
            star2 = new Image();
            star3 = new Image();
            star4 = new Image();
            star5 = new Image();
            star1.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = ItemTappedCommand,
                CommandParameter = commandParameterForStar1
            });
            star2.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = ItemTappedCommand,
                CommandParameter = commandParameterForStar2
            });
            star3.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = ItemTappedCommand,
                CommandParameter = commandParameterForStar3
            });
            star4.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = ItemTappedCommand,
                CommandParameter = commandParameterForStar4
            });
            star5.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = ItemTappedCommand,
                CommandParameter = commandParameterForStar5
            });
            stkRattingbar.Children.Add(star1);
            stkRattingbar.Children.Add(star2);
            stkRattingbar.Children.Add(star3);
            stkRattingbar.Children.Add(star4);
            stkRattingbar.Children.Add(star5);
            PanGestureRecognizer panGestureRecognizer = new PanGestureRecognizer();
            panGestureRecognizer.PanUpdated += PanGestureRecognizer_PanUpdated;
            stkRattingbar.GestureRecognizers.Add(panGestureRecognizer);
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs obj)
        {
            double width = star1.Width;
            if (FlowDirection == FlowDirectionEnum.LeftToRight)
            {
                if (obj.TotalX > 0.0)
                {
                    fillStar(1, this);
                    SelectedStarCommand?.Execute(1);
                }

                if (obj.TotalX > width * 2.0)
                {
                    fillStar(2, this);
                    SelectedStarCommand?.Execute(2);
                }

                if (obj.TotalX > width * 3.0)
                {
                    fillStar(3, this);
                    SelectedStarCommand?.Execute(3);
                }

                if (obj.TotalX > width * 4.0)
                {
                    fillStar(4, this);
                    SelectedStarCommand?.Execute(4);
                }

                if (obj.TotalX > width * 5.0)
                {
                    fillStar(5, this);
                    SelectedStarCommand?.Execute(5);
                }
            }
            else
            {
                if (obj.TotalX > 0.0)
                {
                    fillStar(5, this);
                    SelectedStarCommand?.Execute(5);
                }

                if (obj.TotalX > width * 2.0)
                {
                    fillStar(4, this);
                    SelectedStarCommand?.Execute(4);
                }

                if (obj.TotalX > width * 3.0)
                {
                    fillStar(3, this);
                    SelectedStarCommand?.Execute(3);
                }

                if (obj.TotalX > width * 4.0)
                {
                    fillStar(2, this);
                    SelectedStarCommand?.Execute(2);
                }

                if (obj.TotalX > width * 5.0)
                {
                    fillStar(1, this);
                    SelectedStarCommand?.Execute(1);
                }
            }
        }

        private static void fillStar(double selectedValue, SelfSendableRatingBar obj)
        {
            obj.SelectedStarValue = selectedValue;
            if (obj.FlowDirection == FlowDirectionEnum.RightToLeft)
            {
                switch ((int)selectedValue)
                {
                    case 1:
                        obj.star1.Source = obj.emptyStarImage;
                        obj.star2.Source = obj.emptyStarImage;
                        obj.star3.Source = obj.emptyStarImage;
                        obj.star4.Source = obj.emptyStarImage;
                        obj.star5.Source = obj.fillStarImage;
                        break;
                    case 2:
                        obj.star1.Source = obj.emptyStarImage;
                        obj.star2.Source = obj.emptyStarImage;
                        obj.star3.Source = obj.emptyStarImage;
                        obj.star4.Source = obj.fillStarImage;
                        obj.star5.Source = obj.fillStarImage;
                        break;
                    case 3:
                        obj.star1.Source = obj.emptyStarImage;
                        obj.star2.Source = obj.emptyStarImage;
                        obj.star3.Source = obj.fillStarImage;
                        obj.star4.Source = obj.fillStarImage;
                        obj.star5.Source = obj.fillStarImage;
                        break;
                    case 4:
                        obj.star1.Source = obj.emptyStarImage;
                        obj.star2.Source = obj.fillStarImage;
                        obj.star3.Source = obj.fillStarImage;
                        obj.star4.Source = obj.fillStarImage;
                        obj.star5.Source = obj.fillStarImage;
                        break;
                    case 5:
                        obj.star1.Source = obj.fillStarImage;
                        obj.star2.Source = obj.fillStarImage;
                        obj.star3.Source = obj.fillStarImage;
                        obj.star4.Source = obj.fillStarImage;
                        obj.star5.Source = obj.fillStarImage;
                        break;
                }
            }
            else
            {
                switch ((int)selectedValue)
                {
                    case 1:
                        obj.star1.Source = obj.fillStarImage;
                        obj.star2.Source = obj.emptyStarImage;
                        obj.star3.Source = obj.emptyStarImage;
                        obj.star4.Source = obj.emptyStarImage;
                        obj.star5.Source = obj.emptyStarImage;
                        break;
                    case 2:
                        obj.star1.Source = obj.fillStarImage;
                        obj.star2.Source = obj.fillStarImage;
                        obj.star3.Source = obj.emptyStarImage;
                        obj.star4.Source = obj.emptyStarImage;
                        obj.star5.Source = obj.emptyStarImage;
                        break;
                    case 3:
                        obj.star1.Source = obj.fillStarImage;
                        obj.star2.Source = obj.fillStarImage;
                        obj.star3.Source = obj.fillStarImage;
                        obj.star4.Source = obj.emptyStarImage;
                        obj.star5.Source = obj.emptyStarImage;
                        break;
                    case 4:
                        obj.star1.Source = obj.fillStarImage;
                        obj.star2.Source = obj.fillStarImage;
                        obj.star3.Source = obj.fillStarImage;
                        obj.star4.Source = obj.fillStarImage;
                        obj.star5.Source = obj.emptyStarImage;
                        break;
                    case 5:
                        obj.star1.Source = obj.fillStarImage;
                        obj.star2.Source = obj.fillStarImage;
                        obj.star3.Source = obj.fillStarImage;
                        obj.star4.Source = obj.fillStarImage;
                        obj.star5.Source = obj.fillStarImage;
                        break;
                }
            }
        }

        private static void EmptyStarImagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SelfSendableRatingBar SelfSendableRatingBar = (SelfSendableRatingBar)bindable;
            if (SelfSendableRatingBar != null)
            {
                SelfSendableRatingBar.emptyStarImage = (string)newValue;
                SelfSendableRatingBar.star1.Source = SelfSendableRatingBar.emptyStarImage;
                SelfSendableRatingBar.star2.Source = SelfSendableRatingBar.emptyStarImage;
                SelfSendableRatingBar.star3.Source = SelfSendableRatingBar.emptyStarImage;
                SelfSendableRatingBar.star4.Source = SelfSendableRatingBar.emptyStarImage;
                SelfSendableRatingBar.star5.Source = SelfSendableRatingBar.emptyStarImage;
                if (!string.IsNullOrEmpty(SelfSendableRatingBar.fillStarImage))
                {
                    fillStar(SelfSendableRatingBar.SelectedStarValue, SelfSendableRatingBar);
                }
            }
        }

        private static void FillStarImagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SelfSendableRatingBar SelfSendableRatingBar = (SelfSendableRatingBar)bindable;
            if (SelfSendableRatingBar != null)
            {
                SelfSendableRatingBar.fillStarImage = (string)newValue;
                if (!string.IsNullOrEmpty(SelfSendableRatingBar.emptyStarImage))
                {
                    fillStar(SelfSendableRatingBar.SelectedStarValue, SelfSendableRatingBar);
                }
            }
        }

        private static void StarHeightRequestPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SelfSendableRatingBar SelfSendableRatingBar = (SelfSendableRatingBar)bindable;
            if (SelfSendableRatingBar != null && newValue != null)
            {
                double heightRequest = (double)newValue;
                SelfSendableRatingBar.star1.HeightRequest = heightRequest;
                SelfSendableRatingBar.star2.HeightRequest = heightRequest;
                SelfSendableRatingBar.star3.HeightRequest = heightRequest;
                SelfSendableRatingBar.star4.HeightRequest = heightRequest;
                SelfSendableRatingBar.star5.HeightRequest = heightRequest;
            }
        }

        private static void StarWidthRequestPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SelfSendableRatingBar SelfSendableRatingBar = (SelfSendableRatingBar)bindable;
            if (SelfSendableRatingBar != null && newValue != null)
            {
                double widthRequest = (double)newValue;
                SelfSendableRatingBar.star1.WidthRequest = widthRequest;
                SelfSendableRatingBar.star2.WidthRequest = widthRequest;
                SelfSendableRatingBar.star3.WidthRequest = widthRequest;
                SelfSendableRatingBar.star4.WidthRequest = widthRequest;
                SelfSendableRatingBar.star5.WidthRequest = widthRequest;
            }
        }

        private static void SelectedStarValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SelfSendableRatingBar SelfSendableRatingBar = (SelfSendableRatingBar)bindable;
            SelfSendableRatingBar.SelectedStarValue = (double)newValue;
            if (SelfSendableRatingBar != null && !string.IsNullOrEmpty(SelfSendableRatingBar.fillStarImage) && !string.IsNullOrEmpty(SelfSendableRatingBar.emptyStarImage))
            {
                fillStar(SelfSendableRatingBar.SelectedStarValue, SelfSendableRatingBar);
            }
        }

        private static void FlowDirectionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SelfSendableRatingBar SelfSendableRatingBar = (SelfSendableRatingBar)bindable;
            if (SelfSendableRatingBar != null)
            {
                if ((FlowDirectionEnum)newValue == FlowDirectionEnum.RightToLeft)
                {
                    SelfSendableRatingBar.star1.GestureRecognizers.Clear();
                    SelfSendableRatingBar.star2.GestureRecognizers.Clear();
                    SelfSendableRatingBar.star3.GestureRecognizers.Clear();
                    SelfSendableRatingBar.star4.GestureRecognizers.Clear();
                    SelfSendableRatingBar.star5.GestureRecognizers.Clear();
                    SelfSendableRatingBar.commandParameterForStar1 = 5;
                    SelfSendableRatingBar.commandParameterForStar2 = 4;
                    SelfSendableRatingBar.commandParameterForStar3 = 3;
                    SelfSendableRatingBar.commandParameterForStar4 = 2;
                    SelfSendableRatingBar.commandParameterForStar5 = 1;
                    SelfSendableRatingBar.star1.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = SelfSendableRatingBar.ItemTappedCommand,
                        CommandParameter = SelfSendableRatingBar.commandParameterForStar1
                    });
                    SelfSendableRatingBar.star2.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = SelfSendableRatingBar.ItemTappedCommand,
                        CommandParameter = SelfSendableRatingBar.commandParameterForStar2
                    });
                    SelfSendableRatingBar.star3.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = SelfSendableRatingBar.ItemTappedCommand,
                        CommandParameter = SelfSendableRatingBar.commandParameterForStar3
                    });
                    SelfSendableRatingBar.star4.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = SelfSendableRatingBar.ItemTappedCommand,
                        CommandParameter = SelfSendableRatingBar.commandParameterForStar4
                    });
                    SelfSendableRatingBar.star5.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = SelfSendableRatingBar.ItemTappedCommand,
                        CommandParameter = SelfSendableRatingBar.commandParameterForStar5
                    });
                    SelfSendableRatingBar.stkRattingbar.Children.Add(SelfSendableRatingBar.star1);
                    SelfSendableRatingBar.stkRattingbar.Children.Add(SelfSendableRatingBar.star2);
                    SelfSendableRatingBar.stkRattingbar.Children.Add(SelfSendableRatingBar.star3);
                    SelfSendableRatingBar.stkRattingbar.Children.Add(SelfSendableRatingBar.star4);
                    SelfSendableRatingBar.stkRattingbar.Children.Add(SelfSendableRatingBar.star5);
                }

                if (!string.IsNullOrEmpty(SelfSendableRatingBar.fillStarImage) && !string.IsNullOrEmpty(SelfSendableRatingBar.emptyStarImage))
                {
                    fillStar(SelfSendableRatingBar.SelectedStarValue, SelfSendableRatingBar);
                }
            }
        }

        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent()
        {
            if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
            {
                AssemblyName = typeof(SelfSendableRatingBar).GetTypeInfo().Assembly.GetName(),
                ResourcePath = "SelfSendableRatingBar.xaml",
                Instance = this
            }))
            {
                __InitComponentRuntime();
                return;
            }

            if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(GetType()) != null)
            {
                __InitComponentRuntime();
                return;
            }

            SelfSendableRatingBar SelfSendableRatingBar;
            VisualDiagnostics.RegisterSourceInfo(SelfSendableRatingBar = this, new Uri("SelfSendableRatingBar.xaml" + ";assembly=" + "SelfSendableRatingBarControl", UriKind.RelativeOrAbsolute), 2, 2);
            NameScope nameScope = (NameScope)(NameScope.GetNameScope(SelfSendableRatingBar) ?? new NameScope());
            NameScope.SetNameScope(SelfSendableRatingBar, nameScope);
            ((INameScope)nameScope).RegisterName("stkRattingbar", (object)SelfSendableRatingBar);
            if (SelfSendableRatingBar.StyleId == null)
            {
                SelfSendableRatingBar.StyleId = "stkRattingbar";
            }

            stkRattingbar = SelfSendableRatingBar;
            SelfSendableRatingBar.SetValue(View.HorizontalOptionsProperty, LayoutOptions.StartAndExpand);
            SelfSendableRatingBar.SetValue(StackLayout.OrientationProperty, StackOrientation.Horizontal);
        }

        private void __InitComponentRuntime()
        {
            this.LoadFromXaml(typeof(SelfSendableRatingBar));
            stkRattingbar = this.FindByName<StackLayout>("stkRattingbar");
        }
    }
}