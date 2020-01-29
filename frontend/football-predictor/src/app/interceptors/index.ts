import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { Provider } from '@angular/core';
import { ErrorInterceptor } from './error.interceptor';

export const httpInterceptorProviders: Provider[] = [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true}
];
