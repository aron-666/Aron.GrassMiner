1. 安裝docker
Windows: 
https://www.docker.com/products/docker-desktop/

2. 匯入docker映像檔 
docker pull aron666/aron.grassminer
//這裡上docker hub了，不用下載tar

3. 編輯docker-compose.ymal
GRASS_USER=你的Garss帳號
GRASS_PASS=你的Garss密碼
ADMIN_USER=後臺管理帳號(自訂義)
ADMIN_PASS=後臺管理密碼(自訂義)
PROXY_ENABLE=true / false
PROXY_HOST=http(s)://host:port
PROXY_USER=user
PROXY_PASS=pass

Port 5001 
會在你電腦上開一個Port，要讓區網連請開防火牆 Port 5001

4. 執行
//cmd請先 cd 到資料夾目錄
docker compose up
再來就可以用網址看後臺狀態了

本機: http://localhost:5001
其他設備: 先開cmd 打ipconfig/ifconfig 找到你的區網IP 
http://IP:5001
*關掉網頁還會繼續執行
*Windows 要開機自動執行要去設定改

Github: https://github.com/aron-666/Aron.GrassMiner

好用請支持，使用我的推薦碼註冊: RI3NGc63lVmUQix
https://app.getgrass.io/register/?referralCode=RI3NGc63lVmUQix
