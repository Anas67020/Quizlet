using System.Windows;
using System.Windows.Controls;

namespace Quizlet.Views
{
    public partial class StatsPage : Page
    {
        public StatsPage()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}
