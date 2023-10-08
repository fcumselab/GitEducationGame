**004 - git status**

**指令類型介紹：**
"git status" 是與 "暫存區域" 有關的指令類型。

透過這個指令類型，可以了解目前 Git 管理系統裡，"暫存區域" 的狀態。

[小提醒：]
在實際使用 Git 時，輸出內容會顯示在命令行視窗中。
而本遊戲將這些輸出內容，獨立成 "暫存區域" 視窗。
視窗的設計盡量保持與現實的輸出結果相似
======
*指令的格式：*
"git status"

*指令的用途：* 
開啟 "暫存區域" 視窗
會將執行指令時，"暫存區域" 裡的內容顯示在視窗中。

*指令需要注意的地方：* 
1. 無法即時更新內容：
在使用與 "暫存區域" 互動的指令時（git add、git reset 等）。
雖然檔案確實被推入、移出 "暫存區域" 中，但是仍舊無法即時看到區域中的狀態。
所以在使用以上指令後，請再次透過這個指令來查看更新後的狀態。

*指令使用情境：*
1. 需要確認目前 "暫存區域" 狀態時
2. 將檔案推入、移出 "暫存區域" 後
3. 確認要新建 "提交" 前

======
