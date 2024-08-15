# Q1 開啟 '提交記錄' 視窗
Q1（提示）：
如果您不確定如何查看提交記錄
這裡有一些提示可以幫助您完成目標：

1. 確認您的 '命令行' 視窗已經開啟

2. 確認您的命令行路徑位於專案上
   （可以看到 .git 隱藏資料夾）

3. Git 指令中，有個用於查看提交記錄狀態的指令
   詳細請通過遊戲手冊查找

4. 執行後，會出現 '提交記錄' 視窗
   您將可以看到目前儲存庫中的版本記錄

Q1（解答）：
為了確認當前專案的提交歷史記錄
您需要使用 '<color=#CF001C>git log</color>' 指令

在 '命令行' 視窗中，請確保其路徑位於專案上
本次關卡的專案創建於 '<color=#CF001C>Home</color>' 路徑上
（可以看到 .git 隱藏資料夾）

確認完後，請在 '命令行' 視窗中的指令輸入區域
輸入 '<color=#CF001C>git log</color>'
然後執行它來查看提交記錄的內容


# Q2 合併 'add-score-feature' 分支，並更新 '提交記錄' 視窗"
Q2（提示）：
如果您不確定如何合併分支
這裡有一些提示可以幫助您完成目標：

1. 確認要合併的兩個分支間的關係：
   <color=#CF001C>輸入分支的提交記錄合併到目前專案所在的分支</color>

2. Git 指令中，有個切換至合併分支的指令
   需要在指令後方加入分支名稱
   詳細請通過遊戲手冊查找

3. 在確認當前專案位於的分支正確後
   請執行指令來合併分支

4. 合併分支後
   請記得確認提交記錄的狀態

Q2（解答）：
為了將 'add-score-feature' 分支合併至 'master'
您需要執行 '<color=#CF001C>git merge 分支名稱</color>' 指令

首先在 '命令行' 視窗中，請確保其路徑位於專案上
本次關卡的專案創建於 '<color=#CF001C>Home</color>' 路徑上
（可以看到 .git 隱藏資料夾）

在合併之前，請先確認當前專案版本已經位於 'master' 分支
您可以通過 '<color=#CF001C>git checkout master</color>' 指令
來切換到 'master' 分支

接著，執行 '<color=#CF001C>git merge add-score-feature</color>' 指令
這樣就可以將 'add-score-feature' 合併至 'master' 分支

當合併成功後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態

# Q3 合併 'reset-score-feature' 分支，並更新 '提交記錄' 視窗"
Q3（提示）：
如果您不確定如何合併分支
這裡有一些提示可以幫助您完成目標：

1. 確認要合併的兩個分支間的關係：
   <color=#CF001C>輸入分支的提交記錄合併到目前專案所在的分支</color>

2. Git 指令中，有個切換至合併分支的指令
   需要在指令後方加入分支名稱
   詳細請通過遊戲手冊查找

3. 在確認當前專案位於的分支正確後
   請執行指令來合併分支

4. 合併分支後
   系統會提醒您合併發生衝突
   請記得確認提交記錄的狀態

Q3（解答）：
為了將 'reset-score-feature' 分支合併至 'master'
您需要執行 '<color=#CF001C>git merge 分支名稱</color>' 指令

首先在 '命令行' 視窗中，請確保其路徑位於專案上
本次關卡的專案創建於 '<color=#CF001C>Home</color>' 路徑上
（可以看到 .git 隱藏資料夾）

在合併之前，請先確認當前專案版本已經位於 'master' 分支
您可以通過 '<color=#CF001C>git checkout master</color>' 指令
來切換到 'master' 分支

接著，執行 '<color=#CF001C>git merge reset-score-feature</color>' 指令
這樣就可以將 'reset-score-feature' 合併至 'master' 分支

當合併成功後
系統會提醒您合併發生衝突
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態

# Q4 開啟 '遊戲程式.js' 檔案內容
Q4（提示）：
看來您不太清楚如何開啟檔案以查看文件內容
這裡有一些提示可以幫助您完成目標：

1. 開啟 '檔案管理' 視窗
   開啟視窗的按鈕可以在遊戲中的工具欄找到

2. 在專案中找到想要開啟的檔案

3. 對著檔案點擊滑鼠左鍵即可開啟 '檔案內容' 視窗

Q4（解答）：
為了確認 '遊戲程式.js' 檔案的內容
您需要先在 '檔案管理' 視窗找到它

