# 🔧 Unity 疑難排解與 Reference 缺失修復指南

> ⚠️ **前言與重要說明：**
> 本專案由於結合了 Odin Inspector、Playmaker 等大型第三方套件，在首次透過 Git Clone（或執行 Checkout、Pull）到新電腦時，Unity 的序列化機制偶爾會發生異常，導致部分場景或 Prefab 產生 **Missing Reference (引用遺失)**。
>
> **💡 請先執行以下確認：**
> 這個現象並非每次都會發生（有時重新 Clone 或在同台電腦換個資料夾下載，Unity 又會神秘地自動恢復正常）。因此，請依照 **`4. 終極驗證：確認遊戲運行正常`** 進行實測。如果實測流程完全正常、沒有報錯，代表你運氣很好，可以直接開始開發！反之，若遊玩過程中報錯，再回來依序執行下方的修復步驟。
> _(註：此修復通常只需在環境異常時手動處理一次，後續長期開發將不會再遇到。)_

---

## 🛠️ 問題一：Game Stage 中的 Windows Reference 缺失修復

- **異常現象：** 關卡物件中的 `all window Dict` 遺失引用，導致關卡視窗無法正常彈出。
- **修復步驟：**
  1. 在 Unity 的 **Project 視窗** 中搜尋 `001`。
  2. 找到名為 `001-Game Introduction (Practice)` 或 `001-Game Introduction (Tutorial)` 的物件。
  3. 點擊該物件，在右側的 **Inspector 視窗** 下方找到 **`Stage Manager (Script)`** 組件，將其內部的 **`Window`** 列表展開，即可看到 **`All Window Dict`**。（請注意：上方可能會有同名的文件，必須找到含有 **`Stage Manager (Script)`** 的 Object）。
  4. 清除 Project 視窗的搜尋欄，此時可以看到專案內所有的關卡物件（包含 `001 - 017` 的 Tutorial 與 Practice 版本）。
  5. 在剛才找到的、**數值完全正常（Value 沒有顯示 None 的 List）** 的物件上，對著 `all window Dict` 欄位**按右鍵 ➔ 選擇 Copy**。
  6. 接著在專案中**全選所有關卡的物件**，選中後在相同位置**按右鍵 ➔ 選擇 Paste**。即可一鍵將正確的 Reference 套用到所有關卡。

---

## 🛠️ 問題二：Game Manual 條目 Reference 缺失修復

- **異常現象：** `GameManualWindow` 物件下的 `Game Manual Content Dict` 展開後，部分引用變成 `None`（大約有 4 個遺失）。
- **重要警告：** 如果你直接將這 4 個遺失的 Reference 連續拖曳補回，Unity 會觸發未知的序列化錯誤，導致整個 Dictionary 的所有引用「再次集體變回 None」。請務必嚴格遵守以下修復步驟。
- **修復步驟：**
  1. 在 **Project 視窗** 搜尋 `GameManualWindow`，並點擊該唯一同名物件。
  2. 在右側 **Inspector 視窗** 中 找到 **`Stage Manager (Script)`** 組件，展開其中的 **`Prefabs`** 清單，並進一步展開 **`Game Manual Content Dict`**。
  3. 尋找顯示為 `None` 的欄位，複製該欄位的 **Key 名稱** 到 Project 視窗中搜尋，找到正確的對應物件後，將其**拖曳**回對應的 `None` 位置。
     - 💡 _特別提示：其中一個 Key 名稱為 **`.gitAndGitCoreAreas`**，請在 Project 視窗中直接搜尋 **`gitAndGitCoreAreas`**（去掉前面的點）即可找到對應物件。_
  4. 🔥 **規避 Bug 操作：** 請先用上述方法手動修復其中 **3 個** 遺失的 Reference。
  5. 記住最後剩下那 1 個未修復欄位的 **Key 名稱**。
  6. 在 Dictionary 中 **直接刪除 (叉號)** 這最後一個遺失的項目。
  7. 點擊 Dictionary 右上角的 **`+` (加號)** 新增一個空欄位，手動輸入剛才記下的 **Key 名稱**，並將對應的正確物件拖曳進去。

---

## 🛠️ 問題三：Save Manager 裡的 Localization Manager 翻譯引用缺失

- **異常現象：** 進入遊戲主畫面後，部分中英文翻譯遺失。導致遊戲直接無法運行。
- **修復步驟：**
  1. 在 **Project 視窗** 搜尋 `title screen`，雙擊開啟此場景（此為遊戲主畫面場景）。
  2. 在左側 **Hierarchy 視窗** 中找到名為 **`Save Manager`** 的物件並展開它。
  3. 點擊其子物件 **`Localization Manager`**，此時在右側 **Inspector** 會看到同名的 C# 腳本欄位。
  4. 展開其中的 **`Stage CSV Dict English`** 與 **`Stage CSV Dict Chinese`**，會發現裡面遺失了大量的翻譯引用。
  5. **尋找正確來源：** 回到 **Project 視窗** 搜尋 `Save Manager`，**雙擊該物件進入 Prefab 編輯模式**。
  6. 在 Prefab 的 Hierarchy 中同樣點擊 `Localization Manager`，移到相同的位置，會發現這裡的 `Stage CSV Dict English` 與 `Stage CSV Dict Chinese` 內部的 Reference **完全沒有缺失，是完好的**。
  7. 分別對著這兩個完好的 Dict **按右鍵 ➔ 選擇 Copy**。
  8. 點擊 Hierarchy 左上角的 **`<` (返回箭頭)** 退出 Prefab 模式，回到 `Title Screen` 場景。
  9. 再次選取場景中的 `Localization Manager`，在剛才缺失的欄位上**按右鍵 ➔ 選擇 Paste**，將正確的內容覆蓋上去。
  10. 確保英、中兩個 Dict 都複製貼上完成後，即可確保遊戲語系與資料順利運行。
