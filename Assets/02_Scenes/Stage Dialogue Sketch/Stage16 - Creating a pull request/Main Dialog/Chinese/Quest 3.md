在進入 Git 服務平台後
我們要先學習如何創建一個 Pull Request

首先，請在首頁上方的分頁中選擇 'Pull Requests'

選擇該分頁後，
畫面將顯示所有追蹤中的請求
不過目前這個專案還沒有創建任何 Pull Request

請點擊 '創建 Pull Request' 按鈕
來新建合併請求

點擊按鈕後
需要先選擇要合併的兩個目標分支

請注意：
base（基底）表示要被合併的分支
compare（比較）會把修改內容合併到 base 上

因此，要合併到遠端主分支的話
請將 base 設為 'master'
compare 設為 'update-readme'

設定完成後
系統將比較兩個分支的提交記錄
並告訴使用者是否需要進行合併

就和 'git merge' 相同
當兩個分支的內容有衝突
會提醒使用者出現合併衝突

選擇正確的兩個目標分支後
請點擊 '新建 Pull Request' 按鈕
Git 會要求您輸入合併請求訊息

在填寫訊息時
請填入與開發分支有關的內容
讓團隊知道創建這個請求的目的

完成訊息後
請按下 '新建 Pull Request' 按鈕
來創建合併請求