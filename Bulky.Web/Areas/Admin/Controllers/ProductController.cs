using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bulky.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        return View(_unitOfWork.Product.GetAll());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        if (!ModelState.IsValid) return View(product);

        _unitOfWork.Product.Add(product);
        _unitOfWork.Save();

        TempData["success"] = "Product created successfully";

        return RedirectToAction("Index", "Product");
    }

    public IActionResult Edit(int? id)
    {
        if (id is null or 0)
        {
            return NotFound();
        }

        var productToEdit = _unitOfWork.Product.Get(c => c.Id == id);

        if (productToEdit == null)
        {
            return NotFound();
        }

        return View(productToEdit);
    }


    [HttpPost]
    public IActionResult Edit(Product product)
    {
        if (!ModelState.IsValid) return View(product);

        _unitOfWork.Product.Update(product);
        _unitOfWork.Save();
        TempData["success"] = "Product updated successfully";

        return RedirectToAction("Index", "Product");
    }

    public IActionResult Delete(int? id)
    {
        if (id is null or 0)
            return NotFound();

        var product = _unitOfWork.Product.Get(c => c.Id == id);

        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }


    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        var product = _unitOfWork.Product.Get(c => c.Id == id);
        if (product == null)
            return NotFound();

        _unitOfWork.Product.Remove(product);
        _unitOfWork.Save();
        TempData["success"] = "Product deleted successfully";

        return RedirectToAction("Index", "Product");
    }
}