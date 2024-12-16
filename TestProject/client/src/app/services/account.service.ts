import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { IUser } from '../models/user';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<IUser | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }
  login(email: string, parola: string){
    return this.http.post<IUser>(this.baseUrl + "account/login",{Email: email, Password: parola}).pipe(
      map((response: IUser) => {
        const user = response;
        if(user)
        {
          localStorage.setItem('userDistributedMachine', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  register(email: string, parola: string, nume: string){
    return this.http.post<IUser>(this.baseUrl + "account/register",{Email: email, Password: parola, Nume: nume}).pipe(
      map((response: IUser) => {
        const user = response;
        if(user)
        {
          localStorage.setItem('userDistributedMachine', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  logOut(){
    localStorage.removeItem('userDistributedMachine');
    this.currentUserSource.next(null as any);
    this.router.navigateByUrl('/');
  }    
}
