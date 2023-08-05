using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name field cannot be empty")]
        [MaxLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        [Range(1, 100, ErrorMessage = "Display order must be between 1 and 100")]
        [DefaultValue(100)]
        [DisplayName("Display Order")]
        public int? DisplayOrder { get; set; }
    }
}

