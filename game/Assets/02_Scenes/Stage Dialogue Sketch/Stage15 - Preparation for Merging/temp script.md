開頭：
Hello there!
Welcome to the 'Preparation for Merging' stage!
The current mode is 'Practice Mode'.

In this mode,
you will need to apply what you learned in Tutorial Mode
to complete the stage objectives on your own.

However, if you run into any trouble,
you can always click the Game Manual or the chat button to get help.

Next, let's set up the simulated environment for this stage.

在本次關卡裡，您需要進行合併前的確認
以確保當遠端 'update-readme' 分支合併至遠端主分支時
不會造成合併衝突

關於合併前的確認流程
您可以查閱遊戲手冊來複習

Next, please follow the stage instructions to complete the objectives.
Good luck!

Q5 中途內容:
很好！您順利地完成 '合併前的準備' 流程

由於您和其他成員修改的檔案內容不同
所以並沒有造成合併衝突

不過請您開啟合併後的兩個專案檔案
以確認合併是否成功

確認後，請您將完成合併的 'update-readme' 分支上傳到遠端
以為創建 pull request 做準備

結尾：
Excellent!
You have completed this stage.

經過本次的練習
您應該更熟悉如何進行合併前的準備
以為創建 Pull Request 做準備

在下一關卡中
您將扮演創建 Pull Request 的角色
解決在合併請求過程中遇到的問題

Finally, congratulations on clearing this stage!
You did great!

環境：

Q2 ans
為了順利將開發內容合併至遠端 'master' 分支
需要先進行 '合併前的確認'

首先需要同步 'master' 分支
因此您需要先通過 '<color=#CF001C>git checkout master</color>' 指令
先確認專案目前已經切換到 'master' 上

Q4 ans
為了順利將開發內容合併至遠端 'master' 分支
需要先進行 '合併前的確認'

如果要將 'master' 分支合併至 'update-readme'
您需要執行 '<color=#CF001C>git merge 分支名稱</color>' 指令

在合併之前，請先確認當前專案版本已經位於 'update-readme' 分支
您可以通過 '<color=#CF001C>git checkout update-readme</color>' 指令
來切換到 'update-readme' 分支

接著，執行 '<color=#CF001C>git merge master</color>' 指令
這樣就可以將 'master' 合併至 'update-readme' 分支

當合併成功後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態

Q5 hint
If you are unsure how to reach your objective,
here are some hints to help you:

1. 在本遊戲中，只需要開啟所有檔案確認一次後即可完成目標

Q5 ans
根據本次背景故事
我們需要開啟 'README.md' 和 '主程式.cpp'
來查看它是否包含了兩個分支的內容

請您從 '檔案管理' 視窗中
找到它們並點擊它

Q6 ans

當我們順利完成 '合併前的確認' 流程後
即可將合併完成的本地分支上傳至遠端
以為創建 Pull Request 做準備
