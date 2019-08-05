using System.Windows;

namespace Gyd.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            new WpfAboutView.AboutDialog
            {
                Owner = this,
                AboutView = (WpfAboutView.AboutView)Application.Current.Resources["AboutView"]
            }.ShowDialog();
        }
    }
}