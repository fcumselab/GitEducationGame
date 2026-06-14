# 💾 MongoDB Database Configuration Guide

This guide will walk you through how to download, install, and start the MongoDB database in your local environment, ensuring that the backend server (Node.js) can successfully connect and store data.

---

## 📥 Step 1: Download MongoDB Community Server

For local development, we use the free Community Edition provided officially by MongoDB:

1. Please proceed to the [MongoDB Official Download Center](https://www.mongodb.com/try/download/community).
2. On the right side under **Version**, select the latest stable version (the default option is recommended).
3. Under **Platform**, select your operating system (e.g., Windows or macOS).
4. Under **Package**, select `MSI` (for Windows) or `TGZ` (for macOS).
5. Click **Download** to start downloading the installer.

---

## 🛠️ Step 2: Install MongoDB and Compass GUI

Once the download is complete, please execute the installer and pay **special attention** to the following steps:

1. **Choose Setup Type:** It is recommended to click **Complete** (Full Installation).
2. **Service Configuration:** Keep the default settings (check _Run service as a Network Service user_). This allows MongoDB to automatically run in the background every time your computer boots up, eliminating the need to start it manually each time.
3. **Install MongoDB Compass:**
   In the final steps of the installation process, you will see a checkbox asking whether to **"Install MongoDB Compass."** Please make sure to check it.
   - _Note: MongoDB Compass is the official graphical management tool, allowing you to view and modify database content directly through a visual interface without typing blind commands. (Even if you missed it during setup, you can search for **MongoDB Compass** to download and install it separately later)._

4. Click **Next** and wait for the installation to finish.

---

## 🔍 Step 3: Verify the Database is Running Successfully

After the installation is complete, search for **MongoDB Compass** in your computer's applications and open it.

1. Once MongoDB Compass is open, you will see a connection screen. Click **`Add new connection`**.
2. The screen will display a default connection string (URI):

   `mongodb://localhost:27017`

   This is the default database address, which refers to your local database. If you need to connect to another remote database in the future, we will need to adjust the content of this URL.

3. Click **`Save & Connect`** to successfully enter and view the internal contents of the `localhost:27017` database.
