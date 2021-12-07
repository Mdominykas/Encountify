using Encountify.Services;
using System;
using System.IO;
using Xamarin.Forms;

namespace Encountify.Models
{
    public class ScoreboardCell
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public ImageSource DownloadedImageSource { get; set; }
        public Image ImageOpenClose { get; set; }

        public ScoreboardCell(ScoreboardEntry entry)
        {
            Name = entry.Name;
            Score = entry.Score;
            IUser userData = DependencyService.Get<IUser>();
            User user = userData.GetAsync(entry.UserId).Result;

            ImageOpenClose = new Image() { Source = ImageSource.FromStream(() => new MemoryStream(user.Picture)) };
            //ImageOpenClose.
/*            ImageOpenClose = new Lazy<Image>(() => {
                IUser userData = DependencyService.Get<IUser>();
                User user = userData.GetAsync(entry.UserId).Result;
                return new Image() { Source = ImageSource.FromStream(() => new MemoryStream(user.Picture)) };
            });*/
        }

    }
}
