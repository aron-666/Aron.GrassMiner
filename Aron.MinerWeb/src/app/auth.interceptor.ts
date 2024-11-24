import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from './services/auth.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService, private router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    // 如果在api路徑才加入token
    if (!request.url.includes('/api/')) {
      return next.handle(request);
    }
    const token = this.authService.getToken(); // 從 AuthService 获取 token

    if (token) {
      // 如果有 token，克隆请求并添加 Authorization header
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401 || error.status === 403) {
          // 401 或 403 錯誤，自動登出並導向首頁
          this
            .authService
            .logout()
            .subscribe(() => {
              this.router.navigate(['/Account/Login']);
            });
        }
        return throwError(error); // 拋出錯誤，讓其他程式碼可以處理
      })
    );
  }
}
