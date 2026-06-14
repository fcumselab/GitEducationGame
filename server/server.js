const express = require("express");
const mongoose = require("mongoose");
const fs = require("fs");
const bodyParser = require("body-parser");
const cors = require("cors");
const myPackage = require("./package.json");
const path = require("path");

const {
  updateLeaderBoardData,
  updatePlayerSaveData,
} = require("./requestFunction");

const {
  GameDefaultDataModel,
  PlayerSaveDataModel,
  GlobalLeaderBoardDataModel,
  EventDataModel,
} = require("./modelDesign");

const app = express();

//GameDefaultData
let defaultPlayerSaveData;
let is_debug_mode = false;

async function insertGameDefaultData() {
  try {
    // Deploy Network Setting
    const username = process.env.MONGO_USERNAME;
    const password = process.env.MONGO_PASSWORD;
    const hostname = process.env.MONGO_HOST;
    const port = process.env.MONGO_PORT;
    const database = process.env.MONGO_DB;

    // const url = `mongodb://${username}:${password}@${hostname}:${port}/${database}`;

    // Testing Local Setting
    const url = `mongodb://localhost:27017/GEG-database`;

    await mongoose.connect(url, {
      useNewUrlParser: true,
      useUnifiedTopology: true,
    });

    // Step 1: Get player default save data
    defaultPlayerSaveData = await GameDefaultDataModel.findOne({
      key: "player-save-data",
    });

    if (!defaultPlayerSaveData) {
      // path: public/GameDefaultData/PlayerSaveData.json
      const jsonPath = path.join(
        __dirname,
        "public",
        "GameDefaultData",
        "PlayerSaveData.json",
      );

      // 讀取並解析 JSON 檔案
      const rawData = fs.readFileSync(jsonPath, "utf-8");
      const localJsonData = JSON.parse(rawData);

      // 寫入資料庫，並將結果存回變數
      defaultPlayerSaveData = await GameDefaultDataModel.create({
        key: "player-save-data",
        data: localJsonData,
      });
    }

    // Step 2: Get all default global leaderboard data
    const leaderboardCount = await GlobalLeaderBoardDataModel.countDocuments();
    if (leaderboardCount === 0) {
      const leaderboardJsonPath = path.join(
        __dirname,
        "public",
        "GameDefaultData",
        "globalleaderboarddatas-collection.json",
      );
      const leaderboardRawData = fs.readFileSync(leaderboardJsonPath, "utf-8");
      const leaderboardListData = JSON.parse(leaderboardRawData);

      await GlobalLeaderBoardDataModel.insertMany(leaderboardListData);
      console.log(
        `Initialize ${leaderboardListData.length} leaderboard data successfully.`,
      );
    }

    console.log("資料庫連接、初始化成功");
    console.log("歡迎使用 GEG-Server！目前版本: " + myPackage.version);
  } catch (err) {
    console.log("err! : " + err);
  }
}

// 配置 CORS
app.use(
  cors({
    // 只允許環境變數中設定的遊戲網址連入
    //For deploy, restrict some requests.
    // origin: process.env.GAME_ORIGIN,
    //For testing.
    origin: true,
    credentials: true,
  }),
);

insertGameDefaultData();

// 使用 body-parser 中間件來解析 application/x-www-form-urlencoded 格式的數據
// 這是獲取WWWform必要的解析
app.use(bodyParser.urlencoded({ extended: true }));

app.post("/SendEventTracker", function (req, res) {
  if (is_debug_mode) {
    console.log("SendEventTracker");
  }
  const { eventData } = req.body;
  let transformData = JSON.parse(eventData);

  EventDataModel.create({
    player: transformData.player,
    eventName: transformData.eventName,
    eventDetail: transformData.eventDetail,
    gameScene: transformData.gameScene,
    eventTime: new Date(transformData.eventTime),
  })
    .catch((e) => {
      console.log("create failed" + e);
      res.status(500).send("error");
    })
    .then((result) => {
      if (result != undefined) {
        res.status(200).send("success");
      }
    });
});

