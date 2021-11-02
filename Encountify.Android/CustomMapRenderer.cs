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

        public View GetInfoWindow(Marker marker)
        {
            var inflater = Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
            if (inflater != null)
            {
                View view;

                view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);

                var infoPoints = view.FindViewById<TextView>(Resource.Id.InfoWindowPoints);
                var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);
                var infoDistance = view.FindViewById<TextView>(Resource.Id.InfoWindowDistance);

                if (infoPoints != null)
                {
                    infoPoints.Text = "500 POINTS";
                }
                if (infoTitle != null)
                {
                    infoTitle.Text = marker.Title;
                }
                if (infoSubtitle != null)
                {
                    infoSubtitle.Text = marker.Snippet;
                }
                if (infoSubtitle != null)
                {
                    infoSubtitle.Text = marker.Snippet;
                }
                if (infoDistance != null)
                {
                    infoDistance.Text = "3km";
                }

                return view;
            }

            return null;
        }

        async void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
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

        protected void Map_RendererNeedToRefreshWindow(object sender, CustomPin e)
        {
            Task.Delay(500).ContinueWith(delegate (Task arg)
            {
                RefreshWindow(e);
            });
        }

        public View GetInfoContents(Marker marker)
        {
            return null;
        }
    }
}