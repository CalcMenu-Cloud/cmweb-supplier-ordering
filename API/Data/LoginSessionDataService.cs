using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingAPI.Data
{
    public class LoginSessionDataService
    {

        private readonly string _connectionString;

        public LoginSessionDataService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool setSNUserSession(string ClientId,string CodeUser,string SessionKey,string RefreshToken)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("setSNUserSession", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@SupplierType", 3); // Replace with actual values
                    command.Parameters.AddWithValue("@ClientId", ClientId ?? "-1");
                    command.Parameters.AddWithValue("@CodeUser", CodeUser ?? "-1");
                    command.Parameters.AddWithValue("@DepartmentId", 0);
                    command.Parameters.AddWithValue("@SesssionID", SessionKey);
                    command.Parameters.AddWithValue("@RefreshToken", RefreshToken);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool setSNUserTokenWithId(string Codeuser,  string SessionKey, string RefreshToken)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("setSNUserTokenWithId", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@SesssionID", SessionKey); // Replace with actual values
                    command.Parameters.AddWithValue("@RefreshToken", RefreshToken);
                    command.Parameters.AddWithValue("@Codeuser", Codeuser);
          

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool setSNUserSession(string SessionKey,string RefreshToken )
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("setSNUserToken", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SessionKey", SessionKey); // Replace with actual values
                    command.Parameters.AddWithValue("@RefreshToken", RefreshToken);
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Models.UserSession getSNUserSession(string SessionId)
        {
            Models.UserSession usersession = new Models.UserSession();
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("getSNUserSession", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SesssionID", SessionId); // Replace with actual values
                    connection.Open();
                    // Create a DataTable to hold the results
                    usersession = null;
                    // Use SqlDataAdapter to fill the DataTable with the results of the SqlCommand
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count > 0)
                        {
                            usersession = new Models.UserSession();
                            usersession.Id = dataTable.Rows[0]["Id"].ToString();
                            usersession.SupplierType = dataTable.Rows[0]["SupplierType"].ToString();
                            usersession.ClientId = dataTable.Rows[0]["ClientId"].ToString();
                            usersession.CodeUser = dataTable.Rows[0]["CodeUser"].ToString();
                            usersession.DepartmentId = dataTable.Rows[0]["DepartmentId"].ToString();
                            usersession.SessionId = dataTable.Rows[0]["SesssionID"].ToString();
                            usersession.RefreshToken = dataTable.Rows[0]["RefreshToken"].ToString();
                            usersession.DateCreated = dataTable.Rows[0]["DateCreated"].ToString();
                            usersession.ModifiedDate = dataTable.Rows[0]["ModifiedDate"].ToString();
                            usersession.ExpiredDate = dataTable.Rows[0]["ExpiredDate"].ToString();
                            usersession.Status = dataTable.Rows[0]["Status"].ToString();
                            usersession.Fullname = dataTable.Rows[0]["Fullname"].ToString();
                            usersession.Email = dataTable.Rows[0]["Email"].ToString();
                        }

                        return usersession;
                    }
                }
            }
            catch (Exception ex)
            {
                return usersession;
            }
        }

        public string GetRefreshTokenById(string id)
        {
            string refreshToken = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("getSNRefreshTokenById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", id);

                        SqlParameter refreshTokenParam = new SqlParameter("@RefreshToken", SqlDbType.NVarChar, -1);
                        refreshTokenParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(refreshTokenParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        refreshToken = refreshTokenParam.Value as string;
                    }
                }
            }
            catch(Exception ex)
            {

            }

            return refreshToken;
        }

        public void SetRefreshTokenById(string id, string refreshToken)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("setSNRefreshTokenById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@RefreshToken", refreshToken);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool ValidateCredential(string username,string password,ref int CodeUser)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("ValidateSNUserLogin", connection))
                    {
                        // Set the command type to Stored Procedure
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.Add("@IsValidLogin", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@CodeUser", SqlDbType.Int).Direction = ParameterDirection.Output;
                        connection.Open();
                        // Execute the command
                        cmd.ExecuteNonQuery();

                        // Retrieve the output parameter value
                        bool isValidLogin = (bool)cmd.Parameters["@IsValidLogin"].Value;

                        if (isValidLogin)
                        {
                            CodeUser= (int)cmd.Parameters["@CodeUser"].Value;
                            return true;
                        }
                        // Now you can use 'isValidLogin' to determine the result of the login validation
                    }
                }


              
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }


        }
    }
