import { Component, OnInit } from '@angular/core';
import {PlayerModel} from "../models/playermodel";
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

  constructor() { }

  ngOnInit(): void {
    this.player = <PlayerModel>JSON.parse(<string>localStorage.getItem('playerinfo'));
    this.setupSignalR();
  }

  setupSignalR(): void {
    let self = this;
    this.connection = $.hubConnection(this.hubURL);
    this.gameProxy = this.connection.createHubProxy('gameHub');


    this.gameProxy.on('newPlayer', function(playerList:Array<string>, playersNeeded:number) {
      console.log(playersNeeded);
      self.playersNeeded = playersNeeded;
      self.playerList = playerList;
    });

    this.gameProxy.on('startGame', function(questionList:Array<any>) {
      console.log(questionList);
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
      self.gameProxy.invoke('joinGameRoom', self.player).then((result:any) => {});
    });
  }
}

