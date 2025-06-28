using System;
using UnityEditor;

namespace GameLogic {
	public class Player : IPlayer {
		public GUID ID => new();
		public GridPosition Position { get; set; }
		public event Action OnDestroy;

		public void LogicDestroy() { }

		public bool Move(Direction direction) {
			if (GameMgr.Inst.Grid.IsWalkable(Position + DirectionToPosition.Convert(direction))) {
				Position += DirectionToPosition.Convert(direction);
				TickMgr.Inst.Tick();
				return true;
			} else {
				return false;
			}
		}

		public void Tick() {
			;
		}
	}
}