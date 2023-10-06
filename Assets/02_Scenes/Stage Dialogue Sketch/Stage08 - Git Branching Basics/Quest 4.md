好的，您更新了 "提交記錄" 視窗的內容

首先，您會注意到
分支欄位中新增了一個分支名稱
就是我們創建好的分支 "new-feature"

此外，當您點擊 "提交" 來開啟詳細訊息時
會發現在分支欄位的位置增加了 "new-feature" 分支

但是，請注意在分支欄位顯示的 "HEAD -> master" 文字
表示 HEAD 目前還是指向 "master" 分支

這是因為 "git branch 分支名稱" 指令只用於創建分支
HEAD 並不會自動切換到新創建的分支上

所以在建立分支後
請不要忘記使用 "git checkout 分支名稱" 指令
來切換到新創建的分支，以開始在新分支上工作

在開始切換分支之前
讓我們先學習如何快速查看分支的方法

通過 "<color=#CF001C>git branch</color>" 指令
我們可以查看 Git 管理系統中包含的所有分支名稱
來試試看吧