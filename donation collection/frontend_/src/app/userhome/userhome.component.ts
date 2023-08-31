import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api.service';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-userhome',
  templateUrl: './userhome.component.html',
  styleUrls: ['./userhome.component.css']
})
export class UserhomeComponent implements OnInit{

  constructor(private api:ApiService,private auth:AuthService,private route:Router){}

  userdetails:any[]=[]

  userpaymentdetais:any[]=[]

  otp!:number;

  enteredOtp!:number;



  userid!:number;
  year!:number;
  month!:any;
  amount!:number;


ngOnInit(): void {

  this.api.userdetails().subscribe(x=>{
    console.log(x);
    this.userdetails=x;

  })



  this.api.userPaymentdetails().subscribe(x=>{
    console.log(x);
    this.userpaymentdetais=x;

  })





}



logout() {
  this.auth.logout();
  this.route.navigateByUrl('');
}




optgenrator(index:number){

  this.api.otpgenrator().subscribe(x=>{
    console.log(x);
    this.otp=x;

  })

  this.userid=this.userdetails[0].id;
  this.month=this.userpaymentdetais[index].month;
  this.year=this.userpaymentdetais[index].year;
  this.amount=this.userpaymentdetais[index].amount;


  console.log(this.month,this.year,this.amount)

}

payment(){
 // alert(this.enteredOtp);
  if(this.enteredOtp==this.otp){
this.api.Finalpayment(this.userid,this.month,this.year).subscribe(x=>{
  console.log(x);
  alert("payment succesfully");
  this.api.userPaymentdetails().subscribe(x=>{
    console.log(x);
    this.userpaymentdetais=x;

  })

})
  }
  else{
    alert("payment failed");
  }





}



}
