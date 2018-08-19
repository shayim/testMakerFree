import { Injectable, Inject, PLATFORM_ID } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ITokenResponse } from '../models/toke-response.interface';
import { Observable, of } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';
import { tap, switchMap, catchError } from 'rxjs/operators';

@Injectable()
export class AuthService {
    apiUrl = "token/auth"
    constructor(
        private http: HttpClient,
        @Inject('BASE_URL') private baseUrl: string,
        @Inject(PLATFORM_ID) private platformId: any) { }

    login(cred: { username: string, password: string }): Observable<boolean> {
        if (this.isLoggedIn(cred.username)) {
            return of(true)
        } else {
            return this.getTokenFromApi(cred).pipe(
                tap(jwt => this.setTokenInlocalStorage(cred.username, jwt)),
                switchMap(_ => of(true)),
                catchError(error => {
                    console.log(error);
                    return of(false)
                }))
        }
    }

    logout(username: string) {
        this.setTokenInlocalStorage(username, null);
    }

    isLoggedIn(username: string): boolean {
        var jwt = this.getTokenFromlocalStorage(username);
        if (jwt) {
            return new Date() < new Date(jwt.expiration)
        }
        return false;
    }

    getTokenFromApi(data: any): Observable<ITokenResponse> {
        const url = `${this.baseUrl}/${this.apiUrl}`;
        return this.http.post<ITokenResponse>(url, data);
    }

    getTokenFromlocalStorage(username: string): ITokenResponse | null {
        if (isPlatformBrowser(this.platformId)) {
            var jwt = localStorage.getItem(`Auth:${username}`)
            if (jwt) {
                return JSON.parse(jwt);
            }
        }
        return null;
    }

    setTokenInlocalStorage(username: string, jwt: ITokenResponse | null): boolean {
        if (isPlatformBrowser(this.platformId)) {
            if (jwt) {
                localStorage.setItem(`Auth:${username}`, JSON.stringify(jwt));
            } else {
                localStorage.removeItem(`Auth:${username}`);
            }
        }
        return true;
    }
}