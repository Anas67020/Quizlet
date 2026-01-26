using System.Windows;
using System.Windows.Controls;
using Quizlet.Views;

namespace Quizlet
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Navigate(new AuthPage());
            ClearHistory();
        }

        public void Navigate(Page page)
        {
            MainFrame.Navigate(page);
        }

        public void ClearHistory()
        {
            if (MainFrame.NavigationService == null)
                return;

            while (MainFrame.NavigationService.RemoveBackEntry() != null) { }
        }
    }
}
