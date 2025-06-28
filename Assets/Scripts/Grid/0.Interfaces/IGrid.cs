using System.Xml;

namespace GameLogic {
	/// <summary>
	/// 整个网格的接口
	/// </summary>
	public interface IGrid {
		int Height { get; set; }
		int Width { get; set; }
		bool LoadLevel(XmlDocument xmlDoc);
		bool IsWalkable(GridPosition pos);
		/// <summary>
		/// 检测某个点被多少个危险区域覆盖
		/// </summary>
		int CountDanger(GridPosition pos);

	}
}