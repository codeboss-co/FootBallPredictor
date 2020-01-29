import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { environment } from '../environments/environment';
import { ENV } from './const';
import { TeamsModule } from './features/teams/teams.module';
import { CoreModule } from './shared/core/core.module';

@NgModule( {
    declarations: [
        AppComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,

        // Features
        TeamsModule,

        // System
        CoreModule
    ],
    providers: [
        {provide: ENV, useValue: environment}
    ],
    bootstrap: [AppComponent]
} )
export class AppModule {
}
