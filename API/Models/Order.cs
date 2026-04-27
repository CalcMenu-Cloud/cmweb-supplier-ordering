using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderingAPI.Models
{

// Models/egswSupOrder.cs
public class Order
    {
        public int Id { get; set; }
        public int SupplierType { get; set; }
        public int CustomerCode { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Terms { get; set; }
        public string Note { get; set; }
        public int Status { get; set; }
        public List<OrderDetails> Details { get; set; }

    }

    // Models/OrderSupDetails.cs
    public class OrderDetails
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }


    public class SNOrder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SupplierType { get; set; }
        public int ClientId { get; set; }

        public int CodeUser { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Terms { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public string DepartmentId { get; set; }
        
        public List<SNOrderDetails> OrderDetails { get; set; }



        public string getProductOrderFormat()
        {
            try
            {
                List<Models.Hogashop.ProductOrder> orderproduct = new List<Hogashop.ProductOrder>();

                Models.Hogashop.ProductOrder p;
                foreach (var o in OrderDetails)
                {
                    p = new Models.Hogashop.ProductOrder();

                    p.product = new Hogashop.HGProducts();
                    p.product.id = o.ProductId;
                    p.amount = o.Quantity;
                    p.deliveryDate = o.DeliveryDate.ToString("yyyy-MM-dd");

                    orderproduct.Add(p);
                }

                string json = JsonSerializer.Serialize(orderproduct);

                return json;
            }
            catch(Exception ex)
            {
                return "";
            }
        }
    }

    public class SNOrderDetails
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int Codeliste { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Supplier { get; set; }
        public string Category { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
       public DateTime DeliveryDate { get; set; }
        public decimal BasePrice { get; set; }
        public string BaseUnit { get; set; }
        public decimal SellingPrice { get; set; }
        public string SellingUnit { get; set; }
        public decimal Ratio1 { get; set; }

        public int ProductId { get; set; }
        public string PicturePath { get; set; }

    }

    public class LoginInfo
    {
        public string returnUrl { get; set; }
        public string sessionKey { get; set; }
    }

    public class LoginInfoHG
    {
        public string sessionKey { get; set; }
        public string codeuser { get; set; }
        public string baseurl { get; set; }
        public string callbackurl { get; set; }

    }


}
