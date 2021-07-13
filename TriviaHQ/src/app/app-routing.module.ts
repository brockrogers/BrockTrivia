import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {LoginComponent} from "./login/login-component.component";
import {GameboardComponent} from "./gameboard/gameboard.component";

const routes: Routes = [
  { path:'',redirectTo:'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent},
  { path: 'gameboard', component: GameboardComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {useHash:true, anchorScrolling:'enabled'})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
