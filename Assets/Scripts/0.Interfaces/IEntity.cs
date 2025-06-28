using System;
using UnityEditor;

namespace GameLogic {
	/// <summary>
	/// 在网格上的物体的接口
	/// </summary>
	public interface IEntity {
		GUID ID { get; }
		GridPosition Position { get; set; }
		void LogicDestroy();
		event Action OnDestroy;
		void Tick();
	}
}