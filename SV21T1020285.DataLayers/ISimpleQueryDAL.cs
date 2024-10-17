namespace SV21T1020285.DataLayers
{
    /// <summary>
    /// Định nghĩa các phép xử lý dữ liệu "chung"
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISimpleQueryDAL<T> where T : class
    {
       List<T> List();
    }
}
