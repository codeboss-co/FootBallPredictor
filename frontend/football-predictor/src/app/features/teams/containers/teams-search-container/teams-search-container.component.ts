import { Component, OnInit } from '@angular/core';
import { TeamsApiService } from '../../services/teams-api.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-teams-search-container',
  templateUrl: './teams-search-container.component.html',
  styleUrls: ['./teams-search-container.component.scss']
})
export class TeamsSearchContainer implements OnInit {

    public teams$: Observable<any>;

  constructor(private api: TeamsApiService) { }

  ngOnInit() {
      this.teams$ = this.api.get$();
  }

}
