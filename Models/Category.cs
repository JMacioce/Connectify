using System.ComponentModel.DataAnnotations;

namespace Connectify.Models
{
    public class Category
    {
        public int ID { get; set; }

        [Required]
        public string? AppUserID { get; set; }

        [Required]
        [Display(Name ="Category Name")]
        public string? Name { get; set;}

        //Virtuals
        public virtual AppUser? AppUser { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; } = new HashSet<Contact>();
    }
}