app.get("/getGlobalLeaderBoardData", function (req, res) {
  if (is_debug_mode) {
    console.log("Get getGlobalLeaderBoardData");
  }
  const leaderBoardType = req.query.type;
  const stageName = req.query.stageName;

  if (leaderBoardType == "ClearStageBestRecord") {
    console.log("Input: " + leaderBoardType + " : " + stageName);

    GlobalLeaderBoardDataModel.findOne({
      leaderBoardType: leaderBoardType,
      stageName: stageName,
    }).then((result) => {
      // playerScore descending, playerClearTime ascending
      let sortedData = result.leaderBoardData.sort(function (a, b) {
        return (
          b.playerScore - a.playerScore || a.playerClearTime - b.playerClearTime
        );
      });

      let place = 1;
      let placeList = [];
      for (let i = 0; i < sortedData.length; i++) {
        if (
          i > 0 &&
          sortedData[i].playerScore === sortedData[i - 1].playerScore &&
          sortedData[i].playerClearTime === sortedData[i - 1].playerClearTime
        ) {
          placeList.push(placeList[i - 1]);
        } else {
          placeList.push(place);
        }

        place++;
      }

      res.json({
        placeList: placeList,
        returnLeaderBoardData: sortedData,
      });
    });
  } else {
    GlobalLeaderBoardDataModel.findOne({
      leaderBoardType: leaderBoardType,
    }).then((result) => {
      let sortedData;
      let placeList = [];
      if (leaderBoardType == "GameProgress") {
        // gameProgress descending, playTime ascending
        sortedData = result.leaderBoardData.sort(function (a, b) {
          return b.gameProgress - a.gameProgress || a.playTime - b.playTime;
        });

        let place = 1;

        for (let i = 0; i < sortedData.length; i++) {
          if (
            i > 0 &&
            sortedData[i].gameProgress === sortedData[i - 1].gameProgress &&
            sortedData[i].playTime === sortedData[i - 1].playTime
          ) {
            placeList.push(placeList[i - 1]);
          } else {
            placeList.push(place);
          }

          place++;
        }
      } else if (leaderBoardType == "TotalScore") {
        // totalScore descending, playTime ascending
        sortedData = result.leaderBoardData.sort(function (a, b) {
          return b.totalScore - a.totalScore || a.playTime - b.playTime;
        });

        let place = 1;

        for (let i = 0; i < sortedData.length; i++) {
          if (
            i > 0 &&
            sortedData[i].totalScore === sortedData[i - 1].totalScore &&
            sortedData[i].playTime === sortedData[i - 1].playTime
          ) {
            placeList.push(placeList[i - 1]);
          } else {
            placeList.push(place);
          }

          place++;
        }
      }

      res.json({
        placeList: placeList,
        returnLeaderBoardData: sortedData,
      });
    });
  }
});

app.post("/UpdateGlobalLeaderBoardData", function (req, res) {
  if (is_debug_mode) {
    console.log("POST UpdateGlobalLeaderBoardData");
  }
  const { leaderBoardType } = req.body;
  let newPlayerData = null;
  switch (leaderBoardType) {
    case "GameProgress":
      newPlayerData = {
        playerName: req.body.playerName,
        gameProgress: parseInt(req.body.gameProgress),
        playTime: parseInt(req.body.playTime),
      };
      stageName = "";
      break;
    case "TotalScore":
      newPlayerData = {
        playerName: req.body.playerName,
        totalScore: parseInt(req.body.totalScore),
        playTime: parseInt(req.body.playTime),
      };
      stageName = "";
      break;
    case "ClearStageBestRecord":
      newPlayerData = {
        playerName: req.body.playerName,
        playerStar: parseInt(req.body.playerStar),
        playerScore: parseInt(req.body.playerScore),
        playerClearTime: parseInt(req.body.playerClearTime),
      };
      stageName = req.body.stageName;
      break;
  }

  updateLeaderBoardData(leaderBoardType, newPlayerData, stageName);
  res.json({ result: "Completed : " + stageName });
});

app.post("/UpdatePlayerData", function (req, res) {
  if (is_debug_mode) {
    console.log("POST UpdatePlayerData");
  }
  const { userName, saveData } = req.body;
  updatePlayerSaveData(userName, saveData);
  res.json({ result: "PlayerSaveData Uploaded" });
});

app.post("/signUp", function (req, res) {
  if (is_debug_mode) {
    console.log("POST signUp");
  }
  const { username, password } = req.body;

  PlayerSaveDataModel.findOne({ username: username }).then((result) => {
    if (!result) {
      PlayerSaveDataModel.create({
        username: username,
        password: password,
        saveData: defaultPlayerSaveData.data,
      });
      res.json({ status: "successful" });
    } else {
      res.json({ status: "already sign up" });
    }
  });
});

app.post("/login", function (req, res) {
  if (is_debug_mode) {
    console.log("GET login");
  }
  const { username, password } = req.body;

  PlayerSaveDataModel.findOne({ username: username }).then((result) => {
    if (!result) {
      res.json({ status: "username not found" });
    } else {
      if (result.password != password) {
        res.json({ status: "password incorrect" });
      } else {
        res.json({
          status: "successful",
          playerSaveData: result.saveData,
        });
      }
    }
  });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, (err) => {
  if (!err) {
    console.log(`Server is running on port ${PORT}`);
  } else console.log(err);
});

// 應用程序關閉時斷開連接
app.on("close", () => {
  mongoose.disconnect();
  console.log("Database connection closed.");
});
