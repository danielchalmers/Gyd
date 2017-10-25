using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NYoutubeDL;

namespace Gyd.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _dialogText = string.Empty;
        private bool _isDialogOpen;

        public MainViewModel()
        {
            CloseDialogCommand = new RelayCommand<bool>(CloseDialogExecute);
            Clients = new ObservableCollection<YoutubeDL>();
            Clients.CollectionChanged += Clients_CollectionChanged;
        }

        public ObservableCollection<YoutubeDL> Clients { get; }

        public ICommand CloseDialogCommand { get; }

        public string DialogText
        {
            get => _dialogText;
            set => Set(ref _dialogText, value);
        }

        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set => Set(ref _isDialogOpen, value);
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

        private void CloseDialogExecute(bool result)
        {
            IsDialogOpen = false;

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