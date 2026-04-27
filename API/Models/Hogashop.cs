using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingAPI.Models.Hogashop
{
    public class Customer
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Item
    {
        public int id { get; set; }
        public string name { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public Customer customer { get; set; }
        public object itemsInBasket { get; set; }
        public string gln { get; set; }
    }

    public class Sorting
    {
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
    }

    public class Departments
    {
        public int total { get; set; }
        public List<Item> items { get; set; }
        public Sorting sorting { get; set; }
    }


    public class ProductOrder
    {
        public decimal amount { get; set; }
        public string deliveryDate { get; set; }
        public HGProducts product { get; set; }
    }

    public class HGProducts
    {
        public int id { get; set; }
    }

    public class AddProductResult
    {
        public bool Success { get; set; }
        public int StatusCode  { get; set; }
        public string Message { get; set; }
        public string BasketRevision { get; set; }
    }
}

namespace OrderingAPI.Models.Hogashop.Basket
{

    public class PerUnit
    {
        public string Value { get; set; }
    }

    public class Price
    {
        public PerUnit PerUnit { get; set; }
        public string PerSellingUnit { get; set; }
        public string Total { get; set; }
    }

    public class FirstName
    {
        public string Language { get; set; }
        public string Value { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public List<FirstName> Name { get; set; }
    }

    public class SellingUnit
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

    public class AllUnits
    {
        public Dictionary<string, List<FirstName>> Units { get; set; }
    }

    public class Image
    {
        public string Id { get; set; }
        public string ContentType { get; set; }
        public string Ext { get; set; }
        public int Length { get; set; }
        public string Filename { get; set; }
        public string Md5 { get; set; }
    }

    public class ContactingPerson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Flags
    {
        public bool CanReadCustomerInput { get; set; }
    }

    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string EmailInfo { get; set; }
        public string Website { get; set; }
        public ContactingPerson ContactingPerson { get; set; }
        public Flags Flags { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public Supplier Supplier { get; set; }
        public string ExternalId { get; set; }
        public int Type { get; set; }
        public List<FirstName> Name { get; set; }
        public Category Category { get; set; }
        public string BaseUnit { get; set; }
        public List<SellingUnit> SellingUnits { get; set; }
        public AllUnits AllUnits { get; set; }
        public string RecipeUnit { get; set; }
        public double NetWeight { get; set; }
        public Image Image { get; set; }
        public string PreSupplierId { get; set; }
        public List<string> EanCodes { get; set; }
        public List<int> AlternativeProducts { get; set; }
        public double Vat { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsWithoutPrice { get; set; }
        public bool IsNew { get; set; }
    }

    public class Part
    {
        public List<Product> Items { get; set; }
        public Supplier Supplier { get; set; }
        public bool IsCustomerInputAllowed { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string FreeShippingFrom { get; set; }
        public string PartDeliveryFee { get; set; }
        public string PartTotal { get; set; }
    }

    public class Brand
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Hostname { get; set; }
    }

    public class CsvFormat
    {
        public string Encoding { get; set; }
        public string Delimiter { get; set; }
        public string LineBreaker { get; set; }
    }

    public class ContactingPerson2
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Fax { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public Brand Brand { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Canton { get; set; }
        public string Gender { get; set; }
        public string Street { get; set; }
        public string Company { get; set; }
        public string Country { get; set; }
        public string Comments { get; set; }
        public string Language { get; set; }
        public CsvFormat CsvFormat { get; set; }
        public ContactingPerson2 ContactingPerson { get; set; }
        public ContactingPerson HogalogContactingPerson { get; set; }
    }

    public class RootObject
    {
        //public List<Part> parts { get; set; }
        public string Total { get; set; }
        //public Customer Customer { get; set; }
        public string GrandTotal { get; set; }
        public string DeliveryFee { get; set; }
        public int SumOfAmounts { get; set; }
        public int CountOfProducts { get; set; }
                
    }


}