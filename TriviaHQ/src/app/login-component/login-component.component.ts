import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-login-component',
  templateUrl: './login-component.component.html',
  styleUrls: ['./login-component.component.css']
})
export class LoginComponent implements OnInit {
  newPlayerName = "";
  constructor(httpClient:HttpClient) { }

  ngOnInit(): void {

  }

  async createNewPlayer() {
      
  }
}
