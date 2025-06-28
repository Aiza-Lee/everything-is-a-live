using System.Collections.ObjectModel;
using System.Xml;

namespace GameLogic {
	/// <summary>
	/// 整个网格的接口，所有entity的管理类
	/// </summary>
	public interface IGrid {
		int Height { get; set; }
		int Width { get; set; }
		ReadOnlyDictionary<string, IEntity> Entities_ReadOnly { get; }
		bool LoadLevel(XmlNode root, string csvMap, out IPlayer player, out GridPosition playerWinPosition, out GridPosition playerStartPosition);
		bool IsWalkable(GridPosition pos);
		/// <summary>
		/// 检测某个点被多少个危险区域覆盖
		/// </summary>
		int CountLight(GridPosition pos);
		/// <summary>
		/// 计算所有实体的地图信息
		/// </summary>
		void CalculateMaps();

		void LogicDestroy();
		event System.Action OnDestroy;

	}
}