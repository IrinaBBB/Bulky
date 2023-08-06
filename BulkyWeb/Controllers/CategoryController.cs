using Bulky.DataAccess.Data;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.Categories.ToListAsync());
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

            _db.Categories.Add(category);
            _db.SaveChanges();
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index", "Category");
        }

        public IActionResult Edit(int? id)
        {
            if (id is null or 0)
            {
                return NotFound();
            }

            var categoryToEdit = _db.Categories.FirstOrDefault(c => c.Id == id);
            
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

            _db.Categories.Update(category);
            _db.SaveChanges();
            TempData["success"] = "Category updated successfully";

            return RedirectToAction("Index", "Category");
        }

        public IActionResult Delete(int? id)
        {
            if (id is null or 0)
                return NotFound();

            var category = _db.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var category = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) 
                return NotFound();
            
            _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully"; 

            return RedirectToAction("Index", "Category");
        }
    }
}
