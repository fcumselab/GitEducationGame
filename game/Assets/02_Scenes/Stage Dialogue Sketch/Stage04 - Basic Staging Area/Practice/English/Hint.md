Q1/2 (Hint)
same as Tutorial

Q1 (Ans)
same as Tutorial

Q2 (Ans)
path: Home > Shopping_Website_Project

Q3 (Hint)
Looks like you are not quite sure how to achieve the objective for this stage.
Here are some hints to help you:

1. Among Git commands, there are specific commands
   to add files to or remove them from the staging area.
   Both require you to append the target file name after the command.
   Please look them up in the Game Manual for details.

2. After confirming the target file names,
   please execute these commands in the input command field of the 'Command Line' window.

3. Please check the contents of 'Requirements.txt'.
   Based on the requirements,
   add or remove the specified files to/from the staging area.

4. Execute the command that displays the staging area status.
   The system will compare it with the requirements in 'Requirements.txt'.
   If they match, the current objective will be cleared.

Q3 (Ans)
To complete the objective for this stage,
please first click on 'Requirements.txt' to view the current requirements.

You will find that only the 'Member_Data.csv' file
should not be added to the staging area,
while all other files should be added.

First, please navigate your command line path to
'<color=#CF001C>Home > Shopping_Website_Project'</color>.
This path contains all of our project files.

To add the 'Website_Homepage.html' file to Git's staging area,
you need to execute the
'<color=#CF001C>git add Website_Homepage.html</color>' command.

Next, to add the 'Login_System.js' file to Git's staging area,
you need to execute the
'<color=#CF001C>git add Login_System.js</color>' command.

Finally, the 'Member_Data.csv' file should be removed from Git's staging area.
You need to execute the
'<color=#CF001C>git reset Member_Data.csv</color>' command.

Once all the files have been added to or removed from the staging area
according to the requirements,
please execute '<color=#CF001C>git status</color>'.
This command allows you to view what is currently inside the staging area.

When you execute this command,
the system will compare the status with the requirements in 'Requirements.txt'.
If they match, the current objective is cleared!
