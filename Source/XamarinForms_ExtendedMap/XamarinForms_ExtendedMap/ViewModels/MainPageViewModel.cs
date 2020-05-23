using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XamarinForms_ExtendedMap.Views;

namespace XamarinForms_ExtendedMap.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        #region Members

        private Position _mapCenterPosition;
        public Position MapCenterPosition
        {
            get => _mapCenterPosition;
            set
            {
                _mapCenterPosition = value;
                NotifyPropertyChanged(nameof(MapCenterPosition));
            }
        }

        private Distance _mapDistance;
        public Distance MapDistance
        {
            get => _mapDistance;
            set
            {
                _mapDistance = value;
                NotifyPropertyChanged(nameof(MapDistance));
            }
        }

        private ObservableRangeCollection<Models.Location> _mapLocations;
        public ObservableRangeCollection<Models.Location> MapLocations
        {
            get => _mapLocations;
            set
            {
                _mapLocations = value;
                NotifyPropertyChanged(nameof(MapLocations));
            }
        }

        private int _zoomDistance;
        public int ZoomDistance
        {
            get => _zoomDistance;
            set
            {
                _zoomDistance = value;
                NotifyPropertyChanged(nameof(ZoomDistance));
            }
        }

        #endregion


        #region Commands

        private Command _zoomCommand;
        public Command ZoomCommand
        {
            get
            {
                return (_zoomCommand) ??
                    new Command
                    (
                        async () =>
                        {
                            Location currentLocation = await GetLocation();

                            if (currentLocation != null)
                            {
                                await ZoomOnMap(ZoomDistance, currentLocation.Latitude, currentLocation.Longitude);
                            }
                        }
                    );
            }
        }

        #endregion


        #region Constructors

        public MainPageViewModel()
        {
            Initialize();
        }

        #endregion


        #region Functions

        private async void Initialize()
        {
            ZoomDistance = 50;
            Location currentLocation = await GetLocation();

            if (currentLocation != null)
            {
                await ZoomOnMap(ZoomDistance, currentLocation.Latitude, currentLocation.Longitude);
            }
        }

        public static async Task<Location> GetLocation()
        {
            Location location = null;
            GeolocationRequest geolocationRequest = new GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromSeconds(2));

            try
            {
                location = await Geolocation.GetLocationAsync(geolocationRequest);
            }
            catch (Exception exception)
            {
                // TODO: Log exception.
                Debug.WriteLine(exception.Message);
            }

            return location;
        }

        private async Task ZoomOnMap(double zoomDistance, double latitude, double longitude)
        {
            if (latitude != 0.0 && longitude != 0.0)
            {
                MapCenterPosition = new Position(latitude, longitude);
            }
            else // Use the user's location.
            {
                Location currentLocation = await GetLocation();

                if (currentLocation != null)
                {
                    MapCenterPosition = new Position(Convert.ToDouble(currentLocation.Latitude), Convert.ToDouble(currentLocation.Longitude));
                }
            }

            MapDistance = Distance.FromMiles(zoomDistance);
        }

        #endregion


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
