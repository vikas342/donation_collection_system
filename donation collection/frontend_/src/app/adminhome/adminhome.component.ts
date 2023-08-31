import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { ApiService } from '../services/api.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-adminhome',
  templateUrl: './adminhome.component.html',
  styleUrls: ['./adminhome.component.css'],
})
export class AdminhomeComponent implements OnInit {
  data: any = [];

  donationReq: number[] = [];

  adduser!: FormGroup;

  userid!: number;

  donation!: FormGroup;

  //donationuser details

  donationcollector_user: any[] = [];

  donationuserids: any[] = [];

  //for edit

  username!: string;
  useremail!: string;

  constructor(
    private auth: AuthService,
    private route: Router,
    private api: ApiService,
    private fb: FormBuilder
  ) {}

  logout() {
    this.auth.logout();
    this.route.navigateByUrl('');
  }




  get firstName(){
    return this.adduser.get("firstName")
  }
  get middleName(){
    return this.adduser.get("middleName")
  }

  get lastName(){
    return this.adduser.get("lastName")
  }
  get photo(){
    return this.adduser.get("photo")
  }
  get email(){
    return this.adduser.get("email")
  }
  get flatno(){
    return this.adduser.get("flatno")
  }
  get area(){
    return this.adduser.get("area")
  }
  get state(){
    return this.adduser.get("state")
  } get city(){
    return this.adduser.get("city")
  }
  get pincode(){
    return this.adduser.get("pincode")
  }
  get createddate(){
    return this.adduser.get("createddate")
  }



  ngOnInit() {
    this.api.getuserdetais().subscribe((x: any) => {
      this.data = x;
      console.log(this.data);

    this.api.donationuserlist().subscribe((x) => {
      this.donationuserids = x;
      console.log(this.donationuserids);
    });

    });
    this.adduser = this.fb.group({
      firstName: ['',[Validators.required,]],
      middleName: ['',[Validators.required,]],
      lastName: ['',[Validators.required,]],
      photo: ['',[Validators.required,]],
      email: ['',[Validators.required,Validators.email]],

      flatno: ['',[Validators.required,]],
      area: ['',[Validators.required,]],

      state: ['',[Validators.required,]],

      city: ['',[Validators.required,]],

      pincode: ['',[Validators.required,Validators.pattern('^[0-9]*$'),Validators.maxLength(6)]],

      // password: [''],
      createddate: ['',[Validators.required,]],
    });





    this.donation = this.fb.group({
      UID: [this.userid],
      Month: ['',[Validators.required,Validators.pattern('^([1-9]|1[012])$')]],
      Year: ['',[Validators.required]],
      Amount: ['',[Validators.required]],
      Satus: [2],
    });
  }


  get Month(){
    return this.donation.get("Month")
  }
  get Amount(){
    return this.donation.get("Amount")
  }
  get Year(){
    return this.donation.get("Year")
  }



  delete(x: number) {
    console.log(x);
    this.api.deletedata(x).subscribe((x)=>{

      this.api.getuserdetais().subscribe((x: any) => {
        this.data = x;
        console.log(this.data);
      });
    })
  }


  submit() {
    console.log(this.adduser.value);
    this.auth.register(this.adduser.value).subscribe(
      (x) => {
        this.adduser.reset();
        this.api.getuserdetais().subscribe((x: any) => {
          this.data = x;
          
        });
        alert('user created');
      },
      (error) => {
        console.log(error);
        alert('errror');
      }
    );
  }

  donationREQ() {
    console.warn(this.donation.value);
    var x = this.donation.value.UID;

    //array
    this.donationReq.push(x);
    console.log(this.donationReq);

    this.api.donationRequest(this.donation.value).subscribe(
      (x) => {
        console.log(x);
        alert('Donation request  raised');
      },
      (error) => {
        console.log('errror');
        alert('Donation request already raised');
      }
    );
  }

  //submitting donation
  c_donation(x: number) {
    //console.log(x)
    this.userid = x;
    console.log(this.userid);
    this.donation.get('UID')?.setValue(x);
  }

  //coolecct donations from user
  d_donation(x: number) {
    //console.log(x)
    this.userid = x;
    console.log(this.userid);

    this.api.donationcollector_user(x).subscribe((x) => {
      console.log(x);
      this.donationcollector_user = x;
    });
  }

  //finaldonation colllect from user

  // finalcollect() {
  //   this.api.Collectdonation(this.userid).subscribe((x) => {
  //     console.log(x);
  //   });
  // }






  //editedetails


  edit(index: number) {
    console.log(index);
    this.userid = this.data[index].id;
    this.username = this.data[index].fullName;
    this.useremail = this.data[index].email;
    console.log(this.useremail, this.username, this.userid);
  }

  editdetails() {
    this.api
      .editdetails(this.userid, this.username, this.useremail)
      .subscribe((x) => {
        console.log(x);
        alert('profile updated');
      });

      this.api.getuserdetais().subscribe((x: any) => {
        this.data = x;
        console.log(this.data);
      });
  }
}
