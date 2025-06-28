using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;

namespace GameLogic {
	public class MechanismConfig {
		public string ID { get; set; }
		public bool IsBlock { get; set; }
		public bool IsBlockLight { get; set; }
		public Dictionary<int, MechanismLevelConfig> MechanismLevels { get; } = new();

		public MechanismConfig(XmlNode root) {
			ID = root.SelectSingleNode("ID").InnerText;
			IsBlock = bool.Parse(root.SelectSingleNode("IsBlock").InnerText);
			IsBlockLight = bool.Parse(root.SelectSingleNode("IsBlockLight").InnerText);
			var levelConfigs = root.SelectSingleNode("LevelConfigs");
			if (levelConfigs == null) return;

			var levelConfigNodes = levelConfigs.SelectNodes("LevelConfig").Cast<XmlNode>().ToList();
			foreach (var config in levelConfigNodes) {
				var res = new MechanismLevelConfig();

				var level = int.Parse(config.Attributes["level"].Value);

				var detectRangeNode = config.SelectSingleNode("DetectRange");
				if (detectRangeNode != null) {
					var range = new Range();
					detectRangeNode.SelectNodes("GridPosition")
						.Cast<XmlNode>()
						.ToList()
						.ForEach(gridNode => {
							int x = int.Parse(gridNode.Attributes["x"].Value);
							int y = int.Parse(gridNode.Attributes["y"].Value);
							range.AddPosition(new GridPosition(x, y));
						});
					res.DetectRange = range;
				}

				var moveActionsNode = config.SelectSingleNode("Action/MoveLogic");

				var rotateLogicNode = config.SelectSingleNode("Action/RotateLogic");
				rotateLogicNode?.InnerText.Split(',')
						.Select(rot => RotationExtensions.FromString(rot.Trim()))
						.ToList()
						.ForEach(r => res.RotateActions.Add(r));

				MechanismLevels.Add(level, res);
			}
		}
	}
	public class MechanismLevelConfig {
		public Range DetectRange { get; set; }
		
		public List<GridPosition> MoveActions { get; } = new();
		public List<Rotation?> RotateActions { get; } = new();
		public List<bool> WorkingActions { get; } = new();
		/// <summary>
		/// 是否为自动跳转别的等级的等级
		/// </summary>
		public bool IsJump { get; set; }
		/// <summary>
		/// 等待跳转的时间
		/// </summary>
		public int Wait { get; set; }
		/// <summary>
		/// 跳转目标
		/// </summary>
		public int TargetLevel { get; set; }
	}
}