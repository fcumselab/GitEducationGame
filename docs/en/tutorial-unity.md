# 🎮 Unity Frontend Development Environment Configuration

This guide will walk you through how to download the correct Unity version, configure the development environment, and successfully open the game project.

---

## 📥 Step 1: Download Unity Hub & Align the Editor Version

This project specifies a particular Unity version. Please make sure to install the exact same version to avoid file conflicts or build failures.

1. Please proceed to the [Official Unity Website](https://unity.com/download) to download and install **Unity Hub**.
2. Open Unity Hub and log in with your Unity account.
3. The Unity version used in this project is: **`e.g., 2021.3.6f1`** (or other corresponding versions).
4. Go to the **Installs** page in Unity Hub and click **Install Editor**. Since this project uses an older version, you will need to click the **Archive** link to locate and download the correct version.

---

## 🛠️ Step 2: Mandatory Support Modules to Check

During the installation process of the Unity Editor, a **Modules** selection window will pop up. **Please ensure that you check the following items**:

- **[x] WebGL Build Support**
  - _Note: Because this project ultimately needs to be built into a web-based game (placed inside `gegGame/`), this module must be installed to perform WebGL development and compilation._
- **[x] Microsoft Visual Studio (or your preferred IDE)**
  - _Note: Used for writing and reading C# game scripts._

> 💡 **Tip:**
> If you have already installed Unity without this module, you can add it anytime. Go to the **Installs** page in Unity Hub, click the "gear icon (Settings)" next to the specific version, and select **Add modules** to install WebGL Build Support.

---

## 📥 Step 3: Import the Project into Unity Hub

Since the project has already been cloned to your local machine via Git, we simply need to add it to the Unity Hub list:

1. Open **Unity Hub** and switch to the **Projects** page.
2. Click the **Add** button in the top right corner (or select **Add project from disk** from the dropdown menu).
3. In the pop-up file browser, navigate to the main directory of your cloned project, and select the **`game/`** folder (this folder is the actual root of the Unity project).
4. Click confirm, and the project will appear in your list. Now, click the project name and wait for the Unity Editor to open.
5. ⚠️ **Note:** Opening the project for the first time will take a considerable amount of time to parse and download assets—potentially **up to 30 minutes**. Please be patient. Subsequent openings will not take nearly as long.

---

## ⚙️ Step 4: In-Editor Project Configuration

Once you have successfully entered the Unity Editor, please prioritize verifying the following settings to ensure consistency in the development environment:

### 1. Verify and Switch the Target Platform to WebGL

1. In the top menu of Unity, click **File ➔ Build Settings...**
2. Locate **WebGL** in the Platform list and click on it.
3. Check whether the button in the bottom right corner says **Build**. If it does not, click the **Switch Platform** button to perform the switch.
4. _Note: In the future, when game development is complete and the web frontend needs to be updated, you will need to click the **Build** button here to pack the game and overwrite the files inside `gegGame/`._

### 2. Load the Project Development Layout

1. In the top menu of Unity, click **Window ➔ Layouts ➔ Load Layout From File...**
2. Select the **`GITainment layout.wlt`** file located under the `game/` root directory.
3. Upon successful loading, you will notice that the window distribution (environment configuration) of the Unity Editor automatically changes to the recommended development layout for this project.

### 3. Environment Toggle Setup: Local vs. Remote Switch

When conducting feature development or debugging locally, please make sure to switch the environment to **Local Mode** (this is configured by default). This project manages network connections by toggling the activation status of specific game objects in the scenes:

1. In Unity's **Project Window**, navigate to **`Assets ➔ 02_Scenes`**. You will see the three core scenes included in this game:
   - **Title Screen:** The first screen players see when entering the game, used for account registration and login.
   - **Stage Select:** Used for choosing game levels and modes.
   - **Play Game:** Generates the corresponding gameplay content based on the selected level.

2. **【IMPORTANT】The following verification and setup must be executed across ALL THREE scenes:**
   - In the scene's **Hierarchy**, locate the **`URLSettings`** game object and expand it. It contains two child objects: `URLSetting (Online Deploy)` and `URLSetting (Local Testing)`.
   - 🛠️ **To enable Local Mode (Local Development):** Please **Activate** the `Local` object and **Deactivate** the `Online` object.
   - 🌐 **To enable Remote Mode (Remote Testing):** Please **Activate** the `Online` object and **Deactivate** the `Local` object. Meanwhile, you must click on the `Online` object and, in the **Inspector** on the right side under the `Url Setting (Script)` component, modify the URL field to your deployed remote web address (e.g., `http://xxx.xxx.xx.xx:xxxx/`).

### 4. Final Verification: Confirming Smooth Gameplay

Now, we will perform an end-to-end connectivity test for the entire stack (Unity + Node.js + MongoDB):

1. Ensure that your **MongoDB** is running in the background and that the **Node.js backend server** has started (the terminal should display _Server is running on port 3000_).
2. Inside Unity, double-click to open the **`Title Screen`** scene, and click the **Play** button at the top.
3. Try registering a new account, logging in, and playing through the Tutorial and Practice modes of the first stage.
4. If the test run proceeds smoothly with absolutely no red error messages in the console, you can skip the troubleshooting guide and jump straight into game development!

> 🚨 **Encountering Missing errors, frozen screens, or missing translations?**  
> Due to occasional serialization failures when pairing Unity with Git, if you encounter "Missing Reference" or other error messages during your first test after cloning, please immediately click here to fix them:  
> **[🔧 Unity Troubleshooting & Missing Reference Fix Guide](unity-faq.md)**

---
