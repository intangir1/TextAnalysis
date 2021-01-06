import { NgModule } from '@angular/core';
import { DatePipe } from '@angular/common'
import { HttpClientModule } from '@angular/common/http';
import { ImageCropperModule } from 'ngx-image-cropper';
import { CookieService } from 'ngx-cookie-service';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { CommonModule } from '@angular/common';
import { RoutingModule } from './modules/routing.module';
import { Store } from './redux/store';
import { Reducer } from './redux/reducer';
import { NgReduxModule, NgRedux } from 'ng2-redux';

import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatIconModule } from '@angular/material/icon';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input'
import { MatSelectModule } from '@angular/material/select';
import { MatSortModule } from '@angular/material/sort';
import { MatDatepickerModule } from '@angular/material/datepicker'
import { MatNativeDateModule } from '@angular/material/core';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip'
import { MatMenuModule } from '@angular/material/menu';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatRadioModule } from '@angular/material/radio'
import { MatSliderModule } from '@angular/material/slider';


import { DragDropModule } from '@angular/cdk/drag-drop';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { LayoutComponent } from './components/design-elements/layout/layout.component';
import { FooterComponent } from './components/design-elements/footer/footer.component';
import { HeaderComponent } from './components/design-elements/header/header.component';
import { SignInComponent } from './components/user-elements/sign-in/sign-in.component';
import { SignUpComponent } from './components/user-elements/sign-up/sign-up.component';
import { CheckTextComponent } from './components/data-elements/check-text/check-text.component';
import { AddWordsComponent } from './components/data-elements/add-words/add-words.component';
import { AdminAddWordsComponent } from './components/data-elements/admin-add-words/admin-add-words.component';
import { UserStatisticsComponent } from './components/data-elements/user-statistics/user-statistics.component';
import { ControlMessagesComponent } from './components/data-elements/ControlMessagesComponent';


@NgModule({
  declarations: [
    LayoutComponent,
    FooterComponent,
    HeaderComponent,
    SignInComponent,
    SignUpComponent,
    CheckTextComponent,
    AddWordsComponent,
    AdminAddWordsComponent,
    UserStatisticsComponent,
    ControlMessagesComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    HttpClientModule,
    NgReduxModule,
    CommonModule,
    RoutingModule,
    ImageCropperModule,
    BrowserAnimationsModule,
    DragDropModule,

    MatFormFieldModule,
    MatButtonToggleModule,
    MatIconModule,
    MatExpansionModule,
    MatButtonModule,
    MatInputModule,
    MatSelectModule,
    MatSortModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTableModule,
    MatTooltipModule,
    MatMenuModule,
    MatRadioModule,
    MatSliderModule
  ],
  providers: [CookieService, DatePipe],
  bootstrap: [LayoutComponent]
})
export class AppModule { 
  public constructor(redux:NgRedux<Store>){
    redux.configureStore(Reducer.reduce, new Store());
  }
}