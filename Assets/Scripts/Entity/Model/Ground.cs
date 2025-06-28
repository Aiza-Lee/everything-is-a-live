using System;
using UnityEditor;

namespace GameLogic {
	public class Ground : IGround {
		public GUID ID { get; } = new GUID();
		public GridPosition Position { get; set; }
		public event Action OnDestroy;
		public void LogicDestroy() {}
		public void Tick() {}
	}
}