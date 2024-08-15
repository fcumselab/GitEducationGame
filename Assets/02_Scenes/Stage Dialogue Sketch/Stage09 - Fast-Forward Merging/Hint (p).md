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
本次關卡的專案創建於 '<color=#CF001C>Home > 角色專案</color>' 路徑上
（可以看到 .git 隱藏資料夾）

確認完後，請在 '命令行' 視窗中的指令輸入區域
輸入 '<color=#CF001C>git log</color>'
然後執行它來查看提交記錄的內容


# Q2 在 'master' 分支上創建分支，並更新 '提交記錄' 視窗
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

5. 當創建分支後
   請記得確認提交記錄的狀態


Q2（解答）：
為了確保已經完成的內容不會被影響
我們可以執行 '<color=#CF001C>git branch 分支名稱</color>' 指令
這個指令可以用於創建分支

在創建分支前，請確保目前位於 'master' 分支
您可以通過 'git checkout master' 指令
來移動至 'master' 分支上

首先，請確保命令行路徑位於專案上
本次關卡的專案創建於 '<color=#CF001C>Home > 角色專案</color>' 路徑上
（可以看到 .git 隱藏資料夾）

接著，請輸入 '<color=#CF001C>git branch </color>'
請記得在指令前面加一個空格 

在這個情況下
使用快速填入功能 或 按下 Tab 鍵來使用填入分支名稱功能

使用後，將彈出視窗
請您根據三個選項中選擇最為合適的提交訊息

當正確選擇後，執行指令 '<color=#CF001C>git branch 選擇的分支名稱</color>'
即可完成目標





# Q3 在開發分支上增加角色攻擊行為偵測
Q3（提示）：
看起來您在新增檔案內容的地方遇到了一些問題
這裡有一些提示可以幫助您完成目標：

1. 請確認當前目標的要求
   在正確的分支、檔案上新增內容

2. 在 '檔案管理' 視窗中，請點擊要編輯的文件
   以此來開啟 '檔案內容' 視窗

3. 檔案內容的最下方有個加號按鈕
   點擊它後即可新增內容

Q3（解答）：
為了新增 '角色行為程式.cs' 文件中的內容
請先確認您已經在 'new-character-action' 分支上
如果沒有，請先通過 '<color=#CF001C>git checkout new-character-action</color>' 切換分支

接著，在 '檔案管理' 視窗中找到並點擊它來開啟 '檔案內容' 視窗

在 '檔案內容' 視窗中
請點擊文字內容下方的加號按鈕來加入新的文字

# Q4 創建新的提交，並更新 '提交記錄' 視窗"
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
首先，請確保 '角色行為程式.cs' 位於暫存區域
您可以使用 '<color=#CF001C>git status</color>' 再次確認

如果目標不在暫存區域
使用 '<color=#CF001C>git add 角色行為程式.cs</color>' 來推入

接著，請輸入 '<color=#CF001C>git commit -m ""</color>'
這是新建提交需要使用的指令結構

在這個情況下
使用快速填入功能 或 按下 Tab 鍵來使用填入提交訊息功能

使用後，將彈出視窗
請您根據三個選項中選擇最為合適的提交訊息

當創建提交後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態

# Q5 切換到 'master' 分支 並更新 '提交記錄' 視窗
Q5（提示）：
如果您不確定如何切換分支
這裡有一些提示可以幫助您完成目標：

1. 查看 '提交記錄' 視窗
   可以得知目前儲存庫所有的分支名稱

2. Git 指令中，有個切換至指定分支的指令
   需要在指令後方加入指定的分支名稱
   詳細請通過遊戲手冊查找

3. 當切換至該分支後
   請記得確認提交記錄的狀態

Q5（解答）：
為了開始將 'new-character-action' 分支合併至 'master'
您需要執行 '<color=#CF001C>git checkout master</color>' 指令
來切換到 'master' 分支

首先在 '命令行' 視窗中，請確保其路徑位於專案上
本次關卡的專案創建於 '<color=#CF001C>Home > 角色專案</color>' 路徑上
（可以看到 .git 隱藏資料夾）

接著，執行 '<color=#CF001C>git checkout master</color>' 指令
這樣就可以將當前專案版本移動回 'master' 分支

當完成切換分支後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態


# Q6 合併 'new-character-action' 分支 並更新 '提交記錄' 視窗
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
   請記得確認提交記錄的狀態

Q6（解答）：
為了將 'new-character-action' 分支合併至 'master'
您需要執行 '<color=#CF001C>git merge 分支名稱</color>' 指令

首先在 '命令行' 視窗中，請確保其路徑位於專案上
本次關卡的專案創建於 '<color=#CF001C>Home > 角色專案</color>' 路徑上
（可以看到 .git 隱藏資料夾）

在合併之前，請先確認當前專案版本已經位於 'master' 分支
您可以通過 '<color=#CF001C>git checkout master</color>' 指令
來切換到 'master' 分支

接著，執行 '<color=#CF001C>git merge new-character-action</color>' 指令
這樣就可以將 'new-character-action' 合併至 'master' 分支

當合併成功後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態


# Q7 刪除 'new-character-action' 分支 並更新 '提交記錄' 視窗
Q7（提示）：
如果您不確定如何刪除分支
這裡有一些提示可以幫助您完成目標：

1. 查看 '提交記錄' 視窗
   可以得知目前儲存庫所有的分支名稱

2. Git 指令中，有個刪除指定分支的指令
   需要在指令後方加入指定的分支名稱
   詳細請通過遊戲手冊查找

3. 在刪除指定的分支後
   請記得確認提交記錄的狀態

Q7（解答）：
當分支的主要目標已經達成並合併至主分支後
即可通過 '<color=#CF001C>git branch -d 分支名稱</color>' 指令
來刪除指定分支

在刪除之前，請先確認當前專案版本已經位於其他分支
您可以通過 '<color=#CF001C>git checkout master</color>' 指令
來切換到 'master' 分支

接下來，請執行 '<color=#CF001C>git branch -d new-character-action</color>'
這樣即可刪除 'new-character-action' 分支

當刪除掉分支後
請使用 '<color=#CF001C>git log</color>' 來確認提交記錄的狀態