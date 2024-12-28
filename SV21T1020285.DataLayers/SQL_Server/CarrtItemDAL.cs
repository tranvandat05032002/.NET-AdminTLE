using SV21T1020285.DomainModels;
using Dapper;
using Azure;
using System.Data;

namespace SV21T1020285.DataLayers.SQL_Server
{
    public class CartItemDAL : BaseDAL, ICartDAL
    {
        public CartItemDAL(string connectionString) : base(connectionString)
        {
            
        }

        // Cart
        public int Add(int CustomerID) 
        {
            int id = 0;
            using(var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Cart where CustomerID = @CustomerID)
                                select -1;
                            else
                                begin
                                    insert into Cart(CustomerID)
                                    VALUES(@CustomerID)
                                    select SCOPE_IDENTITY()
                                end";
                var parameters = new
                {
                    CustomerID
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return id;
        }
        
        public bool InUsed(int CustomerID)
        {
            bool result;
            using(var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Cart where CustomerID = @CustomerID)
                                select 1
                            else
                                select 0";

                var parameters = new
                {
                    CustomerID
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }

        // CartItem
        public bool CheckExists(int CartID, int ProductID) 
        {
             bool result;
            using(var connection = OpenConnection())
            {
                var sql = @"if exists(select * from CartItem  where CartID = @CartID and ProductID = @ProductID)
                                select 1
                            else
                                select 0";

                var parameters = new
                {
                    CartID,
                    ProductID
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public bool Update(int CartID, int Quantity)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE CartItem SET Quantity = Quantity + @Quantity, UpdatedAt = GETDATE() WHERE CartID = @CartID AND ProductID = @ProductID";
                var parameters = new
                {
                    CartID,
                    Quantity
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public int Add(CartItemSQL data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @" insert into CartItem(CartID, ProductID, Quantity, Price)
                                    values (@CartID, @ProductID, @Quantity, @Price)
                                    select scope_identity()
                            ";
                var parameters = new
                {
                    CartID = data.CartID,
                    ProductID = data.ProductID,
                    Quantity = data.Quantity,
                    Price = data.Price
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            
            return id;
        }

        public List<CartItemSQL> List(int CartID)
        {  
            List<CartItemSQL> data = new List<CartItemSQL>();
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT 
                                ci.CartItemID,
                                ci.CartID,
                                ci.ProductID,
                                p.ProductName,
                                p.Photo,
                                ci.Quantity,
                                ci.Price,
                                (ci.Quantity * ci.Price) AS TotalPrice
                            FROM 
                                CartItem ci
                            JOIN 
                                Products p ON ci.ProductID = p.ProductID
                            WHERE 
                                ci.CartID = @CartID;";
                var parameters = new
                {
                    CartID
                };
                data = connection.Query<CartItemSQL>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
            }

            return data;
        }
    }
}