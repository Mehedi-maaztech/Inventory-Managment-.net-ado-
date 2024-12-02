using inventory_managment.DataAccessLayer;
using inventory_managment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace inventory_managment.Controllers
{
    public class ItemController : Controller
    {
        private IConfiguration _configuration;
        public ItemGateway ItemGateway { get; set; }
        public CategoryGateway CategoryGateway { get; set; }
        public BrandGateway BrandGateway { get; set; }

        public ItemController(IConfiguration configuration)
        {
            _configuration = configuration;
            ItemGateway = new ItemGateway(_configuration);
            CategoryGateway = new CategoryGateway(_configuration);
            BrandGateway = new BrandGateway(_configuration);
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<Item> items = new List<Item>();
            items = ItemGateway.GetAllItem();
            return View(items);
        }


        [HttpGet]
        public ActionResult AddItem()
        {
            Item item = new Item();
            ViewBag.CategoryList = CategoryGateway.GetAllCategory();
            ViewBag.BrandList = BrandGateway.GetAllBrand();
            return View(item);
        }

        [HttpPost]
        public ActionResult AddItem(Item item)
        {
            ViewBag.CategoryList = CategoryGateway.GetAllCategory();
            ViewBag.BrandList = BrandGateway.GetAllBrand();
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Model State Error";
                //return View(new Item());
                return RedirectToAction("Index");
            }
            int rowAffected = ItemGateway.AddNewItem(item);
            if(rowAffected == 1){
                ViewBag.Message = "Saved Successfully";
                //return View(new Item());
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Failled";
                //return View(new Item());
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult UpdateItem(int Id)
        {
            
            Item item = new Item();
            item = ItemGateway.GetItemById(Id);
            if (item == null)
            {
                ViewBag.Message = "Item Not Found";
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult UpdateItem(Item item)
        {
            if (!ModelState.IsValid || item.Id == 0)
            {
                return View(item);
            }

            int rowAffected = ItemGateway.UpdateItem(item);
            if (rowAffected == 1)
            {
                ViewBag.Message = "Updated Successfull";
                return View(item);
            }
            else
            {
                ViewBag.Message = "Not Updated";
                return View(item);
            }
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            Item item = ItemGateway.GetItemById(Id);
            int rowAffected = 0;
            if (item == null)
            {
                ViewBag.Message = "Item Deleted : Id :" + Id + ", Name : " + item.Name;
                return View("Index");
            }
            else
            {
                rowAffected = ItemGateway.RemoveItem(Id);
            }
            return RedirectToAction("Index");
        }
    }
}
