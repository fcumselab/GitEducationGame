開頭：
Hello there!
Welcome to the 'Keeping Branches in Sync' stage!
The current mode is 'Practice Mode'.

In this mode,
you will need to apply what you learned in Tutorial Mode
to complete the stage objectives on your own.

However, if you run into any trouble,
you can always click the Game Manual or the chat button to get help.

Next, let's set up the simulated environment for this stage.

本次關卡需要您同步 'update-readme' 分支
以繼續之前的開發進度

Next, please follow the stage instructions to complete the objectives.
Good luck!

Q7 中途內容:
很好！您成功獲得遠端 'update-readme' 分支
現在可以繼續之前未完成的工作了

突然，同學 A 傳訊息給您
想詢問您關於程式方面的問題

如果要查看並測試對方的程式碼
我們可以在這台電腦獲取對方的遠端分支

結尾：
Excellent!
You have completed this stage.

現在這台電腦的 'update-readme' 分支
已經與遠端的 'update-readme' 分支同步了

並且您通過指令獲得了團隊成員的分支

在下一關裡，我們將學習 '合併前的準備' 流程
這是為了創建 Pull Request 做準備

Finally, congratulations on clearing this stage!
You did great!

環境：

Q2 hint
如果您不確定如何同步本地、遠端分支
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

5. 當同步分支後
   請記得確認提交記錄的狀態

Q2 ans
為了順利同步遠端分支
請先執行 '<color=#CF001C>git checkout master</color>' 指令
確認專案目前已經切換到 'master' 上

同步分支的指令為：
'<color=#CF001C>git pull 遠端儲存庫別名 分支名稱</color>'

遠端儲存庫別名可以通過 '<color=#CF001C>git remote -v</color>' 得知
一般情況下要上傳的儲存庫別名應該是 'origin'

接下來，請執行同步分支指令
'<color=#CF001C>git pull origin master</color>'

完成後，請執行 '<color=#CF001C>git log</color>' 指令
查看提交記錄的目前的狀態

Q3 hint
如果您不確定如何查看遠端儲存庫的所有分支
這裡有一些提示可以幫助您完成目標：

1. 確認您的 '命令行' 視窗已經開啟

2. 確認您的命令行路徑位於專案上
   （可以看到 .git 隱藏資料夾）

3. Git 指令中，有個用於查看遠端儲存庫分支的指令
   這個指令類型後方加入 '<color=#CF001C>-r</color>' 即可查看
   詳細請通過遊戲手冊查找

4. 執行後，會出現所有遠端儲存庫遠端分支

Q3 ans
為了確認對方的開發分支
我們需要通過 '<color=#CF001C>git branch -r</color>' 指令
或是到 GitHub 上，查看專案中的所有遠端分支名稱

請在 '命令行' 視窗中的指令輸入區域
輸入 '<color=#CF001C>git branch -r</color>'
然後執行它來查看所有遠端分支的名稱

Q4 hint
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

Q4 ans
為了獲取對方的遠端分支 'main-script'
需要通過 '<color=#CF001C>git branch 本地分支名 遠端分支名</color>' 指令
來獲取分支到本地電腦上

通過 '<color=#CF001C>git branch -r</color>' 指令
可以得知遠端分支的全名 '<color=#CF001C>origin/main-script</color>'

執行 '<color=#CF001C>git branch main-script origin/main-script</color>' 指令
即可成功創建本地 'main-script' 分支
並獲得與遠端分支相同的內容

完成後，請在 '命令行' 視窗中的指令輸入區域
輸入 '<color=#CF001C>git log</color>'
然後執行它來查看提交記錄的內容
