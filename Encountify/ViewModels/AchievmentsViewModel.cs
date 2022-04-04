using Encountify.Models;
using Encountify.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Encountify.ViewModels
{
    public class AchievmentsViewModel : BaseViewModel
    {
        private AchievmentsViewModel _selectedAchievment;

        public ObservableCollection<Achievment> Achievments { get; }
        public Command LoadAchievmentsCommand { get; }

        public AchievmentsViewModel()
        {
            Achievments = new ObservableCollection<Achievment>();
            LoadAchievmentsCommand = new Command(async () => await ExecuteLoadAchievmentsCommand());
        }

        async Task ExecuteLoadAchievmentsCommand()
        {
            IsBusy = true;

            try
            {
                Achievments.Clear();
                var achievments = await AchievmentData.GetAllAsync();
                var assignedAchievments = await AssignedAchievmentData.GetAllAsync();

                foreach (var assignedAchievment in assignedAchievments)
                {
                    if(assignedAchievment.UserId == App.UserID)
                    {
                        foreach (var achievment in achievments)
                        {
                            if(achievment.Id == assignedAchievment.AchievmentId)
                            {
                                Achievments.Add(achievment);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}
