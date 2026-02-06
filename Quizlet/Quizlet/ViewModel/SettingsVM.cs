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
        private enum SettingsSection
        {
            Username,
            Password,
            Email,
            Profilbild
        }

        private SettingsSection currentSection;

        private MainVM main;
        private ModelUserService model;

        // Sichtbarkeiten
        private Visibility usernameVisibility;
        public Visibility UsernameVisibility { get { return usernameVisibility; } set { SetProperty(ref usernameVisibility, value); } }

        private Visibility passwordVisibility;
        public Visibility PasswordVisibility { get { return passwordVisibility; } set { SetProperty(ref passwordVisibility, value); } }

        private Visibility emailVisibility;
        public Visibility EmailVisibility { get { return emailVisibility; } set { SetProperty(ref emailVisibility, value); } }
        private Visibility profilVisibility;
        public Visibility ProfilVisibility { get { return profilVisibility; } set { SetProperty(ref profilVisibility, value); } }

        private string statusText;
        public string StatusText { get { return statusText; } set { SetProperty(ref statusText, value); } }

        // Eingaben
        private string oldUsername;
        public string OldUsername { get { return oldUsername; } set { SetProperty(ref oldUsername, value); } }

        private string newUsername;
        public string NewUsername { get { return newUsername; } set { SetProperty(ref newUsername, value); } }

        private string newUsername2;
        public string NewUsername2 { get { return newUsername2; } set { SetProperty(ref newUsername2, value); } }

        // WICHTIG:
        // OldPassword = "aktuelles Passwort" (für Username/Email Änderung) UND "altes Passwort" (für Passwort ändern)
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
        private string oldProfil;
        public string OldProfil { get { return oldProfil; } set { SetProperty(ref oldProfil, value); } }

        private string newProfil;
        public string NewProfil { get { return newProfil; } set { SetProperty(ref newProfil, value); } }

        public ICommand ShowUsernameCommand { get; private set; }
        public ICommand ShowPasswordCommand { get; private set; }
        public ICommand ShowEmailCommand { get; private set; }
        public ICommand ShowProfilCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand BackCommand { get; private set; }

        public SettingsVM(MainVM main)
        {
            this.main = main;
            model = new ModelUserService(main.Session);

            ShowUsernameCommand = new DelegateCommand(ShowUsername);
            ShowPasswordCommand = new DelegateCommand(ShowPassword);
            ShowEmailCommand = new DelegateCommand(ShowEmail);
            ShowProfilCommand = new DelegateCommand(ShowProfil);
            SaveCommand = new DelegateCommand(async () => await SaveAsync());
            BackCommand = new DelegateCommand(Back);

            ShowUsername();
        }

        private void Back()
        {
            main.ShowLobby();
        }

        public Quizlet.Model.AppSession Session
        {
            get { return main.Session; }
        }


        private void ShowUsername()
        {
            currentSection = SettingsSection.Username;

            UsernameVisibility = Visibility.Visible;
            PasswordVisibility = Visibility.Collapsed;
            EmailVisibility = Visibility.Collapsed;
            ProfilVisibility = Visibility.Collapsed;

            StatusText = "";
            ClearInputs();
        }

        private void ShowPassword()
        {
            currentSection = SettingsSection.Password;

            UsernameVisibility = Visibility.Collapsed;
            PasswordVisibility = Visibility.Visible;
            EmailVisibility = Visibility.Collapsed;
            ProfilVisibility = Visibility.Collapsed;

            StatusText = "";
            ClearInputs();
        }

        private void ShowEmail()
        {
            currentSection = SettingsSection.Email;

            UsernameVisibility = Visibility.Collapsed;
            PasswordVisibility = Visibility.Collapsed;
            ProfilVisibility = Visibility.Collapsed;
            EmailVisibility = Visibility.Visible;

            StatusText = "";
            ClearInputs();
        }

        private void ShowProfil()
        {
            currentSection = SettingsSection.Profilbild;

            UsernameVisibility = Visibility.Collapsed;
            PasswordVisibility = Visibility.Collapsed;
            EmailVisibility = Visibility.Collapsed;
            ProfilVisibility = Visibility.Visible;

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

            if (currentSection == SettingsSection.Username)
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

            if (currentSection == SettingsSection.Password)
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

            if (currentSection == SettingsSection.Email)
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

                if (string.IsNullOrWhiteSpace(OldPassword))
                {
                    StatusText = "Bitte aktuelles Passwort eingeben.";
                    return;
                }

                bool ok = await model.ChangeEmailAsync(OldPassword, NewEmail);

                if (ok)
                {
                    StatusText = "E-Mail geändert. Bitte ggf. E-Mail bestätigen.";
                    ClearInputs();
                    return;
                }

                StatusText = model.LastError;
                return;
            }
        }
    }
}
