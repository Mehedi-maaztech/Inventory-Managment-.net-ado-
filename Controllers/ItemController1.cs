using inventory_managment.Models;
using Microsoft.AspNetCore.Mvc;

namespace inventory_managment.Controllers
{
    public class ItemController1 : Controller
    {
        public ActionResult Index()
        {
            List<Item> item = new List<Item>();
            return View();
        }
    }
}
