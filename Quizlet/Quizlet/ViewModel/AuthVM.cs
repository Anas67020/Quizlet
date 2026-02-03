using Prism.Commands;
using Prism.Mvvm;
using Quizlet.Model;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    public class AuthVM : BindableBase
    {
        private MainVM main;
        private ModelUserService model;

        private string statusText;
        public string StatusText { get { return statusText; } set { SetProperty(ref statusText, value); } }

        private string username;
        public string Username { get { return username; } set { SetProperty(ref username, value); } }

        private string password;
        public string Password { get { return password; } set { SetProperty(ref password, value); } }

        private string regUsername;
        public string RegUsername { get { return regUsername; } set { SetProperty(ref regUsername, value); } }

        private string regFullname;
        public string RegFullname { get { return regFullname; } set { SetProperty(ref regFullname, value); } }

        private string regPassword;
        public string RegPassword { get { return regPassword; } set { SetProperty(ref regPassword, value); } }

        private string regEmail;
        public string RegEmail { get { return regEmail; } set { SetProperty(ref regEmail, value); } }

        private Visibility loginVisibility;
        public Visibility LoginVisibility { get { return loginVisibility; } set { SetProperty(ref loginVisibility, value); } }

        private Visibility registerVisibility;
        public Visibility RegisterVisibility { get { return registerVisibility; } set { SetProperty(ref registerVisibility, value); } }

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

            // Model bekommt die Session-Instanz
            model = new ModelUserService(main.Session);

            LoginCommand = new DelegateCommand(async () => await LoginAsync());
            RegisterCommand = new DelegateCommand(async () => await RegisterAsync());
            ShowRegisterCommand = new DelegateCommand(ShowRegister);
            ShowLoginCommand = new DelegateCommand(ShowLogin);

            ShowLogin();
        }

        private void ShowRegister()
        {
            StatusText = "";
            LoginVisibility = Visibility.Collapsed;
            RegisterVisibility = Visibility.Visible;
        }

        private void ShowLogin()
        {
            StatusText = "";
            RegisterVisibility = Visibility.Collapsed;
            LoginVisibility = Visibility.Visible;
        }

        private async Task LoginAsync()
        {
            StatusText = "";

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                StatusText = "Bitte Username/E-Mail und Passwort eingeben.";
                return;
            }

            bool ok = await model.LoginAsync(Username, Password);

            if (ok)
            {
                Password = "";
                main.ShowLobby();
                return;
            }

            StatusText = model.LastError;
        }

        private async Task RegisterAsync()
        {
            MessageBox.Show("RegisterCommand wurde ausgelöst");//test das das programm überhaupt soweit kommt

            StatusText = "";

            if (string.IsNullOrWhiteSpace(RegEmail) ||
                string.IsNullOrWhiteSpace(RegUsername) ||
                string.IsNullOrWhiteSpace(RegFullname) ||
                string.IsNullOrWhiteSpace(RegPassword))
            {
                StatusText = "Bitte alle Felder ausfüllen.";
                return;
            }

            bool ok = await model.RegisterAsync(RegEmail, RegUsername, RegFullname, RegPassword);

            if (ok)
            {
                StatusText = "Account erstellt. Bitte E-Mail bestätigen, dann einloggen.";
                ShowLogin();

                RegEmail = "";
                RegUsername = "";
                RegFullname = "";
                RegPassword = "";
                return;
            }

            StatusText = model.LastError;
        }
    }
}
