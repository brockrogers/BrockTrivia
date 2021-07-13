import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {LoginComponent} from "./login-component/login-component.component";

const routes: Routes = [
  { path:'',redirectTo:'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {useHash:true, anchorScrolling:'enabled'})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
