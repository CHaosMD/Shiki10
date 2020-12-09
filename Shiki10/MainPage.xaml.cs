using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.IO;
using System.Xml;
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
using System.Net;
using Shiki10.Classes;
using Shiki10.Pages;
using Windows.Storage;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace Shiki10
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string ShikiSearchURL = @"https://shikimori.org/api/animes?search=";
        List<AnimeClass> Animes;
        public List<AnimeClass> animes;

        public MainPage()
        {
            this.InitializeComponent();
            string searchURL = ShikiSearchURL +";limit=100";
            var jsonContent = new WebClient().DownloadString(searchURL);
            var animeCollections = JsonConvert.DeserializeObject<AnimeClass[]>(jsonContent);
            //aa.Source = new BitmapImage(new Uri("https://shikimori.org" + animeCollections[0].image.original));
            int arrayLength = animeCollections.Length;
            try
            {
                Random rnd = new Random();
                int j = rnd.Next(arrayLength);
                backGround.Source = new BitmapImage(new Uri("https://shikimori.org/" + animeCollections[j].image.original, UriKind.Absolute));
            }
            catch { }
            Animes = new List<AnimeClass>();
            for (int i = 0; i < arrayLength; i++)
            {
                Animes.Add(new AnimeClass() { id = animeCollections[i].id, imagepath = "https://shikimori.org" + animeCollections[i].image.original, name = animeCollections[i].name, russian = animeCollections[i].russian, url = animeCollections[i].url });
            }
            animes = Animes;
        }
        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            string searchURL = ShikiSearchURL + sender.Text + ";limit=100";
            var jsonContent = new WebClient().DownloadString(searchURL);
            var animeCollections= JsonConvert.DeserializeObject<AnimeClass[]>(jsonContent);
            //aa.Source = new BitmapImage(new Uri("https://shikimori.org" + animeCollections[0].image.original));
            int arrayLength = animeCollections.Length;
            try
            {
                NotFoundText.Text = "";
                Random rnd = new Random();
                int j = rnd.Next(arrayLength);
                backGround.Source = new BitmapImage(new Uri("https://shikimori.org/" + animeCollections[j].image.original, UriKind.Absolute));
            }
            catch { NotFoundText.Text = "Ничего нет"; }
                Animes = new List<AnimeClass>();
            for (int i = 0; i < arrayLength; i++)
            {
                Animes.Add(new AnimeClass(){ id = animeCollections[i].id, imagepath = "https://shikimori.org"+animeCollections[i].image.original, name=animeCollections[i].name, russian=animeCollections[i].russian, url=animeCollections[i].url});
            }
            animes = Animes;
            gridAnime.ItemsSource = animes;
            
        }

        private void gridAnime_ItemClick(object sender, ItemClickEventArgs e)
        {
            var nani = (AnimeClass)e.ClickedItem;
            string currentAnimeId = nani.id.ToString();

            Frame.Navigate(typeof(CurrentAnimePage),currentAnimeId);
        }
    }
}
