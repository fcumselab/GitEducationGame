可能可以用在ErrorDetect的內容

在執行指令的過程中，您會遇到三種不同的圖示和顏色：
1. 執行成功：
當您執行的指令正確且不會對專案造成問題時。
指令執行結果為綠色打勾圖示，表示操作成功。

1. 無法執行：
指令無法執行，但不會對專案造成嚴重問題。
以下情況可能無法執行指令：
1. 未解鎖、當前關卡不可使用的指令
2. 遊戲不支持、Git 沒有的指令
3. 沒有按照當前目標執行指令，但不會影響專案時
4. 實際使用 Git 指令時，當前情況也無法執行指令時

在這種情況下，指令執行結果為黃色三角形圖示。
表示操作未成功，但不會對專案造成嚴重的問題。

3. 指令錯誤：
如果您執行了不正確的指令或可能對專案造成嚴重問題。

例如：
1. 要合併分支時，合併了錯誤的分支。
這會導致提交記錄出現問題，需要做額外操作才能恢復。
2. 刪除了未完成目標的分支：
這會造成開發功能的時間和精力全部浪費，
因此這個操作無法執行，並視為操作錯誤。

在這些情況下，指令執行結果為紅色叉號圖示，
通常需要額外的時間和精力來解決問題。
表示指令沒有執行成功，並計為操作錯誤。

>小提醒
黃色和紅色類型的主要區別在於：
紅色指令執行結果可能會對專案造成問題，需要額外處理。
而黃色表示執行未成功，不會影響專案。