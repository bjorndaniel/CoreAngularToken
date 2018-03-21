import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';

import { HomeDetails } from '../models/home.details.interface';
import { ConfigService } from '../../shared/utils/config.service';

import { BaseService } from '../../shared/services/base.service';

import { Observable } from 'rxjs/Observable';

// Add the RxJS Observable operators we need in this app.

@Injectable()
export class DashboardService extends BaseService {
  baseUrl = '';

  constructor(private http: Http, private configService: ConfigService) {
    super();
    this.baseUrl = configService.getApiURI();
  }

  getHomeDetails(): Observable<HomeDetails> {
    const headers = new Headers();
    headers.append('Content-Type', 'application/json');
    const authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);

    return this.http
      .get(this.baseUrl + '/dashboard/home', { headers })
      .map(response => response.json())
      .catch(this.handleError);
  }
}
