開頭：
Hello there!
Welcome to the 'Auto Merging' stage!
The current mode is 'Practice Mode'.

In this mode,
you will need to apply what you learned in Tutorial Mode
to complete the stage objectives on your own.

However, if you run into any trouble,
you can always click the Game Manual or the chat button to get help.

Next, let's set up the simulated environment for this stage.

In this stage, you will need to continue developing the game logic,
and finally merge all the completed development branches back into the main branch.

Next, please follow the stage instructions to complete the objectives.
Good luck!

Q4 中途劇情：
很好！您完成了遊戲邏輯的開發

接下來，請您將所有分支合併回主分支
請先從團隊成員的分支開始

結尾：
非常好！
您完成了本次的遊戲關卡

您成功地合併 'ui-design' 和 'java-activity' 至主分支
最後也刪除這些已經完成目標的分支

在這個關卡中，您遇到了 '快進合併' 和 '自動合併' 的情景
在下一關卡中，我們將遇到合併衝突的情況

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
   詳細請通過遊戲手冊查找

4. 創建完成後，請使用切換分支的指令
   它需要在後方加入指定的分支名稱
   詳細請通過遊戲手冊查找

5. 當切換至該分支後
   請記得確認提交記錄的狀態

Q2 ans
(前面內容相同)

完成創建後，請通過 '<color=#CF001C>git checkout new-article</color>' 指令
將專案切換到 'new-article' 分支上

當完成切換分支後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態

Q5 Ans
為了開始將開發的內容一一合併至 'master' 分支
您需要執行 '<color=#CF001C>git checkout master</color>' 指令
來切換到 'master' 分支

First, please make sure your command line path is set to the project.
The project for this stage is located at the
'<color=#CF001C>Home</color>' path.
(You should be able to see the hidden .git folder.)

接著，執行 '<color=#CF001C>git checkout master</color>' 指令
這樣就可以將當前專案版本移動回 'master' 分支

當完成切換分支後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態
