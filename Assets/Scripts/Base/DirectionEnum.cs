namespace GameLogic {
	public enum Direction {
		Zero,
		Left,
		Right,
		Up,
		Down
	}
	public static class DirectionExtensions {
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
	}
}