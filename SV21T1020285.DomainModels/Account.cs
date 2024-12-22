namespace SV21T1020285.DomainModels
{
    /// <summary>
    /// Người dùng
    /// </summary>
    public class Account
    {
        public int AccountID { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
    }
}