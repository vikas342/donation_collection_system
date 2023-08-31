import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AdminhomeComponent } from './adminhome/adminhome.component';
import { UserhomeComponent } from './userhome/userhome.component';
import { AuthgaurdGuard } from './authgaurd.guard';

const routes: Routes = [

  {
    path:'',
    component:HomeComponent,
  },
  {
    path:'login',
    component:HomeComponent,

  },{
    path:'Adminhome',
    component:AdminhomeComponent,
    // canActivate:[AuthgaurdGuard]

  },
  {
    path:'userhome',
    component:UserhomeComponent,
    // canActivate:[AuthgaurdGuard]

  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
