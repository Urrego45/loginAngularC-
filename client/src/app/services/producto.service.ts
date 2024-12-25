import { inject, Injectable } from '@angular/core';
import { appSettings } from '../settings/appSettings';
import { HttpClient } from '@angular/common/http';
import { ResponseProducto } from '../interfaces/ResponseProducto';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductoService {

    private http = inject(HttpClient);
    private baseUrl: string = appSettings.apiUrl;

  constructor() { }

  lista(): Observable<ResponseProducto>{
    return this.http.get<ResponseProducto>(`${this.baseUrl}Producto/Lista`)
  }
}
