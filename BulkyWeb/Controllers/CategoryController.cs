using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bulky.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            return View( _categoryRepository.GetAll().ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString()) 
            {
                ModelState.AddModelError("name", "The Display Order cannot be the same as the Name");
            }

            if (!ModelState.IsValid) return View(category);

            _categoryRepository.Add(category);
            _categoryRepository.Save();

            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index", "Category");
        }

        public IActionResult Edit(int? id)
        {
            if (id is null or 0)
            {
                return NotFound();
            }

            var categoryToEdit = _categoryRepository.Get(c => c.Id == id);
            
            if (categoryToEdit == null)
            {
                return NotFound();
            }

            return View(categoryToEdit);
        }


        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid) return View(category);

            _categoryRepository.Update(category);
            _categoryRepository.Save();
            TempData["success"] = "Category updated successfully";

            return RedirectToAction("Index", "Category");
        }

        public IActionResult Delete(int? id)
        {
            if (id is null or 0)
                return NotFound();

            var category = _categoryRepository.Get(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var category = _categoryRepository.Get(c => c.Id == id);
            if (category == null) 
                return NotFound();
            
            _categoryRepository.Remove(category);
            _categoryRepository.Save();
            TempData["success"] = "Category deleted successfully"; 

            return RedirectToAction("Index", "Category");
        }
    }
}
