using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gyd.Views;
using MaterialDesignThemes.Wpf;
using NYoutubeDL;
using static NYoutubeDL.Helpers.Enums;

namespace Gyd.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            AboutCommand = new RelayCommand(About);

            Clients = new ObservableCollection<YoutubeDL>();
            Clients.CollectionChanged += Clients_CollectionChanged;

            DialogViewModel = new DialogViewModel();

            DialogClosingHandler += OnDialogClosing;
        }

        public ICommand AboutCommand { get; }

        public ObservableCollection<YoutubeDL> Clients { get; }

        public DialogClosingEventHandler DialogClosingHandler { get; set; }

        public DialogViewModel DialogViewModel { get; }

        private void About()
        {
            new About().ShowDialog();
        }

        private void Clients_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var client in e.NewItems.OfType<YoutubeDL>())
                {
                    StartClientDownload(client);
                }
            }

            if (e.OldItems != null)
            {
                foreach (var client in e.OldItems.OfType<YoutubeDL>())
                {
                    StopClientDownload(client);
                }
            }
        }

        private void OnDialogClosing(object sender, DialogClosingEventArgs e)
        {
            // A false parameter is sent when the dialog is cancelled.
            // If the closing parameter is false, don't perform any actions.
            if (!(bool)e.Parameter)
            {
                return;
            }

            foreach (var url in DialogViewModel.GetURLs())
            {
                var client = new YoutubeDL();

                client.VideoUrl = url;

                // Set up post-processing options for the selected format.
                // Format can be of type video or audio.
                var format = DialogViewModel.SelectedFormat.Object;
                if (format is VideoFormat)
                {
                    client.Options.VideoFormatOptions.Format = (VideoFormat)format;
                }
                else if (format is AudioFormat)
                {
                    client.Options.PostProcessingOptions.AudioFormat = (AudioFormat)format;
                    client.Options.PostProcessingOptions.ExtractAudio = true;
                }

                // Add client to active collection.
                // When added, the client will start to download.
                Clients.Add(client);
            }

            DialogViewModel.ClearInput();
        }

        private void StartClientDownload(YoutubeDL client)
        {
            // Prepare arguments for the youtube-dl process.
            client.PrepareDownload();

            // Create the process and start downloading.
            client.Download();
        }

        private void StopClientDownload(YoutubeDL client)
        {
            client.KillProcess();
        }
    }
}