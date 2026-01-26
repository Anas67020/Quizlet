using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    public class LobbyVM : BindableBase
    {
        public event EventHandler NewGameRequested;
        public event EventHandler StatsRequested;
        public event EventHandler SettingsRequested;
        public event EventHandler LogoutRequested;

        public ICommand NewGameCommand { get; private set; }
        public ICommand StatsCommand { get; private set; }
        public ICommand SettingsCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }

        public LobbyVM()
        {
            NewGameCommand = new DelegateCommand(NewGame);
            StatsCommand = new DelegateCommand(Stats);
            SettingsCommand = new DelegateCommand(Settings);
            LogoutCommand = new DelegateCommand(Logout);
        }

        private void NewGame()
        {
            if (NewGameRequested != null) NewGameRequested(this, EventArgs.Empty);
        }

        private void Stats()
        {
            if (StatsRequested != null) StatsRequested(this, EventArgs.Empty);
        }

        private void Settings()
        {
            if (SettingsRequested != null) SettingsRequested(this, EventArgs.Empty);
        }

        private void Logout()
        {
            if (LogoutRequested != null) LogoutRequested(this, EventArgs.Empty);
        }
    }
}
