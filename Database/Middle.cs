using System.ComponentModel.DataAnnotations;

namespace E_commerce_website_Techcareer.Database
{
    public class Middle
    {
        [Key]
        public string transId { get;set; }
        public string UserId { get; set; }
        public string prodID { get; set; }

    }
}
