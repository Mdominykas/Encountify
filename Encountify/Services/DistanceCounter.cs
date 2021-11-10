using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using System;
using Encountify.Models;
using System.Diagnostics;
using Xamarin.Essentials;

namespace Encountify.Services
{
    delegate Task<string> GetDistanceDelegate(Models.Location location);
    public class DistanceCounter
    {
        private static GetDistanceDelegate getDistance = new GetDistanceDelegate(DistanceInMetersAsync);
        private static bool IsInMeters = false;
        public static async Task<string> DistanceInMetersAsync(Models.Location location)
        {
            var locator = CrossGeolocator.Current;
            Position userPosition = new Position(0, 0);
            try
            {
                userPosition = await locator.GetPositionAsync(TimeSpan.FromSeconds(5));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return "Could not get distance";
            }
            double distance = Xamarin.Essentials.Location.CalculateDistance(userPosition.Latitude, userPosition.Longitude, location.Latitude, location.Longitude, DistanceUnits.Kilometers) * 1000;
            string answer = string.Format("{0:N2} m", distance);
            if (distance > 1000.0)
                answer = string.Format("{0:N2} km", distance / 1000.0);
            return answer;
        }

        public static async Task<string> DistanceInYardsAsync(Models.Location location)
        {
            var locator = CrossGeolocator.Current;
            Position userPosition;
            try
            {
                userPosition = await locator.GetPositionAsync(TimeSpan.FromSeconds(5));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return "Could not get distance";
            }
            double distInMeters = Xamarin.Essentials.Location.CalculateDistance(userPosition.Latitude, userPosition.Longitude, location.Latitude, location.Longitude, DistanceUnits.Kilometers) * 1000;
            double distInYards = distInMeters * 0.914;
            string answer = string.Format("{0:N2} yd", distInYards);
            if(distInYards > 1760.0)
                answer = string.Format("{0:N2} mi", distInYards / 1760.0);
            return answer;
        }

        public static async Task<string> GetFormattedDistance(Models.Location location)
        {
            return await getDistance(location);
        }
        public static void ChangeScale()
        {
            bool var = IsInMeters;
            if (IsInMeters)
                getDistance = new GetDistanceDelegate(DistanceInYardsAsync);
            else
                getDistance = new GetDistanceDelegate(DistanceInMetersAsync);
            IsInMeters = !IsInMeters;
        }
    }
}
