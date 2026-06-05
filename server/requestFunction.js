const {
  GameDefaultDataModel,
  PlayerSaveDataModel,
  GlobalLeaderBoardDataModel,
} = require("./modelDesign");

async function updateLeaderBoardData(
  leaderBoardType,
  newPlayerData,
  stageName = ""
) {
  try {
    let result;
    if (stageName == "") {
      result = await GlobalLeaderBoardDataModel.findOne({
        leaderBoardType: leaderBoardType,
      });
    } else {
      result = await GlobalLeaderBoardDataModel.findOne({
        leaderBoardType: leaderBoardType,
        stageName: stageName,
      });
    }

    let index = result.leaderBoardData.findIndex(
      (item) => item.playerName == newPlayerData.playerName
    );

    //Found -> Replace 
    if (index != -1) {
      switch(leaderBoardType){
        case "GameProgress":
          if(result.leaderBoardData[index].gameProgress < newPlayerData.gameProgress){
            result.leaderBoardData[index] = newPlayerData;
          }
          break;
        case "TotalScore":
          if(result.leaderBoardData[index].totalScore < newPlayerData.totalScore){
            result.leaderBoardData[index] = newPlayerData;
          }
          break;
        case "ClearStageBestRecord":
          result.leaderBoardData[index] = newPlayerData;
          break;
      }
    } else {
      result.leaderBoardData.push(newPlayerData);
    }

    // Stage or TotalProgress/Score
    if (stageName == "") {
      await GlobalLeaderBoardDataModel.findOneAndUpdate(
        { leaderBoardType: leaderBoardType },
        { $set: { leaderBoardData: result.leaderBoardData } },
        { new: true }
      );
    } else {
      await GlobalLeaderBoardDataModel.findOneAndUpdate(
        { leaderBoardType: leaderBoardType, stageName: stageName },
        { $set: { leaderBoardData: result.leaderBoardData } },
        { new: true }
      );
    }
  } catch (error) {
    console.error("Error:", error);
  }
}

async function updatePlayerSaveData(userName, saveData) {
  try {
    await PlayerSaveDataModel.findOneAndUpdate(
      { username: userName },
      { $set: { saveData: JSON.parse(saveData) } },
    );
  } catch (error) {
    console.error("Error:", error);
  }
}

module.exports = {
  updateLeaderBoardData,
  updatePlayerSaveData,
};
