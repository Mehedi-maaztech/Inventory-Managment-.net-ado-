using inventory_managment.DataAccessLayer;
using inventory_managment.Models;
using inventory_managment.Models.VM;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace inventory_managment.Controllers
{
    public class RecieveController : Controller
    {
        private IConfiguration _configuration;
        public ItemGateway ItemGateway { get; set; }
        public CategoryGateway CategoryGateway { get; set; }
        public BrandGateway BrandGateway { get; set; }
        public RecieveVM ReciveVM { get; set; }
        public RecieveGateway RecieveGateway { get; set; }

        public RecieveController(IConfiguration configuration)
        {
            _configuration = configuration;
            ItemGateway = new ItemGateway(_configuration);
            CategoryGateway = new CategoryGateway(_configuration);
            BrandGateway = new BrandGateway(_configuration);
            RecieveGateway = new RecieveGateway(_configuration);
        }
        public IActionResult Index()
        {
            List<RecieveMaster> masters = RecieveGateway.GetReceiveMasters();
            return View(masters);
        }
        [HttpGet]
        public ActionResult Add_Recieve()
        {
            ViewBag.Items = ItemGateway.GetAllItem();
            RecieveVM recieveVM = new RecieveVM();
            return View(recieveVM);
        }

        [HttpPost]
        public ActionResult Add_Recieve(RecieveVM recieveVM)
        {

            ViewBag.Items = ItemGateway.GetAllItem();
            if (!ModelState.IsValid)
            {
                return View(recieveVM);
            }

            int result = RecieveGateway.AddNewrcv(recieveVM);

            if (result == 1)
            {

                ViewBag.Message = "Saved";

            }
            return View(new RecieveVM());
        }


        public ActionResult Update_Recieve(int Id)
        {
            ViewBag.Items = ItemGateway.GetAllItem();
            RecieveVM recieveVM = new RecieveVM();
            recieveVM = RecieveGateway.GetReceiveMasterById(Id);

            return View(recieveVM);
        }

        [HttpPost]
        public ActionResult Update_Recieve(RecieveVM recieveVM)
        {

            ViewBag.Items = ItemGateway.GetAllItem();
            if (!ModelState.IsValid)
            {
                return View(recieveVM);
            }

            int result = RecieveGateway.UpdateRecieve(recieveVM);

            if (result == 1)
            {

                ViewBag.Message = "Saved";

            }
            return View(new RecieveVM());
        }
         public ActionResult Delete(int Id)
        {
            RecieveVM rcv = RecieveGateway.GetReceiveMasterById(Id);
            int rowAffected = 0;
            if (rcv == null)
            {
                ViewBag.Message = "Item Deleted : Id :" + Id + ", Name : " + rcv.RecieveMaster.RefNum;
                return View("Index");
            }
            else
            {
                rowAffected = RecieveGateway.RemoveItem(Id);
            }
            return RedirectToAction("Index");
        }
    }
}
