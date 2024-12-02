namespace inventory_managment.Models
{
    public class RecieveMaster
    {
        public int Id { get; set; }
        public string RefNum { get; set; }
        public DateTime Recieve_date { get; set; }
        public string Supplier_name { get; set; }
        public string Supplier_mobile { get; set; }
        public decimal Total_amount { get; set; }
        public bool Is_Cancelled { get; set; }
    }

    public class RecieveDetail
    {
        public int Id { get; set; }
        public int RecieveMaster_Id { get; set; }
        public int Item_Id { get; set; }
        public int Purches_rate { get; set; }
        public int Qty { get; set; }
        public decimal Amount { get; set; }
    }
}
