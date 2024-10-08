**004 - git status**

**指令類型介紹：**
'git status' 屬於與 '暫存區域' 相關的指令類型。

透過這個指令，
您可以查看目前 Git 管理系統中的 '暫存區域' 狀態。
以便了解哪些檔案已被追蹤，哪些檔案已被修改，
以及是否準備好進行提交。

>小提醒：
在實際使用 Git 時，指令的輸出內容會顯示在命令行視窗中。
本遊戲將輸出內容，整合到 '暫存區域' 視窗中。
以便更容易查看內容，視窗的設計和實際輸出相似。

（以下是指令細節，玩家可以翻頁）
**第一頁：**
*指令的格式：*
```
git status
```

*指令的用途：* 
開啟 '暫存區域' 視窗，以顯示當前 '暫存區域' 的狀態。

*指令使用情境：*
1. 需要確認當前 '暫存區域' 的狀態。
2. 將檔案推入或移出 '暫存區域' 後。
3. 創建新的 '提交' 前，確認追蹤的檔案和狀態。

*指令使用範例：*
當使用者 A 在將修改完成的檔案推入或移出 '暫存區域' 後，
執行以下指令來查看區域中檔案的狀態：
```
git status
```

*指令需要注意的地方：* 
1. 無法即時更新內容：
在執行與 '暫存區域' 互動的指令
（例如：git add、git reset 等）後。

雖然檔案的狀態已更改，但您無法立即看到視窗中的更新。
因此，在執行這些指令後，
請再次使用 'git status' 指令以查看最新的狀態。