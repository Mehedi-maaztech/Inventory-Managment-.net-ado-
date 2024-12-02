using inventory_managment.Models;
using inventory_managment.Models.VM;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace inventory_managment.DataAccessLayer
{
    public class RecieveGateway
    {
        private IConfiguration _configuration;
        private string? _ConnStr { get; set; }
        public RecieveGateway(IConfiguration configuration)
        {
            _configuration = configuration;
            _ConnStr = _configuration.GetConnectionString("DefaultConnection");
        }

        

        public RecieveVM GetReceiveMasterById(int id)
        {
            RecieveVM recieveVM = new RecieveVM();
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                int master_id = 0;
                #region master

                string query = @"Select
                                       Id,
                                       RefNum,
                                       Recieve_date,
                                       Supplier_name,
                                       Supplier_mobile,
                                       Total_amount,
                                       Is_Cancelled

                                from ReciveMaster where Id=@Id";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Id_", id);

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    recieveVM.RecieveMaster.Id = Convert.ToInt32(reader["Id"].ToString());
                    recieveVM.RecieveMaster.RefNum= reader["RefNum"].ToString();
                    recieveVM.RecieveMaster.Recieve_date= Convert.ToDateTime(reader["Recieve_date"].ToString());
                    recieveVM.RecieveMaster.Supplier_name= reader["Supplier_name"].ToString();
                    recieveVM.RecieveMaster.Supplier_mobile= reader["Supplier_mobile"].ToString();
                    recieveVM.RecieveMaster.Total_amount = Convert.ToDecimal(reader["Total_amount"].ToString());
                    recieveVM.RecieveMaster.Is_Cancelled = Convert.ToBoolean(reader["Is_Cancelled"].ToString());
                }
                conn.Close();
                #endregion

                #region detail
                if (recieveVM.RecieveMaster.Id > 0)
                {
                    query = @"Select 
                                    Id ,
                                    RecieveMaster_Id,
                                    Item_Id,
                                    Purches_rate,
                                    Qty,
                                    Amount
                                from [dbo].[ReciveDetail] where Recive_Master_Id = @RecieveMaster_Id";
                    command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@Recive_Master_Id_", recieveVM.RecieveMaster.Id);

                    conn.Open();
                    SqlDataReader reader_details = command.ExecuteReader();
                    while (reader_details.Read())
                    {
                        RecieveDetail detail = new RecieveDetail();
                        detail.Id = Convert.ToInt32(reader_details["Id"].ToString());
                        detail.RecieveMaster_Id = Convert.ToInt32(reader_details["RecieveMaster_Id"].ToString());
                        detail.Item_Id = Convert.ToInt32(reader_details["Item_Id"].ToString());
                        detail.Qty = Convert.ToInt32(reader_details["Qty"].ToString());
                        detail.Purches_rate = Convert.ToInt32(reader_details["Purches_rate"].ToString());
                        detail.Amount = Convert.ToDecimal(reader_details["Amount"].ToString());
                        recieveVM.RecieveDetail.Add(detail);
                    }
                    conn.Close();
                }

                #endregion
                return recieveVM;
            }
        }
        public List<RecieveMaster> GetReceiveMasters()
        {
            List<RecieveMaster> recieveMasterlist = new List<RecieveMaster>();
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {

                #region master

                string query = @"Select
                                       Id,
                                       RefNum,
                                       Recieve_date,
                                       Supplier_name,
                                       Supplier_mobile,
                                       Total_amount,
                                       Is_Cancelled                            

                                from RecieveMaster";
                SqlCommand command = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    RecieveMaster recieveMaster = new RecieveMaster();
                    recieveMaster.Id = Convert.ToInt32(reader["Id"].ToString());
                    recieveMaster.RefNum= reader["RefNum"].ToString();
                    recieveMaster.Recieve_date= Convert.ToDateTime(reader["Recieve_date"].ToString());
                    recieveMaster.Supplier_name= reader["Supplier_name"].ToString();
                    recieveMaster.Supplier_mobile= reader["Supplier_mobile"].ToString();
                    recieveMaster.Total_amount = Convert.ToDecimal(reader["Total_amount"].ToString());
                    recieveMaster.Is_Cancelled = Convert.ToBoolean(reader["Is_Cancelled"].ToString()); 
                    recieveMasterlist.Add(recieveMaster);
                }
                conn.Close();
                #endregion


                return recieveMasterlist;
            }
        }
        public int AddNewrcv(RecieveVM vm)
        {
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                int master_id = 0;
                #region master
                string query = @"INSERT INTO RecieveMaster
                                       (RefNum,
                                       Recieve_date,
                                       Supplier_name,
                                       Supplier_mobile,
                                       Total_amount,
                                       Is_Cancelled)
                                 VALUES
                                      (@RefNum,
                                       @Recieve_date,
                                       @Supplier_name,
                                       @Supplier_mobile,
                                       @Total_amount,
                                       @Is_Cancelled )
                                 SELECT SCOPE_IDENTITY() Id;";
                SqlCommand command = new SqlCommand(query, conn);

                command.Parameters.AddWithValue("@RefNum", vm.RecieveMaster.RefNum);
                command.Parameters.AddWithValue("@Recieve_date", vm.RecieveMaster.Recieve_date);
                command.Parameters.AddWithValue("@Supplier_name", vm.RecieveMaster.Supplier_name);
                command.Parameters.AddWithValue("@Supplier_mobile", vm.RecieveMaster.Supplier_mobile);
                command.Parameters.AddWithValue("@Total_amount", vm.RecieveMaster.Total_amount);
                command.Parameters.AddWithValue("@Is_Cancelled ", vm.RecieveMaster.Is_Cancelled);

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    master_id = Convert.ToInt32(reader["Id"].ToString());
                }
                conn.Close();
                #endregion

                #region detail
                if (master_id > 0)
                {
                    foreach (var item in vm.RecieveDetail)
                    {
                        query = @"INSERT INTO RecieveDetail
                                     ( RecieveMaster_Id,
                                       Item_Id,
                                       Purches_rate,
                                       Qty,
                                       Amount)
                                 VALUES
                                      (@RecieveMaster_Id,
                                       @Item_Id,
                                       @Purches_rate,
                                       @Qty,
                                       @Amount)";
                        command = new SqlCommand(query, conn);
                        command.Parameters.AddWithValue("@Recive_Master_Id", master_id);
                        command.Parameters.AddWithValue("@Item_Id", item.Item_Id);
                        command.Parameters.AddWithValue("@Purchase_Rate", item.Purches_rate);
                        command.Parameters.AddWithValue("@Qty", item.Qty);
                        command.Parameters.AddWithValue("@Amount", item.Amount);
                        conn.Open();
                        var res = command.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                else
                {
                    return 0;
                }
                #endregion
                return 1;
            }
            //return -1;
        }
        public int UpdateRecieve(RecieveVM vm)
        {
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                int master_id = 0;
                #region master

                string query = @"Update RecieveMaster Set 
                                       
                                        RefNum = @RefNum
                                       ,Recieve_date = @Recieve_date
                                       ,Supplier_name =@SSupplier_name
                                       ,Supplier_mobile = @Supplier_mobile
                                       ,Total_amount = @Total_amount
                                       ,Is_Cancelled = @Is_Cancelled
                                        
                                 where Id=@Id_;";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Id_", vm.RecieveMaster.Id);
                command.Parameters.AddWithValue("@RefNum ", vm.RecieveMaster.RefNum );
                command.Parameters.AddWithValue("@Recieve_date ", vm.RecieveMaster.Recieve_date );
                command.Parameters.AddWithValue("@Supplier_name ", vm.RecieveMaster.Supplier_name );
                command.Parameters.AddWithValue("@Supplier_mobile ", vm.RecieveMaster.Supplier_mobile );
                command.Parameters.AddWithValue("@Total_amount ", vm.RecieveMaster.Total_amount );
                command.Parameters.AddWithValue("@Is_Cancelled ", vm.RecieveMaster.Is_Cancelled);


                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    master_id = Convert.ToInt32(reader["Id"].ToString());
                }
                conn.Close();
                #endregion

                #region detail
                if (vm.RecieveMaster.Id > 0)
                {
                    foreach (var item in vm.RecieveDetail)
                    {
                        query = @"Update [dbo].[ReciveDetail] set  
                                        RecieveMaster_Id=@RecieveMaster_Id,
                                        Item_Id=@Item_Id,
                                        Purches_rate= Purches_rate,
                                        Qty=@Qty,
                                        Amount=@Amount
                                    where Id=@Id ";
                        command = new SqlCommand(query, conn);
                        command.Parameters.AddWithValue("@Id", item.Id);
                        command.Parameters.AddWithValue("@Recive_Master_Id", master_id);
                        command.Parameters.AddWithValue("@Item_Id", item.Item_Id);
                        command.Parameters.AddWithValue("@Purches_rate", item.Purches_rate);
                        command.Parameters.AddWithValue("@Qty", item.Qty);
                        command.Parameters.AddWithValue("@Amount", item.Amount);
                        conn.Open();
                        var res = command.ExecuteNonQuery();
                        conn.Close();
                    }

                }
                else
                {
                    return 0;
                }
                #endregion
                return 1;
            };
        }
        public int RemoveItem(int Id)
        {
            using (SqlConnection conn = new SqlConnection(_ConnStr))
            {
                string query = "delete from RecieveMaster where Id = @Id";
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
