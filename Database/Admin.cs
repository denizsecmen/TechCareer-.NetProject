using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_website_Techcareer
{
    [Table("Admin")]
    public class Admin
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int AdminID { get; set; }

        [Required]
        [StringLength(50)]
        public string AdminName { get; set; }
        [Required]
        public string AdminPass { get; set; }
        [Required]
        public string Imageroad { get; set; }
    }
}
