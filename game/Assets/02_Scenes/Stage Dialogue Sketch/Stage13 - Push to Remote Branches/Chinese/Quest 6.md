很好！
您使用 'git push 遠端儲存庫別名 本地分支名稱' 指令
將 'update-readme' 本地分支上傳至遠端儲存庫 'origin' 中

查看 '提交記錄' 視窗
發現遠端新增了 'origin/update-readme' 分支
其提交記錄和本地 'update-readme' 分支相同

上傳至遠端後
就可以在 Git 服務平台查看更新後的內容

在首頁上點選分支選單
並選擇 'update-readme' 分支
視窗中將顯示 'update-readme' 分支的內容

選擇分支後
頁面上的 'README' 的文件有著我們新增的內容
表示我們執行的動作是成功的

學會了如何創建遠端分支後
現在來學習如何刪除遠端分支

在 'git push 遠端儲存庫別名 本地分支名稱' 中加入 '-d' 符號 (Delete 刪除)
組成以下指令：
'<color=#CF001C>git push -d 遠端儲存庫別名 分支名稱</color>'
就可以删除指定遠端儲存庫的遠端分支

如果要刪除 'update-readme' 遠端分支
執行 '<color=#CF001C>git push -d origin update-readme</color>'
就可以完成動作了