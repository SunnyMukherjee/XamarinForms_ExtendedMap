using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms.Maps;

namespace XamarinForms_ExtendedMap.Models
{
    public class Location : INotifyPropertyChanged
    {
        public string Address { get; }
        public string Description { get; set; }
        public string Label { get; set; }

        private Position _position;
        public Position Position
        {
            get => _position;
            set
            {
                _position = value;
                NotifyPropertyChanged(nameof(Position));
            }
        }

        public Location(Position position)
        {
            Position = position;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
