namespace GameLogic {
	public enum Direction {
		Zero,
		Left,
		Right,
		Up,
		Down
	}
	public static class DirectionExtensions {
		/// <summary>
		/// 将字符串转换为方向
		/// </summary>
		public static GridPosition FromString(string str) {
			if (string.IsNullOrEmpty(str)) {
				return new GridPosition(0, 0);
			}
			var res = new GridPosition(0, 0);
			for (int i = 0; i < str.Length; ++i) {
				res += str[i] switch {
					'L' or 'l' => new GridPosition(-1, 0),
					'R' or 'r' => new GridPosition(1, 0),
					'U' or 'u' => new GridPosition(0, 1),
					'D' or 'd' => new GridPosition(0, -1),
					_ => new GridPosition(0, 0),
				};
			}
			return res;
		}
		/// <summary>
		/// 旋转方向
		/// </summary>
		public static Direction Rotate(this Direction self, Rotation rotation) {
			return rotation switch {
				Rotation.Zero => self switch {
					Direction.Zero => Direction.Zero,
					Direction.Left => Direction.Left,
					Direction.Right => Direction.Right,
					Direction.Up => Direction.Up,
					Direction.Down => Direction.Down,
					_ => Direction.Zero
				},
				Rotation.ClockWise => self switch {
					Direction.Zero => Direction.Zero,
					Direction.Left => Direction.Up,
					Direction.Right => Direction.Down,
					Direction.Up => Direction.Right,
					Direction.Down => Direction.Left,
					_ => Direction.Zero
				},
				Rotation.CounterClockWise => self switch {
					Direction.Zero => Direction.Zero,
					Direction.Left => Direction.Down,
					Direction.Right => Direction.Up,
					Direction.Up => Direction.Left,
					Direction.Down => Direction.Right,
					_ => Direction.Zero
				},
				Rotation.Hemi => self switch {
					Direction.Zero => Direction.Zero,
					Direction.Left => Direction.Right,
					Direction.Right => Direction.Left,
					Direction.Up => Direction.Down,
					Direction.Down => Direction.Up,
					_ => Direction.Zero
				},
				Rotation.FlipX => self switch {
					Direction.Zero => Direction.Zero,
					Direction.Left => Direction.Right,
					Direction.Right => Direction.Left,
					Direction.Up => Direction.Up,
					Direction.Down => Direction.Down,
					_ => Direction.Zero
				},
				Rotation.FlipY => self switch {
					Direction.Zero => Direction.Zero,
					Direction.Left => Direction.Left,
					Direction.Right => Direction.Right,
					Direction.Up => Direction.Down,
					Direction.Down => Direction.Up,
					_ => Direction.Zero
				},
				_ => self
			};
		}
		/// <summary>
		/// 获取从一个方向到另一个方向的旋转
		/// </summary>
		public static Rotation GetRotation(Direction from, Direction to) {
			if (from == to) return Rotation.Zero;
			if (from == Direction.Zero || to == Direction.Zero) return Rotation.Zero;

			// 顺时针序列
			Direction[] order = { Direction.Up, Direction.Right, Direction.Down, Direction.Left };
			int idxFrom = System.Array.IndexOf(order, from);
			int idxTo = System.Array.IndexOf(order, to);
			if (idxFrom == -1 || idxTo == -1) return Rotation.Zero;

			int diff = (idxTo - idxFrom + 4) % 4;
			return diff switch {
				1 => Rotation.ClockWise,
				2 => Rotation.Hemi,
				3 => Rotation.CounterClockWise,
				_ => Rotation.Zero
			};
		}
	}
}