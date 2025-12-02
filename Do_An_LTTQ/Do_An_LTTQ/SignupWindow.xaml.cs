using System.Windows;

namespace Do_An_LTTQ
{
    public partial class SignupWindow : Window
    {
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
                MessageBox.Show("Those passwords didn’t match. Try again. ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 3. Nếu mọi thứ OK -> Thông báo và quay về Login
            MessageBox.Show("Sign up successful! Returning to login screen.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            this.Close(); // Đóng cửa sổ đăng ký
        }
    }
}