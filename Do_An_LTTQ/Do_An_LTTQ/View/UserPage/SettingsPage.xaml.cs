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
            txtUsername.Text = App.CurrentUsername;
            txtEmail.Text = App.CurrentEmail;

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

            //Language
            if (lgVN.IsChecked == true)
                txtCurrentLanguage.Text = $"Current: {lgVN.Content}";
            else if (lgEL.IsChecked == true)
                txtCurrentLanguage.Text = $"Current: {lgEL.Content}";
            ;

            //Theme


        }
        private void ChangeTheme(object sender, RoutedEventArgs e)
{
    if (sender is Button btn && btn.Tag != null)
    {
        // 1. Lấy tên theme từ Tag của nút (VD: "Blue", "Light")
        string theme = btn.Tag.ToString(); 

        // 2. Cập nhật dòng chữ hiển thị "Current: ..."
        if (txtCurrentTheme != null)
        {
            txtCurrentTheme.Text = $"Current: {theme}";
        }

        // 3. XỬ LÝ ĐỔI FILE MÀU (Quan trọng)
        string uriPath = $"Theme/{theme}.xaml"; // Lưu ý: Kiểm tra folder là "Theme" hay "Themes" trong dự án của bạn
        try 
        {
            var newTheme = new ResourceDictionary 
            { 
                Source = new Uri(uriPath, UriKind.Relative) 
            };

            // Xóa theme cũ và thêm theme mới
            var appResources = Application.Current.Resources.MergedDictionaries;
            appResources.Clear(); // Xóa sạch các dictionary cũ
            
            // Nạp lại file từ điển chính (nếu có dùng icon hay style chung)
            appResources.Add(new ResourceDictionary { Source = new Uri("/Resources/Dictionary.xaml", UriKind.Relative) });
            
            // Nạp theme mới vào
            appResources.Add(newTheme);

            // 4. Lưu lại tên theme để lần sau mở app nó nhớ
            Application.Current.Resources["CurrentTheme"] = theme;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi không tìm thấy file màu: " + uriPath);
        }
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
            if (txtCurrentLanguage == null) return;
            
            if (sender is RadioButton rb)
            {
                txtCurrentLanguage.Text = $"Current: {rb.Content}";
            }
        }

        private void SaveProfile(object sender, RoutedEventArgs e)
        {
            string newUsername = txtUsername.Text.Trim();
            string newEmail = txtEmail.Text.Trim();
            string newPhone = txtPhone.Text.Trim();
            string username = txtUsername.Text;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;

            App.CurrentUsername = newUsername;
            App.CurrentEmail = newEmail;

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
