無法修復的問題，可能是unity自己無法讓git記錄這些改動，這裡可能是Unity自己的問題，為了能運行我們只能手動處理：

儘管將所有reference都做好並且上傳github。
clone下來的話（可能checkout、pull也會）
但是只要沒有一直更換電腦或是檔案狀態的需要 長期開發下來不會再遇到這個問題

目標：
這些缺失的reference會導致開發者遊戲過程中報錯，並且玩家無法遊玩遊戲
我們需要將以下內容先reference重新手動找到才可以還原到正常的開發環境

1. Game Stage 中的 Windows Reference：
   通過unity的project搜尋001找到001-Game Introduction (Practice) 或是 001-Game Introduction (Tutorial)
   然後右側的inspetor下面應該要有 all window Dict （上面有同名的文件，需找到含有all window dict的object）
   之後清除搜索project
   可以看到遊戲中的所有關卡 （001 - 017，Tutorial 和Practice都有）
   請找到一個object含有所有windows reference都在的object（value沒有None的list）
   之後對著all window dict 右鍵有copy選項
   複製後全選所有關卡的object，選擇完後右鍵Paste即可處理第一個reference問題。

2.game manual 條目reference：
通過unity的project搜尋 GameManualWindow 找到 唯一的一個同名object
然後右側的inspetor下面prefab清單展開，Game Manual Content Dict展開
會發現有些reference變成None了
可以搜尋這些object通過複製key到project搜尋。然後把找到的object拖到這些位置即可

注意，遺失reference的大概有4個，但是如果4個都是這麼移動的話，Unity會未知的把所有reference再次變成None
我們可以先用好3個，之後記住某個的key名字（一定要同樣的key），然後先刪除其中的dict，後面通過右上角的加號新增一個同樣key和對應object的dict
這樣即可完成reference問題

3. Save Manager 裡面的 Localization Manager reference 缺失
   通過unity的project搜尋 title screen 找到 一個場景雙擊後進入場景
   這個是遊戲的主畫面，不過重點在Hierarchy中列表有個叫做Save Manager的object
   展開後有個Localization Manager object點擊一下後
   下拉在inspetor可以看到一個叫做Localization Manager的Script
   會發現一個是Stage CSV Dict English的Dict，以及Stage CSV Dict Chinese的Dict
   會發現裡面缺失很多中文、英文翻譯

   我們可以在project搜尋Save Manager後雙擊這個object進入prefab
   Hierarchy點擊Localization Manager後
   找到同一個位置（Stage CSV Dict English的Dict 、 Stage CSV Dict Chinese）
   會發現reference完全沒有缺失
   請右鍵這個Dict後Copy
   然後在Hierarchy看到 < 符號點擊退回主畫面
   之後在Title Screen下，一樣在Hierarchy找到這個Localization Manager
   同個位置下（Stage CSV Dict English的Dict 、Stage CSV Dict Chinese）右鍵Paste即可完成Reference缺失問題
   這裡需要複製貼上兩個Dict的內容就可以確保遊戲可以順利進行
