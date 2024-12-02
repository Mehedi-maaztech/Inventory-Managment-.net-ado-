using inventory_managment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace inventory_managment.Controllers
{
    public class CustomerController : Controller
    {
        public AppDbContext _context { get; set; }
        public CustomerController()
        {
            _context = new AppDbContext();
        }
        [HttpGet]
        public ActionResult Index()
        {
            List<Customer> items = new List<Customer>();
            using (SqlConnection conn = new SqlConnection(_context.ConnectionString))
            {
                string query = "Select * from Customer";
                SqlCommand command = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Customer item = new Customer();
                    item.Id = Convert.ToInt32(reader["Id"].ToString());
                    item.Name = reader["Name"].ToString();
                    item.Mobile = reader["Mobile"].ToString();
                    item.Email = reader["Email"].ToString();

                    items.Add(item);
                }
                conn.Close();

            }
            return View(items);
        }

        [HttpGet]
        public ActionResult AddCustomer()
        {
            Customer item = new Customer();
            return View(item);
        }

        [HttpPost]
        public ActionResult AddCustomer(Customer item)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            using (SqlConnection conn = new SqlConnection(_context.ConnectionString))
            {
                string insert = "Insert into Customer (Name,Mobile,Email) Values (@Name,@Mobile,@Email)";
                SqlCommand command = new SqlCommand(insert, conn);

                command.Parameters.AddWithValue("@Name", item.Name);
                command.Parameters.AddWithValue("@Mobile", item.Mobile);
                command.Parameters.AddWithValue("@Email", item.Email);

                conn.Open();
                int res = command.ExecuteNonQuery();
                conn.Close();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdateCustomer(int Id)
        {
            Customer item = new Customer();
            using (SqlConnection conn = new SqlConnection(_context.ConnectionString))
            {
                string insert = "Select * from Customer where Id = @Id";
                SqlCommand command = new SqlCommand(insert, conn);
                command.Parameters.AddWithValue("@Id", Id);


                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    item.Id = Convert.ToInt32(reader["Id"].ToString());
                    item.Name = reader["Name"].ToString();
                    item.Mobile = reader["Mobile"].ToString();
                    item.Email = reader["Email"].ToString();
                }
                conn.Close();
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult UpdateCustomer(Customer item)
        {
            if (item.Id == 0)
            {
                return View();
            }
            using (SqlConnection conn = new SqlConnection(_context.ConnectionString))
            {
                string insert = "update Customer Set Name = @Name, Mobile = @Mobile, Email = @Email Where Id = @Id";
                SqlCommand command = new SqlCommand(insert, conn);

                command.Parameters.AddWithValue("@Id", item.Id);
                command.Parameters.AddWithValue("@Name", item.Name);
                command.Parameters.AddWithValue("@Mobile", item.Mobile);
                command.Parameters.AddWithValue("@Email", item.Email);

                conn.Open();
                int res = command.ExecuteNonQuery();
                conn.Close();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            int rowAffected = 0;
            Customer item = new Customer();
            using (SqlConnection conn = new SqlConnection(_context.ConnectionString))
            {
                string query = "delete from Customer where Id = @Id";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Id", Id);


                conn.Open();
                rowAffected = command.ExecuteNonQuery();
                conn.Close();
            }
            return RedirectToAction("Index");
        }
    }
}
