using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using GalaSoft.MvvmLight;
using MaterialDesignThemes.Wpf;
using NYoutubeDL;

namespace Gyd.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _dialogText = string.Empty;

        public MainViewModel()
        {
            Clients = new ObservableCollection<YoutubeDL>();
            Clients.CollectionChanged += Clients_CollectionChanged;

            DialogClosingHandler += OnDialogClosing;
        }

        public ObservableCollection<YoutubeDL> Clients { get; }

        public DialogClosingEventHandler DialogClosingHandler { get; set; }

        public string DialogText
        {
            get => _dialogText;
            set => Set(ref _dialogText, value);
        }

        private void AddVideo(string url)
        {
            var client = new YoutubeDL();

            client.VideoUrl = url;

            Clients.Add(client);
        }

        private void Clients_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var client in e.NewItems.OfType<YoutubeDL>())
            {
                StartClientDownload(client);
            }
        }

        private void OnDialogClosing(object sender, DialogClosingEventArgs e)
        {
            var result = (bool)e.Parameter;

            if (result)
            {
                // Split dialog text into URLs by newline.
                var urls = DialogText.Split('\n').Where(x => !string.IsNullOrWhiteSpace(x));

                // Clear dialog text.
                DialogText = string.Empty;

                // Add URLs.
                foreach (var url in urls)
                {
                    AddVideo(url);
                }
            }
        }

        private void StartClientDownload(YoutubeDL client)
        {
            client.Download(prepareDownload: true);
        }
    }
}