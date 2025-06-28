using System;
using UnityEditor;

namespace GameLogic {
	public class Ground : IGround {
		public GridPosition Position { get; set; }

		public string GID { get; } = Guid.NewGuid().ToString();

		public event Action OnLogicDestroy;
		public void LogicDestroy() {
			OnLogicDestroy?.Invoke();
			OnLogicDestroy = null;
		}
		public void Tick() {}

		public Ground(GridPosition position) {
			Position = position;
		}
	}
}