很好！
通過 'git branch -r' 指令
確認了遠端儲存庫的分支情況

查看命令行的執行結果
會發現 'origin/member-branch'
就是這位成員正在開發的分支

第 2 步，獲取對方的分支到本地儲存庫：
我們可以通過指令：
'<color=#CF001C>git branch 本地分支名 遠端分支名</color>'
來獲取遠端分支

這個指令有些複雜，
讓我們來詳細了解指令結構：

'git branch' 主要功能是用於管理分支
我們在 '分支管理' 主題關卡中學過

'git branch 分支名稱' 用來創建一個新的本地分支
'<color=#CF001C>HEAD 的位置會決定此分支的提交記錄</color>' 

而在前面加入 '遠端分支名'
表示新創建的本地分支
'<color=#CF001C>提交記錄會和指定的遠端分支相同</color>'

接下來，請試著執行指令
'<color=#CF001C>git branch member-branch origin/member-branch</color>' 
來獲取團隊成員正在開發中的分支吧