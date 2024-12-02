namespace inventory_managment.Models
{
    public class AppDbContext
    {
        public string ConnectionString { get; set; }

        public AppDbContext()
        {
            ConnectionString = "Server=DESKTOP-MEI3TFD\\SQLEXPRESS;Database=StockManagment;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=true";
        }
    }
}
