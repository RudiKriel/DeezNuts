import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from '../../../Models/member';
import { MembersService } from '../../../Services/members.service';
import { PresenceService } from '../../../Services/presence.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() member!: Member;

  constructor(private memberService: MembersService, private toastr: ToastrService, public presence: PresenceService) { }

  ngOnInit(): void {
  }

  addLike(member: Member) {
    this.memberService.addLike(member.username).subscribe({
      next: () => this.toastr.success(`You have liked ${member.knownAs}`)
    });
  }
}
