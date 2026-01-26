using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    public class SettingsVM : BindableBase
    {
        private bool usernameVisible;
        private bool passwordVisible;
        private bool emailVisible;

        private string statusText;

        // Username Felder
        private string oldUsername;
        private string newUsername;
        private string newUsername2;

        // Passwort Felder
        private string oldPassword;
        private string newPassword;
        private string newPassword2;

        // Email Felder
        private string oldEmail;
        private string newEmail;
        private string newEmail2;

        public bool UsernameVisible
        {
            get { return usernameVisible; }
            set { SetProperty(ref usernameVisible, value); }
        }

        public bool PasswordVisible
        {
            get { return passwordVisible; }
            set { SetProperty(ref passwordVisible, value); }
        }

        public bool EmailVisible
        {
            get { return emailVisible; }
            set { SetProperty(ref emailVisible, value); }
        }

        public string StatusText
        {
            get { return statusText; }
            set { SetProperty(ref statusText, value); }
        }

        public string OldUsername { get { return oldUsername; } set { SetProperty(ref oldUsername, value); } }
        public string NewUsername { get { return newUsername; } set { SetProperty(ref newUsername, value); } }
        public string NewUsername2 { get { return newUsername2; } set { SetProperty(ref newUsername2, value); } }

        public string OldPassword { get { return oldPassword; } set { SetProperty(ref oldPassword, value); } }
        public string NewPassword { get { return newPassword; } set { SetProperty(ref newPassword, value); } }
        public string NewPassword2 { get { return newPassword2; } set { SetProperty(ref newPassword2, value); } }

        public string OldEmail { get { return oldEmail; } set { SetProperty(ref oldEmail, value); } }
        public string NewEmail { get { return newEmail; } set { SetProperty(ref newEmail, value); } }
        public string NewEmail2 { get { return newEmail2; } set { SetProperty(ref newEmail2, value); } }

        public ICommand ShowUsernameCommand { get; private set; }
        public ICommand ShowPasswordCommand { get; private set; }
        public ICommand ShowEmailCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public SettingsVM()
        {
            ShowUsernameCommand = new DelegateCommand(ShowUsername);
            ShowPasswordCommand = new DelegateCommand(ShowPassword);
            ShowEmailCommand = new DelegateCommand(ShowEmail);
            SaveCommand = new DelegateCommand(Save);

            // Standard: Username anzeigen
            ShowUsername();
        }

        private void ShowUsername()
        {
            UsernameVisible = true;
            PasswordVisible = false;
            EmailVisible = false;

            StatusText = "";
        }

        private void ShowPassword()
        {
            UsernameVisible = false;
            PasswordVisible = true;
            EmailVisible = false;

            StatusText = "";
        }

        private void ShowEmail()
        {
            UsernameVisible = false;
            PasswordVisible = false;
            EmailVisible = true;

            StatusText = "";
        }

        private void Save()
        {
            // Hier nur einfache Prüfung + Demo.
            // Später würdest du hier wirklich den User ändern (Model/DB).

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

                StatusText = "Username geändert (Demo).";
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

                StatusText = "Passwort geändert (Demo).";
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

                StatusText = "E-Mail geändert (Demo).";
                return;
            }
        }
    }
}
