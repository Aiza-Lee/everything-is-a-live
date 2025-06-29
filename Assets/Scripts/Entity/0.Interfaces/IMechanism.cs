using System;

namespace GameLogic {
	public enum ActionLogic {
		Random,
		Loop,
	}
	public interface IMechanism : IEntity {
		/// <summary>
		/// 检测范围（朝上）
		/// </summary>
		Range DetectRange { get; }
		bool RangeActive { get; }
		/// <summary>
		/// 当前朝向
		/// </summary>
		Direction CurDrct { get; set; }
		bool IsBlock { get; }
		bool IsBlockLight { get; }
		string TypeID { get; }

		void TriggerFunc();
		void SetLevel(string level);

		event Action<Direction> OnDirectionChanged;
		event Action<GridPosition> OnPositionChanged;

	}
}