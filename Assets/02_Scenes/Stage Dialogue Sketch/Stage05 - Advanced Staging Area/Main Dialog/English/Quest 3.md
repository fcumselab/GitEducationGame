Alright, you've opened the 'Staging Area' window. 
Now, let's observe its contents.

You may have noticed that in the 'Untracked' area,
there are only two items: 'Folder B/' and 'File C.txt'.

Next, let's understand how Git determines whether a file should be tracked.

First, let's start with 'Folder A'. 
It doesn't have files.

Git's primary purpose is to track and manage files, 
so it doesn't need to track an empty folder.

Next is 'Folder B', which contains other untracked files.

When there are no tracked files in 'Folder B', 
it is simply displayed as 'Folder B/'.

The final, file 'File C.txt'. 
According to Git's default rules,
all '<color=#CF001C>non-folder type</color>' files will be tracked.

After understanding how Git tracks files,
let's move on to learning about Git commands.

The 'git add' and 'git reset' commands we learned in the previous stage.
They are not only can add '<color=#CF001C>target file name</color>', but also add '<color=#CF001C>folder name</color>'.

This is the high flexibility of Git commands.
Most commands can include additional parameters to achieve different functions.

To help you effectively learn these commands.
This game will focus on the key Git commands.
You only need to learn the commands mentioned in the stage.

Back to the topic,
this time please use '<color=#CF001C>git add folder_name</color>' to execute the operation.

Try executing the command with 'Folder B'.
And don't forget to use 'git status' to check the updated 'Staging Area' status.