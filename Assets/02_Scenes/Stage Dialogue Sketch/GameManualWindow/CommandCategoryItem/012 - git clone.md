**012 - git clone**

**指令類型介紹：**
'git clone' 是與 '遠端管理' 有關的指令類型，
在使用遠端儲存庫時要執行的第一個指令。

這個指令類型允許您將遠端儲存庫的內容複製到本地設備，
同時創建本地設備和遠端儲存庫之間的連接。

通常情況下，複製成功的遠端儲存庫會被 Git 命名為 'origin'，
這些連接的遠端儲存庫會有一個別名，對應著遠端儲存庫的網址。

（以下是指令細節，玩家可以翻頁）
**第一頁：**
*指令的格式：* 
```
git clone 遠端儲存庫網址
```

*指令的用途：*
將遠端儲存庫的內容複製到本地設備中。

*指令使用情境：*
1. 準備開始使用遠端儲存庫進行開發時。
2. 需要在新的設備上繼續專案開發時。

*指令使用範例：*
當使用者 A 在 Git 服務平台（例如：GitHub、GitLab 等）上
創建了一個新的遠端儲存庫（例如：https://github.com/user/New-Project.git）。

如果要在自己的本地設備上開發專案時，
可以執行以下指令：
```
git clone https://github.com/user/New-Project.git
```

*指令需要注意的地方：* 
1. 確認當前命令行的路徑：
當執行此指令後，
Git 會在命令行路徑下創建一個與遠端儲存庫相同名稱的資料夾。
資料夾內包含了遠端儲存庫的所有檔案。

2. 了解指令執行失敗的原因：
指令執行失敗的原因可能是：
1. 網路連接問題
2. 權限問題
3. 當前路徑的狀態

如果想要避免 '當前路徑的狀態' 造成指令執行失敗。
請您確保當前路徑中不包含與遠端儲存庫同名的資料夾，
並且該資料夾內容為空。