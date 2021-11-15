using System.Threading.Tasks;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Geolocation = Xamarin.Essentials.Geolocation;
using Locations = Xamarin.Essentials.Location;

namespace Encountify.Services
{
    delegate Task<string> GetDistanceDelegate(Locations location);
    public class DistanceCounter
    {
        private static GetDistanceDelegate getDistance = new GetDistanceDelegate(DistanceInMetersAsync);
        public static bool IsInMeters = true;
        public static async Task<string> DistanceInMetersAsync(Locations location)
        {
            Locations userLocation;
            Locations pinLocation = new Locations(location.Latitude, location.Longitude);

            try
            {
                userLocation = await Geolocation.GetLastKnownLocationAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return "Could not get distance";
            }

            double distance = Locations.CalculateDistance(userLocation, pinLocation, DistanceUnits.Kilometers) * 1000;
            string answer = string.Format("{0:N2} m", distance);
            if (distance > 1000.0)
                answer = string.Format("{0:N2} km", distance / 1000.0);
            return answer;
        }

        public static async Task<string> DistanceInYardsAsync(Locations location)
        {
            Locations userLocation;
            Locations pinLocation = new Locations(location.Latitude, location.Longitude);

            try
            {
                userLocation = await Geolocation.GetLastKnownLocationAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return "Could not get distance";
            }

            double distInMeters = Locations.CalculateDistance(userLocation, pinLocation, DistanceUnits.Kilometers) * 1000;
            double distInYards = distInMeters * 0.914;
            string answer = string.Format("{0:N2} yd", distInYards);
            if(distInYards > 1760.0)
                answer = string.Format("{0:N2} mi", distInYards / 1760.0);
            return answer;
        }

        public static async Task<string> GetFormattedDistance(Locations location)
        {
            return await getDistance(location);
        }
        public static void ChangeScale()
        {
            if (IsInMeters)
                getDistance = new GetDistanceDelegate(DistanceInYardsAsync);
            else
                getDistance = new GetDistanceDelegate(DistanceInMetersAsync);
            IsInMeters = !IsInMeters;
            var final = IsInMeters;
        }
    }
}
