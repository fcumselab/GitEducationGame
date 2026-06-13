# 💾 MongoDB 資料庫配置指南

本指南將引導你如何在本地端（Local）下載、安裝並啟動 MongoDB 資料庫，以便後端伺服器（Node.js）能順利連接並儲存資料。

---

## 📥 步驟一：下載 MongoDB Community Server

我們在本地開發使用的是官方提供的免費社群版：

1. 請前往 [MongoDB 官方下載中心](https://www.mongodb.com/try/download/community)。
2. 在右側的 **Version** 選擇最新的穩定版本（預設即可）。
3. **Platform** 選擇你的作業系統（例如：Windows 或 macOS）。
4. **Package** 選擇 `MSI` (Windows) 或 `TGZ` (macOS)。
5. 點擊 **Download** 開始下載安裝檔。

---

## 🛠️ 步驟二：安裝 MongoDB 與 Compass 圖形介面

下載完成後，請執行安裝檔，並請**特別注意**以下幾個步驟：

1. **選擇安裝類型：** 建議點選 **Complete**（完整安裝）。
2. **服務設定 (Service Configuration)：** 維持預設（勾選 _Run service as a Network Service user_），這樣 MongoDB 就會在每次電腦開機時自動在背景運行，不需要每次手動開啟。
3. **Install MongoDB Compass：**
   在安裝流程的最後幾步，會看到一個勾選項詢問是否安裝 **"Install MongoDB Compass"**，請務必勾選它。
   - _說明：MongoDB Compass 是官方的圖形化管理工具，能讓你直接用視覺化介面查看、修改資料庫內容，不需盲打指令。（即便安裝過程中漏掉了，事後也可以自行搜尋 **MongoDB Compass** 額外下載安裝）_

4. 點擊 **Next** 並等待安裝完成。

---

## 🔍 步驟三：驗證資料庫是否成功運行

安裝完成後，請在電腦的應用程式中搜尋 **MongoDB Compass** 並開啟它。

1. 開啟 MongoDB Compass 後，你會看到一個連接畫面，請點擊 **`Add new connection`**。
2. 畫面上預設會有一行連接字串（URI）：

   `mongodb://localhost:27017`

   這是預設的資料庫位址，也就是你的本機（Local）資料庫。如果未來需要連接到其他遠端資料庫，我們需要調整這個 URL 的內容。

3. 點擊 **`Save & Connect`** 即可成功進入並看到 `localhost:27017` 的資料庫內部內容。
