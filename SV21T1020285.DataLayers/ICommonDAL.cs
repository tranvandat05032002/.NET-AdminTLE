namespace SV21T1020285.DataLayers
{
    /// <summary>
    /// Định nghĩa các phép xử lý dữ liệu "chung"
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommonDAL<T> where T : class
    {
        /// <summary>
        /// Tìm kiếm & lấy danh sách dữ liệu dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">Số dòng hiển thị trên mỗi trang (bằng 0: không phân trang)</param>
        /// <param name="searchValue">Giá trị cần tìm (chuỗi rỗng: lấy toàn bộ dữ liệu)</param>
        /// <returns></returns>
        List<T> List(int page = 1, int pageSize = 0, string searchValue = "");
        // List<T> List(string searchValue = "");
        /// <summary>
        /// Đếm số lượng dòng dữ liệu tìm được
        /// </summary>
        /// <param name="searchValue">Giá trị cần tìm (chuỗi rỗng: lấy toàn bộ dữ liệu)</param>
        /// <returns></returns>
        int Count(string searchValue = "");
        /// <summary>
        /// Lấy một dòng dữ liệu dựa vào khóa chính/id (trả về null nếu dữ liệu không tồn tại, T?: T có thể bằng null)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T? Get(int id);
        /// <summary>
        /// Bổ sung một bản ghi vào CSDL, trả về ID của dữ liệu vừa được bổ sung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(T data);
        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(T data);
        /// <summary>
        /// Xóa một dòng dữ liệu dựa vào giá trị của khóa chính/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);
        /// <summary>
        /// Kiểm tra xem một dòng duex liệu có khóa id hiện có dữ liệu tham chiếu hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool InUse(int id);
    }
}
