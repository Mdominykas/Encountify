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
        public ObservableCollection<ScoreboardEntry> Scoreboard { get; }

        public ScoreboardPageViewModel()
        {
            Scoreboard = new ObservableCollection<ScoreboardEntry>();
            var scoreboardCreator = new ScoreboardCreation();
            var list = scoreboardCreator.CreateScoreboard();
            Scoreboard = new ObservableCollection<ScoreboardEntry>();
            foreach (var element in list)
                Scoreboard.Add(element);

        }
    }
}