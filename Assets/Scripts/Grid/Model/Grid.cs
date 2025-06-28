using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace GameLogic {
	public class Grid : IGrid {
		public int Height { get; set; }
		public int Width { get; set; }

		public ReadOnlyDictionary<string, IEntity> Entities_ReadOnly => new(_entities);

		// GID -> IEntity
		private Dictionary<string, IEntity> _entities;

		public bool LoadLevel(XmlNode root, string csvMap, out IPlayer player, out GridPosition playerWinPosition, out GridPosition playerStartPosition) {
			Height = int.Parse(root.SelectSingleNode("Height").InnerText);
			Width = int.Parse(root.SelectSingleNode("Width").InnerText);
			playerStartPosition = new GridPosition(1, 1);
			playerWinPosition = new GridPosition(Width, Height);
			player = new Player();
			_entities = new Dictionary<string, IEntity>();
			/* CSV_INFO */
			var rows = csvMap.Split('\n');
			var visited = MarkEnclosedArea(rows);
			for (int y = 0; y < Height; y++) {
				var row = rows[y].Trim().Split(',');
				for (int x = 0; x < Width; x++) {
					var cell = row[x].Trim();
					if (cell == "W") {
						AddEntity(_entities, new Ground(new GridPosition(x + 1, y + 1)));
						AddEntity(_entities, new Mechanism("Wall", new GridPosition(x + 1, y + 1)));
					}
					if (!visited[x, y]) continue;
					AddEntity(_entities, new Ground(new GridPosition(x + 1, y + 1)));
					switch (cell) {
						case "S":
							player.Position = new GridPosition(x + 1, y + 1);
							AddEntity(_entities, player);
							playerStartPosition = player.Position;
							break;
						case "Curtain":
							AddEntity(_entities, new Mechanism("Curtain", new GridPosition(x + 1, y + 1)));
							break;
						case "T":
							playerWinPosition = new GridPosition(x + 1, y + 1);
							break;
					}
				}
			}
			/* XML_INFO */
			var mechanismNodes = root.SelectNodes("Mechanism");
			foreach (XmlNode mechanismNode in mechanismNodes) {
				AddEntity(_entities, new Mechanism(mechanismNode));
			}
			CalculateMaps();
			return true;
		}

		public bool IsWalkable(GridPosition pos) {
			return !_blockMap[pos.X][pos.Y];
		}
		public int CountLight(GridPosition pos) {
			return _lightMap[pos.X][pos.Y];
		}
		public void CalculateMaps() {
			ClearMaps();
			// 先更新BlockMap，然后根据BlockMap更新LightMap
			foreach (var entity in _entities.Values) {
				if (entity is IMechanism mechanism) {
					if (mechanism.IsBlock) {
						_blockMap[mechanism.Position.X][mechanism.Position.Y] = true;
					}
					if (mechanism.IsBlockLight) {
						_blockLightMap[mechanism.Position.X][mechanism.Position.Y] = true;
					}
				}
			}
			foreach (var entity in _entities.Values) {
				if (entity is IMechanism mechanism) {
					var origin = mechanism.Position;
					foreach (var offset in mechanism.DetectRange.Positions) {
						var target = origin + offset;
						if (target.X < 1 || target.X > Width || target.Y < 1 || target.Y > Height)
							continue;
						if (!IsLineClear(origin, target))
							continue;
						_lightMap[target.X][target.Y]++;
					}
				}
			}
		}
		public void LogicDestroy() {
			foreach (var entity in _entities.Values) {
				entity.LogicDestroy();
			}
			_entities.Clear();
			OnDestroy?.Invoke();
			OnDestroy = null;
		}
		public event Action OnDestroy;


		private readonly List<List<int>> _lightMap = new();
		private readonly List<List<bool>> _blockMap = new();
		private readonly List<List<bool>> _blockLightMap = new();

		private void ClearMaps() {
			_lightMap.Clear();
			_blockMap.Clear();
			_blockLightMap.Clear();
			for (int i = 0; i <= Width; i++) {
				_lightMap.Add(new List<int>(Height + 1));
				_blockMap.Add(new List<bool>(Height + 1));
				_blockLightMap.Add(new List<bool>(Height + 1));
				for (int j = 0; j <= Height; j++) {
					_lightMap[i].Add(0);
					_blockMap[i].Add(false);
					_blockLightMap[i].Add(false);
				}
			}
		}

		private void AddEntity(Dictionary<string, IEntity> entities, IEntity entity) {
			if (entities.ContainsKey(entity.GID)) {
				throw new Exception($"Entity with GID {entity.GID} already exists.");
			}
			entities.Add(entity.GID, entity);
		}

		/// <summary>
		/// bfs标记内部节点
		/// </summary>
		private bool[,] MarkEnclosedArea(string[] rows) {
			var visited = new bool[Width, Height];

			var queue = new Queue<GridPosition>();
			// 从(1,1)开始，假设(1,1)一定在内部且不是墙体
			if (Width > 1 && Height > 1) {
				var startCell = rows[1].Trim().Split(',')[1].Trim();
				if (startCell != "W") {
					queue.Enqueue(new GridPosition(1, 1));
					visited[1, 1] = true;
				}
			}

			// 标记所有与(1,1)连通的格子为内部
			while (queue.Count > 0) {
				var pos = queue.Dequeue();
				int x = pos.X, y = pos.Y;
				int[] dx = { 0, 1, 0, -1 };
				int[] dy = { 1, 0, -1, 0 };
				for (int dir = 0; dir < 4; dir++) {
					int nx = x + dx[dir], ny = y + dy[dir];
					if (nx >= 0 && nx < Width && ny >= 0 && ny < Height && !visited[nx, ny]) {
						var cellVal = rows[ny].Trim().Split(',')[nx].Trim();
						if (cellVal != "W") {
							visited[nx, ny] = true;
							queue.Enqueue(new GridPosition(nx + 1, ny + 1));
						}
					}
				}
			}
			return visited;
		}

		// 判断两点连线是否被block阻挡
		private bool IsLineClear(GridPosition from, GridPosition to) {
			double x0 = from.X, y0 = from.Y, x1 = to.X, y1 = to.Y;
			double dx = x1 - x0, dy = y1 - y0;
			int steps = (int)Math.Max(Math.Abs(dx), Math.Abs(dy));
			if (steps == 0) {
				// 起点终点重合，直接检查
				int ix = (int)Math.Round(x0);
				int iy = (int)Math.Round(y0);
				if (ix >= 1 && ix < _blockLightMap.Count && iy >= 1 && iy < _blockLightMap[0].Count) {
					if (_blockLightMap[ix][iy]) return false;
				}
				return true;
			}
			double sx = dx / steps, sy = dy / steps;
			double x = x0, y = y0;
			for (int i = 0; i <= steps; i++) {
				int ix = (int)Math.Round(x);
				int iy = (int)Math.Round(y);
				if (ix >= 1 && ix < _blockLightMap.Count && iy >= 1 && iy < _blockLightMap[0].Count) {
					if (_blockLightMap[ix][iy]) return false;
				}
				x += sx;
				y += sy;
			}
			return true;
		}
	}
}