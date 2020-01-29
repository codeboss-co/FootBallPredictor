import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { httpInterceptorProviders } from '../../interceptors';

const MODULES = [
    // Angular Modules
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
];

@NgModule( {
    declarations: [],
    imports: [
        CommonModule,

        MODULES
    ],
    providers: [httpInterceptorProviders],
    exports: [MODULES]
} )
export class CoreModule {}
