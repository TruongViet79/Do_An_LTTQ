using Do_An_LTTQ.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Do_An_LTTQ
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void SigninButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = pwPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(
                    "Please enter your username and password.",
                    "Missing Information",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            Do_An_LTTQ.View.MainWindow main = new Do_An_LTTQ.View.MainWindow();
            main.Show();
            this.Close();
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            SignupWindow signup = new SignupWindow();
            signup.Show();
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}