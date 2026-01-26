using Prism.Commands;
using Prism.Mvvm;
using Quizlet.Model;
using System;
using System.Windows;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    public class AuthVM : BindableBase
    {
        private ModelUser mu = new ModelUser();

        private string username;
        private string password;

        private string regUsername;
        private string regPassword;
        private string regEmail;

        private bool loginVisible;
        private bool registerVisible;

        public event EventHandler AuthSucceeded;

        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }

        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        public string RegUsername
        {
            get { return regUsername; }
            set { SetProperty(ref regUsername, value); }
        }

        public string RegPassword
        {
            get { return regPassword; }
            set { SetProperty(ref regPassword, value); }
        }

        public string RegEmail
        {
            get { return regEmail; }
            set { SetProperty(ref regEmail, value); }
        }

        public bool LoginVisible
        {
            get { return loginVisible; }
            set { SetProperty(ref loginVisible, value); }
        }

        public bool RegisterVisible
        {
            get { return registerVisible; }
            set { SetProperty(ref registerVisible, value); }
        }

        public ICommand LoginCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }
        public ICommand ShowRegisterCommand { get; private set; }
        public ICommand ShowLoginCommand { get; private set; }

        public AuthVM()
        {
            LoginVisible = true;
            RegisterVisible = false;

            LoginCommand = new DelegateCommand(Login);
            RegisterCommand = new DelegateCommand(Register);
            ShowRegisterCommand = new DelegateCommand(ShowRegister);
            ShowLoginCommand = new DelegateCommand(ShowLogin);
        }

        private void ShowRegister()
        {
            LoginVisible = false;
            RegisterVisible = true;
        }

        private void ShowLogin()
        {
            RegisterVisible = false;
            LoginVisible = true;
        }

        public void Login()
        {
            try
            {
                int userid = mu.CheckUser(Username, Password);

                if (userid != -1)
                {
                    MessageBox.Show("Hallo " + Username + ", ID: " + userid);

                    if (AuthSucceeded != null)
                        AuthSucceeded(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("Login fehlgeschlagen!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Register()
        {
            // Demo-Register: direkt "erfolgreich"
            MessageBox.Show("Registrierung (Demo) erfolgreich!");

            if (AuthSucceeded != null)
                AuthSucceeded(this, EventArgs.Empty);
        }
    }
}
