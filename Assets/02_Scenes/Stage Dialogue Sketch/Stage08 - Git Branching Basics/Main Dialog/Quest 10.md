好的，在我們更新了 "提交記錄" 內容後

您可以發現 HEAD 移動到了
"master" 分支裡最新的 "提交"
也就是第 3 個 "提交"

並且，新創建的第 4 個 "提交" 顯示的顏色
與其他 3 個 "提交" 都不同
它與 "new-feature" 分支代表的顏色相同

由此可知
"master" 分支和 "new-feature" 分支之間
的修改內容互不影響

假設我們在 "master" 分支進行提交
那麼 "new-feature" 分支的內容不會受到影響

在俄羅斯方塊新增了一個遊戲功能後
我們開始測試遊戲的運行狀況

然而，在遊玩途中
發現這個新功能不僅沒有實現
而且連最基本的玩法都無法進行

這下我們有兩種選擇：
1. 繼續開發這項新功能
2. 直接捨棄掉這個功能

考慮到這個功能並沒有增加遊戲的樂趣
我們試著將這個分支的內容刪除吧

本關卡要學習的最後一個指令是
"<color=#CF001C>git branch -d 分支名稱</color>" 
這個指令可以刪除掉指定的分支
並將分支中所有的 "提交記錄" 刪除掉

"git branch 分支名稱" 是用來創建分支的指令
而在指令中加入 "<color=#CF001C>-d</color>" (Delete 刪除)
就變成刪除分支的指令

不過在刪除分支之前
<color=#CF001C>請確保 HEAD 已經移動到了其他分支</color>

在關卡過程中
我們已經位於 "master" 分支

接下來，請您執行 "<color=#CF001C>git branch -d new-feature</color>" 指令
刪除 "new-feature" 分支吧