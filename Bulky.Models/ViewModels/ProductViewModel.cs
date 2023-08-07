using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Bulky.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}

