using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulky.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webEnvironment;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webEnvironment = webEnvironment;
    }

    public IActionResult Index()
    {
        return View(_unitOfWork.Product.GetAll("Category"));
    }

    public IActionResult Upsert(int? id)
    {
        var viewModel = new ProductViewModel()
        {
            Categories = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            }),
            Product = new Product()
        };

        if (id is not null)
        {
            viewModel.Product = _unitOfWork.Product.Get(u => u.Id == id, "Category");
        }

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Upsert(ProductViewModel viewModel, IFormFile file)
    {
        if (!ModelState.IsValid)
        {
            viewModel.Categories = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            });

            return View(viewModel);
        }

        var rootPath = _webEnvironment.WebRootPath;
        if (file != null)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var productPath = Path.Combine(rootPath, @"images/product");

            if (!string.IsNullOrEmpty(viewModel.Product.ImageUrl))
            {
                var oldImagePath =
                    Path.Combine(rootPath, viewModel.Product.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            using var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create);
            file.CopyTo(fileStream);

            viewModel.Product.ImageUrl = @"\images\product\" + fileName;
        }

        if (viewModel.Product.Id == 0)
        {
            _unitOfWork.Product.Add(viewModel.Product);
        }
        else
        {
            _unitOfWork.Product.Update(viewModel.Product);
        }

        _unitOfWork.Save();

        TempData["success"] = "Product created successfully";

        return RedirectToAction("Index", "Product");
    }

    //public IActionResult Delete(int? id)
    //{
    //    if (id is null or 0)
    //        return NotFound();

    //    var product = _unitOfWork.Product.Get(c => c.Id == id);

    //    if (product == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(product);
    //}


    //[HttpPost, ActionName("Delete")]
    //public IActionResult DeletePost(int? id)
    //{
    //    var product = _unitOfWork.Product.Get(c => c.Id == id);
    //    if (product == null)
    //        return NotFound();

    //    _unitOfWork.Product.Remove(product);
    //    _unitOfWork.Save();
    //    TempData["success"] = "Product deleted successfully";

    //    return RedirectToAction("Index", "Product");
    //}

    #region API CALLS

    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _unitOfWork.Product.GetAll("Category");
        return Json(new { data = productList });
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var product = _unitOfWork.Product.Get(p => p.Id == id);
        if (product == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }

        var imagePath = Path.Combine(_webEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));

        if (System.IO.File.Exists(imagePath))
        {
            System.IO.File.Delete(imagePath);
        }

        _unitOfWork.Product.Remove(product);
        _unitOfWork.Save();

        return Json(new { success = true, message = "Delete Successful"});
    }

    #endregion
}