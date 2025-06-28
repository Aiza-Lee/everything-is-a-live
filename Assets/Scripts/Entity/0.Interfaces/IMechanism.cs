using System;
using System.Xml;

namespace GameLogic {
	public enum ActionLogic {
		Random,
		Loop,
	}
	public interface IMechanism : IEntity {
		/// <summary>
		/// 检测范围（朝上）
		/// </summary>
		Range DetectRange { get; set; }
		/// <summary>
		/// 当前朝向
		/// </summary>
		Direction Direction { get; set; }
		bool IsBlock { get; }

		event Action<Direction> OnDirectionChanged;
		event Action<GridPosition> OnPositionChanged;

		#region Default Implementations
		bool Detect(GridPosition pos) => DetectRange.CheckContained(Position, pos);
		#endregion

	}
}