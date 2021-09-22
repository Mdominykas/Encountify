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
    class Location
    {
        [MaxLength(30), Unique]
        public string LocationName { get; set; }
    }
}