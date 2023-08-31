import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {


  role:string='';

  iaAdmin(){
    if(this.role=='admin'){
      return true;
    }
    else {
      return false;
    }
  }

  constructor(private http:HttpClient) { }


  login(formdata:any){
    return this.http.post('https://localhost:7259/api/AuthCotroller/login',formdata)
  }


  register(formdata:any){
    return this.http.post('https://localhost:7259/api/AuthCotroller/register',formdata)



  }
  savetoken(tkn:any){

    var x=tkn.token;
    console.log(x.value)
    console.log(tkn.role)
    this.role=tkn.role;

    localStorage.setItem('token',x.value)
    localStorage.setItem('role',tkn.role)


  }

  gettoken(){
  return localStorage.getItem('token');

  }


  logout(){
    localStorage.clear();
  }
}
