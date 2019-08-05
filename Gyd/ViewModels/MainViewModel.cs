using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using NYoutubeDL;
using static NYoutubeDL.Helpers.Enums;

namespace Gyd.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            DropCommand = new RelayCommand<DragEventArgs>(Drop);

            Clients = new ObservableCollection<YoutubeDL>();
            Clients.CollectionChanged += Clients_CollectionChanged;

            DialogViewModel = new DialogViewModel();

            DialogClosingHandler += OnDialogClosing;
        }

        public ICommand DropCommand { get; }

        public ObservableCollection<YoutubeDL> Clients { get; }

        public DialogClosingEventHandler DialogClosingHandler { get; set; }

        public DialogViewModel DialogViewModel { get; }

        private void Drop(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat) && e.Data.GetData(DataFormats.StringFormat) is string dropText)
            {
                DialogViewModel.InputText = dropText;
                OnDialogClosing(this, null);
            }
        }

        private void Clients_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                Parallel.ForEach(e.NewItems.OfType<YoutubeDL>(), async client =>
                {
                    await client.PrepareDownloadAsync();

                    // The bindings are broken.
                    // When the client is added to Clients, the DataGrid registers bindings to Client.Info properties.
                    // But before the download starts, Info is null, and doesn't raise a notification when created.
                    // RaisePropertyChanged(nameof(client.Info));

                    await client.DownloadAsync();
                });
            }

            if (e.OldItems != null)
            {
                Parallel.ForEach(e.OldItems.OfType<YoutubeDL>(), client =>
                {
                    client.CancelDownload();
                });
            }
        }

        private void OnDialogClosing(object sender, DialogClosingEventArgs e)
        {
            // A false parameter is sent when the dialog is cancelled.
            // If the closing parameter is false, don't perform any actions.
            if (e?.Parameter is bool cancelled && !cancelled)
                return;

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
    }
}