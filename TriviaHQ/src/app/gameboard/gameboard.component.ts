import { Component, OnInit } from '@angular/core';
import {PlayerModel} from "../models/playermodel";
import {HttpClient, HttpHeaders} from "@angular/common/http";
declare var $: any; // This is Jquery which is brought in by a script tag in index.html

@Component({
  selector: 'app-gameboard',
  templateUrl: './gameboard.component.html',
  styleUrls: ['./gameboard.component.css']
})
export class GameboardComponent implements OnInit {
  connection:any;
  hubURL = 'http://triviaservice-dev.us-west-2.elasticbeanstalk.com/signalr/hubs';
  gameProxy:any = null;
  player:any = null;
  playerList:Array<string> = new Array<string>();
  playersNeeded = 0;
  questionList:Array<any> = new Array<any>();
  currentQuestion = 0;
  gameStarted = false;
  gameOver = false;
  currentAnswer = 'A';
  baseURL = 'http://triviaservice-dev.us-west-2.elasticbeanstalk.com/api/Trivia/';
  isAnswering = false;
  invalidAnswer = false;

  constructor(private httpClient:HttpClient) { }

  ngOnInit(): void {
    this.player = <PlayerModel>JSON.parse(<string>localStorage.getItem('playerinfo'));
    this.setupSignalR();
  }

  setupSignalR(): void {
    let self = this;
    this.connection = $.hubConnection(this.hubURL);
    this.gameProxy = this.connection.createHubProxy('gameHub');


    this.gameProxy.on('newPlayer', function(playerList:Array<string>, playersNeeded:number) {
      self.playersNeeded = playersNeeded;
      self.playerList = playerList;
    });

    this.gameProxy.on('startGame', function(questionList:Array<any>) {
      self.questionList = questionList;
      self.gameStarted = true;
    });

    let tryingToReconnect = false;

    this.connection.reconnecting(function () {
      console.log('TRYING TO RECONNECT');
      tryingToReconnect = true;
    });
    this.connection.reconnected(function () {
      console.log('RECONNECTED');
      tryingToReconnect = false;
    });
    this.connection.disconnected(function () {
      if (tryingToReconnect) {
        console.log('RECONNECTING');
      }
    });

    self.setUpJoinRoom();
  }

  setUpJoinRoom() {
    let self = this;
    this.connection.start( {jsonp: true}).done(function() {
      self.gameProxy.invoke('joinGameRoom', self.player).then((result:any) => {
        self.playersNeeded = result.PlayersNeeded;
        self.playerList = result.Players;
      });
    });
  }

  getPlayerHeader() {
    let headerText = "Get ready " + this.player.PlayerName;

    if(this.gameStarted)
      headerText = "Good luck " + this.player.PlayerName + "!";
    else if(this.gameOver) {
      if(this.invalidAnswer)
        headerText = "Thanks for playing " + this.player.PlayerName + "!  Unfortunately you did not get the right answer.";
      else
        headerText = "Way to go " + this.player.PlayerName + "!  You got all answers correct!";
    }
    else if(this.playersNeeded > 0)
      headerText += " the game will start once " + this.playersNeeded + " more players join the game!"
    else
      headerText += " the game is about to begin!";

    return headerText;
  }

  async submitAnswer():Promise<void> {
    this.isAnswering = true;
    const answer = {QuestionId:this.questionList[this.currentQuestion].Id, answer:this.currentAnswer, PlayerId:this.player.PlayerId};

    try {
      const headers = new HttpHeaders().append('Content-Type', 'application/json');
      const validAnswer = await this.httpClient.post<boolean>(this.baseURL + 'Answer', answer,
        {headers:headers}).toPromise();
      if(!validAnswer) {
        this.currentQuestion = this.questionList.length;
        this.invalidAnswer = true;
      }

    }
    catch(error) {
      this.gameStarted = false;
      this.gameOver = true;
      this.invalidAnswer = true;
    }

    this.isAnswering = false;

    if(this.currentQuestion + 1 >= this.questionList.length) {
      this.gameStarted = false;
      this.gameOver = true;
    }
    else
      this.currentQuestion++;

    this.currentAnswer = 'A';
  }
}

