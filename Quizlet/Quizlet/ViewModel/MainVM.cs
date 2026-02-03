using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    public class MainVM : BindableBase
    {
        private object currentViewModel;
        public object CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }

        private Quizlet.Model.AppSession session;
        public Quizlet.Model.AppSession Session
        {
            get { return session; }
            private set { SetProperty(ref session, value); }
        }

        public MainVM()
        {
            Session = new Quizlet.Model.AppSession();
            ShowAuth();
        }

        public void ShowAuth()
        {
            CurrentViewModel = new AuthVM(this);
        }

        public void ShowLobby()
        {
            CurrentViewModel = new LobbyVM(this);
        }

        public void ShowSettings()
        {
            CurrentViewModel = new SettingsVM(this);
        }
    }
}
