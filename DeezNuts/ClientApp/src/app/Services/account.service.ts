import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { User } from '../Models/user';
import { UserParams } from '../Models/userParams';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  userParams!: UserParams;
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private presence: PresenceService) { }

  login(model: any) {
    return this.http.post<User>(`${this.baseUrl}account/login`, model).pipe(
      map((response: User) => {
        const user = response;

        if (user && Object.keys(user).length > 0) {
          this.setCurrentUser(user);
          this.presence.createHubConnection(user);
          this.userParams = new UserParams(user);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post<User>(`${this.baseUrl}account/register`, model).pipe(
      map((user: User) => {
        if (user && Object.keys(user).length > 0) {
          this.setCurrentUser(user);
          this.presence.createHubConnection(user);
        }

        return user;
      })
    );
  }

  setCurrentUser(user: User) {
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;

    if (roles !== '') {
      Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);

      localStorage.setItem('user', JSON.stringify(user));
      this.currentUserSource.next(user);
    }
  }

  resetUserParams() {
    return this.currentUser$.pipe(
      map((user: User) => {
        return new UserParams(user);
      })
    );
  }

  logout() {
    localStorage.removeItem('user');

    this.currentUserSource.next(null!);
    this.presence.stopHubConnection();
  }

  getDecodedToken(token: string) {
    return token ? JSON.parse(atob(token.split('.')[1])) : '';
  }
}
