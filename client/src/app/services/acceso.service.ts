import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { appSettings } from '../settings/appSettings';
import { Usuario } from '../interfaces/Usuario';
import { Observable } from 'rxjs';
import { ResponseAcceso } from '../interfaces/ResponseAcceso';
import { Login } from '../interfaces/Login';

@Injectable({
  providedIn: 'root'
})
export class AccesoService {

  private http = inject(HttpClient);
  private baseUrl: string = appSettings.apiUrl;

  constructor() { }

  registrarse(objeto:Usuario): Observable<ResponseAcceso>{
    return this.http.post<ResponseAcceso>(`${this.baseUrl}Acceso/Registrarse`, objeto)
  }

  login(objeto:Login): Observable<ResponseAcceso>{
    return this.http.post<ResponseAcceso>(`${this.baseUrl}Acceso/Login`, objeto)
  }

  validarToken(token: string): Observable<ResponseAcceso> {
    return this.http.get<ResponseAcceso>(`${this.baseUrl}Acceso/ValidarToken?token=${token}`)
}
}
