import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { IdentityComponent } from '../identity/identity.component'; // 引入 LoginComponent
import { RouterLink } from '@angular/router';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, IdentityComponent, RouterLink],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {
  title = 'Aron.MinerWeb';
  env = environment;
}
