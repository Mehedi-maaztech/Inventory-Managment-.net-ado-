using inventory_managment.DataAccessLayer;
using inventory_managment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace inventory_managment.Controllers
{
    public class CategoryController : Controller
    {
        private IConfiguration _configuration;
        public CategoryGateway CategoryGateway { get; set; }

        public CategoryController(IConfiguration configuration)
        {
            _configuration = configuration;
            CategoryGateway = new CategoryGateway(_configuration);
        }
        [HttpGet]
        public ActionResult Index()
        {
            List<Category> categorys = new List<Category>();
            categorys = CategoryGateway.GetAllCategory();
            return View(categorys);
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            Category category = new Category();
            return View(category);
        }

        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Model State Error";
                return View();
            }
            int rowAffected = CategoryGateway.AddNewCategory(category);
            if (rowAffected == 1)
            {
                ViewBag.Message = "Saved Successfully";
                return View();
            }
            else
            {
                ViewBag.Message = "Failled";
                return View();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdateCategory(int Id)
        {
            Category category = new Category();
            category = CategoryGateway.GetCategoryById(Id);
            if (category == null)
            {
                ViewBag.Message = "Category Not Found";
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult UpdateCategory(Category category)
        {
            if (!ModelState.IsValid || category.Id == 0)
            {
                return View();
            }
            int rowAffected = CategoryGateway.UpdateCategory(category);
            if (rowAffected == 1)
            {
                ViewBag.Message = "Saved Successfull";
                return View();
            }
            else
            {
                ViewBag.Message = "Saved Unsuccessfull";
                return View();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            Category category = CategoryGateway.GetCategoryById(Id);
            int rowAffected = 0;
            if (category == null)
            {
                ViewBag.Message = "Category Deleted : Id :" + Id + ", Name : " + category.Name;
                return View("Index");
            }
            else
            {
                rowAffected = CategoryGateway.RemoveCategory(Id);
            }
            return RedirectToAction("Index");
        }
    }
}
