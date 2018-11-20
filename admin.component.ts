import {Component, OnDestroy, OnInit} from '@angular/core';
import {DataService} from "../provider/data.service";
import {Router} from '@angular/router';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit, OnDestroy {

  id: string;
  name: string;
  permission: string;
  countAllCoil: string;
  countAvgWeekCoil: string;
  countEmptyCell: string;
  countBusyCell: string;
  countAllUsers: string;
  countWorker: string;

  rowOne: string;
  rowTwo: string;
  rowThree: string;
  rowFour: string;
  rowFive: string;

  rowProOne: string;
  rowProTwo: string;
  rowProThree: string;
  rowProFour: string;
  rowProFive: string;

  constructor(private data: DataService, private router: Router) {

    this.id = sessionStorage.getItem('id');
    this.name = sessionStorage.getItem('name');
    this.permission = sessionStorage.getItem('permission');
    if (!this.id || this.permission == '2' || this.permission == '0') {
      this.router.navigate([""]);
    }
  }

  ngOnInit() {


    let body = document.getElementsByTagName('body')[0];
    body.classList.add('nav-md');


    this.data.countStatistic()
      .subscribe(data => {
          console.log(data);
          this.countAllCoil = data.message[0];
          this.countAvgWeekCoil = data.message[1];
          this.countEmptyCell = data.message[2];
          this.countBusyCell = data.message[3];
          this.countAllUsers = data.message[4];
          this.countWorker = data.message[5];
        },
        error => {
          console.log(error.error.text);
          if (error.error.text === undefined) {
            alert('There is no database connection');
          }
        });

    this.data.rowStatistic()
      .subscribe(data => {
          console.log(data);
          this.rowOne = data.message[0];
          this.rowTwo = data.message[1];
          this.rowThree = data.message[2];
          this.rowFour = data.message[3];
          this.rowFive = data.message[4];

        },
        error => {
          console.log(error.error.text);
          if (error.error.text === undefined) {
            alert('There is no database connection');
          }
        });

    this.data.rowProStatistic()
      .subscribe(data => {

          this.rowProOne = data.message[0];
          this.rowProTwo = data.message[1];
          this.rowProThree = data.message[2];
          this.rowProFour = data.message[3];
          this.rowProFive = data.message[4];

        },
        error => {
          console.log(error.error.text);
          if (error.error.text === undefined) {
            alert('There is no database connection');
          }
        });

  }

  ngOnDestroy() {
    let body = document.getElementsByTagName('body')[0];
    body.classList.remove("nav-md");
  }

  moderator(){
    this.router.navigate(["/moderator"]);
  }

  logout() {
    sessionStorage.clear();
    this.router.navigate([""]);
  }
}
