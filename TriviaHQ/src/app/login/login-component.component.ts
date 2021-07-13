import { Component, OnInit } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {PlayerModel} from "../models/playermodel";
import {Router} from "@angular/router";

@Component({
  selector: 'app-login-component',
  templateUrl: './login-component.component.html',
  styleUrls: ['./login-component.component.css']
})
export class LoginComponent implements OnInit {
  newPlayerName = '';
  baseURL = 'http://triviaservice-dev.us-west-2.elasticbeanstalk.com/api/Trivia/';
  errorText = '';
  isLoggingIn = false;

  constructor(private httpClient:HttpClient, private router:Router) { }

  ngOnInit(): void {
    localStorage.clear();
  }

  async createNewPlayer() {
    this.isLoggingIn = true;
    try {
      const headers = new HttpHeaders().append('Content-Type', 'application/json');
      var newPlayer = await this.httpClient.post<PlayerModel>(this.baseURL + 'NewPlayer', JSON.stringify(this.newPlayerName),
        {headers:headers}).toPromise();
      localStorage.setItem('playerinfo', JSON.stringify(newPlayer));
      this.isLoggingIn = false;
      this.router.navigate(['gameboard']);
    }
    catch(error) {
      this.errorText = "We're sorry, an error has occurred processing your request.  Please try again.";
      this.isLoggingIn = false;
    }
  }
}
