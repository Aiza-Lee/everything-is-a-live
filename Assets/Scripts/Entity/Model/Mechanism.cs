using System;
using System.Collections.Generic;
using UnityEditor;

namespace GameLogic {
	public class Mechanism : IMechanism {
		public Range DetectRange { get; set; }
		public Direction Direction { get; set; }

		public bool IsBlock { get; private set; }

		public GUID ID;

		public GridPosition Position { get; set; }

		GUID IEntity.ID => throw new NotImplementedException();

		public event Action<Direction> OnDirectionChanged;
		public event Action<GridPosition> OnPositionChanged;
		public event Action OnDestroy;

		public void LogicDestroy() {
			throw new NotImplementedException();
		}

		public void Tick() {
			throw new NotImplementedException();
		}


		private long _tickCnt;
		private int _curLevel;
		private List<Rotation?> _rotateAct;
		private List<Direction> _moveAct;
		private List<bool> _detectAct; 
	}
}