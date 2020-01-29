import { Component, Input, OnInit } from '@angular/core';

@Component( {
    selector: 'app-teams-list',
    templateUrl: './teams-list.component.html',
    styleUrls: ['./teams-list.component.scss']
} )
export class TeamsListComponent implements OnInit {

    @Input() list: any[] = [];

    constructor() {
    }

    ngOnInit() {
    }

}
