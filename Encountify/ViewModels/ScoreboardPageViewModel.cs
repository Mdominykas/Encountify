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
        public ObservableCollection<ScoreboardCell> Scoreboard { get; }

        public ScoreboardPageViewModel()
        {
            Scoreboard = new ObservableCollection<ScoreboardCell>();
            var scoreboardCreator = new ScoreboardCreation();
            var list = scoreboardCreator.CreateScoreboard();

            foreach (var element in list)
                Scoreboard.Add(new ScoreboardCell(element));
        }
    }
}