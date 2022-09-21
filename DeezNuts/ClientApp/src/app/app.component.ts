import { Component, OnInit } from '@angular/core';
import { User } from './Models/user';
import { AccountService } from './Services/account.service';
import { PresenceService } from './Services/presence.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  title = 'DeezNuts';
  users: any;

  constructor(private accountService: AccountService, private presence: PresenceService) { }

  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const user: User = JSON.parse(localStorage.getItem('user') || '{}');

    if (user && Object.keys(user).length > 0 && user.roles[0]) {
      this.accountService.setCurrentUser(user);
      this.presence.createHubConnection(user);
    }
  }
}
