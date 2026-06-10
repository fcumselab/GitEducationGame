開頭：
Hello there!
Welcome to the 'Creating and Pushing Remote Branches' stage!
The current mode is 'Practice Mode'.

In this mode,
you will need to apply what you learned in Tutorial Mode
to complete the stage objectives on your own.

However, if you run into any trouble,
you can always click the Game Manual or the chat button to get help.

Next, let's set up the simulated environment for this stage.

本次關卡需要您創建一個本地分支
並在這個分支上增加 'README' 檔案內容

當您完成開發後，再請您上傳到遠端儲存庫中

Next, please follow the stage instructions to complete the objectives.
Good luck!

Q7 中途內容:
很好！您成功將 'update-readme' 分支上傳至遠端儲存庫了

最後，請記得將本地與遠端的 'test-readme' 分支刪除

結尾：
Excellent!
You have completed this stage.

現在在遠端儲存庫中
包含了寫好 '貪吃蛇遊戲介紹' 的 'update-readme' 分支

在本次關卡中
您掌握了如何將創建、刪除遠端分支

在下一關裡，我們將學習如何獲取遠端分支
以確保本地儲存庫的版本與遠端相同

Finally, congratulations on clearing this stage!
You did great!

環境：

Q2 hint
如果您不確定如何完成本次目標
這裡有一些提示可以協助您：

1. 確認命令行路徑位於專案上

2. 根據故事背景和當前目標
   確認要創建新分支的位置

3. Git 指令中，有個用於創建分支的指令
   並且遊戲提供了自動填入分支名稱的功能
   詳細請通過遊戲手冊查找 4. 創建完成後，請使用切換分支的指令
   它需要在後方加入指定的分支名稱
   詳細請通過遊戲手冊查找

Q2 ans
根據開發目標來分成多個分支
是一個很好的習慣

本次需要在本地儲存庫中創建分支並完成開發

接著，請使用 '<color=#CF001C>git branch 分支名稱</color>'
來創建一個新的分支

完成創建後，
請通過 '<color=#CF001C>git checkout update-readme</color>' 指令
將專案切換到 'update-readme' 分支上

Q6 ans
本地 'update-readme' 分支的功能開發完畢後
即可將本地分支上傳至遠端

上傳遠端儲存庫的指令為：
'<color=#CF001C>git push 遠端儲存庫別名 本地分支名稱</color>' (我這裡遊戲的手冊為remote_repository_alias local_branch_name)

遠端儲存庫別名可以通過 '<color=#CF001C>git remote -v</color>' 得知
一般情況下要上傳的儲存庫別名應該是 'origin'

接下來，請執行 '<color=#CF001C>git push origin update-readme</color>'
即可將本地 'update-readme' 分支上傳至 'origin' 儲存庫中

當上傳完成後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態

Q6 hint
如果您不清楚如何將本地分支上傳到遠端儲存庫
這裡有一些提示可以幫助您完成目標：

1. 確認您目前要上傳到遠端存儲庫的本地分支名稱

2. 確認您要上傳的指定遠端存儲庫別名
   一般情況下是 'origin'
   可以通過 Git 指令來查看儲存庫別名
   詳細請通過遊戲手冊查找

3. 在 Git 指令中，有個將本地分支推入遠端儲存庫的指令
   需要在指令後面分別加入 '<color=#CF001C>遠端存儲庫別名</color>' 和 '<color=#CF001C>本地分支名稱</color>'
   詳細請通過遊戲手冊查找

4. 執行上傳分支指令後
   請記得確認提交記錄的狀態

Q7 hint
如果您不確定如何達成目標
這裡有一些提示可以幫助您：

1. 刪除本地分支和遠端分支使用的指令類型不同

2. 刪除本地分支需要在指令後方加入指定的分支名稱
   詳細請通過遊戲手冊查找

3. 刪除遠端分支需要在指令後方分別加入
   '遠端儲存庫別名' 和 '指定的分支名稱'
   詳細請通過遊戲手冊查找

4. 在刪除指定的分支後
   請記得確認提交記錄的狀態

Q7 ans
當確認遠端 'master' 分支已經有我們開發的功能後
即可刪除本地、遠端的 'update-readme' 分支

首先，'<color=#CF001C>git branch -d 分支名稱</color>' 指令
來刪除本地指定分支

在刪除之前，請先確認當前專案版本已經位於其他分支
您可以通過 '<color=#CF001C>git checkout master</color>' 指令
來切換到 'master' 分支

接下來，請執行 '<color=#CF001C>git branch -d update-readme</color>'
來刪除本地 'update-readme' 分支

接著，通過指令 '<color=#CF001C>git push -d 遠端存儲庫別名 分支名稱</color>'
來刪除遠端儲存庫中的分支

遠端儲存庫別名可以通過 '<color=#CF001C>git remote -v</color>' 得知
一般情況下的儲存庫別名應該是 'origin'

執行 '<color=#CF001C>git push -d origin update-readme</color>'
即可移除遠端儲存庫中的 'update-readme' 分支

當刪除分支完成後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態

T13 Q6 hint
如果您不清楚如何刪除遠端儲存庫中的分支
這裡有一些提示可以幫助您完成目標：

1. 確認目標分支所在的遠端存儲庫別名
   一般情況下是 'origin'
   可以通過 Git 指令來查看儲存庫別名
   詳細請通過遊戲手冊查找

2. 在 Git 指令中，有個將指定遠端儲存庫中的遠端分支刪除的指令
   需要在指令後面分別加入 '<color=#CF001C>遠端存儲庫別名</color>' 和 '<color=#CF001C>分支名稱</color>'
   詳細請通過遊戲手冊查找

3. 執行刪除遠端分支指令後
   請記得確認提交記錄的狀態

T13 Q6 ans
如果想要移除掉遠端儲存庫中的分支
需要使用指令 '<color=#CF001C>git push -d 遠端存儲庫別名 分支名稱</color>'

遠端儲存庫別名可以通過 '<color=#CF001C>git remote -v</color>' 得知
一般情況下的儲存庫別名應該是 'origin'

請開啟 '命令行' 視窗
並執行 '<color=#CF001C>git push -d origin update-readme</color>'
即可移除遠端儲存庫中的 'update-readme' 分支

刪除分支完成後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態
