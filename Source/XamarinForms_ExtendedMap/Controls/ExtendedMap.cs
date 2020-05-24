using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace XamarinForms_ExtendedMap.Controls
{
    public class ExtendedMap : Map
    {
        #region Members

        private static Position _centerPosition;

        public Position CenterPosition
        {
            get { return (Position)GetValue(CenterPositionProperty); }
            set { SetValue(CenterPositionProperty, value); }
        }

        public static readonly BindableProperty CenterPositionProperty =
           BindableProperty.Create
            (
                propertyName: nameof(CenterPosition),
                returnType: typeof(Position),
                declaringType: typeof(ExtendedMap),
                defaultValue: null,
                propertyChanged: (sender, oldValue, newValue) =>
                {
                    ExtendedMap map = (ExtendedMap)sender;

                    if (newValue is Position position)
                    {
                        _centerPosition = new Position(position.Latitude, position.Longitude);

                        map.MoveToRegion
                        (
                            MapSpan.FromCenterAndRadius
                            (new Position(position.Latitude, position.Longitude), _distance)
                        );
                    }
                }
           );

        private static Distance _distance;

        public Distance Distance
        {
            get { return (Distance)GetValue(DistanceProperty); }
            set { SetValue(DistanceProperty, value); }
        }

        public static readonly BindableProperty DistanceProperty =
           BindableProperty.Create
            (
                propertyName: nameof(DistanceProperty),
                returnType: typeof(Distance),
                declaringType: typeof(ExtendedMap),
                defaultValue: null,
                propertyChanged: (sender, oldValue, newValue) =>
                {
                    ExtendedMap map = sender as ExtendedMap;

                    if (newValue is Distance distance)
                    {
                        _distance = distance;
                    }

                    map.MoveToRegion
                        (
                            MapSpan.FromCenterAndRadius(_centerPosition, _distance)
                        );
                }
           );

        public MapSpan MapSpan
        {
            get { return (MapSpan)GetValue(MapSpanProperty); }
            set { SetValue(MapSpanProperty, value); }
        }

        public static readonly BindableProperty MapSpanProperty = BindableProperty.Create
            (
                propertyName: nameof(MapSpan),
                returnType: typeof(MapSpan),
                declaringType: typeof(ExtendedMap),
                defaultValue: null,
                defaultBindingMode: BindingMode.TwoWay,
                validateValue: null,
                propertyChanged: MapSpanPropertyChanged
            );

        public ObservableRangeCollection<Pin> PinsCollection
        {
            get { return (ObservableRangeCollection<Pin>)GetValue(PinsCollectionProperty); }
            set { SetValue(PinsCollectionProperty, value); }
        }

        public static readonly BindableProperty PinsCollectionProperty = BindableProperty.Create
            (
                propertyName: nameof(PinsCollection),
                returnType: typeof(ObservableRangeCollection<Pin>),
                declaringType: typeof(ExtendedMap),
                defaultValue: null,
                defaultBindingMode: BindingMode.TwoWay,
                validateValue: null,
                propertyChanged: PinsCollectionPropertyChanged
            );

        #endregion


        #region Commands



        #endregion


        #region Constructors

        public ExtendedMap() : base()
        {

        }

        public ExtendedMap(MapSpan region) : base(region)
        {
            MapSpan = region;
        }

        #endregion


        #region Event Handlers

        private static void MapSpanPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ExtendedMap mapObj = bindable as ExtendedMap;
            MapSpan newSpan = newValue as MapSpan;

            mapObj?.MoveToRegion(newSpan);
        }

        private void PinsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdatePinsSource(this, sender as IEnumerable<Pin>);
        }

        #endregion


        #region Functions

        private static void PinsCollectionPropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {
            ExtendedMap extendedMap = bindable as ExtendedMap;
            extendedMap.Pins.Clear();

            ObservableRangeCollection<Pin> collection = (ObservableRangeCollection<Pin>)newValue;

            foreach (var item in collection)
                extendedMap.Pins.Add(item);

            collection.CollectionChanged += (sender, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    switch (e.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                        case NotifyCollectionChangedAction.Replace:
                        case NotifyCollectionChangedAction.Remove:
                            if (e.OldItems != null)
                                foreach (var item in e.OldItems)
                                    extendedMap.Pins.Remove((Pin)item);
                            if (e.NewItems != null)
                                foreach (var item in e.NewItems)
                                    extendedMap.Pins.Add((Pin)item);
                            break;
                        case NotifyCollectionChangedAction.Reset:
                            extendedMap.Pins.Clear();
                            break;
                    }
                });
            };
        }

        private static void UpdatePinsSource(Map bindableMap, IEnumerable<Pin> newPins)
        {
            bindableMap.Pins.Clear();

            foreach (var pin in newPins)
                bindableMap.Pins.Add(pin);
        }

        #endregion
    }
}
