import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/User';

@Injectable({
  providedIn: 'root',
})
export class UserService {

  private url = 'http://localhost:5005/api/user'

  constructor(private http: HttpClient){}

  public LoginUsuario(user : User){
    return this.http.post(`${this.url}/login`,user)
  }

}
