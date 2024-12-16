import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { AccountService } from './services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'client';
  email:string = '';
  parola:string = '';
  rezultat:string = '';

  constructor(public accountService: AccountService, private toastr: ToastrService){

  }

  login(){
    this.accountService.login(this.email, this.parola).subscribe(() =>
    {
      this.toastr.success("Succes");
    }
    );
  }
}
