using System;

namespace GameLogic {
	/// <summary>
	/// 在网格上的物体的接口
	/// </summary>
	public interface IEntity {
		string GID { get; }
		GridPosition Position { get; set; }
		void LogicDestroy();
		event Action OnLogicDestroy;
		void Tick();
	}
}