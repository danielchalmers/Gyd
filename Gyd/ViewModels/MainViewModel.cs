using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Gyd.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _dialogText;
        private bool _isDialogOpen;

        public MainViewModel()
        {
            CloseDialogCommand = new RelayCommand<bool>(CloseDialogExecute);
        }

        public ICommand CloseDialogCommand { get; }

        public string DialogText
        {
            get => _dialogText;
            set => Set(ref _dialogText, value);
        }

        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set
            {
                Set(ref _isDialogOpen, value);
                if (!value)
                {
                    OnNewVideoDialogClosed();
                }
            }
        }

        private void CloseDialogExecute(bool result)
        {
            IsDialogOpen = false;
        }

        private void OnNewVideoDialogClosed()
        {
        }
    }
}