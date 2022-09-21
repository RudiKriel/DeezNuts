import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Member } from '../../../Models/member';
import { Pagination } from '../../../Models/pagination';
import { UserParams } from '../../../Models/userParams';
import { AccountService } from '../../../Services/account.service';
import { MembersService } from '../../../Services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members$!: Observable<Member[]>;
  pagination!: Pagination;
  userParams!: UserParams;
  genderList = [
    { value: 'male', display: 'Males' },
    { value: 'female', display: 'Females' }
  ];

  constructor(private memberService: MembersService, private accountService: AccountService) {
    this.accountService.resetUserParams().subscribe({
      next: params => {
        this.userParams = params;
      }
    });
  }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.setUserParams(this.userParams);

    this.memberService.getMembers(this.userParams).subscribe({
      next: response => {
        this.members$ = of(response.result);
        this.pagination = response.pagination;
      }
    });
  }

  resetFilters() {
    this.accountService.resetUserParams().subscribe({
      next: params => {
        this.userParams = params;
      }
    });
  }

  pageChanged(event: any) {
    this.userParams.pageNumber = event.page;
    this.loadMembers();
  }
}
