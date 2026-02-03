using Prism.Commands;
using Prism.Mvvm;
using Quizlet.Model;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    public class SettingsVM : BindableBase
    {
        private MainVM main;
        private ModelUserService model;

        private Visibility usernameVisibility;
        public Visibility UsernameVisibility { get { return usernameVisibility; } set { SetProperty(ref usernameVisibility, value); } }

        private Visibility passwordVisibility;
        public Visibility PasswordVisibility { get { return passwordVisibility; } set { SetProperty(ref passwordVisibility, value); } }

        private Visibility emailVisibility;
        public Visibility EmailVisibility { get { return emailVisibility; } set { SetProperty(ref emailVisibility, value); } }

        private string statusText;
        public string StatusText { get { return statusText; } set { SetProperty(ref statusText, value); } }

        private string oldUsername;
        public string OldUsername { get { return oldUsername; } set { SetProperty(ref oldUsername, value); } }

        private string newUsername;
        public string NewUsername { get { return newUsername; } set { SetProperty(ref newUsername, value); } }

        private string newUsername2;
        public string NewUsername2 { get { return newUsername2; } set { SetProperty(ref newUsername2, value); } }

        private string oldPassword;
        public string OldPassword { get { return oldPassword; } set { SetProperty(ref oldPassword, value); } }

        private string newPassword;
        public string NewPassword { get { return newPassword; } set { SetProperty(ref newPassword, value); } }

        private string newPassword2;
        public string NewPassword2 { get { return newPassword2; } set { SetProperty(ref newPassword2, value); } }

        private string oldEmail;
        public string OldEmail { get { return oldEmail; } set { SetProperty(ref oldEmail, value); } }

        private string newEmail;
        public string NewEmail { get { return newEmail; } set { SetProperty(ref newEmail, value); } }

        private string newEmail2;
        public string NewEmail2 { get { return newEmail2; } set { SetProperty(ref newEmail2, value); } }

        public ICommand ShowUsernameCommand { get; private set; }
        public ICommand ShowPasswordCommand { get; private set; }
        public ICommand ShowEmailCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand BackCommand { get; private set; }

        public SettingsVM(MainVM main)
        {
            this.main = main;

            // Model bekommt die Session-Instanz
            model = new ModelUserService(main.Session);

            ShowUsernameCommand = new DelegateCommand(ShowUsername);
            ShowPasswordCommand = new DelegateCommand(ShowPassword);
            ShowEmailCommand = new DelegateCommand(ShowEmail);
            SaveCommand = new DelegateCommand(async () => await SaveAsync());
            BackCommand = new DelegateCommand(Back);

            ShowUsername();
        }

        private void Back()
        {
            main.ShowLobby();
        }

        private void ShowUsername()
        {
            UsernameVisibility = Visibility.Visible;
            PasswordVisibility = Visibility.Collapsed;
            EmailVisibility = Visibility.Collapsed;

            StatusText = "";
            ClearInputs();
        }

        private void ShowPassword()
        {
            UsernameVisibility = Visibility.Collapsed;
            PasswordVisibility = Visibility.Visible;
            EmailVisibility = Visibility.Collapsed;

            StatusText = "";
            ClearInputs();
        }

        private void ShowEmail()
        {
            UsernameVisibility = Visibility.Collapsed;
            PasswordVisibility = Visibility.Collapsed;
            EmailVisibility = Visibility.Visible;

            StatusText = "";
            ClearInputs();
        }

        private void ClearInputs()
        {
            OldUsername = "";
            NewUsername = "";
            NewUsername2 = "";

            OldPassword = "";
            NewPassword = "";
            NewPassword2 = "";

            OldEmail = "";
            NewEmail = "";
            NewEmail2 = "";
        }

        private async Task SaveAsync()
        {
            StatusText = "";

            // Username ändern
            if (UsernameVisibility == Visibility.Visible)
            {
                if (string.IsNullOrWhiteSpace(OldUsername) ||
                    string.IsNullOrWhiteSpace(NewUsername) ||
                    string.IsNullOrWhiteSpace(NewUsername2))
                {
                    StatusText = "Bitte alle Felder ausfüllen.";
                    return;
                }

                if (NewUsername != NewUsername2)
                {
                    StatusText = "Neue Usernames stimmen nicht überein.";
                    return;
                }

                if (OldUsername != main.Session.CurrentUsername)
                {
                    StatusText = "Alter Username ist falsch.";
                    return;
                }

                // API braucht current_password
                if (string.IsNullOrWhiteSpace(OldPassword))
                {
                    StatusText = "Bitte aktuelles Passwort eingeben.";
                    return;
                }

                bool ok = await model.ChangeNicknameAsync(OldPassword, NewUsername);
                if (ok)
                {
                    StatusText = "Username geändert.";
                    ClearInputs();
                    return;
                }

                StatusText = model.LastError;
                return;
            }

            // Passwort ändern
            if (PasswordVisibility == Visibility.Visible)
            {
                if (string.IsNullOrWhiteSpace(OldPassword) ||
                    string.IsNullOrWhiteSpace(NewPassword) ||
                    string.IsNullOrWhiteSpace(NewPassword2))
                {
                    StatusText = "Bitte alle Felder ausfüllen.";
                    return;
                }

                if (NewPassword != NewPassword2)
                {
                    StatusText = "Neue Passwörter stimmen nicht überein.";
                    return;
                }

                bool ok = await model.ChangePasswordAsync(OldPassword, NewPassword);
                if (ok)
                {
                    StatusText = "Passwort geändert.";
                    ClearInputs();
                    return;
                }

                StatusText = model.LastError;
                return;
            }

            // Email ändern
            if (EmailVisibility == Visibility.Visible)
            {
                if (string.IsNullOrWhiteSpace(OldEmail) ||
                    string.IsNullOrWhiteSpace(NewEmail) ||
                    string.IsNullOrWhiteSpace(NewEmail2))
                {
                    StatusText = "Bitte alle Felder ausfüllen.";
                    return;
                }

                if (NewEmail != NewEmail2)
                {
                    StatusText = "Neue E-Mails stimmen nicht überein.";
                    return;
                }

                if (Regex.IsMatch(NewEmail, @"^\S+@\S+\.\S+$") == false)
                {
                    StatusText = "Bitte eine gültige E-Mail eingeben.";
                    return;
                }

                // API braucht current_password
                if (string.IsNullOrWhiteSpace(OldPassword))
                {
                    StatusText = "Bitte aktuelles Passwort eingeben.";
                    return;
                }

                bool ok = await model.ChangeEmailAsync(OldPassword, NewEmail);
                if (ok)
                {
                    StatusText = "E-Mail geändert. Bitte E-Mail bestätigen, falls nötig.";
                    ClearInputs();
                    return;
                }

                StatusText = model.LastError;
                return;
            }
        }
    }
}
