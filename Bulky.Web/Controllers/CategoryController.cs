using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bulky.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View( _unitOfWork.Category.GetAll().ToList());
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

            _unitOfWork.Category.Add(category);
            _unitOfWork.Save();

            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index", "Category");
        }

        public IActionResult Edit(int? id)
        {
            if (id is null or 0)
            {
                return NotFound();
            }

            var categoryToEdit = _unitOfWork.Category.Get(c => c.Id == id);
            
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

            _unitOfWork.Category.Update(category);
            _unitOfWork.Save();
            TempData["success"] = "Category updated successfully";

            return RedirectToAction("Index", "Category");
        }

        public IActionResult Delete(int? id)
        {
            if (id is null or 0)
                return NotFound();

            var category = _unitOfWork.Category.Get(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var category = _unitOfWork.Category.Get(c => c.Id == id);
            if (category == null) 
                return NotFound();
            
            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully"; 

            return RedirectToAction("Index", "Category");
        }
    }
}
