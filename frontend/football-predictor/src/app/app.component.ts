import { Component, Inject } from '@angular/core';
import { ENV } from './const';
import { NbMenuItem } from '@nebular/theme';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'football-predictor';

  constructor(@Inject(ENV) public environment) {}

    menu: NbMenuItem[]  = [
        {
            title: 'Teams',
            icon: 'people-outline',
            link: '/teams/home',
            home: true,
        }
    ];

}
