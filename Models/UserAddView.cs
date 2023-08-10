namespace E_commerce_website_Techcareer.Models
{
    public class UserAddView
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string cpassword { get; set; }
        public IFormFile avatar { get; set; }


    }
}
