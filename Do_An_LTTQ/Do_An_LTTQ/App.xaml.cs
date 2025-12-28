using System;
using System.Configuration;
using System.Data;
using System.Windows;
using Do_An_LTTQ.View;

namespace Do_An_LTTQ
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string CurrentUsername { get; set; } = "";
        public static string CurrentEmail { get; set; } = "";
        public static int CurrentUserID { get; set; } = 0;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 1. Kiểm tra xem lần trước có đăng nhập chưa
            bool isLoggedIn = Do_An_LTTQ.Properties.Settings.Default.IsLoggedIn;

            if (isLoggedIn)
            {
                // 2. Nếu ĐÃ đăng nhập -> Lấy lại tên cũ -> Mở luôn MainWindow
                App.CurrentUsername = Do_An_LTTQ.Properties.Settings.Default.SavedUsername;
                App.CurrentUserID = Do_An_LTTQ.Properties.Settings.Default.SavedUserID;
                // (Đảm bảo MainWindow của đại ca nằm đúng namespace, thường là Do_An_LTTQ.View.MainWindow hoặc Do_An_LTTQ.MainWindow)
                // Nếu báo lỗi đỏ chữ MainWindow thì thử gõ: new Do_An_LTTQ.View.MainWindow();
                MainWindow main = new MainWindow();
                main.Show();
            }
            else
            {
                // 3. Nếu CHƯA đăng nhập -> Mở màn hình Login
                LoginWindow login = new LoginWindow();
                login.Show();
            }
        }
    }


}
