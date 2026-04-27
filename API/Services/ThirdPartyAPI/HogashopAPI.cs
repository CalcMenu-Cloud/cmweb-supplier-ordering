using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OrderingAPI.Models.Hogashop;

namespace OrderingAPI.Services.ThirdPartyAPI
{
    public class HogashopAPI : IDisposable
    {

        //public string clientAPILink { get; set; } = "https://clients-api.hogashop.ch"; //PRODUCTION
        public string clientAPILink { get; set; } = "https://clients-api.proto-hogashop.ch"; //PROTO TYPE
        internal string AccessToken {get;set;}
        private readonly HttpClient _httpClient;

        public int totalproductcount = 0;
        public string sessionbasketrevision = "";


        // Dispose method to release unmanaged resources

        public HogashopAPI(string paramAccessToken)
        {
            AccessToken = paramAccessToken;
        }

        public bool GetDepartments(ref OrderingAPI.Models.Hogashop.Departments departments)
        {
            try
            {
                HttpWebRequest req;
                req = WebRequest.Create(clientAPILink+"/v5/paginated/departments?limit=20&offset=0&sortBy=department_name&sortOrder=ASC") as HttpWebRequest;
                req.Method = "GET";

                req.Timeout = 1800000; // 30 mins
                req.ReadWriteTimeout = 1800000; // 30 mins
                req.KeepAlive = true;
                req.Headers.Add("Authorization", string.Format("Bearer {0}", AccessToken));
                req.Headers.Add("accept-language", "de");

                using (HttpWebResponse res = req.GetResponse() as HttpWebResponse)
                {

                    if (res.StatusCode != HttpStatusCode.OK)
                    {
                        

                        return false;
                    }

                    using (StreamReader sr = new StreamReader(res.GetResponseStream()))
                    {       
                            string result = sr.ReadToEnd();
                            departments = JsonSerializer.Deserialize<OrderingAPI.Models.Hogashop.Departments>(result);
                       
                        return true;
                    }
                }




            }
            catch(Exception ex)
            {
                return false;

            }
        }

        ///<summary>
        ///Add items to basket.After this the client should GET whole basket to obtain correct parts and totals.
        ///When the basket has products the merge is performed with this rules:
        ///Products of type 1 are never merged
        ///Same products are merged: the amount of existing basket item will be increased, unless delivery dates are different.
        ///</summary>
        public bool AddProductToBasket(string productOrder,string departmentid )
        {
            try
            {
                HttpWebRequest req;
                req = WebRequest.Create(clientAPILink + "/v5/department/" +departmentid+"/basket/items?source=web&ignoreErrors=0") as HttpWebRequest;
                req.Method = "POST";

                req.Timeout = 1800000; // 30 mins
                req.ReadWriteTimeout = 1800000; // 30 mins
                req.KeepAlive = true;
                req.Headers.Add("Authorization", string.Format("Bearer {0}", AccessToken));
                req.Headers.Add("accept-language", "de");
                req.Headers.Add("Content-Type", "application/json");

                using (HttpWebResponse res = req.GetResponse() as HttpWebResponse)
                {

                    if (res.StatusCode != HttpStatusCode.OK)
                    {
                        return false;
                    }
                    
                    switch(res.StatusCode)
                    {
                     
                        //"message": " ... any message about wrong addToBasketRequest schema ... ",
                        //"code": 40023
                        case HttpStatusCode.BadRequest:
                            return false;

                        //"message": "Department does not exist",
                        //"code": 40411
                        case HttpStatusCode.NotFound:
                            return false;

                        //"message": "Concurrent update of basket",
                        //"code": 4092
                        case HttpStatusCode.Conflict:
                            return false;

                        //A web response with status code 422 indicates that the server understands the request, but it cannot process 
                        //the request due to semantic errors in the request parameters.This status code is typically used when 
                        //the server cannot fulfill the request because the request parameters are valid but semantically incorrect.
                        case HttpStatusCode.UnprocessableEntity:
                            return false;

                        //HTTP status code 207 stands for "Multi-Status".This status code is used in 
                        //WebDAV(Web Distributed Authoring and Versioning) to indicate that the message body 
                        //contains multiple response elements.Each response element contains information about the status of a
                        //separate operation in the request.
                        case HttpStatusCode.MultiStatus:
                            return false;

                    }

                    using (StreamReader sr = new StreamReader(res.GetResponseStream()))
                    {
                        string result = sr.ReadToEnd();
                        return true;
                    }
                }




            }
            catch (Exception ex)
            {
                return false;

            }
        }

