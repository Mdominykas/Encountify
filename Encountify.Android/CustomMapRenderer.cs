using System;
using System.Threading.Tasks;
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

namespace Encountify.Droid
{
    class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter, IOnMapReadyCallback
    {
        bool _firstLoad = true;
        GoogleMap MapView { get; set; }
        CustomMap MapControl { get; set; }
        CustomPin CurrentPinWindow { get; set; }

        public CustomMapRenderer(Context context) : base(context)
        {
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            MapView = googleMap;
            MapView.InfoWindowClick += OnInfoWindowClick;
            MapView.SetInfoWindowAdapter(this);
            MapView.InfoWindowClose += OnInfoWindowClose;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {

                MapControl = (CustomMap)e.NewElement;
                if (_firstLoad)
                {
                    MapControl.RendererNeedToRefreshWindow += Map_RendererNeedToRefreshWindow;
                }

                ((MapView)Control).GetMapAsync(this);
            }
        }

        public async Task<View> GetInfoWindowAsync(Marker marker)
        {
            var inflater = Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;

            if (inflater != null)
            {
                Locations pinLocation = new Locations(marker.Position.Latitude, marker.Position.Longitude);
                var distanceString = await DistanceCounter.GetFormattedDistance(pinLocation);

                var distance = distanceString.Split(" ");

                if (double.TryParse(distance[0], out var distanceDouble))
                {
                    if (distanceDouble <= 30 && distance[1] == "m")
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

            var distance = distanceString.Split(" ");

            if (double.TryParse(distance[0], out var distanceDouble))
            {
                if (distanceDouble <= 30 && distance[1] == "m")
                {
                    //TODO handle UserVisited event.
                }
                else
                {
                    var access = new DatabaseAccess<Location>();
                    var locationList = access.GetAllAsync().Result;

                    foreach (var s in locationList)
                    {
                        if (s.Name == e.Marker.Title)
                        {
                            var id = s.Id;
                            await Shell.Current.GoToAsync($"{nameof(LocationDetailPage)}?{nameof(LocationDetailViewModel.Id)}={id}");
                            break;
                        }
                    }
                }
            } 
        }

        protected void Map_RendererNeedToRefreshWindow(object sender, CustomPin e)
        {
            Task.Delay(500).ContinueWith(delegate (Task arg)
            {
                RefreshWindow(e);
            });
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
                infoPoints.Text = "500 POINTS✨";
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

        void RefreshWindow(CustomPin pin)
        {
            Device.BeginInvokeOnMainThread(delegate ()
            {
                try
                {
                    var marker = pin.Atts["Marker"] as Marker;
                    if (CurrentPinWindow == pin)
                    {
                        marker.HideInfoWindow();
                        marker.ShowInfoWindow();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }
    }
}
