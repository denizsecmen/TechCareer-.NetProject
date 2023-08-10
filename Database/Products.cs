using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_website_Techcareer
{
    [Table("Products")]
    public class Products
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int ItemNo { get; set; }
        [MaxLength(100)]
        [Required]
        [Obsolete]
        public string ItemName { get; set; }
        [Required]
        public int Stok { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string Imageroad { get; set; }
        [Required]
        public string category { get; set; }
    }
}
