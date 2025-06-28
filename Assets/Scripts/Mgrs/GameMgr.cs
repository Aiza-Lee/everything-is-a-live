using System.Xml;
using UnityEngine;

namespace GameLogic {
	public sealed class GameMgr {

		private GameMgr() { }
		public static GameMgr Inst { get; } = new();

		public GridPosition PlayerWinPosition { get; private set; }
		public GridPosition PlayerStartPosition { get; private set; }
		public IGrid Grid { get; private set; }
		public IPlayer Player { get; private set; }

		public bool CheckGameOver() {
			return Grid.CountLight(Player.Position) > 0;
		}
		public bool CheckPlayerWin() {
			return Player.Position == PlayerWinPosition;
		}

		public bool LoadLevel(string levelName) {
			Clear();
			var xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(Resources.Load<TextAsset>($"Config/Level/{levelName}").text);
			var root = xmlDoc.SelectSingleNode("Level");
			var csvStr = Resources.Load<TextAsset>($"Config/Level/{levelName}").text;
			Grid = new Grid();
			if (!Grid.LoadLevel(root, csvStr, out IPlayer player, out GridPosition playerWinPosition, out GridPosition playerStartPosition)) {
				Debug.LogError($"Failed to load level: {levelName}");
				return false;
			}
			PlayerWinPosition = playerWinPosition;
			PlayerStartPosition = playerStartPosition;
			Player = player;
			return true;
		}
		private void Clear() {
			Grid?.LogicDestroy();
			Grid = null;
			Player = null;
		}
	}
}