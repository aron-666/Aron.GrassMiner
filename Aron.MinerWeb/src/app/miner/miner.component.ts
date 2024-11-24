import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

interface MinerRecord {
  Status: string;
  LoginUserName: string;
  IsConnected: boolean;
  ReconnectSeconds: number;
  ReconnectCounts: number;
  Exception: string;
  ExceptionTime: string;
  PublicIp: string;
  Points: number;
  NetworkQuality: string;
  Base64Image: string;
  NeedUpdate: boolean;
  AppVersion: string;
  LastAppVersion: string;
}

@Component({
  selector: 'app-miner', // 選擇器名稱，可以根據您的喜好修改
  templateUrl: './miner.component.html',
  styleUrls: ['./miner.component.scss'],
  imports: [CommonModule]
})
export class MinerComponent implements OnInit {
  minerRecord: MinerRecord | undefined;
  statusDisplayMap: { [key: string]: string } = {
    'Connected': '挖礦中，不要吵我',
    'Disconnected': '幹，斷線了',
    'AppStart': '應用程式啟動中',
    'LoginPage': '登入中',
    'LoginError': '你他媽帳號密碼打錯了',
    'Error': '程式掛了，請回報錯誤訊息',
    'Stop': '程式停止中'
  };
  private intervalId: any;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.intervalId = setInterval(() => {
      this.http.get<MinerRecord>('/api/Miner/GetMinerRecord', { withCredentials: true })
        .subscribe({
          next: (data) => {
            this.minerRecord = data;
          },
          error: (error) => {
            console.error('Error fetching miner record:', error);
          }
        });
    }, 3000);
  }

  ngOnDestroy() {
    clearInterval(this.intervalId); // 清除定时器
  }

  get statusDisplay(): string {
    return this.statusDisplayMap[this.minerRecord?.Status || ''] || this.minerRecord?.Status || '';
  }

  get cardStatus(): string {
    const statusClasses: { [key: string]: string } = {
      'Connected': 'status-connected',
      'Disconnected': 'status-disconnected',
      'Error': 'status-disconnected',
      'Stop': 'status-disconnected',
      'default': 'status-appstart'
    };
    return statusClasses[this.minerRecord?.Status || ''] || statusClasses['default'];
  }

  get updateMessage(): string {
    if (this.minerRecord?.NeedUpdate) {
      return `有更新! 當前版本: ${this.minerRecord.AppVersion}，最新版本: ${this.minerRecord.LastAppVersion}`;
    }
    return '';
  }
}
