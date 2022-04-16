﻿using System;
using System.ComponentModel;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace WorldFestSolution.XamarinApp.Controls
{
    public class DateTimePicker : ContentView, INotifyPropertyChanged
    {
        public Entry _entry { get; private set; } = new Entry();
        public MaterialDatePicker _datePicker { get; private set; } = new MaterialDatePicker() { MinimumDate = DateTime.Today, IsVisible = false };
        public MaterialTimePicker _timePicker { get; private set; } = new MaterialTimePicker() { IsVisible = false };
        string _stringFormat { get; set; }
        public string StringFormat { get => _stringFormat ?? "dd/MM/yyyy HH:mm"; set => _stringFormat = value; }
        public DateTime DateTime
        {
            get => (DateTime)GetValue(DateTimeProperty);
            set { SetValue(DateTimeProperty, value); OnPropertyChanged("DateTime"); }
        }

        private TimeSpan _time
        {
            get => TimeSpan.FromTicks(DateTime.Ticks);
            set => DateTime = new DateTime(DateTime.Date.Ticks).AddTicks(value.Ticks);
        }

        private DateTime _date
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
                _datePicker,
                _timePicker,
                _entry
            }
            };
            _datePicker.SetBinding<DateTimePicker>(MaterialDatePicker.DateProperty, p => p._date);
            _timePicker.SetBinding<DateTimePicker>(MaterialTimePicker.TimeProperty, p => p._time);
            _timePicker.Unfocused += (sender, args) => _time = _timePicker.Time;
            _datePicker.Focused += (s, a) => UpdateEntryText();

            GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() => _datePicker.Focus())
            });
            _entry.Focused += (sender, args) =>
            {
                Device.BeginInvokeOnMainThread(() => _datePicker.Focus());
            };
            _datePicker.Unfocused += (sender, args) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _timePicker.Focus();
                    _date = _datePicker.Date;
                    UpdateEntryText();
                    _datePicker.Unfocus();
                });
            };
        }

        private void UpdateEntryText()
        {
            _entry.Text = DateTime.ToString(StringFormat);
        }

        static void DTPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var timePicker = bindable as DateTimePicker;
            timePicker.UpdateEntryText();
        }
    }
}
