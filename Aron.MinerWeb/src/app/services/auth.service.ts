import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly tokenKey = 'access_token'; // localStorage 的 key

  constructor(private http: HttpClient) { }

  login(credentials: any): Observable<any> {
    const loginUrl = '/api/Identity/Login'; // 你的登入 API 端點
    const requestBody = {
      lang: "",
      resultObj: credentials
    };

    return this.http.post(loginUrl, requestBody).pipe(
      tap((response: any) => {
        if (response.success && response.resultObj.access_token) {
          this.storeToken(response.resultObj.access_token);
        }
      })
    );
  }

  logout(): Observable<any> {
    const logoutUrl = '/api/Identity/Logout'; // 你的登入 API 端點


    return this.http.delete(logoutUrl).pipe(
      tap((response: any) => {
        this.removeToken();
      }),
      catchError((error) => {
        this.removeToken();
        return error;
      }
    ));
  }

  // logout(): void {
  //   this.removeToken();
  // }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  getUserName(): string | null {
    const token = this.getToken();
    if (token) {
      // 解析 JWT 取得使用者名稱，這裡需要根據你的 JWT 結構調整
      const decodedToken = JSON.parse(atob(token.split('.')[1]));
      return decodedToken.sub;
    }
    return null;
  }

  public storeToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  public getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  public removeToken(): void {
    localStorage.removeItem(this.tokenKey);
  }
}
