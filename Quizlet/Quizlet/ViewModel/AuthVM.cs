using Prism.Commands;
using Prism.Mvvm;
using Quizlet.Model;
using System.Windows;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    public class AuthVM : BindableBase
    {
        // Zugriff auf Navigation
        private MainVM main;

        // User-Model
        private ModelUser mu = new ModelUser();

        private string username;
        public string Username { get { return username; } set { SetProperty(ref username, value); } }

        private string password;
        public string Password { get { return password; } set { SetProperty(ref password, value); } }

        private string regUsername;
        public string RegUsername { get { return regUsername; } set { SetProperty(ref regUsername, value); } }

        private string regPassword;
        public string RegPassword { get { return regPassword; } set { SetProperty(ref regPassword, value); } }

        private string regEmail;
        public string RegEmail { get { return regEmail; } set { SetProperty(ref regEmail, value); } }

        private bool loginVisible;
        public bool LoginVisible { get { return loginVisible; } set { SetProperty(ref loginVisible, value); } }

        private bool registerVisible;
        public bool RegisterVisible { get { return registerVisible; } set { SetProperty(ref registerVisible, value); } }

        private ICommand loginCommand;
        public ICommand LoginCommand { get { return loginCommand; } set { SetProperty(ref loginCommand, value); } }

        private ICommand registerCommand;
        public ICommand RegisterCommand { get { return registerCommand; } set { SetProperty(ref registerCommand, value); } }

        private ICommand showRegisterCommand;
        public ICommand ShowRegisterCommand { get { return showRegisterCommand; } set { SetProperty(ref showRegisterCommand, value); } }

        private ICommand showLoginCommand;
        public ICommand ShowLoginCommand { get { return showLoginCommand; } set { SetProperty(ref showLoginCommand, value); } }

        public AuthVM(MainVM main)
        {
            // MainVM merken
            this.main = main;

            // Default: Login anzeigen
            LoginVisible = true;
            RegisterVisible = false;

            // Commands
            LoginCommand = new DelegateCommand(Login);
            RegisterCommand = new DelegateCommand(Register);
            ShowRegisterCommand = new DelegateCommand(ShowRegister);
            ShowLoginCommand = new DelegateCommand(ShowLogin);
        }

        public void ShowRegister()
        {
            LoginVisible = false;
            RegisterVisible = true;
        }

        public void ShowLogin()
        {
            RegisterVisible = false;
            LoginVisible = true;
        }

        public void Login()
        {
            int userid = mu.CheckUser(Username, Password);

            if (userid != -1)
            {
                // Session speichern
                AppSession.CurrentUserId = userid;
                AppSession.CurrentUsername = Username;

                // Auf Lobby wechseln
                main.ShowLobby();
            }
            else
            {
                MessageBox.Show("Login fehlgeschlagen!");
            }
        }

        public void Register()
        {
            MessageBox.Show("Registrierung (Demo) erfolgreich!");

            // Demo-Session
            AppSession.CurrentUserId = 1001;
            AppSession.CurrentUsername = RegUsername;

            // Auf Lobby wechseln
            main.ShowLobby();
        }
    }
}
