![Total Visitors](https://komarev.com/ghpvc/?username=aron-666miner&color=green)

# Aron.GrassMiner

## 好用請支持，使用我的推薦碼註冊: RI3NGc63lVmUQix
[立即註冊](https://app.getgrass.io/register/?referralCode=RI3NGc63lVmUQix)

## 1. Docker 安裝
1. 安裝 Docker
   - Windows: [Docker Desktop](https://www.docker.com/products/docker-desktop/)

2. 匯入 Docker 映像檔 
   ```
   docker pull aron666/aron.grassminer
   //這裡上docker hub了，不用下載tar
   ```

3. 編輯 docker-compose.yml
   ```
   GRASS_USER=你的 Garss 帳號
   GRASS_PASS=你的 Garss 密碼
   ADMIN_USER=後臺管理帳號(自訂義)
   ADMIN_PASS=後臺管理密碼(自訂義)
   ```

   - Port 5001 會在你電腦上開一個 Port，要讓區網連請開防火牆 Port 5001

4. 執行
   ```
   //cmd請先 cd 到資料夾目錄
   docker-compose up
   ```
   再來就可以用網址看後臺狀態了

   - 本機: [http://localhost:5001](http://localhost:5001)
   - 其他設備: 先開 cmd 打 `ipconfig`/`ifconfig` 找到你的區網 IP [http://IP:5001](http://IP:5001)
     - 關掉網頁還會繼續執行
     - Windows 要開機自動執行要去Docker Desktop設定改
