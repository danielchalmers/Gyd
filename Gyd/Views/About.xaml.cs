using System.Windows;

namespace Gyd.Views
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void OKButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}