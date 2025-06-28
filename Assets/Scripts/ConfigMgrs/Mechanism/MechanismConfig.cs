using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace GameLogic {
	/// <summary>
	/// 机关的默认配置
	/// </summary>
	public class MechanismConfig {
		public string TypeID { get; set; }
		public bool IsBlock { get; set; }
		public bool IsBlockLight { get; set; }
		public Dictionary<string, MechanismLevelConfig> MechanismLevels { get; } = new();

		public MechanismConfig(MechanismConfig other) {
			TypeID = other.TypeID;
			IsBlock = other.IsBlock;
			IsBlockLight = other.IsBlockLight;
			foreach (var kvp in other.MechanismLevels) {
				MechanismLevels.Add(kvp.Key, new MechanismLevelConfig(kvp.Value));
			}
		}

		public MechanismConfig(XmlNode root) {
			TypeID = root.SelectSingleNode("ID").InnerText;
			IsBlock = bool.Parse(root.SelectSingleNode("IsBlock").InnerText);
			IsBlockLight = bool.Parse(root.SelectSingleNode("IsBlockLight").InnerText);
			var levelConfigs = root.SelectSingleNode("LevelConfigs");
			if (levelConfigs == null) return;

			var levelConfigNodes = levelConfigs.SelectNodes("LevelConfig").Cast<XmlNode>().ToList();
			foreach (var config in levelConfigNodes) {
				var level = config.Attributes["level"].Value;
				var res = new MechanismLevelConfig(config);
				MechanismLevels.Add(level, res);
			}
		}

		/// <summary>
		/// 覆盖某个等级的配置
		/// 如果该等级已存在，则覆盖；如果不存在，则添加新的等级配置
		/// </summary>
		public void OverrideLevelConfig(MechanismLevelConfig newConfig) {
			if (MechanismLevels.ContainsKey(newConfig.Level)) {
				MechanismLevels[newConfig.Level] = newConfig;
			} else {
				MechanismLevels.Add(newConfig.Level, newConfig);
			}
		}
	}

	public enum FunctionType {
		Order,
		Water,
	}

	public class MechanismLevelConfig {
		public string Level { get; set; }
		public Range DetectRange { get; set; }
		public List<GridPosition> MoveActions { get; } = new();
		public List<Rotation> RotateActions { get; } = new();
		public List<bool> DetectActions { get; } = new();
		/// <summary>
		/// 是否为自动跳转别的等级的等级
		/// </summary>
		public bool IsJump { get; set; }
		/// <summary>
		/// 等待跳转的时间
		/// </summary>
		public int Jump_Param_Wait { get; set; }
		/// <summary>
		/// 跳转目标
		/// </summary>
		public string Jump_Param_TargetLevel { get; set; }

		public FunctionType? FuncType { get; set; }
		public List<string> Order_Param_Target { get; set; }
		public List<string> Order_Param_Level { get; set; }

		public MechanismLevelConfig(MechanismLevelConfig other) {
			Level = other.Level;
			DetectRange = other.DetectRange;
			MoveActions.AddRange(other.MoveActions);
			RotateActions.AddRange(other.RotateActions);
			DetectActions.AddRange(other.DetectActions);
			IsJump = other.IsJump;
			Jump_Param_Wait = other.Jump_Param_Wait;
			Jump_Param_TargetLevel = other.Jump_Param_TargetLevel;
			FuncType = other.FuncType;
			Order_Param_Target = other.Order_Param_Target;
			Order_Param_Level = other.Order_Param_Level;
		}

		public MechanismLevelConfig(XmlNode root) {
			Level = root.Attributes["level"].Value;
			var detectRangeNode = root.SelectSingleNode("DetectRange");
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
				DetectRange = range;
			}

			var moveActionsNode = root.SelectSingleNode("Action/MoveLogic");
			moveActionsNode?.InnerText.Split(',')
					.Select(move => DirectionExtensions.FromString(move.Trim()))
					.ToList()
					.ForEach(d => MoveActions.Add(d));

			var rotateLogicNode = root.SelectSingleNode("Action/RotateLogic");
			rotateLogicNode?.InnerText.Split(',')
					.Select(rot => RotationExtensions.FromString(rot.Trim()))
					.ToList()
					.ForEach(r => RotateActions.Add(r));

			var detectLogicNode = root.SelectSingleNode("Action/DetectLogic");
			detectLogicNode?.InnerText.Split(',')
					.Select(detect => detect.Trim() == "1")
					.ToList()
					.ForEach(d => DetectActions.Add(d));

			var jumpNode = root.SelectSingleNode("IsJump");
			if (jumpNode != null) {
				IsJump = true;
				Jump_Param_Wait = int.Parse(jumpNode.SelectSingleNode("Wait").InnerText);
				Jump_Param_TargetLevel = jumpNode.SelectSingleNode("Target").InnerText;
			} else {
				IsJump = false;
			}

			var funcNode = root.SelectSingleNode("Function");
			if (funcNode != null) {
				FuncType = funcNode.Attributes["type"]?.Value switch {
					"Order" => FunctionType.Order,
					"Water" => FunctionType.Water,
					_ => FunctionType.Order
				};
				// 目前只处理 Order 类型的函数 
				Order_Param_Target = funcNode.SelectSingleNode("Targets").SelectNodes("Target")
					.Cast<XmlNode>()
					.Select(targetNode => targetNode.SelectSingleNode("GID").InnerText)
					.ToList();
				Order_Param_Level = funcNode.SelectSingleNode("Targets").SelectNodes("Target")
					.Cast<XmlNode>()
					.Select(targetNode => targetNode.SelectSingleNode("Level").InnerText)
					.ToList();
			} else {
				FuncType = null;
				Order_Param_Target = new List<string>();
				Order_Param_Level = new List<string>();
			}
		}
	}
}