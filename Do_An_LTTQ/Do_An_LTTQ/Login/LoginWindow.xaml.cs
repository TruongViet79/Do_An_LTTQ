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

            // 1. Kiểm tra database (Sử dụng DatabaseManager của bạn)
            DatabaseManager dbManager = new DatabaseManager();
            // Giả sử bạn có Store Procedure sp_Login hoặc dùng câu lệnh SQL trực tiếp
            string sql = $"SELECT UserID, Username FROM USERS WHERE Username = '{username}' AND PasswordHash = '{password}'";
            // Lưu ý: PasswordHash là tên cột mật khẩu trong DB của bạn

            DataTable dt = dbManager.ExecuteQuery(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                // 2. ĐĂNG NHẬP THÀNH CÔNG: Gán ID động tại đây
                App.CurrentUserID = Convert.ToInt32(dt.Rows[0]["UserID"]);
                App.CurrentUsername = dt.Rows[0]["Username"].ToString();

                // (Tùy chọn) Lưu trạng thái đăng nhập để lần sau không phải nhập lại
                Do_An_LTTQ.Properties.Settings.Default.SavedUserID = App.CurrentUserID;
                Do_An_LTTQ.Properties.Settings.Default.SavedUsername = App.CurrentUsername;
                Do_An_LTTQ.Properties.Settings.Default.IsLoggedIn = true;
                Do_An_LTTQ.Properties.Settings.Default.Save();

                // 3. Mở MainWindow
                Do_An_LTTQ.View.MainWindow main = new Do_An_LTTQ.View.MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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