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


# Q2 在 'master' 分支上創建新的分支，並更新 '提交記錄' 視窗
Q2（提示）：
如果您不確定如何創建分支
這裡有一些提示可以幫助您完成目標：

1. 確認您的 '命令行' 視窗已經開啟

2. 確認您的命令行路徑位於專案上
   （可以看到 .git 隱藏資料夾）

3. Git 指令中，有個用於創建分支的指令
   並且遊戲提供了自動填入分支名稱的功能
   詳細請通過遊戲手冊查找

4. 在執行前
   請確認當前位於的提交與當前目標相同

Q2（解答）：
為了確保已經完成的內容不會被影響
我們可以執行 '<color=#CF001C>git branch 分支名稱</color>' 指令
這個指令可以用於創建分支

首先，請確保命令行路徑位於專案上
本次關卡的專案創建於 '<color=#CF001C>Home > 猜數字專案</color>' 路徑上
（可以看到 .git 隱藏資料夾）

接著，請輸入 '<color=#CF001C>git branch </color>'
請記得在指令前面加一個空格 

在這個情況下
使用快速填入功能 或 按下 Tab 鍵來使用填入分支名稱功能

使用後，將彈出視窗
請您根據三個選項中選擇最為合適的提交訊息

當正確選擇後，執行指令 '<color=#CF001C>git branch 選擇的分支名稱</color>'
即可完成目標


# Q3 在 'your-style-design' 分支上修改手機樣式
Q3（提示）：
看起來您在修改檔案內容的地方遇到了一些問題
這裡有一些提示可以幫助您完成目標：

1. 在 '檔案管理' 視窗中，請點擊要編輯的文件
   以此來開啟 '檔案內容' 視窗

2. 在本遊戲裡，需要修改的內容會用 <color=#CF001C>(需要修改)</color> 標示
   請找到那些行數並準備修改它們

3. 每一行內容左邊的鉛筆圖示
   點擊後可以修改這一行的內容

4. 修改內容之前
   請注意要修改內容的檔案名稱要符合當前目標

Q3（解答）：
為了修改 '手機主界面.xml' 文件中的內容
請先確認您已經在 'your-style-design' 分支上
如果沒有，請先通過 '<color=#CF001C>git checkout your-style-design</color>' 切換分支

接著，在 '檔案管理' 視窗中找到並點擊它來開啟 '檔案內容' 視窗

在 '檔案內容' 視窗中
需要修改的內容會有 <color=#CF001C>(需要修改)</color> 標示
我們要修改的是：
'<color=#CF001C>數字輸入欄</color>' 以及 '<color=#CF001C>猜數字按鈕</color>'
這兩行內容



# Q4 創建新的提交，並更新 '提交記錄' 視窗
Q4（提示）：
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

Q4（解答）：
為了提交修改過後的內容並成功記錄您的更改
首先，請確保 '手機主界面.xml' 位於暫存區域
您可以使用 '<color=#CF001C>git status</color>' 再次確認

如果目標不在暫存區域
請執行 '<color=#CF001C>git add 手機主界面.xml</color>' 指令
來推入到暫存區域中

接著，請輸入 '<color=#CF001C>git commit -m ""</color>'
這是新建提交需要使用的指令結構

在這個情況下
使用快速填入功能 或 按下 Tab 鍵來使用填入提交訊息功能

使用後，將彈出視窗
請您根據三個選項中選擇最為合適的提交訊息

當創建提交後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態


# Q5 將 'member-style-design' 分支合併回主分支上  並更新 '提交記錄' 視窗"
Q5（提示）：
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

Q5（解答）：
為了將 'member-style-design' 分支合併至 'master'
您需要執行 '<color=#CF001C>git merge 分支名稱</color>' 指令

首先在 '命令行' 視窗中，請確保其路徑位於專案上
本次關卡的專案創建於 '<color=#CF001C>Home > 猜數字專案</color>' 路徑上
（可以看到 .git 隱藏資料夾）

在合併之前，請先確認當前專案版本已經位於 'master' 分支
您可以通過 '<color=#CF001C>git checkout master</color>' 指令
來切換到 'master' 分支

