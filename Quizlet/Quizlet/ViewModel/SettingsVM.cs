using Prism.Commands;
using Prism.Mvvm;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    public class SettingsVM : BindableBase
    {
        // Zugriff auf MainVM für Zurück-Navigation
        private MainVM main;

        // Sichtbarkeit der Bereiche
        private bool usernameVisible;
        public bool UsernameVisible { get { return usernameVisible; } set { SetProperty(ref usernameVisible, value); } }

        private bool passwordVisible;
        public bool PasswordVisible { get { return passwordVisible; } set { SetProperty(ref passwordVisible, value); } }

        private bool emailVisible;
        public bool EmailVisible { get { return emailVisible; } set { SetProperty(ref emailVisible, value); } }

        // Status-Text
        private string statusText;
        public string StatusText { get { return statusText; } set { SetProperty(ref statusText, value); } }

        // Username Felder
        private string oldUsername;
        public string OldUsername { get { return oldUsername; } set { SetProperty(ref oldUsername, value); } }

        private string newUsername;
        public string NewUsername { get { return newUsername; } set { SetProperty(ref newUsername, value); } }

        private string newUsername2;
        public string NewUsername2 { get { return newUsername2; } set { SetProperty(ref newUsername2, value); } }

        // Passwort Felder
        private string oldPassword;
        public string OldPassword { get { return oldPassword; } set { SetProperty(ref oldPassword, value); } }

        private string newPassword;
        public string NewPassword { get { return newPassword; } set { SetProperty(ref newPassword, value); } }

        private string newPassword2;
        public string NewPassword2 { get { return newPassword2; } set { SetProperty(ref newPassword2, value); } }

        // Email Felder
        private string oldEmail;
        public string OldEmail { get { return oldEmail; } set { SetProperty(ref oldEmail, value); } }

        private string newEmail;
        public string NewEmail { get { return newEmail; } set { SetProperty(ref newEmail, value); } }

        private string newEmail2;
        public string NewEmail2 { get { return newEmail2; } set { SetProperty(ref newEmail2, value); } }

        // Commands
        private ICommand showUsernameCommand;
        public ICommand ShowUsernameCommand { get { return showUsernameCommand; } set { SetProperty(ref showUsernameCommand, value); } }

        private ICommand showPasswordCommand;
        public ICommand ShowPasswordCommand { get { return showPasswordCommand; } set { SetProperty(ref showPasswordCommand, value); } }

        private ICommand showEmailCommand;
        public ICommand ShowEmailCommand { get { return showEmailCommand; } set { SetProperty(ref showEmailCommand, value); } }

        private ICommand saveCommand;
        public ICommand SaveCommand { get { return saveCommand; } set { SetProperty(ref saveCommand, value); } }

        private ICommand backCommand;
        public ICommand BackCommand { get { return backCommand; } set { SetProperty(ref backCommand, value); } }

        public SettingsVM(MainVM main)
        {
            // MainVM merken
            this.main = main;

            // Commands setzen
            ShowUsernameCommand = new DelegateCommand(ShowUsername);
            ShowPasswordCommand = new DelegateCommand(ShowPassword);
            ShowEmailCommand = new DelegateCommand(ShowEmail);
            SaveCommand = new DelegateCommand(Save);
            BackCommand = new DelegateCommand(Back);

            // Standardansicht
            ShowUsername();
        }

        private void Back()
        {
            // Zur Lobby wechseln
            main.ShowLobby();
        }

        private void ShowUsername()
        {
            UsernameVisible = true;
            PasswordVisible = false;
            EmailVisible = false;

            StatusText = "";
            ClearInputs();
        }

        private void ShowPassword()
        {
            UsernameVisible = false;
            PasswordVisible = true;
            EmailVisible = false;

            StatusText = "";
            ClearInputs();
        }

        private void ShowEmail()
        {
            UsernameVisible = false;
            PasswordVisible = false;
            EmailVisible = true;

            StatusText = "";
            ClearInputs();
        }

        private void ClearInputs()
        {
            // Eingaben leeren
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

        private void Save()
        {
            // Nur Demo: Hier würdest du später Model/DB ansprechen
            if (UsernameVisible)
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

                // Demo-Prüfung: Alter Username muss dem aktuellen entsprechen
                if (OldUsername != AppSession.CurrentUsername)
                {
                    StatusText = "Alter Username ist falsch.";
                    return;
                }

                // Demo: Session ändern
                AppSession.CurrentUsername = NewUsername;
                StatusText = "Username geändert (Demo).";
                ClearInputs();
                return;
            }

            if (PasswordVisible)
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

                // Minimal-Regel für Demo
                if (NewPassword.Length < 3)
                {
                    StatusText = "Neues Passwort ist zu kurz.";
                    return;
                }

                StatusText = "Passwort geändert (Demo).";
                ClearInputs();
                return;
            }

            if (EmailVisible)
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

                // Einfache E-Mail Prüfung
                if (!Regex.IsMatch(NewEmail, @"^\S+@\S+\.\S+$"))
                {
                    StatusText = "Bitte eine gültige E-Mail eingeben.";
                    return;
                }

                StatusText = "E-Mail geändert (Demo).";
                ClearInputs();
                return;
            }
        }
    }
}
