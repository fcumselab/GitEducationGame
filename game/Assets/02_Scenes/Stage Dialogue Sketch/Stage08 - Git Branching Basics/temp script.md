開頭：
Hello there!
Welcome to the 'Git Branching Basics Tutorial' stage!
The current mode is 'Practice Mode'.

In this mode,
you will need to apply what you learned in Tutorial Mode
to complete the stage objectives on your own.

However, if you run into any trouble,
you can always click the Game Manual or the chat button to get help.

Next, let's set up the simulated environment for this stage.

In this stage, you will need to use commands related to branching.
You will practice:
creating, switching, viewing, and deleting branch commands throughout this level.

Next, please follow the stage objectives
and try to design a puzzle game level.
Good luck!

Q8 中途劇情：
You have successfully designed a test level!
The team members reviewed your level
and believe you now understand how the game should be designed.

However, since this is just a test level,
the team wants you to delete this branch for now.

結尾：
Excellent job!
You have completed the objectives for this stage!

Although you ultimately deleted the game level you designed,
you successfully utilized the basic branch commands during this process.

In the next stage,
we will try to merge the content of a development branch into the main branch.

If we design a game level
and the team members agree that it can become a part of the game,
then that level can be merged into the main game.

環境：

Q1 ans
To check the commit history of the current project,
you need to use the '<color=#CF001C>git log</color>' command.

In the 'Command Line' window,
please ensure that the path is set to the project.
The project for this stage is located at the
'<color=#CF001C>Home > Game_Project</color>' path.
(You should be able to see the hidden .git folder.)

After confirming, go to the input command field of the 'Command Line' window,
type '<color=#CF001C>git log</color>',
and then execute it to view the contents of the commit history.

Q2 hint
If you are unsure how to create a branch,
here are some hints to help you reach your objective:

1 Make sure your 'Command Line' window is already open.

2 Verify that your command line path is inside the project directory.
(You should be able to see the hidden .git folder.)

3 Among Git commands,
there is a specific command used to create a branch.
Additionally, the game provides an auto-fill feature for the branch name.
Please look it up in the Game Manual for details.

4 Before executing the command,
please verify that your current commit matches the current objective.

Q2 ans
To ensure that the completed content remains unaffected,
we can execute the '<color=#CF001C>git branch Branch_Name</color>' command.
This command is used to create a branch.

First, please make sure your command line path is set to the project.
The project for this stage is located at the
'<color=#CF001C>Home > Game_Project</color>' path.
(You should be able to see the hidden .git folder.)

Next, please type '<color=#CF001C>git branch</color>'.
Please remember to include a trailing space after the command.

In this scenario,
use the auto fill feature or press the Tab key to trigger auto-message feature.

Upon doing so, a pop-up window will appear.
Please select the most appropriate branch name from the three options provided.

Once you have chosen correctly,
execute the command '<color=#CF001C>git branch Chosen_Branch_Name</color>'
to complete your objective.

Q4 Hint
如果您不確定如何查看所有的本地分支
這裡有一些提示可以幫助您完成目標：

3. Git 指令中，有個用於查看本地分支的指令
   詳細請通過遊戲手冊查找
4. 執行後，會出現儲存庫包含的所有分支名稱
   以及當前所在的分支或提交 ID

Q4 ans
為了查看本地儲存庫中的所有分支
您需要執行 '<color=#CF001C>git branch</color>' 指令
這個指令除了可以顯示所有的本地分支名稱
還可以得知當前專案位於的版本

（這個有重複內容）

接著，請輸入並執行 '<color=#CF001C>git branch</color>' 指令
即可達成目標

Q5 ans
為了切換到新創建的分支 'new-test-level' 以繼續開發專案
您需要執行 '<color=#CF001C>git checkout new-test-level</color>' 指令

（--部分重複內容--）

接著，執行 '<color=#CF001C>git checkout new-test-level</color>' 指令
當前專案版本就會切換至 'new-test-level' 分支
這樣就可以準備在該分支開發新功能

當完成切換分支後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態

Q6 hint

3. 請確認當前目標的要求
   在正確的分支、檔案上新增內容

Q6 ans

為了新增 '測試關卡.scene' 文件中的內容
請先確認您已經在 'new-test-level' 分支上
如果沒有，請先通過 '<color=#CF001C>git checkout new-test-level</color>' 切換分支

接著，在 '檔案管理' 視窗中找到並點擊它來開啟 '檔案內容' 視窗

在 '檔案內容' 視窗中
請點擊文字內容下方的加號按鈕來加入新的文字

Q7 hint

5. 當創建提交後
   請記得確認提交記錄的狀態

Q7 ans
當創建提交後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態

Q8 ans
為了將創建的分支刪除，您需要切換到其他分支上
執行 '<color=#CF001C>git checkout master</color>' 指令
可以將當前專案切換至 'master' 分支上

Q9 hint
如果您不確定如何刪除分支
這裡有一些提示可以幫助您完成目標：

1. 查看 '提交記錄' 視窗
   可以得知目前儲存庫所有的分支名稱

2. Git 指令中，有個刪除指定分支的指令
   需要在指令後方加入指定的分支名稱
   詳細請通過遊戲手冊查找

3. 在刪除指定的分支後
   請記得確認提交記錄的狀態

Q9 ans
根據劇情，我們要刪除剛創建的分支
通過 '<color=#CF001C>git branch -d 分支名稱</color>' 指令
來刪除指定分支

在刪除之前，請先確認當前專案版本已經位於其他分支
您可以通過 '<color=#CF001C>git checkout master</color>' 指令
來切換到 'master' 分支

接下來，請執行 '<color=#CF001C>git branch -d new-test-level</color>'
這樣即可刪除 'new-test-level' 分支

當刪除掉分支後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態
