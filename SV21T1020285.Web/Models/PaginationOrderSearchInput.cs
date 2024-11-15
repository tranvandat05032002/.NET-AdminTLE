namespace SV21T1020285.Web.Models
{
        public class PaginationOrderSearchInput : PaginationSearchInput {

        public int Status { get; set; } = 4;
        public DateTime OrderTime {get; set;}
        public DateTime AcceptTime {get; set;}

    }
}