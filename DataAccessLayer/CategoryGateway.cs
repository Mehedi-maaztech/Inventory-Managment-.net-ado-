using inventory_managment.Models;
using Microsoft.Data.SqlClient;

namespace inventory_managment.DataAccessLayer
{
    public class CategoryGateway
    {
        private IConfiguration _configuration;

        private string? _ConnStr { get; set; }

        public CategoryGateway(IConfiguration configuration)
        {
            _configuration = configuration;
            _ConnStr = _configuration.GetConnectionString("DefaultConnection");
        }
        public List<Category> GetAllCategory()
        {
            List<Category> categorys = new List<Category>();
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                string query = "Select * from Category";
                SqlCommand command = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Category category = new Category();
                    category.Id = Convert.ToInt32(reader["Id"].ToString());
                    category.Name = reader["Name"].ToString();
                    category.IsActive = Convert.ToBoolean(reader["IsActive"].ToString());

                    categorys.Add(category);
                }
                conn.Close();

            }
            return categorys;
        }

        public Category GetCategoryById(int Id)
        {
            Category category = new Category();
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                string insert = "Select * from Category where Id = @Id";
                SqlCommand command = new SqlCommand(insert, conn);
                command.Parameters.AddWithValue("@Id", Id);

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    category.Id = Convert.ToInt32(reader["Id"].ToString());
                    category.Name = reader["Name"].ToString();
                    category.IsActive = Convert.ToBoolean(reader["IsActive"].ToString());
                }
                conn.Close();
            }
            return category;
        }

        public int AddNewCategory(Category category)
        {
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                string insert = @"Insert into Category 
                                (Name,IsActive)
                                Values (@Name,@IsActive)";
                SqlCommand command = new SqlCommand(insert, conn);

                command.Parameters.AddWithValue("@Name", category.Name);
                command.Parameters.AddWithValue("@IsActive", category.IsActive);

                conn.Open();
                int res = command.ExecuteNonQuery();
                conn.Close();
            }
            return -1;
        }
        public int UpdateCategory(Category category)
        {
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                string insert = @"Update Category Set Name=@Name,IsActive=@IsActive where Id = @Id";
                SqlCommand command = new SqlCommand(insert, conn);

                command.Parameters.AddWithValue("@Id", category.Id);
                command.Parameters.AddWithValue("@Name", category.Name);
                command.Parameters.AddWithValue("@IsActive", category.IsActive);

                conn.Open();
                int res = command.ExecuteNonQuery();
                conn.Close();
            }
            return -1;
        }
        public int RemoveCategory(int Id)
        {
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                string query = "delete from Category where Id = @Id";
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
