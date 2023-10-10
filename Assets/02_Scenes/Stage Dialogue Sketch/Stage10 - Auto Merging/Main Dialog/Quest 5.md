很好！
您已成功創建一個新的 "提交"

在更新的 "提交記錄" 中
您會注意到 "new-article" 分支與 "new-image" 分支
它們的 "提交記錄" 分叉成了兩條不同的路徑

接下來，讓我們來了解為什麼會發生這種情況：

當我們在 "master" 分支上創建了這兩個分支時
它們起初都指向同一個 "提交"
也就是相同的起點

當我們在 "new-image" 分支上進行一次提交
該分支的 "提交記錄" 增加了一個新的 "提交"

但由於 "new-article" 分支尚未進行修改
整體的 "提交記錄" 仍然保持一條直線

然而，當您在 "new-article" 分支上新增另一個 "提交" 時
這將導致 "new-image" 和 "new-article" 分支開始分叉
因為它們的 "提交記錄" 不再相同

從這一刻開始，這兩個分支將獨立發展
每個分支都有自己的一系列提交

當您在 "new-article" 分支上繼續添加 "提交" 時
這些新的 "提交" 會在這個分支上繼續向前推進
但不會影響到 "new-image" 分支的提交記錄

接下來，讓我們將這兩個分支的更改
合併回 "master" 分支吧

首先，請您切換到 "master" 分支
為合併分支做好準備