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
    [XamlFilePath("ExtendedRatingBarRatingBar.xaml")]
    public class ExtendedRatingBar : StackLayout
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

        public static readonly BindableProperty SelectedStarCommandProperty = BindableProperty.Create("SelectedStarCommand", typeof(ICommand), typeof(ExtendedRatingBar));

        public static readonly BindableProperty EmptyStarImageProperty = BindableProperty.Create("EmptyStarImage", typeof(string), typeof(ExtendedRatingBar), "", BindingMode.TwoWay, null, EmptyStarImagePropertyChanged);

        public static readonly BindableProperty FillStarImageProperty = BindableProperty.Create("FillStarImage", typeof(string), typeof(ExtendedRatingBar), "", BindingMode.TwoWay, null, FillStarImagePropertyChanged);

        public static readonly BindableProperty StarHeightRequestProperty = BindableProperty.Create("StarHeightRequest", typeof(double), typeof(ExtendedRatingBar), 30.0, BindingMode.TwoWay, null, StarHeightRequestPropertyChanged);

        public static readonly BindableProperty StarWidthRequestProperty = BindableProperty.Create("StarWidthRequest", typeof(double), typeof(ExtendedRatingBar), 30.0, BindingMode.TwoWay, null, StarWidthRequestPropertyChanged);

        public static readonly BindableProperty SelectedStarValueProperty = BindableProperty.Create("SelectedStarValue", typeof(double), typeof(ExtendedRatingBar), default(double), BindingMode.TwoWay, null, SelectedStarValuePropertyChanged);

        public new static readonly BindableProperty FlowDirectionProperty = BindableProperty.Create("FlowDirection", typeof(FlowDirectionEnum), typeof(ExtendedRatingBar), FlowDirectionEnum.LeftToRight, BindingMode.OneWay, null, FlowDirectionPropertyChanged);

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

        public ExtendedRatingBar()
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

        private static void fillStar(double selectedValue, ExtendedRatingBar obj)
        {
            obj.SelectedStarValue = selectedValue;
            if (obj.FlowDirection == FlowDirectionEnum.RightToLeft)
            {
                switch ((int)selectedValue)
                {
                    case 0:
                        obj.star1.Source = obj.emptyStarImage;
                        obj.star2.Source = obj.emptyStarImage;
                        obj.star3.Source = obj.emptyStarImage;
                        obj.star4.Source = obj.emptyStarImage;
                        obj.star5.Source = obj.emptyStarImage;
                        break;
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
                    case 0:
                        obj.star1.Source = obj.emptyStarImage;
                        obj.star2.Source = obj.emptyStarImage;
                        obj.star3.Source = obj.emptyStarImage;
                        obj.star4.Source = obj.emptyStarImage;
                        obj.star5.Source = obj.emptyStarImage;
                        break;
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
            ExtendedRatingBar ExtendedRatingBar = (ExtendedRatingBar)bindable;
            if (ExtendedRatingBar != null)
            {
                ExtendedRatingBar.emptyStarImage = (string)newValue;
                ExtendedRatingBar.star1.Source = ExtendedRatingBar.emptyStarImage;
                ExtendedRatingBar.star2.Source = ExtendedRatingBar.emptyStarImage;
                ExtendedRatingBar.star3.Source = ExtendedRatingBar.emptyStarImage;
                ExtendedRatingBar.star4.Source = ExtendedRatingBar.emptyStarImage;
                ExtendedRatingBar.star5.Source = ExtendedRatingBar.emptyStarImage;
                if (!string.IsNullOrEmpty(ExtendedRatingBar.fillStarImage))
                {
                    fillStar(ExtendedRatingBar.SelectedStarValue, ExtendedRatingBar);
                }
            }
        }

        private static void FillStarImagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ExtendedRatingBar ExtendedRatingBar = (ExtendedRatingBar)bindable;
            if (ExtendedRatingBar != null)
            {
                ExtendedRatingBar.fillStarImage = (string)newValue;
                if (!string.IsNullOrEmpty(ExtendedRatingBar.emptyStarImage))
                {
                    fillStar(ExtendedRatingBar.SelectedStarValue, ExtendedRatingBar);
                }
            }
        }

        private static void StarHeightRequestPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ExtendedRatingBar ExtendedRatingBar = (ExtendedRatingBar)bindable;
            if (ExtendedRatingBar != null && newValue != null)
            {
                double heightRequest = (double)newValue;
                ExtendedRatingBar.star1.HeightRequest = heightRequest;
                ExtendedRatingBar.star2.HeightRequest = heightRequest;
                ExtendedRatingBar.star3.HeightRequest = heightRequest;
                ExtendedRatingBar.star4.HeightRequest = heightRequest;
                ExtendedRatingBar.star5.HeightRequest = heightRequest;
            }
        }

        private static void StarWidthRequestPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ExtendedRatingBar ExtendedRatingBar = (ExtendedRatingBar)bindable;
            if (ExtendedRatingBar != null && newValue != null)
            {
                double widthRequest = (double)newValue;
                ExtendedRatingBar.star1.WidthRequest = widthRequest;
                ExtendedRatingBar.star2.WidthRequest = widthRequest;
                ExtendedRatingBar.star3.WidthRequest = widthRequest;
                ExtendedRatingBar.star4.WidthRequest = widthRequest;
                ExtendedRatingBar.star5.WidthRequest = widthRequest;
            }
        }

        private static void SelectedStarValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ExtendedRatingBar ExtendedRatingBar = (ExtendedRatingBar)bindable;
            ExtendedRatingBar.SelectedStarValue = (double)newValue;
            if (ExtendedRatingBar != null && !string.IsNullOrEmpty(ExtendedRatingBar.fillStarImage) && !string.IsNullOrEmpty(ExtendedRatingBar.emptyStarImage))
            {
                fillStar(ExtendedRatingBar.SelectedStarValue, ExtendedRatingBar);
            }
        }

        private static void FlowDirectionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ExtendedRatingBar ExtendedRatingBar = (ExtendedRatingBar)bindable;
            if (ExtendedRatingBar != null)
            {
                if ((FlowDirectionEnum)newValue == FlowDirectionEnum.RightToLeft)
                {
                    ExtendedRatingBar.star1.GestureRecognizers.Clear();
                    ExtendedRatingBar.star2.GestureRecognizers.Clear();
                    ExtendedRatingBar.star3.GestureRecognizers.Clear();
                    ExtendedRatingBar.star4.GestureRecognizers.Clear();
                    ExtendedRatingBar.star5.GestureRecognizers.Clear();
                    ExtendedRatingBar.commandParameterForStar1 = 5;
                    ExtendedRatingBar.commandParameterForStar2 = 4;
                    ExtendedRatingBar.commandParameterForStar3 = 3;
                    ExtendedRatingBar.commandParameterForStar4 = 2;
                    ExtendedRatingBar.commandParameterForStar5 = 1;
                    ExtendedRatingBar.star1.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = ExtendedRatingBar.ItemTappedCommand,
                        CommandParameter = ExtendedRatingBar.commandParameterForStar1
                    });
                    ExtendedRatingBar.star2.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = ExtendedRatingBar.ItemTappedCommand,
                        CommandParameter = ExtendedRatingBar.commandParameterForStar2
                    });
                    ExtendedRatingBar.star3.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = ExtendedRatingBar.ItemTappedCommand,
                        CommandParameter = ExtendedRatingBar.commandParameterForStar3
                    });
                    ExtendedRatingBar.star4.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = ExtendedRatingBar.ItemTappedCommand,
                        CommandParameter = ExtendedRatingBar.commandParameterForStar4
                    });
                    ExtendedRatingBar.star5.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = ExtendedRatingBar.ItemTappedCommand,
                        CommandParameter = ExtendedRatingBar.commandParameterForStar5
                    });
                    ExtendedRatingBar.stkRattingbar.Children.Add(ExtendedRatingBar.star1);
                    ExtendedRatingBar.stkRattingbar.Children.Add(ExtendedRatingBar.star2);
                    ExtendedRatingBar.stkRattingbar.Children.Add(ExtendedRatingBar.star3);
                    ExtendedRatingBar.stkRattingbar.Children.Add(ExtendedRatingBar.star4);
                    ExtendedRatingBar.stkRattingbar.Children.Add(ExtendedRatingBar.star5);
                }

                if (!string.IsNullOrEmpty(ExtendedRatingBar.fillStarImage) && !string.IsNullOrEmpty(ExtendedRatingBar.emptyStarImage))
                {
                    fillStar(ExtendedRatingBar.SelectedStarValue, ExtendedRatingBar);
                }
            }
        }

        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent()
        {
            if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
            {
                AssemblyName = typeof(ExtendedRatingBar).GetTypeInfo().Assembly.GetName(),
                ResourcePath = "ExtendedRatingBar.xaml",
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

            ExtendedRatingBar ExtendedRatingBar;
            VisualDiagnostics.RegisterSourceInfo(ExtendedRatingBar = this, new Uri("ExtendedRatingBar.xaml" + ";assembly=" + "ExtendedRatingBarControl", UriKind.RelativeOrAbsolute), 2, 2);
            NameScope nameScope = (NameScope)(NameScope.GetNameScope(ExtendedRatingBar) ?? new NameScope());
            NameScope.SetNameScope(ExtendedRatingBar, nameScope);
            ((INameScope)nameScope).RegisterName("stkRattingbar", (object)ExtendedRatingBar);
            if (ExtendedRatingBar.StyleId == null)
            {
                ExtendedRatingBar.StyleId = "stkRattingbar";
            }

            stkRattingbar = ExtendedRatingBar;
            ExtendedRatingBar.SetValue(View.HorizontalOptionsProperty, LayoutOptions.StartAndExpand);
            ExtendedRatingBar.SetValue(StackLayout.OrientationProperty, StackOrientation.Horizontal);
        }

        private void __InitComponentRuntime()
        {
            this.LoadFromXaml(typeof(ExtendedRatingBar));
            stkRattingbar = this.FindByName<StackLayout>("stkRattingbar");
        }
    }
}