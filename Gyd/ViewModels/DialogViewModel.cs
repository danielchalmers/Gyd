using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using Gyd.Models;
using static NYoutubeDL.Helpers.Enums;

namespace Gyd.ViewModels
{
    public class DialogViewModel : ViewModelBase
    {
        private string _inputText;
        private CategorizedObject _selectedFormat;

        public DialogViewModel()
        {
            ClearInput();

            Formats = new ListCollectionView(GetAllCategorizedFormats().ToList());
            Formats.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CategorizedObject.CategoryName)));
        }

        public ListCollectionView Formats { get; }

        public string InputText
        {
            get => _inputText;
            set => Set(ref _inputText, value);
        }

        public CategorizedObject SelectedFormat
        {
            get => _selectedFormat;
            set => Set(ref _selectedFormat, value);
        }

        public void ClearInput()
        {
            InputText = string.Empty;
        }

        public List<string> GetURLs() => InputText.Split('\n').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

        private IEnumerable<CategorizedObject> GetAllCategorizedFormats()
        {
            foreach (var item in Enum.GetValues(typeof(VideoFormat)))
            {
                yield return new CategorizedObject("Video", item);
            }
            foreach (var item in Enum.GetValues(typeof(AudioFormat)))
            {
                yield return new CategorizedObject("Audio", item);
            }
        }
    }
}