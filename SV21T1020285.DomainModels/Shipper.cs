namespace SV21T1020285.DomainModels
{
    /// <summary>
    /// Shipper
    /// </summary>
    public class Shipper
    {
        public int ShipperID { get; set; }
        public string ShipperName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}