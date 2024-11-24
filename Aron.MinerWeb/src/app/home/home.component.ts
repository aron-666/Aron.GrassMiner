import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  title = 'Aron.MinerWeb';
  // redirect to login page
  constructor(private router: Router) {
    this.router.navigate(['/Miner']);
  }
}

