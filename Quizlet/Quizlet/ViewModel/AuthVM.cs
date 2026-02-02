using Prism.Commands;
using Prism.Mvvm;
using Quizlet.Model;
using System.Windows;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    public class AuthVM : BindableBase
    {
        private MainVM main;
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

        // Sichtbarkeiten direkt als Visibility
        private Visibility loginVisibility;
        public Visibility LoginVisibility
        {
            get { return loginVisibility; }
            set { SetProperty(ref loginVisibility, value); }
        }

        private Visibility registerVisibility;
        public Visibility RegisterVisibility
        {
            get { return registerVisibility; }
            set { SetProperty(ref registerVisibility, value); }
        }

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
            this.main = main;

            LoginCommand = new DelegateCommand(Login);
            RegisterCommand = new DelegateCommand(Register);
            ShowRegisterCommand = new DelegateCommand(ShowRegister);
            ShowLoginCommand = new DelegateCommand(ShowLogin);

            // Standard: Login sichtbar
            ShowLogin();
        }

        private void ShowRegister()
        {
            LoginVisibility = Visibility.Collapsed;
            RegisterVisibility = Visibility.Visible;
        }

        private void ShowLogin()
        {
            RegisterVisibility = Visibility.Collapsed;
            LoginVisibility = Visibility.Visible;
        }

        private void Login()
        {
            int userid = mu.CheckUser(Username, Password);

            if (userid != -1)
            {
                AppSession.CurrentUserId = userid;
                AppSession.CurrentUsername = Username;
                main.ShowLobby();
            }
            else
            {
                MessageBox.Show("Login fehlgeschlagen!");
            }
        }

        private void Register()
        {
            MessageBox.Show("Registrierung (Demo) erfolgreich!");

            AppSession.CurrentUserId = 1001;
            AppSession.CurrentUsername = RegUsername;
            main.ShowLobby();
        }
    }
}
