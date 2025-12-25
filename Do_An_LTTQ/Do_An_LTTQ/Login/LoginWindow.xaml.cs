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
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        DatabaseManager dbManager = new DatabaseManager();
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void SigninButton_Click(object sender, RoutedEventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Password; // Đảm bảo txtPassword trùng với x:Name trong XAML 

            if (dbManager.AuthenticateUser(user, pass))
            {
                //Lưu username
                App.CurrentUsername = user;

                //Lấy email từ csdl
                DataTable dt = dbManager.GetUserInfo(user);
                if (dt.Rows.Count > 0)
                {
                    App.CurrentEmail = dt.Rows[0]["Email"].ToString();
                }
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            SignupWindow signup = new SignupWindow();
            signup.Show();
            this.Close();
        }
    }
}
