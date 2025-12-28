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
                    "Vui lòng nhập tài khoản và mật khẩu.",
                    "Thiếu thông tin",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                // 1. Kiểm tra database
                DatabaseManager dbManager = new DatabaseManager();
                // Lưu ý: Nên dùng Parameter để tránh SQL Injection, nhưng ở đây mình giữ theo cách của bạn cho đồng bộ
                string sql = $"SELECT UserID, Username FROM USERS WHERE Username = '{username}' AND PasswordHash = '{password}'";

                DataTable dt = dbManager.ExecuteQuery(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    // 2. ĐĂNG NHẬP THÀNH CÔNG
                    App.CurrentUserID = Convert.ToInt32(dt.Rows[0]["UserID"]);
                    App.CurrentUsername = dt.Rows[0]["Username"].ToString();

                    // --- [QUAN TRỌNG] THÊM ĐOẠN NÀY ĐỂ LẤY AVATAR & EMAIL ---
                    // Gọi hàm reload từ App.xaml.cs để lấy Avatar từ DB lên ngay lập tức
                    if (Application.Current is App myApp)
                    {
                        myApp.ReloadUserDataFromDatabase(App.CurrentUserID);
                    }
                    // ---------------------------------------------------------

                    // 3. Lưu trạng thái đăng nhập (Auto Login)
                    Do_An_LTTQ.Properties.Settings.Default.SavedUserID = App.CurrentUserID;
                    Do_An_LTTQ.Properties.Settings.Default.SavedUsername = App.CurrentUsername;
                    Do_An_LTTQ.Properties.Settings.Default.IsLoggedIn = true;
                    Do_An_LTTQ.Properties.Settings.Default.Save();

                    // 4. Mở MainWindow
                    Do_An_LTTQ.View.MainWindow main = new Do_An_LTTQ.View.MainWindow();
                    main.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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