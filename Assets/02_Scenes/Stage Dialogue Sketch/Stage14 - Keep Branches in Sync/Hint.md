# Q1、3 開啟 '提交記錄' 視窗
Q1（提示）：
如果您不確定如何查看提交記錄
這裡有一些提示可以幫助您完成目標：

1. 確認您的 '命令行' 視窗已經開啟

2. 確認您的命令行路徑位於專案上
   （可以看到 .git 隱藏資料夾）

3. Git 指令中，有個用於查看提交記錄狀態的指令
   詳細請通過遊戲手冊查找

4. 執行後，會出現 '提交記錄' 視窗
   您將可以看到目前儲存庫中的版本記錄

Q1（解答）：
為了確認當前專案的提交歷史記錄
您需要使用 '<color=#CF001C>git log</color>' 指令

在 '命令行' 視窗中，請確保其路徑位於專案上
本次關卡的專案創建於 '<color=#CF001C>Home > shopping-website</color>' 路徑上
（可以看到 .git 隱藏資料夾）

確認完後，請在 '命令行' 視窗中的指令輸入區域
輸入 '<color=#CF001C>git log</color>'
然後執行它來查看提交記錄的內容

# Q2 "切換到 'update-readme' 分支並同步遠端分支的內容"
Q2（提示）：
如果您不確定如何達成本次的目標
這裡有一些提示可以幫助您：

1. 同步本地、遠端分支指令就和 '<color=#CF001C>git merge</color>' 類似
   輸入的遠端分支名稱要與當前專案所在分支相同
   以確保同步（合併）過程不會造成問題

2. 確認遠端分支位於哪一個遠端存儲庫
   可以通過 Git 指令來查看儲存庫別名
   詳細請通過遊戲手冊查找

3. '<color=#CF001C>git pull 遠端儲存庫別名 分支名稱</color>'
   為同步分支的指令結構

4. 更詳細的介紹
   請查閱遊戲手冊中的 'git pull' 指令

Q2（解答）：
為了順利同步本地、遠端分支
輸入的遠端分支名稱要與當前專案所在分支相同

本次目標要同步 'update-readme' 分支
因此您需要先通過 '<color=#CF001C>git checkout update-readme</color>' 指令
先確認專案目前已經切換到 'update-readme' 上

同步分支的指令為：
'<color=#CF001C>git pull 遠端儲存庫別名 分支名稱</color>'

遠端儲存庫別名可以通過 '<color=#CF001C>git remote -v</color>' 得知
一般情況下要上傳的儲存庫別名應該是 'origin'

接下來，請執行同步分支指令
'<color=#CF001C>git pull origin update-readme</color>'

# Q4 "查看遠端儲存庫中的所有分支"
Q4（提示）：
如果您不確定如何查看遠端儲存庫的所有分支
這裡有一些提示可以幫助您完成目標：

1. 確認您的 '命令行' 視窗已經開啟

2. 確認您的命令行路徑位於專案上
   （可以看到 .git 隱藏資料夾）

3. Git 指令中，有個用於查看遠端儲存庫分支的指令
   這個指令類型後方加入 '<color=#CF001C>-r</color>' 即可查看
   詳細請通過遊戲手冊查找

4. 執行後，會出現所有遠端儲存庫遠端分支

Q4（解答）：
為了確認對方的開發分支
我們需要通過 '<color=#CF001C>git branch -r</color>' 指令
或是到 GitHub 上，查看專案中的所有遠端分支名稱

請在 '命令行' 視窗中的指令輸入區域
輸入 '<color=#CF001C>git branch -r</color>'
然後執行它來查看所有遠端分支的名稱

# Q5 "獲取遠端分支 'member-branch' 到本地存儲庫中 並更新 '提交記錄' 視窗"
Q5（提示）：
如果您不確定如何獲取遠端分支到本地存儲庫
這裡有一些提示可以幫助您完成目標：

1. 確認目標的遠端分支
   記下它的分支名稱和屬於哪一個遠端儲存庫中

2. '<color=#CF001C>git branch</color>' 可以將遠端分支獲取到本地端
   需要在後方分別加入 '<color=#CF001C>本地分支名</color>' 和 '<color=#CF001C>遠端分支名</color>'

3. 關於更多的細節
   可以查閱遊戲手冊中的 '<color=#CF001C>git branch</color>' 條目

4. 當獲取分支後
   請記得確認提交記錄的狀態

Q5（解答）：
為了獲取對方的遠端分支 'member-branch'
需要通過 '<color=#CF001C>git branch 本地分支名 遠端分支名</color>' 指令
來獲取分支到本地電腦上

通過 '<color=#CF001C>git branch -r</color>' 指令
可以得知遠端分支的全名 '<color=#CF001C>origin/member-branch</color>'

執行 '<color=#CF001C>git branch member-branch origin/member-branch</color>' 指令
即可成功創建本地 'member-branch' 分支
並獲得與遠端分支相同的內容

完成後，請在 '命令行' 視窗中的指令輸入區域
輸入 '<color=#CF001C>git log</color>'
然後執行它來查看提交記錄的內容