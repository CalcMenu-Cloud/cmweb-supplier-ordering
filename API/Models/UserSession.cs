using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrderingAPI.Models
{
    public class UserSession
    {
        // Properties
        public string Id { get; set; }
        public string SupplierType { get; set; }
        public string ClientId { get; set; }
        public string CodeUser { get; set; }
        public string DepartmentId { get; set; }
        public string SessionId { get; set; }
        public string RefreshToken { get; set; }
        public string DateCreated { get; set; }
        public string ModifiedDate { get; set; }
        public string ExpiredDate { get; set; }
        public string Status { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }

    }


    public class UserLoginInfo
    {
        public string username { get; set; }
        public string password { get; set; }
        public string sessionid { get; set; }
    
    }


    public class Credentialbs64
    {
        public string credential { get; set; }
    
    }

    public class WebResponse
    {
        public string status { get; set; }
        public string message { get; set; }

    }

}
