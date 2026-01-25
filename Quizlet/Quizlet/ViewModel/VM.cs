using Prism.Commands;
using Prism.Mvvm;
using Quizlet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    internal class VM:BindableBase
    {
        ModelUser mu = new ModelUser();
        private string username;
        private string password;
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
        public ICommand bLogin { get; }
        public ICommand bRegister {  get; }
        public VM()
        {
            bLogin = new DelegateCommand(Login);
            bRegister = new DelegateCommand(Register);
        }
        public void Login()
        {
            try
            {
                int userid = mu.CheckUser(Username, Password);
                if (userid != -1) MessageBox.Show("Hallo " + Username + ", ID: " + userid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Register()
        {

        }
        //um passwort aus der passwort box rauszubekommen weil normales binding blockiert ist aus sicherheitsgründen
        public static class PasswordHelper
        {
            public static readonly DependencyProperty PasswordProperty =
                DependencyProperty.RegisterAttached(
                    "Password",
                    typeof(string),
                    typeof(PasswordHelper),
                    new FrameworkPropertyMetadata(
                        "",
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                        OnPasswordChanged));

            public static string GetPassword(DependencyObject obj)
            {
                return (string)obj.GetValue(PasswordProperty);
            }

            public static void SetPassword(DependencyObject obj, string value)
            {
                obj.SetValue(PasswordProperty, value);
            }

            private static void OnPasswordChanged(
                DependencyObject d,
                DependencyPropertyChangedEventArgs e)
            {
                PasswordBox box = d as PasswordBox;
                if (box == null)
                    return;

                if (box.Password != (string)e.NewValue)
                    box.Password = (string)e.NewValue;
            }
        }

    }
}
