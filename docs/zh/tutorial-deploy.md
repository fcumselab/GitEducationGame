# 🚀 伺服器 Docker 部署指南

本指南將引導你如何將本地開發完成的遊戲（包含 Unity WebGL 前端、Node.js 後端、MongoDB 資料庫），透過 Docker 部署到實驗室的實體伺服器主機上，讓所有人可以連線遊玩。

---

## 🛠️ 第一階段：本機端的打包與環境設定準備

在將檔案上傳到伺服器之前，我們必須先在本地電腦將「連線網址」切換為遠端模式，並打包出最新版的遊戲網頁檔案。

### 1. 修改 Node.js 後端連線設定

1. 開啟 `server/` 資料夾中的 `server.js`（或負責連接資料庫的檔案）。
2. 找到以下內容，將 Local 模式註解掉，並**啟用 Remote（遠端生產模式）** 的 `url` 連線：

```javascript
// 遠端生產模式設定 (部署至實驗室伺服器時請啟用此段)
const username = process.env.MONGO_USERNAME;
const password = process.env.MONGO_PASSWORD;
const hostname = process.env.MONGO_HOST;
const port = process.env.MONGO_PORT;
const database = process.env.MONGO_DB;
const url = `mongodb://${username}:${password}@${hostname}:${port}/${database}`;

// 本地開發模式設定 (部署時請註解此段)
// const url = `mongodb://localhost:27017/GEG-database`;
```

### 2. 修改 Unity 前端連線設定

1.  在 Unity 編輯器中，依序打開三個核心場景（`Title Screen`、`Stage Select`、`Play Game`）。
2.  在每個場景的 Hierarchy 中找到 **`URLSettings`** 物件並展開：
    - ❌ **Deactivate (關閉)：** `URLSetting (Local Testing)`
    - ✅ **Activate (開啟)：** `URLSetting (Online Deploy)`

3.  點擊 `URLSetting (Online Deploy)` 物件，在右側 Inspector 的 `Url Setting (Script)` 中，將 **URL 欄位修改為實驗室伺服器的實體 IP 與 Port**（這裡需要填入後端服務的 Port `5051`，例如：`http://140.xxx.xx.xx:5051/`）。

### 3. 打包 Unity WebGL 覆蓋至 `gegGame/`

1.  在 Unity 上方選單點擊 **File ➔ Build Settings**，確認平台為 **WebGL**。
2.  點擊 **Build**。
3.  選擇專案總目錄底下的 **`gegGame/`** 資料夾進行打包輸出（直接覆蓋裡面的舊檔案）。

---

## ⚙️ 第二階段：修改 Docker 設定與遠端連線傳輸

當本機端的程式碼與 WebGL 打包都準備就緒後，我們需要調整 Docker 的設定檔，並將檔案傳輸至實驗室的伺服器主機。

### 1. 修改 `docker-compose.yml` 中的環境變數

1. 開啟 `docker/` 資料夾底下的 `docker-compose.yml` 檔案。
2. 找到 `server` 服務底下的 `environment` 區塊。
3. 將 `GAME_ORIGIN` 後方的 `your_ip` 修改為**實驗室電腦的實體 IP**：

```yaml
server:
  container_name: Kevin-GEG-server
  build: ./server
  ports:
    - "5051:3000"
  environment:
    - GAME_ORIGIN= http://140.xxx.xx.xx:5050 # 👈 請將 your_ip 改為伺服器的實體 IP
    - MONGO_USERNAME=KUser
    - MONGO_PASSWORD=KPass
    - MONGO_HOST=Kevin-GEG-mongo
    - MONGO_PORT=27017
    - MONGO_DB=GEG-database?authSource=admin
```

_（註：其餘如資料庫帳密、Port 號等設定皆已由前人配置完畢，若無特殊需求請勿變動。）_

### 2. 使用 MobaXterm 連線至伺服器

為了把檔案放進實驗室的實體主機，我們需要使用 SSH/SFTP 工具：

