using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_website_Techcareer
{
    [Table("Users")]
    public class Users
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { set; get; }
        [Required]
        [MaxLength(60)]
        [Obsolete]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Pass { get; set; }

        [Required]
        public string Imageroad { get; set; }
    }
}
