using Microsoft.AspNetCore.Mvc;

namespace inventory_managment.Models.VM
{
    public class RecieveVM
    {
        public RecieveMaster RecieveMaster { get; set; }
        public List<RecieveDetail> RecieveDetail { get; set; }

        public RecieveVM()
        {
            RecieveMaster = new RecieveMaster();
            RecieveDetail = new List<RecieveDetail>();
        }
    }



}
