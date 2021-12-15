using System.Threading.Tasks;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Geolocation = Xamarin.Essentials.Geolocation;
using Locations = Xamarin.Essentials.Location;
using System.Linq;
using System.Collections.Generic;

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
            double distInYards = distInMeters * 1.094;
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

        public static Double ConvertedToMetersDistance(string formattedDistance)
        {
            List<Char> remove = new List<char> { 'k', 'm', 'y', 'd', 'i' };
            string end = new string(formattedDistance.Where(c => c != '.' && c != ',' && (c < '0' || c > '9')).ToArray());
            double result = 0;

            if(end.Equals(" km"))
            {
                var distance = string.Concat(formattedDistance.Split(remove.ToArray()));
                Double.TryParse(distance, out result);
                result *= 1000;
            }

            else if(end.Equals(" m"))
            {
                var distance = string.Concat(formattedDistance.Split(remove.ToArray()));
                Double.TryParse(distance, out result);
            }

            else if(end.Equals(" yd"))
            {
                var distance = string.Concat(formattedDistance.Split(remove.ToArray()));
                Double.TryParse(distance, out result);
                result /= 1.094;
                result = (double)System.Math.Round(result,2);
            }

            else if(end.Equals(" mi"))
            {
                var distance = string.Concat(formattedDistance.Split(remove.ToArray()));
                Double.TryParse(distance, out result);
                result *= 1609;
            }

            return result;
        }
    }
}
