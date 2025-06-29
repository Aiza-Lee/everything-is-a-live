using UnityEngine;

namespace GameLogic {

	public static class DirectionToPosition {
		public static GridPosition Convert(Direction dir) => dir switch {
			Direction.Zero => new GridPosition(0, 0),
			Direction.Left => new GridPosition(-1, 0),
			Direction.Right => new GridPosition(1, 0),
			Direction.Up => new GridPosition(0, 1),
			Direction.Down => new GridPosition(0, -1),
			_ => new GridPosition(0, 0)
		};
	}

	public struct GridPosition {
		public int X { get; set; }
		public int Y { get; set; }

		public GridPosition(int x, int y) {
			X = x;
			Y = y;
		}

		public static GridPosition operator +(GridPosition a, GridPosition b) => new(a.X + b.X, a.Y + b.Y);
		public static GridPosition operator -(GridPosition a, GridPosition b) => new(a.X - b.X, a.Y - b.Y);

		public static bool operator ==(GridPosition a, GridPosition b) => a.X == b.X && a.Y == b.Y;
		public static bool operator !=(GridPosition a, GridPosition b) => !(a == b);

		public override readonly bool Equals(object obj) {
			return obj is GridPosition other && this == other;
		}

		public override readonly int GetHashCode() {
			return System.HashCode.Combine(X, Y);
		}

		public override readonly string ToString() {
			return $"({X}, {Y})";
		}
		public readonly Vector2 AsVector2() {
			return new Vector2(X, Y);
		}
	}
}