using Encountify.Models;
using Encountify.Services;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

namespace Encountify
{
    public partial class App : Application
    {

        public App()
        {
            //here it deletes entire previous database
            //it might be a dumb idea, however I don't really
            //want to to a migration
            //after first run it must be removed
            //cause while this is still here data
            //can not be stored between sessions
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseAccessConstants.LocationDatabaseName);
            SQLiteConnection db = new SQLiteConnection(dbPath);
            db.DropTable<Location>();

            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
