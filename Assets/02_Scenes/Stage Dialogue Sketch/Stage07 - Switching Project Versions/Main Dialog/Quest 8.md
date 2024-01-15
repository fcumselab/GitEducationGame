非常好！
HEAD 已經移動到第 3 個提交中

在比對了兩個顏色文字哪個合適後
最後我們決定使用 '橙色'
也就是第 2 個提交

於是，我們決定移動到第 2 個提交上
然後繼續開發專案

然而，在這種情況下
這樣是無法繼續開發專案的

接下來，讓我們來了解當前遇到的問題

首先，在 '提交記錄' 區域中
您會發現 HEAD 看起來變透明了

在 Git 系統中
這種情況稱為<color=#CF001C>分離 HEAD</color> (Detached HEAD)
表示當前 HEAD 並沒有指到任何的分支

在執行 'git checkout 提交ID' 時
HEAD 會指向您指定的提交
導致<color=#CF001C>分離 HEAD</color>的發生

雖然 HEAD 指向了第 3 個提交
它也是 'master' 分支裡最新的提交
然而實際上，HEAD 並沒有指向 'master' 分支

點擊這個提交後
會發現在分支欄位已不再是 'HEAD -> master'

在這種情況下
就算我們新建一個提交 
也不會在 'master' 分支上創建第 4 個提交

同時，您在分離 HEAD 情況下創建的提交
甚至會有遺失的風險

所以請注意，當我們查看完其他提交的內容後
<color=#CF001C>請記得切換回您想要繼續開發的分支！</color>

本關卡要學習的最後一個指令是
'<color=#CF001C>git checkout 分支名稱</color>'
通過這個指令，您就可以將 HEAD 移動回分支上了

試試看在命令行中輸入：'<color=#CF001C>git checkout master</color>'
讓 HEAD 移動回 'master' 分支吧
請加油！