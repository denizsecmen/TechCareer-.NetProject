namespace E_commerce_website_Techcareer.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Prodname { get; set; }
        public int stock { get; set; }
        public IFormFile  image { get; set; }
        public float price { get; set; }
        public string category { get; set; }
    }
}
