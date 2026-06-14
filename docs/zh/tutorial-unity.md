# 🎮 Unity 前端開發環境配置

本指南將引導你如何下載正確的 Unity 版本、配置開發環境，並成功開啟遊戲專案。

---

## 📥 步驟一：下載 Unity Hub 與對齊版本

本專案有指定使用的 Unity 版本，請務必安裝相同版本以避免檔案衝突或建置失敗。

1. 請前往 [Unity 官方網站](https://unity.com/download) 下載並安裝 **Unity Hub**。
2. 開啟 Unity Hub 並登入你的 Unity 帳號。
3. 本專案使用的 Unity 版本為：**`例如：2021.3.6f1`**（或其他對應版本）。
4. 請在 Unity Hub 的 **Installs** 頁面點擊 **Install Editor**，由於本專案使用的是偏舊的版本，你需要點擊 **Archive** 才能找到對應版本下載。

---

## 🛠️ 步驟二：務必勾選的支援模組 (Modules)

在安裝該版本 Unity Editor 的過程中，會跳出模組選擇畫面（Modules），**請務必確保勾選以下項目**：

- **[x] WebGL Build Support**
  - _說明：因為本專案最終需要打包成網頁端遊戲（放入 `gegGame/`），所以必須安裝此模組才能進行 WebGL 的開發與編譯。_
- **[x] Microsoft Visual Studio (或其他你習慣的 IDE)**
  - _說明：用於編寫與閱讀 C# 遊戲腳本。_

> 💡 **提示：**
> 如果你已經安裝了 Unity，也可以隨時在 Unity Hub 的 **Installs** 頁面，點擊該版本旁邊的「齒輪設定」按鈕，選擇 **Add modules** 來補裝 WebGL Build Support。

---

## 📥 步驟三：將專案導入 Unity Hub

因為專案已經透過 Git 下載到本機，我們只需要將它加進 Unity Hub 的列表中：

1. 開啟 **Unity Hub**，切換到 **Projects** 頁面。
2. 點擊右上角的 **Add** 按鈕（或下拉選單中的 **Add project from disk**）。
3. 在彈出的檔案瀏覽器中，選取你 Clone 下來的專案總目錄，並選中裡面的 **`game/`** 資料夾（這個資料夾才是 Unity 專案的本體）。
4. 點擊確認後，專案就會出現在清單中。此時點擊專案名稱，等待 Unity 編輯器開起。
5. ⚠️ **注意：** 第一次開啟會需要花費較多時間解析與下載資源，可能會 **長達 30 分鐘左右** ，請耐心等待。之後再次開啟專案就不會花費這麼多時間了。

---

## ⚙️ 步驟四：進入遊戲後的專案設定

成功進入 Unity 編輯器後，請優先確認以下設定，確保開發環境一致：

### 1. 確認與切換目標平台為 WebGL

1. 在 Unity 上方選單點擊 **File ➔ Build Settings...** 。
2. 在 Platform 清單中找到 **WebGL** 並點擊它。
3. 確認右下角的按鈕是否為 **Build**。如果不是，請點擊 **Switch Platform** 按鈕進行切換。
4. 說明：當未來遊戲完成開發、需要更新網頁端時，需要點擊這裡的 **Build** 按鈕，將遊戲打包並覆蓋放入 `gegGame/` 中。

### 2. 載入專案開發環境 Layout

1. 在 Unity 上方選單點擊 **Window ➔ Layouts ➔ Load Layout From File..**
2. 選擇位於 `game/` 路徑底下的 **`GITainment layout.wlt`** 檔案。
3. 上傳成功後，你會發現 Unity 編輯器的視窗分佈（環境配置）已經自動改變為本專案推薦的開發模式。

### 3. 環境切換設定：本地 (Local) 與 遠端 (Remote) 切換

在本地進行功能開發或 Debug 時，請務必將環境切換為 **Local 模式**（預設已設定好）。本專案透過切換場景中的物件開關來控管連線：

1. 在 Unity 的 **Project 視窗** 中，找到 **`Assets ➔ 02_Scenes`**，你會看到本遊戲包含的三個核心場景：
   - **Title Screen：** 玩家進入遊戲時第一個看到的畫面，用來註冊與登入帳號。
   - **Stage Select：** 用於選擇遊戲關卡和模式。
   - **Play Game：** 根據選擇的關卡生成對應的遊玩內容。

2. **【重要】三個場景都需要執行以下檢查與設定：**
   - 在場景的 Hierarchy 中，找到 **`URLSettings`** 物件並展開它，底下會包含 `URLSetting (Online Deploy)` 和 `URLSetting (Local Testing)` 兩個子物件。

   - 🛠️ **欲啟動 Local 模式（本地開發）：** 請 **Activate (點亮啟用) Local 物件**，並 **Deactivate (不啟用) Online 物件**。
   - 🌐 **欲啟動 Remote 模式（遠端測試）：** 請 **Activate Online 物件**，並 **Deactivate Local 物件**。同時，必須點擊 Online 物件，在右側 Inspector 的 `Url Setting (Script)` 組件中，將 URL 欄位修改為你部署的遠端網址（例如：`http://xxx.xxx.xx.xx:xxxx/`）。

### 4. 最後驗證：確認遊戲運行正常

現在，我們要進行全端（Unity + Node.js + MongoDB）的連通測試：

1.  確保你的 **MongoDB** 已在背景運行，且 **Node.js 後端伺服器** 已啟動（Terminal 顯示 _Server is running on port 3000_）。
2.  在 Unity 中雙擊打開 **`Title Screen`** 場景，並點擊上方的 **Play (播放鍵)**。
3.  嘗試在畫面中註冊一個新帳號、登入，並試著遊玩第一關卡的 Tutorial 與 Practice 模式。
4.  若實測過程一路順暢、完全沒有紅字報錯，則可以忽略該指南，直接開始開發遊戲。

> 🚨 **遇到 Missing 報錯、畫面彈不出來、或是翻譯不見了嗎？**  
> 由於 Unity 與 Git 搭配時序列化偶爾會失效，若你在首次 Clone 專案實測時遇到了 Missing Reference 或 Error 報錯，請立刻點擊此處前往修復：  
> **[🔧 Unity 疑難排解與 Reference 缺失修復指南](unity-faq.md)**

---
