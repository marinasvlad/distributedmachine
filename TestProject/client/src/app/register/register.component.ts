import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  email:string = '';
  parola:string = '';
  nume:string = '';
  rezultat:string = '';
  constructor(private accountService: AccountService, private toastr: ToastrService, private router: Router) { }

  ngOnInit(): void {
  }
  register(){
    this.accountService.register(this.email, this.parola, this.nume).subscribe(() =>
    {
      this.toastr.success("Succes");
      this.router.navigate(['/']);
        }
    );
  }
}
