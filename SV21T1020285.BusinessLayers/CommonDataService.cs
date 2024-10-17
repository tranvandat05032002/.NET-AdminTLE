using SV21T1020285.DataLayers;
using SV21T1020285.DataLayers.SQL_Server;
using SV21T1020285.DomainModels;

namespace SV21T1020285.BusinessLayers
{
    public static class CommonDataService
    {
        private static readonly ISimpleQueryDAL<Province> provinceDB;
        private static readonly ICommonDAL<Customer> customerDB;
        private static readonly ICommonDAL<Category> categoryDB;
        private static readonly ICommonDAL<Supplier> supplierDB;
        private static readonly ICommonDAL<Shipper> shipperDB;

        /// <summary>
        /// Ctor
        /// </summary>
        static CommonDataService()
        {
            string connectionString =
                @"uid=sa;pwd=reallyStrongPwd123;
                            database=LTUDWDB;
                            server=localhost;
                            TrustServerCertificate=true";
            customerDB = new CustomerDAL(connectionString);
            categoryDB = new CategoryDAL(connectionString);
            supplierDB = new SupplierDAL(connectionString);
            shipperDB = new ShipperDAL(connectionString);
            provinceDB = new ProvinceDAL(connectionString);
        }

        /// <summary>
        /// Tìm kiếm và lấy danh sách khách hàng dưới dạng phân trang
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page, pageSize, searchValue);
        }
        public static Customer? GetCustomer(int id) {
            return customerDB.Get(id);
        }
        /// <summary>
        /// Bổ sung một khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>/
        public static int AddCustomer(Customer data) {
            return customerDB.Add(data);
        }

        public static bool UpdateCustomer(Customer data) {
            return customerDB.Update(data);
        }

        public static bool DeleteCustomer(int id) {
            return customerDB.Delete(id);
        }
        public static bool InUsedCustomer(int id) {
            return customerDB.InUse(id);
        }
        // Category
        public static List<Category> ListOfCategories(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize, searchValue);
        }
        public static Category? GetCategory(int id) {
            return categoryDB.Get(id);
        }
        /// <summary>
        /// Bổ sung một khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>/
        public static int AddCategory(Category data) {
            return categoryDB.Add(data);
        }

        public static bool UpdateCategory(Category data) {
            return categoryDB.Update(data);
        }

        public static bool DeleteCategory(int id) {
            return categoryDB.Delete(id);
        }
        public static bool InUsedCategory(int id) {
            return categoryDB.InUse(id);
        }
        // Supplier
        public static List<Supplier> ListOfSuppliers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = supplierDB.Count(searchValue);
            return supplierDB.List(page, pageSize, searchValue);
        }
        public static List<Shipper> ListOfShippers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = shipperDB.Count(searchValue);
            return shipperDB.List(page, pageSize, searchValue);
        }
        public static List<Province> ListOfProvinces()
        {
            return provinceDB.List();
        }
    }
}