using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Do_An_LTTQ
{
    public class DatabaseManager
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["GameStoreConn"].ConnectionString;

        // 1. Hàm lấy danh sách Game cho Dashboard (Dùng Procedure sp_SearchGames trong file SQL của bạn)
        public DataTable GetDashboardGames()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_SearchGames", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                // Truyền các tham số null để lấy tất cả game
                cmd.Parameters.AddWithValue("@SearchTerm", DBNull.Value);
                cmd.Parameters.AddWithValue("@CategoryID", DBNull.Value);
                cmd.Parameters.AddWithValue("@DeveloperID", DBNull.Value);
                cmd.Parameters.AddWithValue("@MinPrice", DBNull.Value);
                cmd.Parameters.AddWithValue("@MaxPrice", DBNull.Value);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // 2. Hàm đăng ký người dùng mới (Dùng Procedure sp_AddUser)
        public bool RegisterUser(string email, string password, string username)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // ĐỔI TÊN Ở ĐÂY để chắc chắn gọi đúng bản của mình
                    SqlCommand cmd = new SqlCommand("sp_RegisterNewUser", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PasswordHash", password);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                // Hiện lỗi thật từ SQL để mình biết đường sửa
                MessageBox.Show("Lỗi SQL: " + ex.Message);
                return false;
            }
        }
        public bool CheckLogin(string username, string password)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_CheckLogin", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@PasswordHash", password);

                    conn.Open();
                    int result = (int)cmd.ExecuteScalar(); // Trả về 1 (đúng) hoặc 0 (sai)
                    return result == 1;
                }
            }
            catch { return false; }
        }

        public bool AuthenticateUser(string username, string password)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Sử dụng câu lệnh SQL đơn giản để kiểm tra
                    string sql = "SELECT COUNT(*) FROM USERS WHERE Username = @user AND PasswordHash = @pass";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    // Dùng .Trim() để loại bỏ khoảng trắng thừa nếu có
                    cmd.Parameters.AddWithValue("@user", username.Trim());
                    cmd.Parameters.AddWithValue("@pass", password); // PasswordHash đang lưu dạng text thuần

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối DB: " + ex.Message);
                return false;
            }
        }

        public DataTable GetUserInfo(string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Truy vấn lấy cả Username và Email dựa trên tên đăng nhập
                string sql = "SELECT Username, Email FROM USERS WHERE Username = @user";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@user", username);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetAllGamesForStore()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Gọi Procedure sp_GetAllGamesForDisplay mà bạn đã viết trong file SQL
                SqlCommand cmd = new SqlCommand("sp_GetAllGamesForDisplay", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);
            }
            return data;
        }
    }
}
