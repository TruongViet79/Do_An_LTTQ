using Microsoft.Win32;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Do_An_LTTQ.View.UserPage
{
    public partial class SettingsPage : Page
    {
        private string _selectedAvatarPath = null;
        // LƯU Ý: Thay đổi chuỗi kết nối này cho đúng với máy của bạn
        private string _connectionString = "Data Source=LAPTOP-H457D0PI\\MSSQLSERVER01;Initial Catalog=GameStoreDB;Integrated Security=True";

        public SettingsPage()
        {
            InitializeComponent();
            LoadCurrentSetting();
        }

        private void LoadCurrentSetting()
        {
            txtUsername.Text = App.CurrentUsername;
            txtEmail.Text = App.CurrentEmail;

            // Load Avatar
            if (!string.IsNullOrEmpty(App.CurrentAvatarURL) && File.Exists(App.CurrentAvatarURL))
            {
                imgAvatarBrush.ImageSource = new BitmapImage(new Uri(App.CurrentAvatarURL));
            }

            // Load Font Size
            double currentSize = 14;
            if (Application.Current.Resources.Contains("FontSizeNormal"))
            {
                currentSize = (double)Application.Current.Resources["FontSizeNormal"];
            }
            if (rbSmall != null && rbMedium != null && rbLarge != null)
            {
                switch (currentSize)
                {
                    case 12: rbSmall.IsChecked = true; break;
                    case 14: rbMedium.IsChecked = true; break;
                    case 16: rbLarge.IsChecked = true; break;
                }
            }

            // Load Auto Login state
            if (chkAutoLogin != null)
            {
                chkAutoLogin.IsChecked = Do_An_LTTQ.Properties.Settings.Default.IsLoggedIn;
            }
        }

        private void ChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

            if (openFileDialog.ShowDialog() == true)
            {
                string sourceFile = openFileDialog.FileName;
                imgAvatarBrush.ImageSource = new BitmapImage(new Uri(sourceFile));
                _selectedAvatarPath = sourceFile;
            }
        }

        // HÀM LƯU CHÍNH (Chỉ giữ lại 1 hàm này thôi)
        private void SaveProfile(object sender, RoutedEventArgs e)
        {
            string newEmail = txtEmail.Text.Trim();
            string finalAvatarPath = null;

            try
            {
                // 1. Copy ảnh vào thư mục dự án (nếu có chọn ảnh mới)
                if (_selectedAvatarPath != null)
                {
                    string destFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserAvatars");
                    if (!Directory.Exists(destFolder)) Directory.CreateDirectory(destFolder);

                    string fileName = $"Avatar_{App.CurrentUserID}_{DateTime.Now.Ticks}{Path.GetExtension(_selectedAvatarPath)}";
                    string destPath = Path.Combine(destFolder, fileName);

                    File.Copy(_selectedAvatarPath, destPath, true);
                    finalAvatarPath = destPath;
                }

                // 2. Lưu vào Database
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    // Dùng câu lệnh Update trực tiếp cho chắc chắn (vì SP của bạn có thể thiếu tham số Email)
                    string query = "UPDATE USERS SET Email = @Email, AvatarURL = ISNULL(@AvatarURL, AvatarURL) WHERE UserID = @UserID";

                    using (SqlCommand cmdUpdate = new SqlCommand(query, conn))
                    {
                        cmdUpdate.Parameters.AddWithValue("@Email", newEmail);
                        cmdUpdate.Parameters.AddWithValue("@UserID", App.CurrentUserID);

                        if (finalAvatarPath != null)
                            cmdUpdate.Parameters.AddWithValue("@AvatarURL", finalAvatarPath);
                        else
                            cmdUpdate.Parameters.AddWithValue("@AvatarURL", DBNull.Value);

                        cmdUpdate.ExecuteNonQuery();
                    }
                }

                // 3. Cập nhật biến toàn cục để Dashboard hiển thị ngay
                App.CurrentEmail = newEmail;
                if (!string.IsNullOrEmpty(finalAvatarPath)) App.CurrentAvatarURL = finalAvatarPath;

                MessageBox.Show("Lưu thay đổi thành công!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeTheme(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                string theme = btn.Tag.ToString();
                if (txtCurrentTheme != null) txtCurrentTheme.Text = $"Current: {theme}";

                string uriPath = $"Theme/{theme}.xaml";
                try
                {
                    var newTheme = new ResourceDictionary { Source = new Uri(uriPath, UriKind.Relative) };
                    var appResources = Application.Current.Resources.MergedDictionaries;
                    appResources.Clear();
                    appResources.Add(new ResourceDictionary { Source = new Uri("/Resources/Dictionary.xaml", UriKind.Relative) });
                    appResources.Add(newTheme);
                    Application.Current.Resources["CurrentTheme"] = theme;
                }
                catch (Exception) { /* Bỏ qua lỗi theme nếu file chưa tồn tại */ }
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb && rb.Tag != null)
            {
                if (double.TryParse(rb.Tag.ToString(), out double baseSize))
                {
                    if (txtPreview != null) txtPreview.FontSize = baseSize;
                    Application.Current.Resources["FontSizeNormal"] = baseSize;
                    Application.Current.Resources["FontSizeLarge"] = baseSize * 1.5;
                    Application.Current.Resources["FontSizeHuge"] = baseSize * 2.0;
                }
            }
        }

        private void Language_Changed(object sender, RoutedEventArgs e)
        {
            if (txtCurrentLanguage != null && sender is RadioButton rb)
            {
                txtCurrentLanguage.Text = $"Current: {rb.Content}";
            }
        }

        private void ResetProfile(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Reset về mặc định?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                txtEmail.Text = ""; // Hoặc giá trị mặc định nào đó
                // Reset logic...
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUsername = null;
            App.CurrentEmail = null;
            App.CurrentAvatarURL = null;

            Properties.Settings.Default.IsLoggedIn = false;
            Properties.Settings.Default.Save();

            // Mở lại Login (đảm bảo bạn đã có LoginWindow)
            // Do_An_LTTQ.Login.LoginWindow login = new Do_An_LTTQ.Login.LoginWindow(); 
            // login.Show();

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            // 4. Đóng cửa sổ hiện tại (MainWindow) SAU
            Window currentWindow = Window.GetWindow(this);
            if (currentWindow != null)
            {
                currentWindow.Close();
            }

        }

        private void chkAutoLogin_Click(object sender, RoutedEventArgs e)
        {
            Do_An_LTTQ.Properties.Settings.Default.IsLoggedIn = chkAutoLogin.IsChecked == true;
            Do_An_LTTQ.Properties.Settings.Default.SavedUsername = chkAutoLogin.IsChecked == true ? App.CurrentUsername : "";
            Do_An_LTTQ.Properties.Settings.Default.Save();
        }
    }
}