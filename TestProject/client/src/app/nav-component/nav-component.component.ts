import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-nav-component',
  templateUrl: './nav-component.component.html',
  styleUrls: ['./nav-component.component.css']
})
export class NavComponentComponent implements OnInit {

  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
  }

  logout(){
    this.accountService.logOut();
  }

}
