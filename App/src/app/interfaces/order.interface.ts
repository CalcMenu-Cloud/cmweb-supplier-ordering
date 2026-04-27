

  export class SNOrder {
    id: number;
    name: string;
    supplierType: number;
    clientId: number;
    codeUser: number;
    dateCreated: string;
    modifiedDate: string;
    datePosted: string;
    deliveryDate: string;
    terms: string;
    note: string;
    status: string;
    departmentId:string;
    orderDetails: SNOrderDetails[];
  }

  export class SNOrderList {
    orders: SNOrder[];
  }
  
  export class SNOrderDetails {
    id: number;
    orderId: number;
    codeliste: number;
    number: string;
    name: string;
    supplier: string;
    category: string;
    quantity: number;
    unit: string;
    price: number;
    deliveryDate: string;

    basePrice: number;
    baseUnit: string;
    sellingPrice: number;
    sellingUnit: string;
    ratio1: number;
    productId: string;
    picturePath: string;
  
  }
  
 