using System.Collections.Generic;
using System.Xml;

namespace GameLogic {
	public class Grid : IGrid {
		public int Height { get; set; }
		public int Width { get; set; }

		public bool IsWalkable(GridPosition pos) {
			throw new System.NotImplementedException();
		}

		public bool LoadLevel(XmlDocument xmlDoc) {
		}

		public int CountDanger(GridPosition pos) {
			throw new System.NotImplementedException();
		}

		private List<List<int>> _dangerMap;
		private 

	}
}