using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    public class StatsVM : BindableBase
    {
        private MainVM main;

        private ICommand backCommand;
        public ICommand BackCommand
        {
            get { return backCommand; }
            set { SetProperty(ref backCommand, value); }
        }
        public Quizlet.Model.AppSession Session
        {
            get { return main.Session; }
        }


        public StatsVM(MainVM main)
        {
            this.main = main;

            BackCommand = new DelegateCommand(Back);
        }

        private void Back()
        {
            // Zur Lobby wechseln
            main.ShowLobby();
        }
    }
}
