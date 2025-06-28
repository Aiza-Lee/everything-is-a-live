namespace GameLogic {
	public enum Rotation {
		Zero,
		ClockWise,
		CounterClockWise,
		FlipX,
		FlipY,
		Hemi,
	}
	public static class RotationExtensions {
		/// <summary>
		/// 将字符串转换为旋转枚举
		/// </summary>
		public static Rotation FromString(string str) {
			return str switch {
				"ClockWise" or "S" or "s" => Rotation.ClockWise,
				"CounterClockWise" or "N" or "n" => Rotation.CounterClockWise,
				"FlipX" or "X" or "x" => Rotation.FlipX,
				"FlipY" or "Y" or "y" => Rotation.FlipY,
				"Hemi" or "SS" or "NN" or "ss" or "nn" => Rotation.Hemi,
				_ => Rotation.Zero,
			};
		}
	}
}