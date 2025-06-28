using System.Collections.ObjectModel;
using System.Xml;

namespace GameLogic {
	public sealed class GameMgr {

		private GameMgr() { }
		public static GameMgr Inst { get; } = new();

		public IGrid Grid { get; } = new Grid();
		public IPlayer Player { get; } = new Player();

		public ReadOnlyCollection<IEntity> Entities => throw new System.NotImplementedException();

		public bool CheckGameOver() {
			throw new System.NotImplementedException();
		}

		public bool LoadLevel(XmlDocument xmlDoc) {
			throw new System.NotImplementedException();
		}
	}
}