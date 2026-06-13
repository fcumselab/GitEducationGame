# 🚀 Server Docker Deployment Guide

This guide will walk you through how to deploy your locally developed game (including the Unity WebGL frontend, Node.js backend, and MongoDB database) onto the laboratory's physical server host using Docker, allowing everyone to connect and play.

---

## 🛠️ Phase 1: Local Packaging & Environment Configuration Preparation

Before uploading files to the server, we must first toggle the "connection URL" to remote mode on our local computer and build the latest version of the game's web files.

### 1. Modify Node.js Backend Connection Configuration

1. Open `server.js` (or the file responsible for database connections) located inside the `server/` folder.
2. Locate the following code block, comment out the Local mode, and **enable the Remote (Remote Production Mode)** `url` connection:

```javascript
// Remote Production Mode Configuration (Enable this block when deploying to the lab server)
const username = process.env.MONGO_USERNAME;
const password = process.env.MONGO_PASSWORD;
const hostname = process.env.MONGO_HOST;
const port = process.env.MONGO_PORT;
const database = process.env.MONGO_DB;
const url = `mongodb://${username}:${password}@${hostname}:${port}/${database}`;

// Local Development Mode Configuration (Comment this block out when deploying)
// const url = `mongodb://localhost:27017/GEG-database`;
```

### 2. Modify Unity Frontend Connection Configuration

1. Inside the Unity Editor, open the three core scenes in sequence (`Title Screen`, `Stage Select`, `Play Game`).
2. In the Hierarchy of each scene, locate and expand the **`URLSettings`** game object:
   - ❌ **Deactivate:** `URLSetting (Local Testing)`
   - ✅ **Activate:** `URLSetting (Online Deploy)`

3. Click on the `URLSetting (Online Deploy)` object. In the **Inspector** on the right side under `Url Setting (Script)`, **modify the URL field to the physical IP and Port of the laboratory server** (you must input the backend service port `5051`, e.g., `http://140.xxx.xx.xx:5051/`).

### 3. Build Unity WebGL & Overwrite `gegGame/`

1. In the top menu of Unity, click **File ➔ Build Settings...** and confirm that the target platform is set to **WebGL**.
2. Click **Build**.
3. Select the **`gegGame/`** folder under the project's root directory as the output destination for the build (directly overwriting the old files inside it).

---

## ⚙️ Phase 2: Modifying Docker Configurations & Remote Connection Transfer

Once the local source code adjustments and WebGL build are fully prepared, we need to adjust the Docker configuration file and transfer the files to the laboratory server host.

### 1. Modify Environment Variables in `docker-compose.yml`

1. Open the `docker-compose.yml` file located under the `docker/` folder.
2. Locate the `environment` block under the `server` service section.
3. Change the `your_ip` placeholder following `GAME_ORIGIN` to the **physical IP of the laboratory server**:

```yaml
server:
  container_name: Kevin-GEG-server
  build: ./server
  ports:
    - "5051:3000"
  environment:
    - GAME_ORIGIN=http://140.xxx.xx.xx:5050 # 👈 Please change your_ip to the physical IP of the server
    - MONGO_USERNAME=KUser
    - MONGO_PASSWORD=KPass
    - MONGO_HOST=Kevin-GEG-mongo
    - MONGO_PORT=27017
    - MONGO_DB=GEG-database?authSource=admin
```

_(Note: Other settings such as database credentials and port numbers have been pre-configured by predecessors; please do not alter them unless there is a specific requirement.)_

### 2. Connect to the Server Using MobaXterm

To transfer files onto the laboratory's physical host, we need to use an SSH/SFTP tool:

1. Download and open **[MobaXterm](https://mobaxterm.mobatek.net/)** on your computer.
2. Click **Session ➔ SSH** in the top left corner.
3. In the **Remote host** field, input the physical IP of the server. Check the **Specify username** box and enter the server's administrator account name.
4. Click OK, then input the password to log in. Upon successful login, you will see the terminal screen, and the **server's file browser panel (SFTP) will appear on the left side**.

### 3. Create a Project Folder on the Server and Upload Files

1. In the MobaXterm terminal, input the following commands to navigate to the directory where you want to store the project (e.g., `/home/username/`), and create a dedicated folder:

   ```bash
   mkdir GITainment
   cd GITainment
   ```

2. In the file browser panel on the left side, navigate into the newly created `GITainment/` folder.

3. Directly **drag and drop** the **following three items** from your local computer into the left panel of MobaXterm to upload them:
   - 📁 **`gegGame/`** folder (contains the newly built WebGL static webpage and its Dockerfile)

   - 📁 **`server/`** folder (contains the backend source code and its Dockerfile)

   - 📄 **`docker-compose.yml`** file (the version where you just updated the IP address)

   > 💡 **Upload Structure Check:** Please ensure that within the `GITainment/` folder on the server, these three items are **on the same directory level (side-by-side)**. This is crucial so that Docker Compose can locate the correct corresponding paths during compilation!

---

## 🚀 Phase 3: Server-Side Docker Startup & Verification

Once all files are uploaded, we can execute commands on the server to let Docker automatically establish and run the entire game ecosystem in one click.

### 1. Execute Docker Compose to Build and Start Containers

1. In the MobaXterm terminal, ensure that your current path is inside the `GITainment/` folder you created.
2. Input the following one-click startup command (since administrative privileges are required, please prefix it with `sudo`):
   ```bash
   sudo docker compose up -d --build
   ```

- 💡 **Command Breakdown:**
  - `up`: Builds, creates, and starts all services defined in the configuration file (game, server, mongo).
  - `-d`: Runs in the background (Detached mode), ensuring the server continues to operate even after closing MobaXterm.
  - `--build`: Forces a recompilation of the latest backend code and WebGL static files, ensuring that your uploaded changes take effect.

3. Wait for the terminal to finish the downloading and compilation progress bars. When the screen displays that all three services are `Started` or `Running`, the startup is successful!

### 2. Open a Browser to Test the Game

1. Open any web browser (Chrome, Edge, etc.) on your local computer.

2. Enter the laboratory server's **IP and the game frontend's Port number (5050)** in the address bar:

   ```
   http://140.xxx.xx.xx:5050
   ```

3. If the game screen loads successfully and you can seamlessly register, log in, and play, your remote deployment is completely successful!

---

## Supplement: How to Connect to the Remote MongoDB to View Data

If you need to check online player accounts or leaderboard data in the future, you can connect directly to the laboratory server using MongoDB Compass from your local machine:

1. Open **MongoDB Compass`** on your own local computer.

2. Click **`Add new connection`**.

3. Based on the settings in the `docker-compose.yml` file, the external Port for the remote MongoDB is **`5052`**, and the credentials are `KUser` / `KPass`. Please modify the connection string (URI) into the following format:

   ```
   mongodb://KUser:KPass@140.xxx.xx.xx:5052/?authSource=admin
   ```

   _(Please replace 140.xxx.xx.xx with the actual laboratory server IP)._

4. Click **`Save & Connect`**, and you will be able to directly and visually manage the `GEG-database` database on the remote server!

---
