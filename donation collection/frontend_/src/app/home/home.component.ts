import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl ,Validator, Validators} from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  data: any = [];

  loginform!: FormGroup;

  constructor(private fb: FormBuilder,private auth:AuthService ,private route:Router) {}

  ngOnInit(): void {
    this.loginform = this.fb.group({
      email: ['',[Validators.email,Validators.required]],
      password: ['',[Validators.required,Validators.maxLength(10)]],
    });
  }


  get email(){
    return this.loginform.get("email")
  }
  get password(){
    return this.loginform.get("password")
  }

  submit(){
    console.log(this.loginform.value)
    this.auth.login(this.loginform.value).subscribe(x=>{

      console.log(x);
      this.auth.savetoken(x);
      if(this.auth.role=="admin"){
        this.route.navigate(['/Adminhome']);
      }
      else{
        this.route.navigate(['/userhome']);

      }
    },
    (error)=>{
      console.log(error)
      alert("invalid username or password")
    }
    )
  }



}
