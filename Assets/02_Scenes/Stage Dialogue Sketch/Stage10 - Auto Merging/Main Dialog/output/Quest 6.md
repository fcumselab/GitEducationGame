好的，在切換到 "master" 分支後\r\n讓我們先來合併 "new-design" 分支的內容
首先，我們先來判斷\r\n"master" 和 "new-design" 這兩個分支在進行合併時\r\n屬於哪一種合併模式
您可以將滑鼠點擊指定的分支列表\r\n來查看不同分支的 "提交記錄"
"new-design" 基於 "master" 分支\r\n創建了兩個新的 "提交"
而 "master" 分支並沒有再新增 "提交"\r\n兩個分支呈現成一條直線，沒有分叉
根據以上的狀態\r\n我們可以判斷這是 "快進合併"
在這種模式下\r\n"master" 分支會更新 "提交記錄"\r\n最後和 "new-design" 分支的 "提交記錄" 相同 
接下來，讓我們來使用 "git merge new-design" 指令\r\n來進行分支的合併吧
合併之後，請更新 "提交記錄" 的狀態\r\n來觀察 "快進合併" 造成的影響