        public async Task<Models.Hogashop.AddProductResult> AddProductToBasketAsync(string productOrder, string departmentId, string accessToken)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = clientAPILink + $"/v5/department/{departmentId}/basket/items?source=web&ignoreErrors=0";
                    var json = productOrder;
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Headers.Add("Authorization", $"Bearer {accessToken}");
                    request.Content = content;

                    var response = await httpClient.SendAsync(request);
                    var statusCode = (int)response.StatusCode;
                    string basketRevision = "";

                    if (response.Headers.Contains("Basket-Revision") && response.Headers.GetValues("Basket-Revision")?.Any() == true)
                    {
                         basketRevision = response.Headers.GetValues("Basket-Revision").FirstOrDefault();
                        // Now you can use the basketRevision variable
                    }
                    else
                    {
                        // Header doesn't exist or is empty
                         basketRevision = ""; // or null, depending on your preference
                    }

                    string message = "";
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                        case HttpStatusCode.Created:
                            using (var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                            {
                                var result = await streamReader.ReadToEndAsync();
                                sessionbasketrevision = basketRevision;
                                return new AddProductResult { Success = true, Message="Product successfully added to basket",StatusCode = 201, BasketRevision = basketRevision };
                            }
                        case HttpStatusCode.BadRequest:
                        case HttpStatusCode.NotFound:
                        case HttpStatusCode.Conflict:
                        case HttpStatusCode.UnprocessableEntity:
                        case HttpStatusCode.MultiStatus:

                          
                                using (var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                                {
                                    using (JsonDocument document = JsonDocument.Parse(await streamReader.ReadToEndAsync()))
                                    {
                                        if (statusCode != 422 && statusCode != 207)
                                        {
                                            message = document.RootElement.GetProperty("message").ToString();
                                            message += "(" + document.RootElement.GetProperty("code").ToString() + ")";
                                        }
                                    else
                                    {
                                        message = "Some of product is not valid. Please check again. " + document.RootElement.ToString();
                                    }
                                    }
                                }
                           
                            return new AddProductResult { Success = false,  Message= message, StatusCode = statusCode, BasketRevision = basketRevision };
                        default:

                            using (var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                            {
                                using (JsonDocument document = JsonDocument.Parse(await streamReader.ReadToEndAsync()))
                                {
                                    message = document.RootElement.GetProperty("message").ToString();
                                    message += "(" + document.RootElement.GetProperty("code").ToString() + ")";
                                }
                            }
                            return new AddProductResult { Success = false, Message = response.ReasonPhrase, StatusCode = statusCode, BasketRevision = basketRevision };
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new AddProductResult { Success = false, StatusCode = -6, BasketRevision = null }; // Set to a default value indicating failure
            }
        }

        public string GetBasket(string departmentid)
        {
            try
            {
                HttpWebRequest req;
                req = WebRequest.Create(clientAPILink + "/v5/department/" + departmentid + "/basket") as HttpWebRequest;
                req.Method = "GET";

                req.Timeout = 1800000; // 30 mins
                req.ReadWriteTimeout = 1800000; // 30 mins
                req.KeepAlive = true;
                req.Headers.Add("Authorization", string.Format("Bearer {0}", AccessToken));
                req.Headers.Add("accept-language", "de");

                using (HttpWebResponse res = req.GetResponse() as HttpWebResponse)
                {
                    // Check if the response status code is not OK
                    if (res.StatusCode != HttpStatusCode.OK)
                    {
                        // Return an empty string if the status code is not OK
                        return "";
                    }

                    string basketRevision = "";
                    string[] basketRevisionValues = res.Headers.GetValues("Basket-Revision");
                    if (basketRevisionValues != null && basketRevisionValues.Length > 0)
                    {
                        basketRevision = basketRevisionValues[0];
                        // Now you can use the basketRevision variable
                    }


                    try
                    {
                        using (StreamReader sr = new StreamReader(res.GetResponseStream()))
                        {
                           string result = sr.ReadToEnd();

                            // Parse the JSON string and extract the value of countOfProducts
                            using (JsonDocument document = JsonDocument.Parse(result))
                            {
                                int countOfProducts = document.RootElement.GetProperty("countOfProducts").GetInt32();

                                totalproductcount = countOfProducts;
                                Console.WriteLine("Count of Products: " + countOfProducts);
                            }
                          
                           
                        }
                    }
                    catch(Exception ex)
                    {

                    }

                    return basketRevision;
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public bool DeleteBasket(string departmentid,string revisionid)
        {
            try
            {
                sessionbasketrevision = "";
                   HttpWebRequest req;
                req = WebRequest.Create(clientAPILink +@"/v5/department/"+ departmentid + "/basket?revision=" + revisionid) as HttpWebRequest;
                req.Method = "DELETE";

                req.Timeout = 1800000; // 30 mins
                req.ReadWriteTimeout = 1800000; // 30 mins
                req.KeepAlive = true;
                req.Headers.Add("Authorization", string.Format("Bearer {0}", AccessToken));
                req.Headers.Add("accept-language", "de");

                using (HttpWebResponse res = req.GetResponse() as HttpWebResponse)
                {
                    // Check if the response status code is not OK
                    if (res.StatusCode != HttpStatusCode.NoContent)
                    {
                        return false;
                        // Return an empty string if the status code is not OK

                    }

                    string basketRevision = "";
                    string[] basketRevisionValues = res.Headers.GetValues("Basket-Revision");
                    if (basketRevisionValues != null && basketRevisionValues.Length > 0)
                    {
                        basketRevision = basketRevisionValues[0];
                        sessionbasketrevision = basketRevision;
                        // Now you can use the basketRevision variable
                    }
                }

                return true;
               
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<Models.Hogashop.AddProductResult> SendOrderAsync(string revision, string departmentId, string accessToken)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = clientAPILink +$"/v5/department/"+ departmentId + "/order/basket-revision/"+ revision;
                    //var json = productOrder;
                    //var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Headers.Add("Authorization", $"Bearer {accessToken}");
                    //request.Content = content;

                    var response = await httpClient.SendAsync(request);
                    var statusCode = (int)response.StatusCode;
                    string basketRevision = "";

                    if (response.Headers.Contains("Basket-Revision") && response.Headers.GetValues("Basket-Revision")?.Any() == true)
                    {
                        basketRevision = response.Headers.GetValues("Basket-Revision").FirstOrDefault();
                        // Now you can use the basketRevision variable
                    }
                    else
                    {
                        // Header doesn't exist or is empty
                        basketRevision = ""; // or null, depending on your preference
                    }


                    string bodymessage = "";
                

                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                        case HttpStatusCode.Created:
                            using (var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                            {
                                var result = await streamReader.ReadToEndAsync();
                                return new AddProductResult { Success = true, Message = "Product successfully send order to supplier", StatusCode = 201, BasketRevision = basketRevision };
                            }
                        case HttpStatusCode.BadRequest:
                        case HttpStatusCode.Forbidden:
                        case HttpStatusCode.NotFound:
                        case HttpStatusCode.Conflict:
                        case HttpStatusCode.InternalServerError:

                            string message = "";
                                using (var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                                {
                                    using (JsonDocument document = JsonDocument.Parse(await streamReader.ReadToEndAsync()))
                                    {
                                     message = document.RootElement.GetProperty("message").ToString();
                                     message += "(" + document.RootElement.GetProperty("code").ToString() + ")" ;
                                    }
                                }
                            return new AddProductResult { Success = false, Message = message, StatusCode = statusCode, BasketRevision = basketRevision };

                        default:
                            using (var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                            {
                                using (JsonDocument document = JsonDocument.Parse(await streamReader.ReadToEndAsync()))
                                {
                                    message = document.RootElement.GetProperty("message").ToString();
                                    message += "(" + document.RootElement.GetProperty("code").ToString() + ")";
                                }
                            }
                            return new AddProductResult { Success = false, Message = message, StatusCode = statusCode, BasketRevision = basketRevision };
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new AddProductResult { Success = false, StatusCode = 0, BasketRevision = null }; // Set to a default value indicating failure
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources
                _httpClient.Dispose();
            }
        }
    }

   
}
