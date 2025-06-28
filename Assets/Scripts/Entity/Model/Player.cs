using System;

namespace GameLogic {
	public class Player : IPlayer {
		public string GID { get; } = Guid.NewGuid().ToString();
		public GridPosition Position { get; set; }
		public event Action OnLogicDestroy;
		public event Action<Direction> OnMove;

		public void LogicDestroy() {
			OnLogicDestroy?.Invoke();
			OnLogicDestroy = null;
			OnMove = null;
		}

		public bool Move(Direction direction) {
			if (direction == Direction.Zero) return true;
			if (GameMgr.Inst.Grid.IsWalkable(Position + DirectionToPosition.Convert(direction))) {
				Position += DirectionToPosition.Convert(direction);
				TickMgr.Inst.Tick();
				OnMove?.Invoke(direction);
				return true;
			} else {
				return false;
			}
		}

		public void Tick() { }
	}
}