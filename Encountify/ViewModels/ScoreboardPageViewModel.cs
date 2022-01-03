using Autofac;
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
            IScoreboardCreation scoreboardCreator;
            using (var scope = App.Container.BeginLifetimeScope())
            {
                scoreboardCreator = scope.Resolve<IScoreboardCreation>();
            }
            //var scoreboardCreator = new ScoreboardCreation();
            var list = scoreboardCreator.CreateScoreboard().Result;

            foreach (var element in list)
                Scoreboard.Add(new ScoreboardCell(element));
        }
    }
}