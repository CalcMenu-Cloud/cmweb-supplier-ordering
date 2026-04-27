using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using OrderingAPI.Models;
namespace OrderingAPI.Data
{
    public class OrderDataService
    {

        private readonly string _connectionString;

        public OrderDataService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void InsertOrderAndDetails(Models.Order order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Insert data into egswSupOrder table
                string insertOrderQuery = @"INSERT INTO egswSupOrder (SupplierType, CustomerCode, DateCreated, ModifiedDate, DatePosted, DeliveryDate, Terms, Note, [Status])
                                            VALUES (@SupplierType, @CustomerCode, GETDATE(), GETDATE(), GETDATE(), @DeliveryDate, @Terms, @Note, @Status); SELECT SCOPE_IDENTITY();";
                SqlCommand insertOrderCommand = new SqlCommand(insertOrderQuery, connection);
                insertOrderCommand.Parameters.AddWithValue("@SupplierType", order.SupplierType);
                insertOrderCommand.Parameters.AddWithValue("@CustomerCode", order.CustomerCode);
                insertOrderCommand.Parameters.AddWithValue("@DeliveryDate", order.DeliveryDate);
                insertOrderCommand.Parameters.AddWithValue("@Terms", order.Terms);
                insertOrderCommand.Parameters.AddWithValue("@Note", order.Note);
                insertOrderCommand.Parameters.AddWithValue("@Status", order.Status);
                //insertOrderCommand.ExecuteNonQuery();

                int orderId = Convert.ToInt32(insertOrderCommand.ExecuteScalar());


                // Get the newly inserted OrderId
                //string orderIdQuery = "SELECT SCOPE_IDENTITY();";
                //SqlCommand orderIdCommand = new SqlCommand(orderIdQuery, connection);
                //int orderId = Convert.ToInt32(orderIdCommand.ExecuteScalar());

                // Insert data into OrderSupDetails table
                foreach (var detail in order.Details)
                {
                    string insertOrderDetailsQuery = @"INSERT INTO OrderSupDetails (OrderId, Number, Name, Quantity, Price)
                                                       VALUES (@OrderId, @Number, @Name, @Quantity, @Price);";
                    SqlCommand insertOrderDetailsCommand = new SqlCommand(insertOrderDetailsQuery, connection);
                    insertOrderDetailsCommand.Parameters.AddWithValue("@OrderId", orderId);
                    insertOrderDetailsCommand.Parameters.AddWithValue("@Number", detail.Number);
                    insertOrderDetailsCommand.Parameters.AddWithValue("@Name", detail.Name);
                    insertOrderDetailsCommand.Parameters.AddWithValue("@Quantity", detail.Quantity);
                    insertOrderDetailsCommand.Parameters.AddWithValue("@Price", detail.Price);
                    insertOrderDetailsCommand.ExecuteNonQuery();
                }
            }
        }

        public SNOrder GetOrderById(int id,ref bool isError )
        {
            SNOrder order = new SNOrder();
            isError = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        // Open the connection
                        connection.Open();
                        // Create a new SqlCommand for calling the stored procedure
                        using (SqlCommand cmd = new SqlCommand("getSNOrderById", connection))
                        {
                            // Set the command type to Stored Procedure
                            cmd.CommandType = CommandType.StoredProcedure;
                            // Add parameter @id
                            cmd.Parameters.AddWithValue("@id", id); // Replace 123 with your actual parameter value
                            // Create a new SqlDataAdapter to fetch data from the stored procedure
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            // Create a new DataTable to store the result
                            DataSet ds = new DataSet();
                            // Fill the DataTable with the result of the stored procedure
                            adapter.Fill(ds);

                            List<SNOrder> orders = Utility.Util.ConvertDataTableToList<SNOrder>(ds.Tables[0]);
                            List<SNOrderDetails> orderDetails = Utility.Util.ConvertDataTableToList<SNOrderDetails>(ds.Tables[1]);
                            order = orders[0];
                            order.OrderDetails = new List<SNOrderDetails>();
                            order.OrderDetails.AddRange(orderDetails);


                        }
                    }
                    catch (Exception ex)
                    {
                        isError = true;
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }


            }
            catch(Exception ex)
            {

            }

            return order;
        }

        public List<SNOrder> GetOrderlistByClientId(string ClientId)
        {
           
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        // Open the connection
                        connection.Open();
                        // Create a new SqlCommand for calling the stored procedure
                        using (SqlCommand cmd = new SqlCommand("getSNOrderlistByClientId", connection))
                        {
                            // Set the command type to Stored Procedure
                            cmd.CommandType = CommandType.StoredProcedure;
                            // Create a new SqlDataAdapter to fetch data from the stored procedure
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            // Create a new DataTable to store the result
                            DataSet ds = new DataSet();
                            // Fill the DataTable with the result of the stored procedure
                            adapter.Fill(ds);

                            List<SNOrder> orders = Utility.Util.ConvertDataTableToList<SNOrder>(ds.Tables[0]);
                            return orders;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return  null;
                      
                    }
                }


         

            
        }


        public bool SaveOrder(Models.SNOrder order)
        {
            try
            {
                return true;
            }
            catch(Exception ex)
            {
                return false;

            }
        }
    }
}
