using System.ComponentModel;

namespace inventory_managment.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayName("Category")]
        public int Category_Id { get; set; }

        [DisplayName("Brand")]
        public int Brand_Id { get; set; }
        public bool IsActive { get; set; }
    }
}
