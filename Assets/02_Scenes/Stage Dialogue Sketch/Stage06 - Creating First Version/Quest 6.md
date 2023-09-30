很好！
我們通過 "git log" 指令
成功開啟了 "提交記錄" 視窗

接下來，讓我們來查看 "提交記錄" 裡的內容

首先，您會注意到視窗中有一個圓形圖示
它代表著我們從 "暫存區域" 中提交的第一個版本

在 Git 系統裡
我們稱它們為 "提交" (Commit)
每一個 "提交" 都代表著不同的版本

您可以點擊 "提交" 圓形圖示
以查看 "提交" 的詳細內容

在彈出的彈窗中
您可以看到提交的日期、作者、以及我們剛才編寫的訊息
這些資訊有助於您理解這個 "提交" 所做的變更

接著，在分支欄位中
您會看到 "HEAD -> master"

分支欄位表示這個 "提交" 屬於哪個分支

"master" 是 Git 系統中的默認分支
在通常情況下
我們會在 "master" 分支上進行提交

在 "Git 的基礎" 主題關卡中
我們只會使用到 "master" 分支

更多關於分支的知識
將在 "分支管理" 主題關卡中進一步介紹

接下來，讓我們來了解什麼是 "HEAD"

請使用滑鼠左鍵
點擊 "提交記錄" 視窗的任意區域
以關閉 "提交" 的詳細內容

然後，您可以在 "提交記錄" 視窗的左側列表中
看到當前 "儲存庫" 的所有分支名稱和對應的圖標

分支會以不同的顏色表示
例如屬於 "master" 分支的 "提交"
將以藍色 M 字母的圖示顯示

旗子圖示的 HEAD 
表示我們目前所處的 "提交" 版本

在 "提交" 詳細訊息中顯示的 "HEAD -> master"
也表示 HEAD 目前指向 "master" 分支

當我們有多個版本時
可以使用特定的指令來切換到不同的版本

這將使 "HEAD" 移動到所選擇的 "提交" 版本，
同時 "工作目錄" 中的檔案將恢復到該版本的內容

接下來，讓我們再次使用 "git commit -m" 指令
提交簡報的第二個版本吧

不過，在這之前
我們需要先修改簡報的內容

然後將這些變更再次移動到 "暫存區域" 中
才能夠使用 "git commit -m" 指令新增第二個提交

請您開啟 "期中報告.ppt"
並修正檔案中錯誤的內容吧
