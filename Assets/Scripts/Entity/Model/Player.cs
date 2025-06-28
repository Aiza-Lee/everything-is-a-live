using System;

namespace GameLogic {
	public class Player : IPlayer {
		public string GID { get; } = Guid.NewGuid().ToString();
		public GridPosition Position { get; set; }
		public event Action OnLogicDestroy;

		public void LogicDestroy() {
			OnLogicDestroy?.Invoke();
			OnLogicDestroy = null;
		}

		public bool Move(Direction direction) {
			if (GameMgr.Inst.Grid.IsWalkable(Position + DirectionToPosition.Convert(direction))) {
				Position += DirectionToPosition.Convert(direction);
				TickMgr.Inst.Tick();
				return true;
			} else {
				return false;
			}
		}

		public void Tick() { }
	}
}