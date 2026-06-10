開頭：
Hello there!
Welcome to the 'Switching Project Versions' stage!
The current mode is 'Practice Mode'.

In this mode,
you will need to apply what you learned in Tutorial Mode
to complete the stage objectives on your own.

However, if you run into any trouble,
you can always click the Game Manual or the chat button to get help.

Next, let's set up the simulated environment for this stage.

In this stage,
you will need to use commands for switching commits and branches
to inspect the file modification history of this project.

This time, please pay attention to the story background description.
You will need to switch to an older commit to find the bug in the code,
and finally return to the 'master' branch to fix it.

As long as you follow the currently displayed objectives,
you will definitely clear this stage.
Good luck!

結尾：
Excellent job!
You have successfully fixed the bug in the BMI calculator!

By clearing this stage,
you have become much more familiar with the commands
for switching commits and branches.

This stage also marks the final level of the 'Git Basics' theme.
You will continue your Git journey in the 'Branch Management' theme.

The commands used in this stage
will be used very frequently in the upcoming levels.

Finally, congratulations on clearing this stage!
You did great!

環境：

Q1 hint
Looks like you are not quite sure how to open a file to view its content.
Here are some hints to help you reach your objective:

1 Open the 'File Manager' window.
The button to open this window can be found in the game's toolbar.

2 Locate the file you want to open within the project.

3 Left-click on the file to open the 'File Content' window.

Q1 ans
To verify the content of the 'BMI_Calculator.c' file,
you first need to find it in the 'File Manager' window.

First, locate and click the 'File Manager' button in the toolbar at the bottom of the screen.
This will open the 'File Manager' window.

Under the path '<color=#CF001C>Home</color>',
you will see 'BMI_Calculator.c'.
Please left-click on it to view the content inside the file.

Q2 hint
If you are unsure how to switch to a different commit version,
here are some hints to help you reach your objective:

1 Check the 'Commit History' window
to find the historical version you want to switch to from all the commits.

2 Click on the commit icon to bring up a pop-up window with the details of that commit.

3 Among Git commands,
there is a specific command to switch to a target commit version.
You need to append the specified Commit ID after the command.
Please look it up in the Game Manual for details.

4 After confirming the target Commit ID,
please execute this command in the input command field of the 'Command Line' window.

5 Once you have finished switching the commit,
please remember to verify the status of the commit history.

Q2 ans
To switch to the 2nd commit version and check
if the bug originates from this commit,
you need to execute the '<color=#CF001C>git checkout Commit_ID</color>' command.

First, in the 'Command Line' window,
please ensure that the path is set to the project.
The project for this stage is located at the '<color=#CF001C>Home</color>' path.
(You should be able to see the hidden .git folder.)

Next, execute the '<color=#CF001C>git log</color>' command.
This command is used to view all the commit history.

Please click on the 2nd commit icon.
The pop-up window that appears will contain the ID of this commit.
Let's assume this Commit ID is 'abc123'.

Return to the 'Command Line' window and execute the
'<color=#CF001C>git checkout abc123</color>' command.
This will allow you to switch the current project version to the 2nd version.

Once you have finished switching the commit,
please use '<color=#CF001C>git log</color>'
to verify the status of the commit history.

Q3 hint
If you are unsure how to switch branches,
here are some hints to help you reach your objective:

1 Check the 'Commit History' window
to find out the names of all the branches currently in the repository.

2 Among Git commands,
there is a specific command to switch to a designated branch.
You need to append the specified branch name after the command.
Please look it up in the Game Manual for details.

3 Once you have switched to that branch,
please remember to verify the status of the commit history.

Q3 ans
To switch to the 'master' branch to fix the bug at the correct location,
you need to execute the '<color=#CF001C>git checkout master</color>' command.

Next, execute the '<color=#CF001C>git checkout master</color>' command.
This will move your current project version back to the 'master' branch.
The latest version of 'master' is also the 3rd commit.

Once you have finished switching the branch,
please use '<color=#CF001C>git log</color>' to verify the status of the commit history.

Q4 hint
Looks like you are having some trouble with the current objective.
Here are some hints to help you:

1 Based on the version differences between the different commits,
you can figure out which piece of content needs to be modified.

2 Click on the file you want to edit in the 'File Manager' window
to open the 'File Content' window.

3 In this game, content that needs to be modified will be
marked with <color=#CF001C>(to be modified).
Please find those lines and get ready to edit them.

4 There is a pencil icon on the left side of each line.
Clicking it will allow you to modify the content of that line.

Q4 ans
Only after returning to the branch
so we can modify and correct the erroneous content.

First, please open the 'BMI_Calculator.c' file.
You can view the latest content on the 'master' branch.

After browsing the previous commits,
we discovered that in the 3rd commit,
an error occurred in the 'BMI Calculation Code'.

Therefore, please locate the pencil icon on the left side of the 'BMI Calculation Code' line.
Click it to modify and correct this file content.
