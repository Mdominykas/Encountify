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
    public class LoginTable
    {
        [PrimaryKey, AutoIncrement, Column("_Id")]
        public int Id { get; set; }
        [MaxLength(25)]

        public string Username { get; set; }

        [MaxLength(15)]

        public string Password { get; set; }
    }
}