using System.Collections.Generic;

namespace GameLogic {
	public struct Range {
		public List<GridPosition> positions;

		public void AddPosition(GridPosition pos) {
			positions ??= new List<GridPosition>();
			positions.Add(pos);
		}

		/// <summary>
		/// 旋转翻转，直接修改成员
		/// </summary>
		public readonly void Rotate90(Rotation rotation) {
			for (int i = 0; i < positions.Count; i++) {
				var p = positions[i];
				positions[i] = rotation switch {
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
		/// 计算相对于中心点的所有位置
		/// </summary>
		public readonly IEnumerable<GridPosition> CalculatePositions(GridPosition center) {
			foreach (var p in positions) {
				yield return new GridPosition(center.X + p.X, center.Y + p.Y);
			}
		}

		/// <summary>
		/// 检测某个位置是否在范围内
		/// </summary>
		public readonly bool CheckContained(GridPosition center, GridPosition pos) {
			foreach (var p in positions) {
				if (new GridPosition(center.X + p.X, center.Y + p.Y) == pos) {
					return true;
				}
			}
			return false;
		}
	}
}