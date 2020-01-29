import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TeamsHomeComponent } from './components/teams-home/teams-home.component';
import { TeamsSearchContainer } from './containers/teams-search-container/teams-search-container.component';


const routes: Routes = [
    { path: 'teams' , redirectTo: 'home', pathMatch: 'full'},
    {
        path: 'home' , component: TeamsHomeComponent,
        children: [
            { path: '' , redirectTo: 'search', pathMatch: 'full'},
            { path: 'search' , component: TeamsSearchContainer }
        ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TeamsRoutingModule { }
