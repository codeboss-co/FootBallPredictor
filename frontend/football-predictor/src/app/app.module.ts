import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { environment } from '../environments/environment';
import { ENV } from './const';
import { TeamsModule } from './features/teams/teams.module';
import { CoreModule } from './shared/core/core.module';
import { NbMenuModule, NbSidebarModule, NbThemeModule } from '@nebular/theme';

@NgModule( {
    declarations: [
        AppComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,

        // this will enable the default theme, you can change this by passing `{ name: 'dark' }` to enable the dark theme
        NbThemeModule.forRoot({ name: 'dark' }),
        NbSidebarModule.forRoot(),
        NbMenuModule.forRoot(),

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
