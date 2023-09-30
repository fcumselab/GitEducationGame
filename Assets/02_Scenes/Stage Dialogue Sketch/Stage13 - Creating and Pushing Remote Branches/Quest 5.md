太好了！
您已在新建立的分支上成功新增了 "README" 的內容

在檢查了更新後的 "提交記錄" 視窗後
您會發現本地儲存庫中多了一個 "提交"
但是，遠端儲存庫的內容並未更新

這是因為我們尚未
將本地所做的修改上傳至遠端儲存庫

接下來，我們要學習如何透過指令上傳這些修改

如果您想要更新遠端儲存庫的內容
可以使用以下指令：
<color=#CF001C>git push 儲存庫別名 分支名稱</color>

讓我們解釋一下這個指令的輸入方式：

首先，第三個欄位是儲存庫別名
通常我們會填入 "origin"
"origin" 對應著遠端儲存庫的網址

因此，我們通常
<color=#CF001C>git push origin 分支名稱</color> 
這樣的格式來做記憶

然後，指令的第四個欄位是分支名稱
即您想要上傳至遠端儲存庫的本地分支名稱
也就是我們的開發分支 "player-update-README"

如果遠端儲存庫中沒有找到對應的分支名稱
Git 將會建立一個同名的遠端分支
並複製相同的 "提交記錄"

如果遠端分支已經存在
則會更新到與本地分支相同的 "提交記錄"

按照上述說明，現在讓我們執行指令
<color=#CF001C>git push origin player-update-README</color>
將分支的修改內容成功上傳至遠端儲存庫吧！