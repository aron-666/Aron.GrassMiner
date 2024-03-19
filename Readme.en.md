![Total Visitors](https://komarev.com/ghpvc/?username=aron-666miner&color=green)

[![en](https://img.shields.io/badge/lang-en-red.svg)](https://github.com/aron-666/Aron.GrassMiner/Readme.en.md)
[![中文](https://img.shields.io/badge/lang-中文-blue.svg)](https://github.com/aron-666/Aron.GrassMiner)

# Aron.GrassMiner
Written in .Net 8

## If you find it useful, support me by using my referral code: RI3NGc63lVmUQix
[Register Now at app.getgrass.io](https://app.getgrass.io/register/?referralCode=RI3NGc63lVmUQix)

Currently, Grass is priced off-exchange at 1:0.0036USDT.

Mining at full capacity for a day yields 1800 tokens, equivalent to 6.48USDT per day.

Grass mining doesn't require specialized hardware but consumes minimal bandwidth (10-30KB/s) for mining.

In the future, when it's listed on exchanges, points can be directly converted into cryptocurrencies.

Others' Introduction: [Binance Article](https://www.binance.com/zh-TC/feed/post/1783966376178)

## Execution Screenshots
1. Login
![image](https://github.com/aron-666/Aron.GrassMiner/blob/master/%E6%88%AA%E5%9C%96/%E5%BE%8C%E8%87%BA%E7%99%BB%E5%85%A5%E7%95%AB%E9%9D%A2.png?raw=true)

2. Mining Information
![image](https://github.com/aron-666/Aron.GrassMiner/blob/master/%E6%88%AA%E5%9C%96/%E6%8C%96%E7%A4%A6%E7%95%AB%E9%9D%A2.png?raw=true)

## 1. Docker Installation
1. Install Docker
   - Windows: [Docker Desktop](https://www.docker.com/products/docker-desktop/)
   - Linux: If you're using Linux, you probably know how to do this already.

2. Edit docker-compose.yml
   ```
   GRASS_USER=Your Grass account
   GRASS_PASS=Your Grass password
   ADMIN_USER=Backend management account (customizable)
   ADMIN_PASS=Backend management password (customizable)
   PROXY_ENABLE=true / false
   PROXY_HOST=http(s)://host:port
   ```

   - Port 5001 will open a port on your computer. Open firewall port 5001 for LAN access.

3. Execute
   ```
   //cmd, navigate to the directory first
   docker compose up
   ```
   Then, you can check the backend status using the following URLs:

   - Local: [http://localhost:5001](http://localhost:5001)
   - Other devices: Open cmd and type `ipconfig`/`ifconfig` to find your LAN IP, then access [http://IP:5001](http://IP:5001)
     - The process continues even if the webpage is closed.
     - For Windows auto-start, adjust settings in Docker Desktop.

## 2. Service Installation
1. Windows: Coming Soon
2. Linux: Coming Soon

## Update Log
2024-03-18: Added UI information, fixed login functionality, UI beautification (I'm terrible at frontend).
2024-03-18: Added version update notification, Proxy settings.
