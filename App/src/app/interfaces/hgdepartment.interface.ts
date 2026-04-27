// Define a class representing the customer
export class Customer {
    id: number;
    name: string;
  }
  
  // Define a class representing an item
  export class Item {
    id: number;
    name: string;
    street: string;
    city: string;
    zip: string;
    customer: Customer;
    itemsInBasket: any; // Change this type to match the actual data type
    gln: string;
  }
  
  // Define a class representing the sorting information
  export class Sorting {
    sortBy: string;
    sortOrder: string;
  }
  
  // Define a class representing the response structure
  export class Department {
    total: number;
    items: Item[];
    sorting: Sorting;
  }



  export interface Supplier {
    id: number;
    name: string;
    street: string;
    zip: string;
    city: string;
    phone: string;
    fax: string;
    emailInfo: string;
    website: string;
    contactingPerson: {
      firstName: string;
      lastName: string;
    };
    flags: {
      canReadCustomerInput: boolean;
    };
  }
  

  export interface Customer {
    id: number;
    fax: string;
    zip: string;
    city: string;
    name: string;
    brand: {
      name: string;
      title: string;
      hostname: string;
    };
    email: string;
    phone: string;
    canton: string;
    gender: string;
    street: string;
    company: string;
    country: string;
    comments: string;
    language: string;
    csvFormat: {
      encoding: string;
      delimiter: string;
      lineBreaker: string;
    };
    contactingPerson: {
      lastName: string;
      firstName: string;
    };
    hogalogContactingPerson: {
      lastName: string;
      firstName: string;
    };
  }
  
  export interface Part {
    supplier: Supplier;
    partTotal: string;
    deliveryDate: string;
    partDeliveryFee: string;
    freeShippingFrom: string;
    isCustomerInputAllowed: boolean;
  }
  
  export interface HGOrder {
    parts: Part[];
    total: string;
    customer: Customer;
    grandTotal: string;
    deliveryFee: string;
    sumOfAmounts: number;
    countOfProducts: number;
  }
  