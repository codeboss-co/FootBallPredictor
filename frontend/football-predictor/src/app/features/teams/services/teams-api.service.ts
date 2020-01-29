import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TeamsApiService {

  constructor(private http: HttpClient) { }

  public get$(): Observable<any> {
      return this.http.get('http://localhost:5000/team').pipe(
          tap( data => console.log(data) )
      );
  }
}
