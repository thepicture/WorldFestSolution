﻿using System;
using System.ComponentModel;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace WorldFestSolution.XamarinApp.Controls
{
    public class DateTimePicker : ContentView, INotifyPropertyChanged
    {
        public Entry Entry { get; private set; } = new Entry();
        public MaterialDatePicker DatePicker { get; private set; } = new MaterialDatePicker() { MinimumDate = DateTime.Today, IsVisible = false };
        public MaterialTimePicker TimePicker { get; private set; } = new MaterialTimePicker() { IsVisible = false };

        private string _stringFormat;
        public string StringFormat { get => _stringFormat ?? "dd/MM/yyyy HH:mm"; set => _stringFormat = value; }
        public DateTime DateTime
        {
            get => (DateTime)GetValue(DateTimeProperty);
            set { SetValue(DateTimeProperty, value); OnPropertyChanged("DateTime"); }
        }

        private TimeSpan Time
        {
            get => TimeSpan.FromTicks(DateTime.Ticks);
            set => DateTime = new DateTime(DateTime.Date.Ticks).AddTicks(value.Ticks);
        }

        private DateTime Date
        {
            get => DateTime.Date;
            set => DateTime = new DateTime(DateTime.TimeOfDay.Ticks).AddTicks(value.Ticks);
        }

        public static BindableProperty DateTimeProperty = BindableProperty.Create("DateTime",
                                                                    typeof(DateTime),
                                                                    typeof(DateTimePicker),
                                                                    DateTime.Now,
                                                                    BindingMode.TwoWay,
                                                                    propertyChanged: DTPropertyChanged);

        public DateTimePicker()
        {
            Content = new StackLayout()
            {
                Children =
            {
                DatePicker,
                TimePicker,
                Entry
            }
            };
            TimePicker.Unfocused += (sender, args) => Time = TimePicker.Time;
            DatePicker.Focused += (s, a) => UpdateEntryText();

            GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() => DatePicker.Focus())
            });
            Entry.Focused += (sender, args) =>
            {
                Device.BeginInvokeOnMainThread(() => DatePicker.Focus());
            };
            DatePicker.Unfocused += (sender, args) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    TimePicker.Focus();
                    Date = DatePicker.Date;
                    UpdateEntryText();
                    DatePicker.Unfocus();
                });
            };
        }

        private void UpdateEntryText()
        {
            Entry.Text = DateTime.ToString(StringFormat);
        }

        static void DTPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var timePicker = bindable as DateTimePicker;
            timePicker.UpdateEntryText();
        }
    }
}
