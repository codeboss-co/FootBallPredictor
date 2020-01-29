import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TeamsRoutingModule } from './teams-routing.module';
import { TeamsHomeComponent } from './components/teams-home/teams-home.component';
import { TeamsSearchContainer } from './containers/teams-search-container/teams-search-container.component';
import { TeamsListComponent } from './components/teams-list/teams-list.component';


@NgModule({
  declarations: [
      TeamsHomeComponent,
      TeamsSearchContainer,
      TeamsListComponent
  ],
  imports: [
    CommonModule,
    TeamsRoutingModule
  ]
})
export class TeamsModule { }
