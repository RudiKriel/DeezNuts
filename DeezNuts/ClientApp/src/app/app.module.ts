import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { SharedModule } from './Modules/shared.module';
import { NgxSpinnerModule } from 'ngx-spinner';

import { NavMenuComponent } from './Views/nav-menu/nav-menu.component';
import { HomeComponent } from './Views/home/home.component';
import { RegisterComponent } from './Views/register/register.component';
import { MemberListComponent } from './Views/members/member-list/member-list.component';
import { MemberDetailComponent } from './Views/members/member-detail/member-detail.component';
import { ListsComponent } from './Views/lists/lists.component';
import { MessagesComponent } from './Views/messages/messages.component';
import { TestErrorsComponent } from './Errors/test-errors/test-errors.component';
import { ErrorInterceptor } from './Interceptors/error.interceptor';
import { NotFoundComponent } from './Errors/not-found/not-found.component';
import { ServerErrorComponent } from './Errors/server-error/server-error.component';
import { MemberCardComponent } from './Views/members/member-card/member-card.component';
import { JwtInterceptor } from './Interceptors/jwt.interceptor';
import { MemberEditComponent } from './Views/members/member-edit/member-edit.component';
import { LoadingInterceptor } from './Interceptors/loading.interceptor';
import { PhotoEditorComponent } from './Views/members/photo-editor/photo-editor.component';
import { TextInputComponent } from './Views/text-input/text-input.component';
import { DateInputComponent } from './Views/date-input/date-input.component';
import { MemberMessagesComponent } from './Views/members/member-messages/member-messages.component';
import { AdminPanelComponent } from './Views/admin/admin-panel/admin-panel.component';
import { HasRoleDirective } from './Directives/has-role.directive';
import { UserManagementComponent } from './Views/admin/user-management/user-management.component';
import { PhotoManagementComponent } from './Views/admin/photo-management/photo-management.component';
import { RolesModalComponent } from './Views/Modals/roles-modal/roles-modal.component';
import { ConfirmDialogComponent } from './Views/Modals/confirm-dialog/confirm-dialog.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    RegisterComponent,
    MemberListComponent,
    MemberDetailComponent,
    ListsComponent,
    MessagesComponent,
    TestErrorsComponent,
    NotFoundComponent,
    ServerErrorComponent,
    MemberCardComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    TextInputComponent,
    DateInputComponent,
    MemberMessagesComponent,
    AdminPanelComponent,
    HasRoleDirective,
    UserManagementComponent,
    PhotoManagementComponent,
    RolesModalComponent,
    ConfirmDialogComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    RouterModule,
    ReactiveFormsModule,
    SharedModule,
    NgxSpinnerModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
