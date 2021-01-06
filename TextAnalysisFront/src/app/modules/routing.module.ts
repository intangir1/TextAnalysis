import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from "@angular/router";

import { CheckTextComponent } from '../components/data-elements/check-text/check-text.component';
import { AddWordsComponent } from '../components/data-elements/add-words/add-words.component';
import { AdminAddWordsComponent } from '../components/data-elements/admin-add-words/admin-add-words.component';
import { UserStatisticsComponent } from '../components/data-elements/user-statistics/user-statistics.component';

import { LoginGuardService } from '../services/Guard/login-guard.service';
import { AdminGuardService } from '../services/Guard/admin-guard.service';
// ng g m modules/routing --spec false --flat

const routes: Routes = [
    { path: "", component: CheckTextComponent },
    { path: "add_words", canActivate: [LoginGuardService], component: AddWordsComponent },
    { path: "add_words_admin", canActivate: [AdminGuardService], component: AdminAddWordsComponent },
    { path: "user_statistics", canActivate: [AdminGuardService], component: UserStatisticsComponent },
    { path: "home", redirectTo: "", pathMatch: "full" }
];

@NgModule({
    declarations: [],
    imports: [CommonModule, RouterModule.forRoot(routes)]
})
export class RoutingModule { }
