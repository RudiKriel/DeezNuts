import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountMemberService } from '../../Services/account-member.service';
import { AccountService } from '../../Services/account.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  model: any = {};

  constructor(public accountService: AccountService, private accountMemberService: AccountMemberService, private router: Router) { }

  ngOnInit() {}

  login() {
    this.accountService.login(this.model).subscribe({
      next: response => {
        this.router.navigateByUrl('/members');
      }
    });
  }

  logout() {
    this.accountMemberService.logout();
    this.router.navigateByUrl('/');
  }
}
