using Do_An_LTTQ.Services;
using System;
using System.Windows;

namespace Do_An_LTTQ
{
    public partial class SignupWindow : Window
    {
        private readonly DatabaseManager _dbManager = new DatabaseManager();
        public SignupWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Kiểm tra xem có bỏ trống ô nào không
            if (string.IsNullOrEmpty(txtUsername.Text) ||
                string.IsNullOrEmpty(pwPassword.Password) ||
                string.IsNullOrEmpty(pwCFPassword.Password))
            {
                MessageBox.Show("Please fill in all fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Kiểm tra mật khẩu và mật khẩu xác nhận có giống nhau không
            if (pwPassword.Password != pwCFPassword.Password)
            {
                MessageBox.Show("Those passwords didn't match. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //3. Luu vào database
            try
            {
                string username = txtUsername.Text.Trim();
                string password = pwPassword.Password;
                // Nếu form của bạn chưa có ô nhập Email, ta tạm để trống hoặc trùng username
                string email = username + "@example.com";
                // Câu lệnh SQL chèn User mới (UserID sẽ tự tăng trong DB)
                // PasswordHash hiện tại đang lưu dạng text thuần để khớp với logic Login của bạn
                string sql = $@"INSERT INTO USERS (Username, PasswordHash, Email) 
                                VALUES ('{username}', '{password}', '{email}')";

                _dbManager.ExecuteQuery(sql);

                MessageBox.Show("Đăng ký tài khoản thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                // 4. Quay lại màn hình Login
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                // Nếu trùng Username, Database sẽ báo lỗi (do cột Username thường là Unique)
                MessageBox.Show("Tên tài khoản đã tồn tại hoặc có lỗi xảy ra: " + ex.Message, "Lỗi đăng ký");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}