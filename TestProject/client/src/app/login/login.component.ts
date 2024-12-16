import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  email:string = '';
  parola:string = '';
  rezultat:string = '';
  constructor(public accountService: AccountService, private toastr: ToastrService, private router: Router) { }

  ngOnInit(): void {
  }
  
  login(){
    this.accountService.login(this.email, this.parola).subscribe(() =>
    {
      this.toastr.success("Succes");
      this.router.navigate(['/']);
    }
    );
  }
}
