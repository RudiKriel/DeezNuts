import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Member } from '../../../Models/member';
import { NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { NgxGalleryImage } from '@kolkov/ngx-gallery';
import { NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { MessageService } from '../../../Services/message.service';
import { Message } from '../../../Models/message';
import { PresenceService } from '../../../Services/presence.service';
import { MembersService } from '../../../Services/members.service';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../../../Services/account.service';
import { User } from '../../../Models/user';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs', {static: true}) memberTabs!: TabsetComponent;
  member!: Member;
  messages: Message[] = [];
  galleryOptions!: NgxGalleryOptions[];
  galleryImages!: NgxGalleryImage[];
  activeTab!: TabDirective;
  user!: User;

  constructor(public presence: PresenceService, private messageService: MessageService, private memberService: MembersService,
    private toastr: ToastrService, private route: ActivatedRoute, private accountService: AccountService, private router: Router) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user = user
    });

    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    this.route.data.subscribe({
      next: data => this.member = data.member
    });

    this.route.queryParams.subscribe({
      next: params => params.tab ? this.selectTab(params.tab) : this.selectTab(0)
    });

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ];

    this.galleryImages = this.getImages();
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

  getImages(): NgxGalleryImage[] {
    const imageURLs = [];

    for (const photo of this.member.photos) {
      imageURLs.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url
      });
    }

    return imageURLs;
  }

  addLike(member: Member) {
    this.memberService.addLike(member.username).subscribe({
      next: () => this.toastr.success(`You have liked ${member.knownAs}`)
    });
  }

  loadMessages() {
    this.messageService.getMessageThread(this.member.username).subscribe({
      next: messages => this.messages = messages
    });
  }

  selectTab(tabId: number) {
    this.memberTabs.tabs[tabId].active = true;
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;

    if (this.activeTab.heading === 'Messages' && this.messages.length === 0) {
      this.messageService.createHubConnection(this.user, this.member.username);
    } else {
      this.messageService.stopHubConnection();
    }
  }
}
