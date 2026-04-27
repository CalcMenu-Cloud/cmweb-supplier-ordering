using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OrderingAPI.Models.Hogashop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OrderingAPI.Services
{
    public class HogashopService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;
        private string _connectionstring;
        private string _sessionid { get; set; }
        private string _sysid { get; set; }

        public bool isRefreshTokenAvailable = false;
        private Models.UserSession _usersession;

        public HogashopService(IConfiguration configuration, HttpContext httpContext)
        {
            _configuration = configuration;
            _httpContext = httpContext;
            _connectionstring = _configuration.GetConnectionString("DefaultConnection");

            _sessionid = "";
            if (_httpContext.Request.Headers.ContainsKey("sessionid"))
            {
                _sessionid = _httpContext.Request.Headers["sessionid"];
            }

            if (_httpContext.Request.Headers.ContainsKey("sysid"))
            {
                _sysid = _httpContext.Request.Headers["sysid"];
            }

            if (_httpContext.Items.Keys.Contains("InternalUserSession"))
            {
                _usersession = _httpContext.Items["InternalUserSession"] as Models.UserSession;
            }


        }

        public OrderingAPI.Models.Hogashop.Departments getDepartments()
        {
            OrderingAPI.Models.Hogashop.Departments department = new Models.Hogashop.Departments();
            try
            {
                Data.LoginSessionDataService logsession = new Data.LoginSessionDataService(_connectionstring);

                string refreshtoken = "";

                    if (_usersession==null && string.IsNullOrEmpty(_sysid))
                    {
                        if (!string.IsNullOrEmpty(_sessionid))
                        {
                       
                        OrderingAPI.Models.UserSession usersessionx = logsession.getSNUserSession(_sessionid);
                        _sysid = usersessionx.Id.ToString();
                        refreshtoken = logsession.GetRefreshTokenById(usersessionx.Id.ToString());
                        }
                   
                }
                else
                {
                    if (!string.IsNullOrEmpty(_sysid))
                    {
                        refreshtoken = logsession.GetRefreshTokenById(_sysid);
                    }

                }

                 



                if(string.IsNullOrEmpty(refreshtoken))
                {
                    return null;
                }

                isRefreshTokenAvailable = true;


                ThirdPartyAPI.HogashopOauth hogaoauth = new ThirdPartyAPI.HogashopOauth();

                ThirdPartyAPI.TokenModel token = hogaoauth.GetRefreshToken(refreshtoken);

                refreshtoken = token.refresh_token;
                string accesstoken = token.access_token;
                logsession.SetRefreshTokenById(_sysid, refreshtoken);

                ThirdPartyAPI.HogashopAPI hogoapi = new ThirdPartyAPI.HogashopAPI(accesstoken);

               if(hogoapi.GetDepartments(ref department))
                {

                }

            }
            catch(Exception ex)
            {
            
            }

            return department;
        }


        public async Task<Models.Hogashop.AddProductResult> SendOrderAsync(Models.SNOrder order)
        {
            OrderingAPI.Models.Hogashop.Departments department = new Models.Hogashop.Departments();
            Models.Hogashop.AddProductResult addproductresult = new Models.Hogashop.AddProductResult();
            try
            {
                Data.LoginSessionDataService logsession = new Data.LoginSessionDataService(_connectionstring);

                string refreshtoken = "";

                string departmentid = order.DepartmentId;
                if(string.IsNullOrEmpty(departmentid))
                {
                    addproductresult.Message = "Missing department Id";
                    addproductresult.StatusCode = -1;
                    return addproductresult;
           
                }

                if (_usersession == null && string.IsNullOrEmpty(_sysid))
                {
                    if (!string.IsNullOrEmpty(_sessionid))
                    {
                        OrderingAPI.Models.UserSession usersessionx = logsession.getSNUserSession(_sessionid);
                        _sysid = usersessionx.Id.ToString();
                        refreshtoken = logsession.GetRefreshTokenById(usersessionx.Id.ToString());
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(_sysid))
                    {
                        refreshtoken = logsession.GetRefreshTokenById(_sysid);
                    }

                }

                if (string.IsNullOrEmpty(refreshtoken))
                {
                    addproductresult.Message = "Missing refresh token";
                    addproductresult.StatusCode = -2;
                    return addproductresult;
                }

                isRefreshTokenAvailable = true;

                ThirdPartyAPI.HogashopOauth hogaoauth = new ThirdPartyAPI.HogashopOauth();

                ThirdPartyAPI.TokenModel token = hogaoauth.GetRefreshToken(refreshtoken);

                string accesstoken = "";

                if(token !=null)
                {
                    refreshtoken = token.refresh_token;
                    accesstoken = token.access_token;
                }

                if (string.IsNullOrEmpty(refreshtoken))
                {
                    addproductresult.Message = "Missing refresh token";
                    addproductresult.StatusCode = -3;
                    return addproductresult;
                }

                logsession.SetRefreshTokenById(_sysid, refreshtoken);
                ThirdPartyAPI.HogashopAPI hogoapi = new ThirdPartyAPI.HogashopAPI(accesstoken);


                //GET BASKET REVISION 
                string revision =  hogoapi.GetBasket( departmentid);

                //CHECK IF TOTAL QTY>1 PERFORM DELETE BASKET ELSE NO
                if(hogoapi.totalproductcount>0)
                {
                    //DELETE BASKET
                    hogoapi.DeleteBasket(departmentid, revision);
                }
                
                //ADD PRODUCT TO BASKET
                Models.Hogashop.AddProductResult result = await hogoapi.AddProductToBasketAsync(order.getProductOrderFormat(), departmentid, accesstoken);


                if(result.StatusCode!=201)
                {

                    return result;
                }

                //addproductresult.Success = true;
                //addproductresult.Message = "Order sent!";
                //addproductresult.StatusCode = 201;

                //return addproductresult;

                result = await hogoapi.SendOrderAsync(hogoapi.sessionbasketrevision, departmentid, accesstoken);

                if(result.Success==false)
                { result.Message = "The product was added to the basket, but the order failed to send.\n<br\\><br\\><b>" + result.Message+"</b>"; }

                return result;

            }
            catch (Exception ex)
            {
                addproductresult.Message = ex.Message ;
                addproductresult.StatusCode = (int)HttpStatusCode.BadRequest;
                return addproductresult;
            }

            
        }

        public string GetAccessToken()
        {
            try
            {
                Data.LoginSessionDataService logsession = new Data.LoginSessionDataService(_connectionstring);

                string refreshtoken = "";


                if (_usersession == null && string.IsNullOrEmpty(_sysid))
                {
                    if (!string.IsNullOrEmpty(_sessionid))
                    {
                        OrderingAPI.Models.UserSession usersessionx = logsession.getSNUserSession(_sessionid);
                        _sysid = usersessionx.Id.ToString();
                        refreshtoken = logsession.GetRefreshTokenById(usersessionx.Id.ToString());
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(_sysid))
                    {
                        refreshtoken = logsession.GetRefreshTokenById(_sysid);
                    }

                }

                if (string.IsNullOrEmpty(refreshtoken))
                {
                    return "";
                }

                isRefreshTokenAvailable = true;


                ThirdPartyAPI.HogashopOauth hogaoauth = new ThirdPartyAPI.HogashopOauth();

                ThirdPartyAPI.TokenModel token = hogaoauth.GetRefreshToken(refreshtoken);


                string accesstoken = "";

                if (token != null)
                {
                    refreshtoken = token.refresh_token;
                    accesstoken = token.access_token;
                }

                if (string.IsNullOrEmpty(refreshtoken))
                {
                    
                    return "";
                }

                logsession.SetRefreshTokenById(_sysid, refreshtoken);

                return accesstoken;


            }
            catch (Exception ex)
            {
                return "";
            }


        }

    }
}
