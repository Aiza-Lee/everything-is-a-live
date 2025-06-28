using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GameLogic {
	public class Range {
		public ReadOnlyCollection<GridPosition> Positions => _positions.AsReadOnly();
		private List<GridPosition> _positions = new();

		public Range() { }
		public Range(Range other) {
			_positions = new List<GridPosition>(other._positions);
		}

		public void AddPosition(GridPosition pos) {
			_positions ??= new List<GridPosition>();
			_positions.Add(pos);
		}

		/// <summary>
		/// 旋转翻转，直接修改成员
		/// </summary>
		public void Rotate(Rotation rotation) {
			for (int i = 0; i < _positions.Count; i++) {
				var p = _positions[i];
				_positions[i] = rotation switch {
					Rotation.ClockWise => new GridPosition(p.Y, -p.X),
					Rotation.CounterClockWise => new GridPosition(-p.Y, p.X),
					Rotation.FlipX => new GridPosition(-p.X, p.Y),
					Rotation.FlipY => new GridPosition(p.X, -p.Y),
					Rotation.Hemi => new GridPosition(-p.X, -p.Y),
					_ => throw new System.NotImplementedException(),
				};
			}
		}

		/// <summary>
		/// 检测某个位置是否在范围内
		/// </summary>
		public bool CheckContained(GridPosition center, GridPosition pos) {
			foreach (var p in _positions) {
				if (new GridPosition(center.X + p.X, center.Y + p.Y) == pos) {
					return true;
				}
			}
			return false;
		}
	}
}