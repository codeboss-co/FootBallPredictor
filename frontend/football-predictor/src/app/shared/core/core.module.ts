import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { httpInterceptorProviders } from '../../interceptors';
import { NebularUIModule } from '../../@theme/nebular-ui.module.';

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

        // UI Theme
        NebularUIModule,

        MODULES
    ],
    providers: [httpInterceptorProviders],
    exports: [MODULES, NebularUIModule]
} )
export class CoreModule {}
