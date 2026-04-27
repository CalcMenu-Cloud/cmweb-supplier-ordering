import { Injectable ,OnInit} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GlobalvarService implements OnInit {


  Islogin: any;

  constructor() { }


  ngOnInit(): void {
   
    this.Islogin=0;
    this.checkSession();
    console.log('Session is login ... ');
 
   }


   checkSession()
   {
    console.log('check my session');
    // Reading session variable
    const mySessionVariable = localStorage.getItem('sessionkey');
    console.log(mySessionVariable); // Output: value

    if (mySessionVariable !== null) {
      this.Islogin=1;
      return  this.Islogin;
      // If mySessionVariable is not null, it contains a value
      
    } else {
      // If mySessionVariable is null, it does not contain a value
      this.Islogin=0;

      return  this.Islogin;
    }
   }

  getSession()
   {

    return localStorage.getItem('sessionkey') ?? '';
   }


   writeSession(sessionkey)
   {
    localStorage.setItem('sessionkey', sessionkey);
   }

   writeuserinfojson(storagekey,userinfo)
   {
    var jsonString = JSON.stringify(userinfo);
    localStorage.setItem("userinfo", jsonString);
   }

  getuserinfojson(keyvalue)
   {

    var jsonString =  localStorage.getItem('userinfo') ?? '';

if(jsonString=='')
{
return null;
}

    var parsedData = JSON.parse(jsonString);

    const searchByKey = (jsonData, key) => {
      // Check if the key exists in the JSON object
      if (jsonData.hasOwnProperty(key)) {
          const value = jsonData[key];
          // Check if the value is not empty (or null/undefined)
          if (value !== "" && value !== null && value !== undefined) {
              return value;
          }
      }
      // Return a default value if the key doesn't exist or the value is empty
      return null; // You can change this default value as per your requirement
  };

    // Example usage:
   return searchByKey(parsedData, keyvalue)??"";


   }


   
   clearSession()
   {
    // Clear session storage
    sessionStorage.clear();

    // Clear local storage
    localStorage.clear();
   }


   getSecretKey()
   {
 
    return localStorage.getItem('gensecretkey') ?? '';
   }


   setSecretKey()
   {
    const mykey=this.generateSecretKey(32);
    localStorage.setItem('gensecretkey', mykey);
    
    return localStorage.getItem('gensecretkey');
   }


   generateSecretKey(length: number): string {
    const charset = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let result = '';
    for (let i = 0; i < length; i++) {
      const randomIndex = Math.floor(Math.random() * charset.length);
      result += charset[randomIndex];
    }
    return result;
  }


}
