import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

export class ErrorInterceptor implements HttpInterceptor {
    intercept( req: HttpRequest<any>, next: HttpHandler ): Observable<HttpEvent<any>> {

        return next.handle(req).pipe(
            retry(2),
            catchError((error: HttpErrorResponse) => {

                if ( error && error.status !== 401 ) {
                    alert('Something has gone wrong');
                }

                return throwError(error);
            })
        );
    }

}
