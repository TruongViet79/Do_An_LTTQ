using Do_An_LTTQ.Services;
using System.Windows;

namespace Do_An_LTTQ
{
    public partial class SignupWindow : Window
    {
        DatabaseManager dbManager = new DatabaseManager();
        public SignupWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Lấy dữ liệu từ giao diện
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = pwPassword.Password;
            string confirmPassword = txtPassword.Password;

            // 2. Kiểm tra bỏ trống
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 3. Kiểm tra mật khẩu khớp nhau
            if (password != confirmPassword)
            {
                MessageBox.Show("Those passwords didn’t match. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 4. GỌI DATABASE ĐỂ LƯU THỰC TẾ
            // Sử dụng hàm RegisterUser bạn đã viết trong DatabaseManager
            bool isSuccess = dbManager.RegisterUser(email, password, username);

            if (isSuccess)
            {
                MessageBox.Show("Sign up successful! Returning to login screen.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Chuyển về màn hình đăng nhập
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            else
            {
                // Nếu thất bại, thường là do trùng Email hoặc Username trong Database
                MessageBox.Show("Sign up failed! The Email or Username might already exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}