import { Component, Inject } from '@angular/core';
import { ENV } from './const';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'football-predictor';

  constructor(@Inject( ENV ) public environment) {}

}
