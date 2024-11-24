import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './shared/layout/layout.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { MinerComponent } from './miner/miner.component';
import { AuthGuard } from './auth.guard';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent, // 使用 LayoutComponent 作為父元件
    children: [ // 設定子路由
      { path: '', component: HomeComponent, title: 'Home' },
      { path: 'Account/Login', component: LoginComponent, title: 'Login' },
      { path: 'Miner', component: MinerComponent, title: 'Miner', canActivate: [AuthGuard] }
    ]
  }
];

