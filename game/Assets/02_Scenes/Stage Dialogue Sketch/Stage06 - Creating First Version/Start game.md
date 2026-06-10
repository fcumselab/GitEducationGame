開頭：
Hello there!
Welcome to the 'Creating Your First Git Project Version' stage!
The current mode is 'Practice Mode'.

In this mode,
you will need to apply what you learned in Tutorial Mode
to complete the stage objectives on your own.

However, if you run into any trouble,
you can always click the Game Manual or the chat button to get help.

Next, let's set up the simulated environment for this stage.

In this stage,
you will need to complete the process of 'creating a new commit'.
This process will cover all the knowledge and commands you learned in the previous stages.

Once you successfully clear this stage,
you will have learned how to create multiple versions for your project,
taking a major step forward in using Git to manage projects in the future.
Good luck!

結尾：
Excellent job!
You have successfully completed the process of 'creating a new commit' twice!

This 'Git_Learning_Diary.txt' now contains two committed versions.You can view them in the 'Commit History' window.

In the next stage,
we will learn how to switch between different project versions using commands.

Once you master this skill,
you will be able to switch freely between the two commits of your 'Git_Learning_Diary.txt'.

Finally, congratulations on clearing this stage!
You did great!

=============
暫存：

Q3 hint

If you are unsure how to create a new commit to save your project version,
here are some hints to help you reach your objective:

1 Make sure all the files you want to save are already in the staging area.
If not, use the command to move the files into it.

2 Open the 'Command Line' window
and verify that the path is currently set to the project.

3 Among Git commands,
there is a specific command to push files from the staging area into the repository.
Additionally, the game provides an auto-fill feature for the commit message.
Please look it up in the Game Manual for details.

4 After completing the commit message,
execute the command to create a new commit.

Q3 ans
To commit the modified content and successfully log your changes,
first, please make sure 'Git_Learning_Diary.txt' is in the staging area.
You can use '<color=#CF001C>git status</color>' to double-check.

If the target is not in the staging area,
use '<color=#CF001C>git add Git_Learning_Diary.txt</color>' to move it in.

Next, please type '<color=#CF001C>git commit -m ""</color>'.
This is the command structure used to create a new commit.

In this scenario,
use the auto fill feature or press the Tab key to trigger auto-message feature.

Upon doing so, a pop-up window will appear.
Please select the most appropriate commit message from the three options provided.

Once you have chosen correctly,
execute this command to complete your objective.

Q4 (hint)

Q4 (ans)
為了確認您的 'Git學習日記.txt' 檔案已經成功創建一個新的版本
您需要使用 '<color=#CF001C>git log</color>' 指令
這個指令可以查看您專案的提交歷史記錄

在 '命令行' 視窗中，請確保其路徑位於專案上
本次關卡的專案創建於 '<color=#CF001C>Home > 課程資料夾</color>' 路徑上
（可以看到 .git 隱藏資料夾）

確認完後，請在 '命令行' 視窗中的指令輸入區域
輸入 '<color=#CF001C>git log</color>'
然後執行它來查看提交記錄的內容
