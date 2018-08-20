import { Injectable, Inject, PLATFORM_ID } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { Observable, of } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';
import { tap, switchMap, catchError } from 'rxjs/operators';
import { ITokenRequest } from '../models/token-request.interface';
import { ITokenResponse } from '../models/toke-response.interface';

@Injectable()
export class AuthService {
    jwtKey = 'Jwt';
    apiUrl = 'token/auth';

    constructor(
        private http: HttpClient,
        @Inject('BASE_URL') private baseUrl: string,
        @Inject(PLATFORM_ID) private platformId: any) { }

    login(cred: ITokenRequest): Observable<boolean> {
        if (this.isLoggedIn()) {
            return of(true);
        } else {
            return this.getTokenFromApi(cred).pipe(
                tap(jwt => this.setTokenInlocalStorage(jwt)),
                switchMap(_ => of(true)),
                catchError(error => {
                    console.log(error);
                    return of(false);
                }));
        }
    }

    logout(username: string) {
        this.setTokenInlocalStorage(null);
    }

    isLoggedIn(): boolean {
        var jwt = this.getTokenFromlocalStorage();
        if (jwt) {
            return new Date() < new Date(jwt.expiration);
        }
        return false;
    }

    getTokenFromApi(data: any): Observable<ITokenResponse> {
        const url = `${this.baseUrl}/${this.apiUrl}`;
        data = { ...data, grant_type: 'password' };
        return this.http.post<ITokenResponse>(url, data);
    }

    getTokenFromlocalStorage(): ITokenResponse | null {
        if (isPlatformBrowser(this.platformId)) {
            var jwt = localStorage.getItem(this.jwtKey);
            if (jwt) {
                return JSON.parse(jwt);
            }
        }
        return null;
    }

    setTokenInlocalStorage(jwt: ITokenResponse | null): boolean {
        if (isPlatformBrowser(this.platformId)) {
            if (jwt) {
                localStorage.setItem(this.jwtKey, JSON.stringify(jwt));
            } else {
                localStorage.removeItem(this.jwtKey);
            }
        }
        return true;
    }
}