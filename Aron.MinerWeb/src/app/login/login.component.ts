import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  imports: [FormsModule, CommonModule]
})
export class LoginComponent {
  credentials = {
    Username: '',
    Password: ''
  };
  errorMessage: string | null = null;

  constructor(private authService: AuthService, private router: Router) { }

  onSubmit(form: any) {
    this.authService.login(this.credentials).subscribe({
      next: () => {
        // 登錄成功，導向首頁
        this.router.navigate(['/']);
      },
      error: (error) => {
        // 處理登錄錯誤，例如顯示錯誤訊息
        console.error('Login failed:', error);
        this.errorMessage = error.error.resultMsg;
      }
    });
  }
}
