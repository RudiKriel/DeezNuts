import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../Services/account.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../Models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private accountService: AccountService, private toastr: ToastrService) {}

  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map((user: User) => {
        console.log(user);
        if (user && Object.keys(user).length > 0) {
          return true;
        }

        this.toastr.error('You shall not pass!');

        return false;
      })
    );
  }
  
}
