using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using System;
using Encountify.Models;
using System.Diagnostics;

namespace Encountify.Services
{
    delegate Task<string> GetDistanceDelegate(Location location);
    public class DistanceCounter
    {
        private static GetDistanceDelegate getDistance = new GetDistanceDelegate(DistanceInMetersAsync);
        private static bool IsInMeters = false;
        public static async Task<string> DistanceInMetersAsync(Location location)
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
            double dist = GetDistance(userPosition.Longitude, userPosition.Latitude, location.Longitude, location.Lattitude);
            string answer = string.Format("{0:N2} m", dist);
            return answer;
        }

        public static async Task<string> DistanceInYardsAsync(Location location)
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
            double distInMeters = GetDistance(userPosition.Longitude, userPosition.Latitude, location.Longitude, location.Lattitude);
            double distInYards = distInMeters * 0.914;
            string answer = string.Format("{0:N2} yd", distInYards);
            return answer;
        }

        public static async Task<string> GetDistance(Location location)
        {
            return await getDistance(location);
        }

        /// <summary>
        /// returns distance in meters. Taken from here: https://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates/51839058#51839058
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="otherLongitude"></param>
        /// <param name="otherLatitude"></param>
        /// <returns></returns>
        public static double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
        {
            var d1 = latitude * (Math.PI / 180.0);
            var num1 = longitude * (Math.PI / 180.0);
            var d2 = otherLatitude * (Math.PI / 180.0);
            var num2 = otherLongitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
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
