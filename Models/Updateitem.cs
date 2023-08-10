namespace E_commerce_website_Techcareer.Models
{
    public class Updateitem
    {
        public string updatedEl { get; set; }
        public string Prodname { get; set; }
        public int stock { get; set; }
        public IFormFile image { get; set; }
        public float price { get; set; }
        public string category { get; set; }
    }
}
