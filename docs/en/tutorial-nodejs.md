# 🧠 Node.js Backend Environment Setup Guide

This guide will walk you through how to set up and run the game's logical backend server in your local environment.

> ⚠️ **Important Prerequisite:**
> The backend server needs to connect to the database upon startup. If MongoDB has not been installed or configured on your computer yet, please refer to the **[💾 MongoDB Database Configuration Guide](tutorial-mongodb.md)** first. Once the database is running locally, return to this document and proceed with Step 3.

---

## 📥 Step 1: Download & Install Node.js

This project recommends using the **LTS (Long Term Support) version** of Node.js to ensure stability.

1. Please proceed to the [Official Node.js Website](https://nodejs.org/).
2. Download and install the **LTS version** suitable for your operating system. If you encounter any execution issues, you can install version `v20.10.0` (keep the default options throughout the installation process and simply click "Next").

---

## 🔍 Step 2: Verify the Installation

After the installation is complete, please check if your computer has successfully detected the Node.js environment:

1. Open your terminal (Windows users please use `CMD` or `PowerShell`; Mac users please use `Terminal`).
2. Input the following command and press Enter:
   ```bash
   node -v
   ```
3. If the installation was successful, the terminal will display the current Node.js version number (e.g., `v20.10.0`).

---

## ⚙️ Step 3: Local vs. Remote Startup Configuration

Before we officially start the server, it's essential to understand the project's environment settings. The project generally involves two operating modes:

- **Local (Local Development Mode):** The server connects to the MongoDB database running on your local computer. This is convenient for development, testing, and debugging, ensuring that online players are not affected. We typically develop in this mode and switch to Remote mode only after completion.
- **Remote (Remote Production Mode):** The server connects to a cloud database or a database on a remote server, usually used for official production or staging servers.

### 🛠️ How to Switch Modes?

Please open the server configuration file located in the `server/` folder. As the main program that boots up the backend, `server.js` provides the toggle for **Local / Remote mode**.

- **To use Local mode (Default):** Make sure to comment out the remote `url` and enable the `localhost` connection.
- **To use Remote mode:** Do the exact opposite.

```javascript
// Remote Production Mode Configuration (Comment this out if you want to use Local)
const username = process.env.MONGO_USERNAME;
const password = process.env.MONGO_PASSWORD;
const hostname = process.env.MONGO_HOST;
const port = process.env.MONGO_PORT;
const database = process.env.MONGO_DB;
// const url = `mongodb://${username}:${password}@${hostname}:${port}/${database}`;

// Local Development Mode Configuration (Currently Enabled)
const url = `mongodb://localhost:27017/GEG-database`;
```

---

## 🚀 Step 4: Install Dependencies and Start the Backend

After ensuring that MongoDB is running in the background and the mode is correctly configured, open your terminal and navigate to the `server/` directory:

1. **Install Project Dependencies:** Execute the following command to download all the required dependency packages for the backend server:

   ```
   npm install
   ```

   Upon successful execution, you should see a newly generated `node_modules/` folder inside the `server/` directory.

2. **Start the Backend Server:** Execute the following command to run `server.js`:
   ```
   node server.js
   ```
3. **Verify the Startup Results:** When the server runs successfully and establishes a smooth connection with the database, the terminal should output the following logs:
   ```
   Server is running on port 3000
   Database connected and initialized successfully
   Welcome to GEG-Server! Current version: x.x.x
   ```

---
