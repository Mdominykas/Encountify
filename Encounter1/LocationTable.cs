using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

namespace Encounter1
{
    public class LocationTable
    {
        [MaxLength(30), Unique]
        public string LocationName { get; set; }
        public double LocationCoordX { get; set; }
        public double LocationCoordY { get; set; }
    }
}