1.  電腦請先下載並開啟 **[MobaXterm](https://mobaxterm.mobatek.net/)**。
2.  點擊左上角的 **Session ➔ SSH**。
3.  在 **Remote host** 欄位輸入伺服器的實體 IP，並勾選 **Specify username** 輸入伺服器的管理員帳號。
4.  點擊 OK 後輸入密碼登入。成功登入後，你會看到終端機畫面，且 **左側會出現伺服器的檔案瀏覽器面板 (SFTP)**。

### 3. 在伺服器建立專案資料夾並上傳檔案

1.  在 MobaXterm 的終端機中，輸入以下指令前進到你想存放專案的目錄（例如 `/home/username/`），並建立一個專屬資料夾：

    ```
    mkdir GITainment
    cd GITainment
    ```

2.  在左側的檔案瀏覽器面板中，切換進入剛剛建立的 `GITainment/` 資料夾。
3.  將你本機電腦中準備好的 **以下三個物件**，直接**拖曳**到 MobaXterm 左側的面板中上傳：
    - 📁 **`gegGame/`** 資料夾（內含剛打包好的 WebGL 靜態網頁與其 Dockerfile）
    - 📁 **`server/`** 資料夾（內含後端原始碼與其 Dockerfile）
    - 📄 **`docker-compose.yml`** 檔案（剛剛改好 IP 的那個版本）

> 💡 **上傳結構檢查：** 請確保伺服器上的 `GITainment/` 資料夾內，這三個物件是**平級（並排）**的，這樣 Docker Compose 編譯時才找不到對應的路徑！

---

## 🚀 第三階段：伺服器端 Docker 啟動與驗證

檔案全部上傳完畢後，我們就可以在伺服器上下指令，讓 Docker 自動一鍵架設與運行整個遊戲生態系。

### 1. 執行 Docker Compose 建立並啟動容器

1. 在 MobaXterm 的終端機中，確保目前路徑處於你建立的 `GITainment/` 資料夾下。
2. 輸入以下一鍵啟動指令（因為需要權限，請在前方加上 `sudo`）：
   `sudo docker compose up -d --build`

- 💡 **指令解析：**
  - `up`: 建立並啟動設定檔中的所有服務（game, server, mongo）。
  - `-d`: 在背景執行（Detached 模式），這樣關閉 MobaXterm 後伺服器依然會持續運作。
  - `--build`: 強制重新編譯最新的程式碼與 WebGL 靜態檔案，確保上傳的改動有生效。

3.  等待終端機跑完下載與編譯進度條。當畫面顯示三個服務都為 `Started` 或 `Running` 時，即代表啟動成功！

### 2. 瀏覽網頁測試遊戲

1.  打開你本機電腦的任意瀏覽器（Chrome / Edge 等）。
2.  在網址列輸入實驗室伺服器的 **IP 與遊戲前端的 Port 號（5050）**：
    ```
    [http://140.xxx.xx.xx:5050](http://140.xxx.xx.xx:5050)
    ```
3.  如果成功看到遊戲畫面，且能夠正常註冊、登入與遊玩，代表遠端部署已經完全大功告成！

---

## 補充：如何連入遠端的 MongoDB 查看資料？

如果未來需要查看線上玩家的帳號或排行榜資料，你可以透過本機的 **MongoDB Compass** 直接連入實驗室的伺服器：

1.  在你自己的本機電腦開啟 **MongoDB Compass**。
2.  點擊 **`Add new connection`**。
3.  根據 `docker-compose.yml` 檔案中的設定，遠端 MongoDB 對外的 Port 號為 **`5052`**，帳密為 `KUser` / `KPass`。請將連接字串（URI）修改為以下格式：
    `mongodb://KUser:KPass@140.xxx.xx.xx:5052/?authSource=admin`
    _（請將 `140.xxx.xx.xx` 替換為實際的實驗室伺服器 IP）_

- 點擊 **`Save & Connect`**，即可直接視覺化管理遠端伺服器上的 `GEG-database` 資料庫！
