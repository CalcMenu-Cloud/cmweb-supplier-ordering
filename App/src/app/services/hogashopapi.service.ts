import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.prod';
import { Department } from '../interfaces/hgdepartment.interface';

@Injectable({
  providedIn: 'root'
})
export class HogashopapiService {
  private apiUrl = environment.api_url;
  constructor(private http: HttpClient) { }

  getHGDepartment(): Observable<Department> {
    return this.http.get<Department>(`${this.apiUrl}/HGShopAPI/GetDapartment`);
  }
  




}
