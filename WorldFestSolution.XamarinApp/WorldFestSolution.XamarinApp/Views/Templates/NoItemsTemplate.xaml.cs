﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorldFestSolution.XamarinApp.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoItemsTemplate : ContentView
    {
        public NoItemsTemplate()
        {
            InitializeComponent();
        }
    }
}