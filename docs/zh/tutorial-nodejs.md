# 🧠 Node.js 後端環境架設指南

本指南將引導你如何在本地端（Local）架設並執行遊戲的邏輯後端伺服器。

> ⚠️ **重要前提：** > 後端伺服器在啟動時需要連接資料庫。如果你的電腦尚未安裝或設定過 MongoDB，請先參考：**[💾 MongoDB 資料庫配置指南](tutorial-mongodb.md)**，將資料庫在本機跑起來後，再回到本文件執行步驟三。

---

## 📥 步驟一：下載與安裝 Node.js

本專案建議使用 Node.js 的 **LTS (長期支援) 版本** 以確保穩定性。

1. 請前往 [Node.js 官方網站](https://nodejs.org/)。
2. 下載並安裝適合你作業系統的 **LTS 版本**，如有運行失敗的問題，可以安裝 `v20.10.0` （安裝過程中皆維持預設選項，直接點擊下一步即可）。

---

## 🔍 步驟二：驗證安裝是否成功

安裝完成後，請檢查你的電腦是否已成功偵測到 Node.js 環境：

1. 開啟你的終端機（Windows 請用 `CMD` 或 `PowerShell`；Mac 請用 `Terminal`）。
2. 輸入以下指令並按下 Enter：
   ```bash
   node -v
   ```
3. 如果安裝成功，終端機會顯示目前的 Node.js 版本號（例如：`v20.10.0`）。

---

## ⚙️ 步驟三：本地 (Local) 與 遠端 (Remote) 啟動設定

在我們正式啟動伺服器之前，需要先了解本專案的環境設定。專案中通常包含兩種運行模式：

- **Local (本地開發模式)：** 伺服器會連接你電腦本機的 MongoDB 資料庫，方便你進行開發、測試與偵錯，且不會影響到線上玩家。一般我們都在這裡進行開發，完成後才使用 Remote 模式。
- **Remote (遠端生產模式)：** 伺服器會連接到雲端或遠端伺服器的資料庫，通常是供正式上線或測試伺服器使用。

### 🛠️ 如何切換模式？

請開啟 `server/` 資料夾中的伺服器設定檔案，`server.js` 作為啟動後端的主程式，提供了**Local / Remote 模式**

- **如果要使用 Local 模式（預設）：** 請確保註解掉遠端的 `url`，並啟用 `localhost` 的連線。
- **如果要使用 Remote 模式：** 則反之。

```javascript
// 遠端生產模式設定 (若要使用 Local 請註解此段)
const username = process.env.MONGO_USERNAME;
const password = process.env.MONGO_PASSWORD;
const hostname = process.env.MONGO_HOST;
const port = process.env.MONGO_PORT;
const database = process.env.MONGO_DB;
// const url = `mongodb://${username}:${password}@${hostname}:${port}/${database}`;

// 本地開發模式設定 (目前啟用)
const url = `mongodb://localhost:27017/GEG-database`;
```

---

## 🚀 步驟四：安裝套件與啟動後端

確保 MongoDB 已在背景運行，且模式設定正確後，請開啟終端機並切換至 `server/` 資料夾路徑下：

1.  **安裝專案所需套件：** 執行以下指令，下載後端伺服器需要的所有依賴套件：
    ```Bash
    npm install
    ```
    成功運行後，你應該可以在 `server/` 資料夾底下看見產生的 `node_modules/` 資料夾。
2.  **啟動後端伺服器：** 執行以下指令來執行 `server.js`：
    ```
    node server.js
    ```
3.  **驗證啟動結果：** 當伺服器成功執行且順利連上資料庫時，終端機應該會輸出以下結果：
    ```
    Server is running on port 3000
    資料庫連接、初始化成功
    歡迎使用 GEG-Server！目前版本: x.x.x
    ```

---
