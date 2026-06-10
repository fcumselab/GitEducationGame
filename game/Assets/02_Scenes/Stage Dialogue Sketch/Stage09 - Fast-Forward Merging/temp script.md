開頭：
Hello there!
Welcome to the 'Fast-Forward Merging' stage!
The current mode is 'Practice Mode'.

In this mode,
you will need to apply what you learned in Tutorial Mode
to complete the stage objectives on your own.

However, if you run into any trouble,
you can always click the Game Manual or the chat button to get help.

Next, let's set up the simulated environment for this stage.

本次關卡需要您在一個新的分支上開發功能
並且團隊成員會要求您將分支內容合併回主分支

接下來，就請您跟著關卡指示完成目標
加油！

Q5 中途劇情：
您為角色行為增加了攻擊偵測
團隊成員認為這些內容可以用在遊戲上
並請您將它合併到主分支上

現在，請您試著將開發分支合併至 'master' 分支

結尾：
非常好！
您完成了本次的遊戲關卡

您成功地新增了角色行動行為
並將開發分支的內容合併到主分支上

本次遇到的情景為快進合併
在下一關卡中，我們將遇到自動合併的情況

Finally, congratulations on clearing this stage!
You did great!

Q2 hint
To ensure that the completed content remains unaffected,
we can execute the '<color=#CF001C>git branch Branch_Name</color>' command.
This command is used to create a branch.

在創建分支前，請確保目前位於 'master' 分支
您可以通過 'git checkout master' 指令
來移動至 'master' 分支上

Q2 ans

5. 當創建分支後
   請記得確認提交記錄的狀態

Q5 ans
為了開始將 'new-character-action' 分支合併至 'master'
您需要執行 '<color=#CF001C>git checkout master</color>' 指令
來切換到 'master' 分支

First, please make sure your command line path is set to the project.
The project for this stage is located at the
'<color=#CF001C>Home > Character_Project</color>' path.
(You should be able to see the hidden .git folder.)

Q6 hint
如果您不確定如何合併分支
這裡有一些提示可以幫助您完成目標：

1 確認要合併的兩個分支間的關係：
<color=#CF001C>輸入分支的提交記錄合併到目前專案所在的分支</color>

2 Git 指令中，有個切換至合併分支的指令
需要在指令後方加入分支名稱
詳細請通過遊戲手冊查找

3 在確認當前專案位於的分支正確後
請執行指令來合併分支

4 合併分支後
請記得確認提交記錄的狀態

Q6 ans

為了將 'new-character-action' 分支合併至 'master'
您需要執行 '<color=#CF001C>git merge 分支名稱</color>' 指令

First, please make sure your command line path is set to the project.
The project for this stage is located at the
'<color=#CF001C>Home > Character_Project</color>' path.
(You should be able to see the hidden .git folder.)

在合併之前，請先確認當前專案版本已經位於 'master' 分支
您可以通過 '<color=#CF001C>git checkout master</color>' 指令
來切換到 'master' 分支

接著，執行 '<color=#CF001C>git merge new-character-action</color>' 指令
這樣就可以將 'new-character-action' 合併至 'master' 分支

當合併成功後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態
