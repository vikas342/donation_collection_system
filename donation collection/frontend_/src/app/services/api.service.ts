import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiService {



  constructor(private http:HttpClient) { }


  //admin screen
  getuserdetais(){
    return this.http.get<any>("https://localhost:7259/api/Admin/getuserdetails");
  }


  deletedata(x:number){
    return this.http.delete<any>("https://localhost:7259/api/Admin/"+x);
  }

  donationRequest(formdata:any){
    return this.http.post("https://localhost:7259/api/Admin/donationrequest",formdata);
  }

  donationuserlist(){
    return this.http.get<any>("https://localhost:7259/api/Admin/donationuserlist");
  }


//edit user details

editdetails(id:number,fullname:string,email:string){

  return this.http.put<any>("https://localhost:7259/api/Admin/editprofile?id="+id+"&fullname="+fullname+"&email="+email,null);

}



//Collectdonation user

  // Collectdonation(x:number){
  //   return this.http.put<any>("https://localhost:7259/api/Admin/collectdonation/"+x,null);
  // }

//donation user details

  donationcollector_user(x:number){
    return this.http.get<any>("https://localhost:7259/api/Admin/paymentcollect/"+x);
  }









  //for user side


  userdetails(){

    return this.http.get<any>("https://localhost:7259/api/user/getnormaluserdetails");


  }


  userPaymentdetails(){

    return this.http.get<any>("https://localhost:7259/api/user/userpaymentdetails");


  }


  otpgenrator(){
    return this.http.get<any>("https://localhost:7259/api/user/userotp");
  }





  Finalpayment(id:number,month:number,year:number){

  // https://localhost:7259/api/user/payment/4?month=56&year=656757
  // https://localhost:7259/api/user/payment?id=3&month=12&year=2023
    return this.http.put<any>("https://localhost:7259/api/user/payment?id="+id+"&month="+month+"&year="+year,null);
    }

}
