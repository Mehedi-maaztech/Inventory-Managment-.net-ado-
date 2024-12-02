using inventory_managment.Models;
using Microsoft.Data.SqlClient;

namespace inventory_managment.DataAccessLayer
{
    public class ItemGateway
    {
        private IConfiguration _configuration;

        private string? _ConnStr { get; set; }

        public ItemGateway(IConfiguration configuration)
        {
            _configuration = configuration;
            _ConnStr = _configuration.GetConnectionString("DefaultConnection");
        }
        public List<Item> GetAllItem() 
        {
            List<Item> items = new List<Item>();
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                string query = "Select * from Item";
                SqlCommand command = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Item item = new Item();
                    item.Id = Convert.ToInt32(reader["Id"].ToString());
                    item.ItemCode = reader["ItemCode"].ToString();
                    item.Name = reader["Name"].ToString();
                    item.Description = reader["Description"].ToString();
                    item.Category_Id = Convert.ToInt32(reader["Category_Id"].ToString());
                    item.Brand_Id = Convert.ToInt32(reader["Brand_Id"].ToString());
                    item.IsActive = Convert.ToBoolean(reader["IsActive"].ToString());

                    items.Add(item);
                }
                conn.Close();

            }
            return items;
        }

        public Item GetItemById(int Id)
        {
            Item item = new Item();
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                string insert = "Select * from Item where Id = @Id";
                SqlCommand command = new SqlCommand(insert, conn);
                command.Parameters.AddWithValue("@Id", Id);

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    item.Id = Convert.ToInt32(reader["Id"].ToString());
                    item.ItemCode = reader["ItemCode"].ToString();
                    item.Name = reader["Name"].ToString();
                    item.Description = reader["Description"].ToString();
                    item.Category_Id = Convert.ToInt32(reader["Category_Id"].ToString());
                    item.Brand_Id = Convert.ToInt32(reader["Brand_Id"].ToString());
                    item.IsActive = Convert.ToBoolean(reader["IsActive"].ToString());
                }
                conn.Close();
            }
            return item;
        }

        public int AddNewItem(Item item)
        {
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                string insert = @"Insert into Item 
                                (ItemCode,Name,Description,Category_Id,Brand_Id,IsActive)
                                Values (@ItemCode,@Name,@Description,@Category_Id,@Brand_Id,@IsActive)";
                SqlCommand command = new SqlCommand(insert, conn);

                command.Parameters.AddWithValue("@ItemCode", item.ItemCode);
                command.Parameters.AddWithValue("@Name", item.Name);
                command.Parameters.AddWithValue("@Description", item.Description);
                command.Parameters.AddWithValue("@Category_Id", item.Category_Id);
                command.Parameters.AddWithValue("@Brand_Id", item.Brand_Id);
                command.Parameters.AddWithValue("@IsActive", item.IsActive);

                conn.Open();
                int res = command.ExecuteNonQuery();
                conn.Close();
            }
            return -1;
        }
        public int UpdateItem(Item item)
        {
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                string insert = @"Update Item Set ItemCode=@ItemCode,Name=@Name,Description=@Description,
                                Category_Id=@Category_Id,Brand_Id=@Brand_Id,IsActive=@IsActive where Id = @Id";
                SqlCommand command = new SqlCommand(insert, conn);

                command.Parameters.AddWithValue("@Id", item.Id);
                command.Parameters.AddWithValue("@ItemCode", item.ItemCode);
                command.Parameters.AddWithValue("@Name", item.Name);
                command.Parameters.AddWithValue("@Description", item.Description);
                command.Parameters.AddWithValue("@Category_Id", item.Category_Id);
                command.Parameters.AddWithValue("@Brand_Id", item.Brand_Id);
                command.Parameters.AddWithValue("@IsActive", item.IsActive);

                conn.Open();
                int res = command.ExecuteNonQuery();
                conn.Close();
            }
            return -1;
        }
        public int RemoveItem(int Id)
        {
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                string query = "delete from Item where Id = @Id";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Id", Id);

                conn.Open();
                int rowAffected = command.ExecuteNonQuery();
                conn.Close();
                return rowAffected;
            }
        }
    }
}
