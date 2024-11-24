import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service'; // 假設你有一個 AuthService 來處理驗證
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';

@Component({
  selector: 'app-identity',
  templateUrl: './identity.component.html',
  styleUrls: ['./identity.component.scss'],
  imports: [CommonModule, RouterLink]
})
export class IdentityComponent
 {
  constructor(public authService: AuthService, private router: Router) { } // 注入 AuthService

  logout() {
    this.authService.logout().subscribe({
      next: () => {
        // 登出成功，導向登入頁
        this.router.navigate(['/Account/Login']);
      },
      error: (error) => {
        // 處理登出錯誤，例如顯示錯誤訊息
        console.error('Logout failed:', error);
        this.router.navigate(['/Account/Login']);

      }
    });
  }
}