首先，在遊戲畫面下方的工具欄中找到 '檔案管理' 按鈕並點擊它
這會開啟 '檔案管理' 視窗

在路徑 '<color=#CF001C>Home</color>' 下
可以看到 '遊戲程式.js'
請用左鍵點擊它來查看檔案裡面的內容

# Q5 解決 '遊戲程式.js' 文件衝突
Q5（提示）：
看來您不太清楚如何解決文件衝突
這裡有一些提示可以幫助您完成目標：

1. 確認當前目標劇情文字與故事背景
   點擊左下角的對話按鈕來回顧劇情文字

2. 根據指示確認文件中的衝突內容
   哪些要保留、哪些要留下

3. 移除不需要的部分（刪除它們）：
   某個分支的修改內容
   由 <color=#CF001C><</color>、<color=#CF001C>=</color>、<color=#CF001C>></color> 三行組成的行數

4. 如果想要了解更多
   請參考遊戲手冊中的 '解決合併衝突' 條目
   
Q5（解答）：
當合併發生衝突時
我們必須把所有的文件衝突解決後才可以繼續合併分支

首先，在 '遊戲程式.js' 檔案內容中
出現了兩個分支的修改內容
以及三行由 <color=#CF001C><</color>、<color=#CF001C>=</color>、<color=#CF001C>></color> 組成的行數

'<<< HEAD' 和 '===' 中的 '增加分數功能'
表示在 HEAD，也就是 'master' 分支上的修改內容

'>>> reset-score-feature' 和 '===' 中的 '重置分數功能'
表示在 'reset-score-feature' 分支上的修改內容

根據故事劇情，我們應該保留兩個分支的內容
以讓網頁遊戲有兩個按鈕功能

因此，只需要移除
<color=#CF001C><</color>、<color=#CF001C>=</color>、<color=#CF001C>></color> 組成的三行即可

# Q6 解決 '遊戲網頁.html' 文件衝突
Q6（提示）：
同Q5

Q6（解答）：
當合併發生衝突時
我們必須把所有的文件衝突解決後才可以繼續合併分支

首先，在 '遊戲網頁.html' 檔案內容中
出現了兩個分支的修改內容
以及三行由 <color=#CF001C><</color>、<color=#CF001C>=</color>、<color=#CF001C>></color> 組成的行數

'<<< HEAD' 和 '===' 中的 '增加分數按鈕'
表示在 HEAD，也就是 'master' 分支上的修改內容

'>>> reset-score-feature' 和 '===' 中的 '重置分數按鈕'
表示在 'reset-score-feature' 分支上的修改內容

根據故事劇情，我們應該保留兩個分支的內容
以讓網頁遊戲有兩個按鈕功能

因此，只需要移除
<color=#CF001C><</color>、<color=#CF001C>=</color>、<color=#CF001C>></color> 組成的三行即可

# Q7 創建新的提交，並更新 '提交記錄' 視窗
Q7（提示）：
如果您不確定如何新增提交來保存您的專案版本
這裡有一些提示可以幫助您完成目標：

1. 確認要保存的所有文件都已經位於暫存區域中
   沒有的話請通過指令將文件推入上去

2. 開啟 '命令行' 視窗
   並確認路徑目前位於專案上

3. Git 指令中，有個將暫存區域的檔案推入儲存庫的指令
   並且遊戲提供了自動填入提交訊息的功能
   詳細請通過遊戲手冊查找
   
4. 完成提交訊息的填寫後，執行指令即可創建一個提交

5. 當創建提交後
   請記得確認提交記錄的狀態

Q7（解答）：
當成功解決所有文件的衝突後
請記得將這些檔案推到儲存庫以做成一個記錄

首先，請確保 '遊戲程式.js'、'我的網頁.html' 位於暫存區域
您可以使用 '<color=#CF001C>git status</color>' 再次確認

如果目標不在暫存區域
分別執行 '<color=#CF001C>git add 遊戲程式.js</color>'
以及 '<color=#CF001C>git add 我的網頁.html</color>'

接著，請輸入 '<color=#CF001C>git commit -m ""</color>'
這是新建提交需要使用的指令結構

在這個情況下
使用快速填入功能 或 按下 Tab 鍵來使用填入提交訊息功能

使用後，將彈出視窗
請您根據三個選項中選擇最為合適的提交訊息

當創建提交後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態