using inventory_managment.DataAccessLayer;
using inventory_managment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace inventory_managment.Controllers
{
    public class BrandController : Controller
    {
        private IConfiguration _configuration;
        public BrandGateway BrandGateway { get; set; }

        public BrandController(IConfiguration configuration)
        {
            _configuration = configuration;
            BrandGateway = new BrandGateway(_configuration);
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<Brand> brands = new List<Brand>();
            brands = BrandGateway.GetAllBrand();
            return View(brands);
        }

        [HttpGet]
        public ActionResult AddBrand()
        {
            Brand brand = new Brand();
            return View(brand);
        }
        
        [HttpPost]
        public ActionResult AddBrand(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Model State Error";
                return View();
            }
            int rowAffected = BrandGateway.AddNewBrand(brand);
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
        public ActionResult UpdateBrand(int Id)
        {
            Brand brand = new Brand();
            brand = BrandGateway.GetBrandById(Id);
            if (brand == null)
            {
                ViewBag.Message = "Brand Not Found";
            }
            return View(brand);
        }
        
        [HttpPost]
        public ActionResult UpdateBrand(Brand brand)
        {
            if (!ModelState.IsValid || brand.Id == 0)
            {
                return View();
            }
            int rowAffected = BrandGateway.UpdateBrand(brand);
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
            Brand brand = BrandGateway.GetBrandById(Id);
            int rowAffected = 0;
            if (brand == null)
            {
                ViewBag.Message = "Brand Deleted : Id :" + Id + ", Name : " + brand.Name;
                return View("Index");
            }
            else
            {
                rowAffected = BrandGateway.RemoveBrand(Id);
            }
            return RedirectToAction("Index");
        }
    }
}