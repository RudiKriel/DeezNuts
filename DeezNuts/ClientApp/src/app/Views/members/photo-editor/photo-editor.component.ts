import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { Member } from '../../../Models/member';
import { Photo } from '../../../Models/photo';
import { User } from '../../../Models/user';
import { AccountService } from '../../../Services/account.service';
import { MembersService } from '../../../Services/members.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() member!: Member;
  uploader!: FileUploader;
  hasBaseDropzoneOver = false;
  baseUrl = environment.apiUrl;
  user!: User;

  constructor(private accountService: AccountService, private memberService: MembersService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user = user
    });
  }

  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e: any) {
    this.hasBaseDropzoneOver = e;
  }

  setMainPhoto(photo: Photo) {
    this.memberService.setMainPhoto(photo.id).subscribe({
      next: user => {
        this.user.photoUrl = photo.url;
        this.accountService.setCurrentUser(this.user);
        this.member.photoUrl = photo.url;

        this.member.photos.forEach(p => {
          if (p.isMain) {
            p.isMain = false;
          }

          if (p.id === photo.id) {
            p.isMain = true;
          }
        });
      }
    });
  }

  deletePhoto(photoId: number) {
    this.memberService.deletePhoto(photoId).subscribe({
      next: () => {
        this.member.photos = this.member.photos.filter(p => p.id !== photoId);
      }
    });
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.user.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      const photo: Photo = JSON.parse(response);
      this.member.photos.push(photo);

      if (photo.isMain) {
        this.user.photoUrl = photo.url;
        this.member.photoUrl = photo.url;
        this.accountService.setCurrentUser(this.user);
      }
    }
  }
}
