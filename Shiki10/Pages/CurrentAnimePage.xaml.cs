using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Shiki10.Classes;
using Windows.UI.Core;
using System.Diagnostics;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Shiki10.Pages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class CurrentAnimePage : Page
    {

        public CurrentAnimePage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            currentView.BackRequested += backButton_Tapped;

        string currentAnimeId = (string)e.Parameter;

            Info_Parse(currentAnimeId);
            Screenshot_Parse(currentAnimeId);
        }


        private void backButton_Tapped(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }




        public void Info_Parse(string animeID)
        {
            try
            {
                string animeURL = "https://shikimori.org/api/animes/" + animeID;
                var jsonContent = new WebClient().DownloadString(animeURL);
                var currentAnimeInfo = JsonConvert.DeserializeObject<AnimeClass>(jsonContent);
                Title_Name.Text = currentAnimeInfo.name;
                Title_Russian.Text = currentAnimeInfo.russian;
                coverImage.Source = new BitmapImage(new Uri("https://shikimori.org/system/animes/original/" + animeID + ".jpg"));

                string rating = currentAnimeInfo.rating;
                if (rating == "rx") { Age_Rating.Text = "Rx"; Age_Rating_Explained.Text = "Хентай"; }
                if (rating == "r_plus") { Age_Rating.Text = "R+"; Age_Rating_Explained.Text = "Лицам до 17 лет просмотр запрещен"; }
                if (rating == "r") { Age_Rating.Text = "R17"; Age_Rating_Explained.Text = "Лицам до 17 лет обязательно присутствие взрослого"; }
                if (rating == "pg_13") { Age_Rating.Text = "PG-13"; Age_Rating_Explained.Text = "Детям до 13 лет просмотр нежелателен"; }
                if (rating == "pg") { Age_Rating.Text = "PG"; Age_Rating_Explained.Text = "Рекомендуется присутствие родителей"; }
                if (rating == "g") { Age_Rating.Text = "G"; Age_Rating_Explained.Text = "Нет возрастных ограничений"; }

                UserScore.Text = currentAnimeInfo.score;

                try
                {
                    StudioName.Text = currentAnimeInfo.studios[0].name;
                }
                catch
                {
                    StudioName.Text = "Неизвестная студия";
                }
            }
            catch { }
        }

        public void Screenshot_Parse(string animeID)
        {
            try {
                string screenshotURL = "https://shikimori.org/api/animes/" + animeID + "/screenshots";
                var jsonContent = new WebClient().DownloadString(screenshotURL);
                var currentAnimeInfo = JsonConvert.DeserializeObject<Screenshot[]>(jsonContent);
                int arrayLength = currentAnimeInfo.Length;
                Random rnd = new Random();
                int i = rnd.Next(arrayLength);
                screenshotSource.Source = new BitmapImage(new Uri("https://shikimori.org/" + currentAnimeInfo[i].original, UriKind.Absolute));
            }
            catch {
                screenshotSource.Source = new BitmapImage(new Uri("https://shikimori.org/system/animes/original/" + animeID + ".jpg", UriKind.Absolute));
            }
        }
    }
}
