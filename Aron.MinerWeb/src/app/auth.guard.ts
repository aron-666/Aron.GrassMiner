import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from './services/auth.service'; // 假设你有一个 AuthService 处理认证逻辑

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean {
    if (this.authService.isLoggedIn()) {
      // 如果用户已登录，允许访问
      return true;
    } else {
      // 如果未登录，跳转到 login 页面
      this.router.navigate(['/Account/Login']);
      return false;
    }
  }
}
