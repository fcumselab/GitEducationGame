# 🎮 GITainment Project Introduction & Deployment Guide

Welcome to the project! This is a full-stack application that integrates a **Unity game frontend**, a **Node.js logical backend**, and a **MongoDB database**. This guide will lead you from scratch through environment configuration to successful deployment.

---

## 🗺️ Architecture

Before getting your hands dirty, let's look at a simple blueprint to understand how these three components communicate with each other:

- **🎮 Frontend (Unity):** Responsible for rendering the game visuals and handling player interactions. When players perform specific actions (e.g., logging in, saving level data, checking leaderboards), network requests are sent to the backend.
- **🧠 Backend (Node.js):** Acts as the game server, responsible for processing game logic, validating player data, and communicating with the database.
- **💾 Database (MongoDB):** Responsible for storing various core player data (e.g., account credentials, game progress) as well as global leaderboard data.

---

## 🛠️ Phase 1: Acquiring the Project & Environment Structure Introduction

First, please clone the project to your local environment:

```
git clone https://github.com/fcumselab/GitEducationGame.git
```

Upon successful cloning, you will see that the project contains the following four main core folders:

| **Folder Name** | **Purpose**          | **Description & Notes**                                                                                         |
| --------------- | -------------------- | --------------------------------------------------------------------------------------------------------------- |
| **`docker/`**   | Environment Setup    | Contains `docker-compose.yml`, used for a one-click setup and deployment of the entire game ecosystem later on. |
| **`server/`**   | Backend Service      | A Node.js project that serves as the game server, handling all API requests.                                    |
| **`game/`**     | Unity Source Code    | The Unity game project folder, used for developing and modifying game content and visuals.                      |
| **`gegGame/`**  | WebGL Static Webpage | The actual WebGL game files built (exported) from Unity.                                                        |

> ⚠️ **Reminder:**
> The files in the `gegGame/` folder **might not be the latest version**.
> If you modify any game content within the `game/` folder, please make sure to rebuild the WebGL files using Unity and overwrite (update) them in this folder. This ensures the web frontend displays the latest game graphics.

---

## Phase 2: Local Project & Environment Setup

To get the entire project up and running smoothly, please read and configure the following three environment tutorial documents in order:

1. **[💾 MongoDB Database Configuration Guide](tutorial-mongodb.md)**: Learn how to start the database locally and set up the connection string.
2. **[🧠 Node.js Backend Environment Setup Guide](tutorial-nodejs.md)**: Learn how to install Node.js, dependencies, and get the server running locally.
3. **[🎮 Unity Frontend Development Environment Configuration](tutorial-unity.md)**: Guides you through downloading the corresponding Unity version, successfully opening the game project, and fixing anomalies.

Once you have read through and successfully established a well-running project environment, you should be able to smoothly develop and test the game on your local computer.

---

## 🚀 Phase 3: Deploying the Game to the Server

When game features are fully developed and the updated version needs to be released to the server, please click the link below and follow the steps to execute a one-click deployment:

1. **[🚀 Server Docker Deployment Guide](tutorial-deploy.md)**: This guide will walk you through how to use the configurations in the `docker/` folder to pack and deploy the Unity web frontend, Node.js backend, and MongoDB all together.

---
