using SV21T1020285.DomainModels;
using Dapper;
using Azure;
using System.Data;

namespace SV21T1020285.DataLayers.SQL_Server
{
    public class SupplierDAL : BaseDAL, ICommonDAL<Supplier>
    {
        public SupplierDAL(string connectionString) : base(connectionString)
        {


        }

        public int Add(Supplier data)
        {
            int id = 0;
            using(var connection = OpenConnection()) {
                var sql = @"insert into Suppliers(
                            SupplierName, ContactName, Province, Address, Phone, Email)
                            values(@SupplierName, @ContactName, @Province, @Address, @Phone, @Email)
                            select scope_identity();
                            ";
                var parameters = new {
                    SupplierName = data.SupplierName ?? "",
                    ContactName = data.ContactName ?? "",
                    Province = data.Province ?? "",
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                };
                id = connection.ExecuteScalar<int>(sql, parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            searchValue = $"%{searchValue.Trim()}%";
            using (var connection = OpenConnection())
            {
                var sql = @"select count(*)
                            from Suppliers
                            where (SupplierName like @searchValue)";
                var parameters = new
                {
                    searchValue
                };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
            }
            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;
            using(var connection = OpenConnection()) {
                var sql = @"delete from dbo.Suppliers where SupplierID = @SupplierID";
                var parameters = new {
                    SupplierID = id
                };
                result = connection.Execute(sql, parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Supplier? Get(int id)
        {
            Supplier? data = null;
            using(var connection = OpenConnection()) {
                var sql = @"select * from Suppliers where SupplierID = @SupplierID";
                var parameters = new {
                    SupplierID = id
                };
                data = connection.QueryFirstOrDefault<Supplier>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUse(int id)
        {
            bool result = false;
            using(var connection = OpenConnection()) {
                var sql = @"
                            if exists(select *from Suppliers where Suppliers = @CustomerID)
                                select 1
                            else 
                                select 0";
                var parameters = new {
                    CustomerID = id
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public List<Supplier> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Supplier> data = new List<Supplier>();
            searchValue = $"%{searchValue}%";
            using (var connection = OpenConnection())
            {
                var sql = @"select *
                            from (
		                            select *,
			                            row_number() over(order by SupplierName) as RowNumber
		                            from Suppliers
		                            where (SupplierName like @searchValue)
	                            ) as t
                            where (@pageSize = 0)
	                            or (RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                            order by RowNumber";
                var parameters = new
                {
                    page,
                    pageSize,
                    searchValue
                };
                data = connection.Query<Supplier>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
            }
            return data;
        }

        public bool Update(Supplier data)
        {
            bool result = false;
            using (var connection = OpenConnection()) {
                var sql = @"
                            update Suppliers
                            set SupplierName = @SupplierName,
                                ContactName = @ContactName,
                                Province = @Province,
                                Address = @Address,
                                Phone = @Phone,
                                Email = @Email
                            where SupplierID = @SupplierID
                        ";
                var parameters = new {
                    SupplierID = data.SupplierID,
                    SupplierName = data.SupplierName ?? "",
                    ContactName = data.ContactName ?? "",
                    Province = data.Province ?? "",
                    Address = data.Address,
                    Phone = data.Phone,
                    Email = data.Email,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
