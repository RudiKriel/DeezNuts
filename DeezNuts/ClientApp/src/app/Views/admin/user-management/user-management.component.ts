import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { User } from '../../../Models/user';
import { AdminService } from '../../../Services/admin.service';
import { RolesModalComponent } from '../../Modals/roles-modal/roles-modal.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  users: Partial<User[]> = [];
  bsModalRef!: BsModalRef;

  constructor(private adminService: AdminService, private modalService: BsModalService) { }

  ngOnInit(): void {
    this.getUserWithRoles();
  }

  getUserWithRoles() {
    this.adminService.getUsersWithRoles().subscribe({
      next: users => {
        this.users = users;
      }
    });
  }

  openRolesModal(user: User) {
    const config = {
      class: 'modal-dialog-centered',
      initialState: {
        user,
        roles: this.getRoles(user)
      }
    };

    this.bsModalRef = this.modalService.show(RolesModalComponent, config);

    this.bsModalRef.content.updateSelectedRoles.subscribe({
      next: (values: any[]) => {
        const rolesToUpdate = {
          roles: [...values.filter(el => el.checked === true).map(el => el.name)]
        };

        if (rolesToUpdate) {
          this.adminService.updateUserRoles(user.username, rolesToUpdate.roles).subscribe({
            next: () => user.roles = [...rolesToUpdate.roles]
          });
        }
      }
    });
  }

  private getRoles(user: User) {
    const roles: any = [];
    const userRoles = user.roles;
    const availableRoles: any[] = [
      { name: 'Admin', value: 'Admin' },
      { name: 'Moderator', value: 'Moderator' },
      { name: 'Member', value: 'Member' }
    ];

    availableRoles.forEach(role => {
      let isMatch = false;

      for (const userRole of userRoles) {
        if (role.name === userRole) {
          isMatch = true;
          role.checked = true;
          roles.push(role);

          break;
        }
      }

      if (!isMatch) {
        role.checked = false;
        roles.push(role);
      }
    });

    return roles;
  }
}
