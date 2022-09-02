import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { take } from 'rxjs/operators';
import { User } from '../Models/user';
import { AccountService } from '../Services/account.service';

@Directive({
  selector: '[appHasRole]' //*appHasRole='["Admin"]'
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[] = [];
  user!: User;

  constructor(private viewContrainerRef: ViewContainerRef, private templateRef: TemplateRef<any>, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user = user
    });
  }
    ngOnInit(): void {
      //clear view if no roles
      if (this.user == null || !this.user?.roles) {
        this.viewContrainerRef.clear();

        return;
      }

      if (this.user?.roles.some(r => this.appHasRole.includes(r))) {
        this.viewContrainerRef.createEmbeddedView(this.templateRef);
      } else {
        this.viewContrainerRef.clear();
      }
    }
}
