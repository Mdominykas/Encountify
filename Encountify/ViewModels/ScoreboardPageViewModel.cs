using Encountify.Models;
using Encountify.Services;
using Encountify.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Encountify.ViewModels
{
    public class ScoreboardPageViewModel : BaseViewModel
    {
        public ObservableCollection<ScoreboardCell> Scoreboard { get; set; }

        public ScoreboardPageViewModel()
        {
            CreateScoreboard();
        }

        public void CreateScoreboard()
        {
            Scoreboard = new ObservableCollection<ScoreboardCell>();
            var scoreboardCreator = new ScoreboardCreation();
            var list = scoreboardCreator.CreateScoreboard().Result;

            foreach (var element in list)
                Scoreboard.Add(new ScoreboardCell(element));

            var a = 5;
        }
    }
}