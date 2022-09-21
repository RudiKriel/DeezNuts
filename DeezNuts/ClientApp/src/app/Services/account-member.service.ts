import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AccountService } from './account.service';
import { MembersService } from './members.service';

@Injectable({
  providedIn: 'root'
})
export class AccountMemberService {
  constructor(private http: HttpClient, private accountService: AccountService, private memberService: MembersService) {}

  logout() {
    if (this.memberService.memberCache) {
      this.memberService.memberCache.clear();
    }
    
    this.accountService.logout();
  }
}
