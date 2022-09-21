import { Component, OnInit } from '@angular/core';
import { Photo } from '../../../Models/photo';
import { AdminService } from '../../../Services/admin.service';

@Component({
  selector: 'app-photo-management',
  templateUrl: './photo-management.component.html',
  styleUrls: ['./photo-management.component.css']
})
export class PhotoManagementComponent implements OnInit {
  photos: Photo[] = [];

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.getPhotosForApproval();
  }

  getPhotosForApproval() {
    this.adminService.getPhotosForApproval().subscribe({
      next: photos => this.photos = photos
    });
  }

  approvePhoto(id: number) {
    this.adminService.approvePhoto(id).subscribe({
      next: () => this.photos.splice(this.photos.findIndex(p => p.id === id), 1)
    });
  }

  rejectPhoto(id: number) {
    this.adminService.approvePhoto(id).subscribe({
      next: () => this.photos.splice(this.photos.findIndex(p => p.id === id), 1)
    });
  }
}
