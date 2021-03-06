using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WorldFestSolution.XamarinApp.Models.Serialized
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class FestivalProgram : INotifyPropertyChanged
    {
        private string title;
        private string description;
        private int durationInMinutes;
        private bool isAddedLocally;
        private bool isDeletedLocally = false;

        public int Id { get; set; }
        public int FestivalId { get; set; }
        public int DurationInMinutes
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DurationInMinutesAsString)
                    || !int.TryParse(DurationInMinutesAsString, out int value))
                {
                    return durationInMinutes;
                }
                else
                {
                    return value;
                }
            }

            set => SetProperty(ref durationInMinutes, value);
        }
        public string FormattedDuration
        {
            get
            {
                if (DurationInMinutes < 60)
                {
                    return $"{DurationInMinutes} мин.";
                }
                else
                {
                    return $"{DurationInMinutes / 60} ч. {DurationInMinutes - DurationInMinutes / 60 * 60} мин.";
                }
            }
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        public DateTime StartDateTime { get; set; }
        [JsonIgnore]
        public bool IsAddedLocally
        {
            get
            {
                if (Id != 0)
                {
                    return true;
                }
                return isAddedLocally;
            }

            set => isAddedLocally = value;
        }
        public bool IsDeletedLocally
        {
            get => isDeletedLocally;
            set => SetProperty(ref isDeletedLocally, value);
        }

        [JsonIgnore]
        public bool IsHasTitleError
        {
            get
            {
                return Title == null || string.IsNullOrWhiteSpace(Title);
            }
        }

        [JsonIgnore]
        public string TitleErrorText => "Укажите название программы";

        [JsonIgnore]
        public bool IsHasDurationInMinutesError
        {
            get
            {
                return DurationInMinutesAsString == null
                       || !int.TryParse(DurationInMinutesAsString, out int value)
                       || value <= 0;
            }
        }

        [JsonIgnore]
        public string DurationInMinutesAsString { get; set; }

        [JsonIgnore]
        public string DurationInMinutesErrorText => "Укажите положительную длительность";

        protected bool SetProperty<T>(ref T backingStore,
                                      T value,
                                      [CallerMemberName] string propertyName = "",
                                      Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}