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
        public Lazy<Image> ImageOpenClose { get; set; }

        public ScoreboardCell(ScoreboardEntry entry)
        {
            // IUser userData = DependencyService.Get<IUser>();
            // User user = userData.GetAsync(entry.UserId).Result;
            Name = entry.Name;
            Score = entry.Score;
            ImageOpenClose = new Lazy<Image>(() => {
                IUser userData = DependencyService.Get<IUser>();
                User user = userData.GetAsync(entry.UserId).Result;
                return new Image() { Source = ImageSource.FromStream(() => new MemoryStream(user.Picture)) };
            });
            //ImageOpenClose = new Lazy<Image>();
            //ImageOpenClose.Value.Source = ImageSource.FromStream(() => new MemoryStream(user.Picture));
        }

    }
}
