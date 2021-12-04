using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Gms.Maps;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.GoogleMaps.Android;
using Encountify.Views;
using Encountify.Models;
using Encountify.Services;
using Encountify.ViewModels;
using View = Android.Views.View;
using Marker = Android.Gms.Maps.Model.Marker;
using Locations = Xamarin.Essentials.Location;
using System.Linq;

namespace Encountify.Droid
{
    class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter, IOnMapReadyCallback
    {
        GoogleMap MapView { get; set; }
        CustomMap MapControl { get; set; }
        Marker CurrentPinWindow { get; set; } = null;

        public delegate void updateVisitingType(int x);
        private updateVisitingType updateVisiting;

        public CustomMapRenderer(Context context) : base(context)
        {
            updateVisiting += new updateVisitingType(AddToDatabase);
        }

        private async void AddToDatabase(int id)
        {
            VisitedLocations newVisit = new VisitedLocations() { LocationId = id, UserId = App.UserID, Points = 100 };
            var visitedAccess = new DatabaseAccess<VisitedLocations>();
            await visitedAccess.AddAsync(newVisit);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            MapView = googleMap;
            MapView.InfoWindowClick += OnInfoWindowClick;
            MapView.SetInfoWindowAdapter(this);
            MapView.InfoWindowClose += OnInfoWindowClose;
            MapView.MyLocationChange += OnMyLocationChange;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                MapControl = (CustomMap)e.NewElement;
                ((MapView)Control).GetMapAsync(this);
            }
        }

        public async Task<View> GetInfoWindowAsync(Marker marker)
        {
            var inflater = Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;

            if (inflater != null)
            {
                CurrentPinWindow = marker;
                Locations pinLocation = new Locations(marker.Position.Latitude, marker.Position.Longitude);
                var distanceString = await DistanceCounter.GetFormattedDistance(pinLocation);

                var distanceNumber = distanceString.Split(" ").Take(1).First();
                var distanceMetric = distanceString.Split(" ").Skip(1).First();

                if (double.TryParse(distanceNumber, out var distanceDouble))
                {
                    if ((distanceDouble <= 30 && distanceMetric == "m") || (distanceDouble <= 32.81 && distanceMetric == "yd"))
                    {
                        var layout = Resource.Layout.VisitedInfoWindow; //TODO make the VisitedInfoWindow actually look good

                        return GetInfoWindowView(layout, inflater, marker, message: "You're here. PRESS ME!");
                    }
                    else //TODO make a InfoWindow for the markers that have already been visited my the user 
                    {
                        var layout = Resource.Layout.MapInfoWindow;

                        return GetInfoWindowView(layout, inflater, marker, distance: distanceString);
                    }
                }
            }

            return null;
        }

        public View GetInfoWindow(Marker marker)
        {
            View view = GetInfoWindowAsync(marker).Result;

            return view;
        }

        async void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            Locations pinLocation = new Locations(e.Marker.Position.Latitude, e.Marker.Position.Longitude);
            var distanceString = await DistanceCounter.GetFormattedDistance(pinLocation);

            var distanceNumber = distanceString.Split(" ").Take(1).First();
            var distanceMetric = distanceString.Split(" ").Skip(1).First();

            if (double.TryParse(distanceNumber, out var distanceDouble))
            {
                if ((distanceDouble <= 30 && distanceMetric == "m") || (distanceDouble <= 32.81 && distanceMetric == "yd")) //TODO handle UserVisited event.
                {
                    var access = new DatabaseAccess<Location>();
                    var locationList = access.GetAllAsync().Result;

                    Location visited = locationList.FirstOrDefault(s => s.Name == e.Marker.Title);
                    if (visited != null)
                    {
                        updateVisiting(visited.Id);
                    }
                }
                else
                {
                    var access = new DatabaseAccess<Location>();
                    var locationList = access.GetAllAsync().Result;

                    var id = locationList.Aggregate(0, (id, next) => next.Name == e.Marker.Title ? next.Id : id);

                    if (id != 0)
                    {
                        await Shell.Current.GoToAsync($"{nameof(LocationDetailPage)}?{nameof(LocationDetailViewModel.Id)}={id}");
                    }
                }
            }
        }

        public View GetInfoWindowView(int layoutXML, LayoutInflater inflater, Marker marker, string distance = "", string message = " away")
        {
            View view;

            view = inflater.Inflate(layoutXML, null);

            var infoPoints = view.FindViewById<TextView>(Resource.Id.InfoWindowPoints);
            var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
            var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);
            var infoDistance = view.FindViewById<TextView>(Resource.Id.InfoWindowDistance);

            if (infoPoints != null)
            {
                infoPoints.Text = "100 POINTS✨";
            }
            if (infoTitle != null)
            {
                infoTitle.Text = marker.Title;
            }
            if (infoSubtitle != null)
            {
                infoSubtitle.Text = marker.Snippet;
            }
            if (infoDistance != null)
            {
                infoDistance.Text = distance + message;
            }

            return view;
        }

        public View GetInfoContents(Marker marker)
        {
            return null;
        }

        void OnInfoWindowClose(object sender, GoogleMap.InfoWindowCloseEventArgs e)
        {
            CurrentPinWindow = null;
        }

        void OnMyLocationChange(object sender, GoogleMap.MyLocationChangeEventArgs e)
        {
            Task.Delay(10).ContinueWith(delegate (Task arg)
            {
                Device.BeginInvokeOnMainThread(delegate ()
                {
                    try //This place might raise an exception during debugging but doesn't "seem" to crash the app
                    {
                        CurrentPinWindow.ShowInfoWindow();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                });
            });
        }
    }
}
