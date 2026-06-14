const mongoose = require('mongoose')

const DefaultDataSchema = new mongoose.Schema({
  key: { type: String },
  data: mongoose.Schema.Types.Mixed,
});

const GlobalLeaderBoardDataSchema = new mongoose.Schema({
  leaderBoardType: { type: String, require: true},
  stageName: { type: String, require: false },
  leaderBoardData: mongoose.Schema.Types.Mixed
});

const PlayerSaveDataSchema = new mongoose.Schema({
  username: { type: String },
  password: { type: String },
  saveData: mongoose.Schema.Types.Mixed,
});

const EventDataSchema = new mongoose.Schema({
  player: { type: String },
  eventName: { type: String },
  eventDetail: { type: String },
  gameScene: { type: String },
  eventTime: { type: Date }
});

const GameDefaultDataModel = mongoose.model('DefaultData', DefaultDataSchema);
const GlobalLeaderBoardDataModel = mongoose.model('GlobalLeaderBoardData', GlobalLeaderBoardDataSchema);
const PlayerSaveDataModel = mongoose.model('PlayerSaveData', PlayerSaveDataSchema);
const EventDataModel = mongoose.model('EventData', EventDataSchema);


module.exports = {
  GameDefaultDataModel,
  PlayerSaveDataModel,
  GlobalLeaderBoardDataModel,
  EventDataModel
}