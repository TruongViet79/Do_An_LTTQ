using System;
using System.Configuration;
using System.Data;
using System.Windows;
using Do_An_LTTQ.View;
using System.Data.SqlClient;

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
        public static string CurrentAvatarURL { get; set; } = "";
        private string _connectionString = "Data Source=.;Initial Catalog=GameStoreDB;Integrated Security=True";
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 1. Kiểm tra xem lần trước có đăng nhập chưa
            bool isLoggedIn = false;
            try
            {
                isLoggedIn = Do_An_LTTQ.Properties.Settings.Default.IsLoggedIn;
            }
            catch { }

            if (isLoggedIn)
            {
                // 2. Nếu ĐÃ đăng nhập -> Lấy lại ID cũ
                App.CurrentUsername = Do_An_LTTQ.Properties.Settings.Default.SavedUsername;
                App.CurrentUserID = Do_An_LTTQ.Properties.Settings.Default.SavedUserID;

                // --- QUAN TRỌNG: GỌI HÀM LẤY THÔNG TIN MỚI NHẤT TỪ DB ---
                ReloadUserDataFromDatabase(App.CurrentUserID);
                // ----------------------------------------------------------

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

        // Hàm mới: Lấy Avatar và Email từ SQL dựa vào UserID
        public void ReloadUserDataFromDatabase(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT AvatarURL, Email FROM USERS WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Lấy Avatar
                                if (reader["AvatarURL"] != DBNull.Value)
                                {
                                    App.CurrentAvatarURL = reader["AvatarURL"].ToString();
                                }
                                else
                                {
                                    App.CurrentAvatarURL = ""; // Hoặc đường dẫn ảnh mặc định
                                }

                                // Lấy Email
                                if (reader["Email"] != DBNull.Value)
                                {
                                    App.CurrentEmail = reader["Email"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Nếu lỗi kết nối thì thôi, chấp nhận dùng mặc định
            }
        }
    }


}