接著，執行 '<color=#CF001C>git merge member-style-design</color>' 指令
這樣就可以將 'member-style-design' 合併至 'master' 分支

當合併成功後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態


# Q6 將 'your-style-design' 分支合併回主分支上  
Q6（提示）：
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

Q6（解答）：
為了將 'your-style-design' 分支合併至 'master'
您需要執行 '<color=#CF001C>git merge 分支名稱</color>' 指令

首先在 '命令行' 視窗中，請確保其路徑位於專案上
本次關卡的專案創建於 '<color=#CF001C>Home > 猜數字專案</color>' 路徑上
（可以看到 .git 隱藏資料夾）

在合併之前，請先確認當前專案版本已經位於 'master' 分支
您可以通過 '<color=#CF001C>git checkout master</color>' 指令
來切換到 'master' 分支

接著，執行 '<color=#CF001C>git merge your-style-design</color>' 指令
這樣就可以將 'your-style-design' 合併至 'master' 分支

當合併成功後
系統會提醒您合併發生衝突


# Q7 解決 '手機主界面.xml' 文件衝突
Q7（提示）：
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
   
Q7（解答）：
當合併發生衝突時
我們必須把所有的文件衝突解決後才可以繼續合併分支

首先，在 '手機主界面.xml' 檔案內容中
出現了兩個分支的修改內容
以及三行由 <color=#CF001C><</color>、<color=#CF001C>=</color>、<color=#CF001C>></color> 組成的行數

'<<< HEAD' 和 '===' 中包含的內容
表示在 HEAD，也就是 'master' 分支上的修改內容
同學 A 在 'member-style-design' 分支上設計了手機界面

'>>> your-style-design' 和 '===' 中包含的內容
表示在 'your-style-design' 分支上的修改內容
這是您設計手機界面時所使用的分支

根據故事劇情，我們應該採用同學 A 的數字輸入欄
以及您設計的猜數字按鈕

因此，您需要移除
<color=#CF001C><</color>、<color=#CF001C>=</color>、<color=#CF001C>></color> 組成的三行
以及 '猜數字按鈕 (同學 A 設計)' 行數
和 '數字輸入欄 (您的設計)' 行數


# Q8 創建新的提交，並更新 '提交記錄' 視窗
Q8（提示）：
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

Q8（解答）：
當成功解決所有文件的衝突後
請記得將這些檔案推到儲存庫以做成一個記錄

首先，請確保 '手機主界面.xml' 位於暫存區域
您可以使用 '<color=#CF001C>git status</color>' 再次確認

如果目標不在暫存區域
請執行 '<color=#CF001C>git add 手機主界面.xml</color>' 指令
來推入到暫存區域中

接著，請輸入 '<color=#CF001C>git commit -m ""</color>'
這是新建提交需要使用的指令結構

在這個情況下
使用快速填入功能 或 按下 Tab 鍵來使用填入提交訊息功能

使用後，將彈出視窗
請您根據三個選項中選擇最為合適的提交訊息

當創建提交後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態


# Q9 刪除完成合併的所有分支   並更新 '提交記錄' 視窗"
Q9（提示）：
如果您不確定如何刪除分支
這裡有一些提示可以幫助您完成目標：

1. 查看 '提交記錄' 視窗
   可以得知目前儲存庫所有的分支名稱

2. Git 指令中，有個刪除指定分支的指令
   需要在指令後方加入指定的分支名稱
   詳細請通過遊戲手冊查找

3. 在刪除指定的分支後
   請記得確認提交記錄的狀態

Q9（解答）：
當分支的主要目標已經達成並合併至主分支後
即可通過 '<color=#CF001C>git branch -d 分支名稱</color>' 指令
來刪除指定分支

在刪除之前，請先確認當前專案版本已經位於其他分支
您可以通過 '<color=#CF001C>git checkout master</color>' 指令
來切換到 'master' 分支

接下來，請執行 '<color=#CF001C>git branch -d your-style-design</color>'
這樣即可刪除 'your-style-design' 分支

之後，請執行 '<color=#CF001C>git branch -d member-style-design</color>'
這樣即可刪除 'member-style-design' 分支

當刪除掉所有合併完畢的分支後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態