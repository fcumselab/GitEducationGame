開頭：
Hello there!
Welcome to the 'Resolving Merge Conflicts' stage!
The current mode is 'Practice Mode'.

In this mode,
you will need to apply what you learned in Tutorial Mode
to complete the stage objectives on your own.

However, if you run into any trouble,
you can always click the Game Manual or the chat button to get help.

Next, let's set up the simulated environment for this stage.

本次關卡需要您創建新的分支
以重新設計手機界面

當您完成設計後
就可以開始合併分支了

Next, please follow the stage instructions to complete the objectives.
Good luck!

Q5 中途劇情：
很好！您重新設計了手機界面的樣式
現在讓我們開始將開發分支合併回主分支

首先，我們先從同學 A 的分支開始合併

Q7 中途劇情：
由於您和同學 A 各自在 '手機主界面.xml' 上做了修改
並且都修改了按鈕和輸入欄的外觀
因此這個文件發生了合併衝突

和同學 A 討論過彼此的手機界面設計後
得出了以下結論：
<color=#CF001C>採用同學 A 的按鈕設計
</color>
<color=#CF001C>採用您的輸入欄設計</color>

接下來，請您根據上述結論
移除掉多餘的按鈕和輸入欄
這些元素都只要保留一個就行

Q8 中途劇情：

非常好，您順利地解決了文件衝突
接下來請您將解決衝突的文件新建一個提交
以讓 Git 繼續合併

Q9 中途劇情：
很好！在您成功新增提交後
Git 將繼續進行合併

在完成合併後
請記得刪除完成目標的所有分支

結尾：
非常好！
您完成了本次的遊戲關卡

猜數字遊戲的畫面改良了
現在擁有同學 A 設計的按鈕
以及您的輸入欄設計

在這個關卡中，您成功解決合併衝突
這是合併模式裡最為複雜的一種

解決本次模擬場景的問題後
代表您已經通過了 '分支管理' 主題關卡

接下來，將進入最後一個主題關卡 — '遠端管理'

最後，恭喜您通過了本次關卡！
這也代表您完成了 '分支管理' 主題關卡
您做得很棒！

環境：

Q6 hint （合併第二個開發分支）
4 合併分支後
系統會提醒您合併發生衝突

Q6 ans （合併第二個開發分支）

當合併成功後
系統會提醒您合併發生衝突

Q7 hint (解決衝突中)
看來您不太清楚如何解決文件衝突
這裡有一些提示可以幫助您完成目標：

1 確認當前目標劇情文字與故事背景
點擊左下角的對話按鈕來回顧劇情文字

2 根據指示確認文件中的衝突內容
哪些要保留、哪些要留下

3 移除不需要的部分（刪除它們）：
某個分支的修改內容
由 <color=#CF001C><</color>、<color=#CF001C>=</color>、<color=#CF001C>></color> 三行組成的行數

4 如果想要了解更多
請參考遊戲手冊中的 '解決合併衝突' 條目

Q7 ans (解決衝突中)
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

根據故事劇情，我們應該採用：
同學 A 的數字輸入欄
您設計的猜數字按鈕

因此，您需要移除
<color=#CF001C><</color>、<color=#CF001C>=</color>、<color=#CF001C>></color> 組成的三行
以及 '猜數字按鈕 (同學 A 設計)' 行數
和 '數字輸入欄 (您的設計)' 行數

Q8 ans (解決後創建提交)
當成功解決所有文件的衝突後
請記得將這些檔案推到儲存庫以做成一個記錄
