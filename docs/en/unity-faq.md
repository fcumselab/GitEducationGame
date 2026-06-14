# 🔧 Unity Troubleshooting & Missing Reference Fix Guide

> ⚠️ **Preface & Important Note:**
> Because this project integrates major third-party plugins like Odin Inspector and Playmaker, when cloning via Git (or executing Checkout/Pull) onto a new computer for the first time, Unity's serialization mechanism can occasionally malfunction. This can cause certain scenes or Prefabs to suffer from **Missing References**.
>
> **💡 Please perform the following check first:**
> This phenomenon does not happen every single time (sometimes re-cloning or downloading into a different folder on the same computer will mysteriously resolve itself automatically). Therefore, please follow **`4. Final Verification: Confirming Smooth Gameplay`** to run a live test first. If your test run goes smoothly with no error logs, you are in luck and can start developing immediately! On the flip side, if errors pop up during gameplay, return here to execute the troubleshooting steps below in order.
> _(Note: This fix usually only needs to be processed manually once when the environment glitches; you will not encounter it again during subsequent long-term development.)_

---

## 🛠️ Issue 1: Fixing Missing Windows References in Game Stages

- **Symptom:** The `all window Dict` inside the stage objects loses its reference, preventing the level windows from popping up normally.
- **Fix Steps:**
  1. In Unity's **Project Window**, search for `001`.
  2. Locate the object named `001-Game Introduction (Practice)` or `001-Game Introduction (Tutorial)`.
  3. Click on the object. In the **Inspector Window** on the right, find the **`Stage Manager (Script)`** component, expand its internal **`Window`** list, and you will see **`All Window Dict`**. (Please note: There might be asset files with identical names above it; you must find the specific object that contains the **`Stage Manager (Script)`**).
  4. Clear the search bar in the Project Window, and you will now see all stage objects in the project (including the Tutorial and Practice versions from `001 - 017`).
  5. On the object you just found where the **values are completely normal (the List does not display "None")**, hover over the `all window Dict` field, **Right-Click ➔ Select Copy**.
  6. Next, **select all stage objects** in the project directory, then hover over the exact same field, **Right-Click ➔ Select Paste**. This will apply the correct reference to all stages in one click.

---

## 🛠️ Issue 2: Fixing Missing References in Game Manual Entries

- **Symptom:** When expanding the `Game Manual Content Dict` under the `GameManualWindow` object, certain references appear as `None` (typically around 4 missing references).
- **CRITICAL WARNING:** If you simply drag and drop to restore these 4 missing references sequentially, Unity will trigger an unknown serialization error, causing all references throughout the entire Dictionary to wipe out and revert to None collectively again. Please strictly adhere to the following fix steps.
- **Fix Steps:**
  1. Search for `GameManualWindow` in the **Project Window**, and click on the single matching object.
  2. In the **Inspector Window** on the right, find the **`Game Manual (Script)`** component, expand the **`Prefabs`** list within it, and further expand the **`Game Manual Content Dict`**.
  3. Look for the fields displaying `None`. Copy the **Key name** of that field and search for it in the Project Window. Once you locate the correct corresponding object, **drag and drop** it back into the respective `None` slot.
     - 💡 _Special Tip: One of the Key names is **`.gitAndGitCoreAreas`**. Please search for **`gitAndGitCoreAreas`** (omitting the leading dot) directly in the Project Window to find the correct object._
  4. 🔥 **Bug Circumvention Operation:** Please manually fix **only 3** of the missing references using the method above first.
  5. Memorize or write down the **Key name** of the last remaining unfixed field.
  6. **Delete (click the "X" icon)** this last missing item directly from the Dictionary.
  7. Click the **`+` (Plus icon)** in the top-right corner of the Dictionary to add a new empty field. Manually type in the **Key name** you just recorded, and then drag and drop the correct corresponding object into it.

---

## 🛠️ Issue 3: Fixing Missing Localization Manager References in Save Manager

- **Symptom:** Upon entering the game's main menu screen, certain English and Chinese translations go missing, directly preventing the game from running properly.
- **Fix Steps:**
  1. In the **Project Window**, search for `title screen` and double-click to open this scene (this is the game's main menu scene).
  2. In the **Hierarchy Window** on the left, locate the game object named **`Save Manager`** and expand it.
  3. Click on its child object **`Localization Manager`**. At this point, you will see the corresponding C# script fields in the **Inspector** on the right.
  4. Expand the **`Stage CSV Dict English`** and **`Stage CSV Dict Chinese`** fields. You will notice that a large number of translation references are missing inside them.
  5. **Locate the Correct Source:** Return to the **Project Window**, search for `Save Manager`, and **double-click the object to enter Prefab editing mode**.
  6. In the Prefab's internal Hierarchy, click on `Localization Manager` as well, and navigate to the same fields. You will find that the references inside `Stage CSV Dict English` and `Stage CSV Dict Chinese` here are **completely intact and undamaged**.
  7. Separately hover over these two undamaged Dictionaries, **Right-Click ➔ Select Copy**.
  8. Click the **`<` (Back Arrow)** in the upper-left corner of the Hierarchy to exit Prefab mode and return to the `Title Screen` scene.
  9. Select the `Localization Manager` inside the scene once again, hover over the previously missing fields, **Right-Click ➔ Select Paste** to overwrite and apply the correct data.
  10. Once you ensure both the English and Chinese Dictionaries have been copied and pasted successfully, the game's languages and data will be fully functional.

---
