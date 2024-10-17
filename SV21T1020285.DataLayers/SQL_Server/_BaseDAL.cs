using Microsoft.Data.SqlClient;
namespace SV21T1020285.DataLayers.SQL_Server {
    public abstract class BaseDAL {
        protected string _connectionString = "";
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public BaseDAL(string connectionString)
        {
            _connectionString = connectionString;
        }
//  private static BaseDAL _instance;
//         private static readonly object _lock = new object();
//         private string _connectionString;
//         private BaseDAL(string connectionString) {
//             _connectionString = connectionString;
//         }


//         // Public method to get the singleton instance
//         public static BaseDAL GetInstance(string connectionString) {
//             // Double-checked locking for thread safety
//             if (_instance == null) {
//                 lock (_lock) {
//                     if (_instance == null) {
//                         _instance = new BaseDAL(connectionString);
//                     }   
//                 }
//             }
//             return _instance;
//         }
        /// <summary>
        /// Tạo & mở kết nối đến CSDL SQL Server
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection()
        {
            //SqlConnection connection = new SqlConnection(@"uid=sa;pwd=123;database=LiteCommerce;server=Duckyy;TrustServerCertificate=true;");
            SqlConnection connection = new SqlConnection(@"Server=localhost;Database=LTUDWDB;User Id=sa;Password=reallyStrongPwd123;Encrypt=true;TrustServerCertificate=true;");
             try
            {
                // Mở kết nối
                connection.Open();
                Console.WriteLine("Connection DB Success!!!");
                // Thực hiện các thao tác với cơ sở dữ liệu ở đây

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Connection Db Error ---> : " + ex.Message);
            }
            finally
            {
                // Đảm bảo rằng kết nối được đóng
                connection.Close();
            }
            return connection;
        }
    }
}