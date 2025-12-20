using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Do_An_LTTQ.View.UserPage
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            LoadCurrentSetting();
        }

        private void LoadCurrentSetting()
        {
            // 1. Lấy cỡ chữ đang dùng trong App ra (nếu chưa có thì mặc định là 14)
            double currentSize = 14;
            if (Application.Current.Resources.Contains("FontSizeNormal"))
            {
                currentSize = (double)Application.Current.Resources["FontSizeNormal"];
            }

            // 2. Tick lại vào đúng cái nút tương ứng
            // Lưu ý: Phải ngắt sự kiện Checked tạm thời nếu không muốn nó chạy logic update dư thừa (tuỳ chọn)
            switch (currentSize)
            {
                case 12: rbSmall.IsChecked = true; break;
                case 14: rbMedium.IsChecked = true; break;
                case 16: rbLarge.IsChecked = true; break;
            }
        }
        private void ChangeTheme(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                string theme = btn.Tag.ToString(); // Lấy "Light", "Dark", "Purple", "Blue"
                txtCurrentTheme.Text = $"Current: {theme}";
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb  && rb.Tag != null)
            {
                double baseSize = double .Parse(rb.Tag.ToString());
                if (txtPreview != null)
                {
                    txtPreview.FontSize = baseSize;
                }

                Application.Current.Resources["FontSizeNormal"] =baseSize;

                Application.Current.Resources["FontSizeLarge"] = baseSize * 1.5;

                Application.Current.Resources["FontSizeHuge"] = baseSize * 2.0;
            }
        }

        private void Language_Changed(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb && rb.Tag != null)
            {
                string language = rb.Content.ToString();
                txtCurrentLanguage.Text = $"Current: {language}";
            }
        }

        private void SaveProfile(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;
            MessageBox.Show($"Profile saved!\n\nUsername: {username}\nEmail: {email}\nPhone: {phone}",
                "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ResetProfile(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Reset to default values?", "Confirm",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                txtUsername.Text = "May1804";
                txtEmail.Text = "may1804@example.com";
                txtPhone.Text = "0123456789";
                MessageBox.Show("Profile reset!", "Success");
            }
        }
    }
}
