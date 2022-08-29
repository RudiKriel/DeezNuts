import { Component, OnInit } from '@angular/core';
import { Member } from '../../../Models/member';
import { Pagination } from '../../../Models/pagination';
import { UserParams } from '../../../Models/userParams';
import { MembersService } from '../../../Services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members!: Member[];
  pagination!: Pagination;
  userParams!: UserParams;
  genderList = [
    { value: 'male', display: 'Males' },
    { value: 'female', display: 'Females' }
  ];

  constructor(private memberService: MembersService) {
    this.userParams = this.memberService.getUserParams();
  }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.setUserParams(this.userParams);

    this.memberService.getMembers(this.userParams).subscribe({
      next: response => {
        this.members = response.result;
        this.pagination = response.pagination;
      }
    });
  }

  resetFilters() {
    this.userParams = this.memberService.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: any) {
    this.userParams.pageNumber = event.page;

    this.memberService.setUserParams(this.userParams);
    this.loadMembers();
  }
}
