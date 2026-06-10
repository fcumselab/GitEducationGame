Q2 (Hint)

If you are unsure how to check the contents of the 'Staging Area',
here are some hints to help you reach your objective:

1. Make sure your 'Command Line' window is already open.

2. Verify that your command line path is inside the project directory.
   (You should be able to see the hidden .git folder.)

3. Among Git commands,
   there is a specific command used to check the status of the staging area.
   Please look it up in the Game Manual for details.

4. Please execute this command in
   the input command field of the 'Command Line' window.

Q2 (Ans)
To check the contents of the 'Staging Area',
you need to use the '<color=#CF001C>git status</color>' command.
This command allows you to view what is currently inside the staging area.

In the 'Command Line' window,
please ensure that the path is set to the project.
The project for this stage is located at the '<color=#CF001C>Home</color>' path.
(You should be able to see the hidden .git folder.)

After confirming, go to the input command field of the 'Command Line' window,
type '<color=#CF001C>git status</color>',
and then execute it to check the status of the staging area.

=======
Q3 (Hint)
Looks like you are not quite sure how to add files to the 'Staging Area'.
Here are some hints to help you reach your objective:

1. Make sure your 'Command Line' window is already open.

2. Navigate through the 'File Manager' window
   until you can see the target file in the window.
   (The command line path will move along with it.)

3. Among Git commands, there is a specific command to move files into the staging area.
   You need to append the name of the file you want to add after the command
   Please look it up in the Game Manual for details.

4. After confirming the target file name,
   please execute this command in the input command field of the 'Command Line' window.

Q3 (Ans)
To add the 'Class_Notes.txt' file to Git's staging area,
you need to execute the '<color=#CF001C>git add Class_Notes.txt</color>' command.

First, please navigate your command line path to '<color=#CF001C>Home</color>'.
This path contains the file '<color=#CF001C>Class_Notes.txt</color>'
that we want to add to the staging area.

Once you have navigated to the location,
type and execute the following command in the input command field:
'<color=#CF001C>git add Class_Notes.txt</color>'
This will add 'Class_Notes.txt' into the staging area.

Q4 (Hint)
If you are unsure how to check the file tracking status,
here are some hints to help you reach your objective:

some are same as Q2

3. Among Git commands,
   there is a specific command used to check the status of the staging area.
   The tracking status of your files will be displayed within that area
   Please look it up in the Game Manual for details.

some are same as Q2

Q4 (Ans)
If you need to verify that your files have been successfully added to the 'Staged' status,
we likewise need to use the '<color=#CF001C>git status</color>' command.
This command allows you to view what is currently inside the staging area.

some are same as Q2

Q5 (hint)
If you are unsure how to remove a file from the 'Staged' status,
here are some hints to help you reach your objective:

some are same as Q3

3 Among Git commands, there is a specific command to remove files from the staging area.
You need to append the name of the file you want to remove after the command.
Please look it up in the Game Manual for details.

some are same as Q3

5. After completing the operation,
   don't forget to check the status of the staging area
   to confirm the file has been removed.

Q5 (ans)

To remove the 'Class_Notes.txt' file from the staging area,
you need to execute the
'<color=#CF001C>git reset Class_Notes.txt</color>' command.

First, please navigate your command line path to '<color=#CF001C>Home</color>'.
This path contains the file '<color=#CF001C>Class_Notes.txt</color>' that we previously added to the staging area.

Once you have navigated to the location,
type and execute the following command in the input command field:
'<color=#CF001C>git reset Class_Notes.txt</color>'
This will remove 'Class_Notes.txt' from the staging area.

After completing this operation,
please execute '<color=#CF001C>git status</color>' again to check the status of the staging area
to ensure that the 'Class_Notes.txt' file has been successfully removed.
