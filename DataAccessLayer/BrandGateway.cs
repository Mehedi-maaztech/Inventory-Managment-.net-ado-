using inventory_managment.Models;
using Microsoft.Data.SqlClient;

namespace inventory_managment.DataAccessLayer
{
    public class BrandGateway
    {
        private IConfiguration _configuration;

        private string? _ConnStr { get; set; }

        public BrandGateway(IConfiguration configuration)
        {
            _configuration = configuration;
            _ConnStr = _configuration.GetConnectionString("DefaultConnection");
        }
        public List<Brand> GetAllBrand()
        {
            List<Brand> Brands = new List<Brand>();
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                string query = "Select * from Brand";
                SqlCommand command = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Brand brand = new Brand();
                    brand.Id = Convert.ToInt32(reader["Id"].ToString());
                    brand.Name = reader["Name"].ToString();
                    brand.IsActive = Convert.ToBoolean(reader["IsActive"].ToString());

                    Brands.Add(brand);
                }
                conn.Close();

            }
            return Brands;
        }

        public Brand GetBrandById(int Id)
        {
            Brand brand = new Brand();
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                string insert = "Select * from Brand where Id = @Id";
                SqlCommand command = new SqlCommand(insert, conn);
                command.Parameters.AddWithValue("@Id", Id);

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    brand.Id = Convert.ToInt32(reader["Id"].ToString());
                    brand.Name = reader["Name"].ToString();
                    brand.IsActive = Convert.ToBoolean(reader["IsActive"].ToString());
                }
                conn.Close();
            }
            return brand;
        }

        public int AddNewBrand(Brand brand)
        {
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                string insert = @"Insert into Brand 
                                (Name,IsActive)
                                Values (@Name,@IsActive)";
                SqlCommand command = new SqlCommand(insert, conn);

                command.Parameters.AddWithValue("@Name", brand.Name);
                command.Parameters.AddWithValue("@IsActive", brand.IsActive);

                conn.Open();
                int res = command.ExecuteNonQuery();
                conn.Close();
            }
            return -1;
        }
        public int UpdateBrand(Brand brand)
        {
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                string insert = @"Update Brand Set BName=@Name,IsActive=@IsActive where Id = @Id";
                SqlCommand command = new SqlCommand(insert, conn);

                command.Parameters.AddWithValue("@Id", brand.Id);
                command.Parameters.AddWithValue("@Name", brand.Name);
                command.Parameters.AddWithValue("@IsActive", brand.IsActive);

                conn.Open();
                int res = command.ExecuteNonQuery();
                conn.Close();
            }
            return -1;
        }
        public int RemoveBrand(int Id)
        {
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                string query = "delete from Brand where Id = @Id";
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
