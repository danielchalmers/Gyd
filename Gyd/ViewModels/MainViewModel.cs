using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using GalaSoft.MvvmLight;
using MaterialDesignThemes.Wpf;
using NYoutubeDL;
using static NYoutubeDL.Helpers.Enums;

namespace Gyd.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Clients = new ObservableCollection<YoutubeDL>();
            Clients.CollectionChanged += Clients_CollectionChanged;

            DialogViewModel = new DialogViewModel();

            DialogClosingHandler += OnDialogClosing;
        }

        public ObservableCollection<YoutubeDL> Clients { get; }

        public DialogClosingEventHandler DialogClosingHandler { get; set; }

        public DialogViewModel DialogViewModel { get; }

        private void Clients_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var client in e.NewItems.OfType<YoutubeDL>())
            {
                StartClientDownload(client);
            }
        }

        private void OnDialogClosing(object sender, DialogClosingEventArgs e)
        {
            var vm = DialogViewModel;
            var result = (bool)e.Parameter;

            if (result)
            {
                foreach (var url in vm.GetURLs())
                {
                    var client = new YoutubeDL();

                    client.VideoUrl = url;

                    // Set up post-processing options for selected format.
                    // Format can be video or audio.
                    var format = vm.SelectedFormat.Object;
                    if (format is VideoFormat)
                    {
                        client.Options.VideoFormatOptions.Format = (VideoFormat)format;
                    }
                    else if (format is AudioFormat)
                    {
                        client.Options.PostProcessingOptions.AudioFormat = (AudioFormat)format;
                        client.Options.PostProcessingOptions.ExtractAudio = true;
                    }

                    Clients.Add(client);
                }

                vm.ClearInput();
            }
        }

        private void StartClientDownload(YoutubeDL client)
        {
            client.Download(prepareDownload: true);
        }
    }
}