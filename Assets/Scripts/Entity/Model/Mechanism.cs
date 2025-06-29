using System;
using System.Xml;
using UnityEngine;

namespace GameLogic {
	public class Mechanism : IMechanism {
		public Range DetectRange { get; private set; } = new();
		public bool RangeActive { get; private set; } = true;
		public Direction CurDrct { get; set; }

		public bool IsBlock { get; private set; }
		public bool IsBlockLight { get; private set; }
		public GridPosition Position { get; set; }
		public string GID { get; private set; }

		public string TypeID { get; private set; }

		public event Action<Direction> OnDirectionChanged;
		public event Action<GridPosition> OnPositionChanged;
		public event Action OnLogicDestroy;

		/// <summary>
		/// 从关卡xml信息创建的机关
		/// </summary>
		public Mechanism(XmlNode root) {
			TypeID = root.SelectSingleNode("ID").InnerText;
			LoadDefaulConfig();
			GID = root.SelectSingleNode("GID").InnerText;
			Position = new GridPosition(
				int.Parse(root.SelectSingleNode("GridPosition").Attributes["x"].Value),
				int.Parse(root.SelectSingleNode("GridPosition").Attributes["y"].Value)
			);
			var initRotate = RotationExtensions.FromString(root.SelectSingleNode("InitRotate").InnerText);
			CurDrct = Direction.Up;
			_curLevel = root.SelectSingleNode("InitLevel").InnerText;

			var overrideConfig = root.SelectSingleNode("LevelConfigs")?.SelectNodes("LevelConfig");
			if (overrideConfig != null) {
				foreach (XmlNode config in overrideConfig) {
					var res = new MechanismLevelConfig(config);
					_config.OverrideLevelConfig(res);
				}
			}

			DetectRange = new(_config.MechanismLevels[_curLevel].DetectRange);
			Rotate(initRotate);
		}
		/// <summary>
		/// 加载默认配置
		/// 该方法会从配置管理器中获取默认的机关配置，并设置
		/// </summary>
		private void LoadDefaulConfig() {
			_config = MechanismConfigMgr.Inst.GetMechanismConfig(TypeID);
			IsBlock = _config.IsBlock;
			IsBlockLight = _config.IsBlockLight;
			TypeID = _config.TypeID;
		}
		/// <summary>
		/// special constructor for stationary mechanism
		/// <para>Wall, Ground, etc.</para>
		/// </summary>
		public Mechanism(string typeID, GridPosition position) {
			GID = Guid.NewGuid().ToString();
			Position = position;
			CurDrct = Direction.Up;
			IsBlock = typeID switch {
				"Wall" => true,
				"Curtain" => true,
				_ => false
			};
			IsBlockLight = typeID switch {
				"Wall" => true,
				"Curtain" => true,
				_ => false
			};
			TypeID = typeID;
		}

		public void TriggerFunc() {
			if (TypeID == "Curtain" || TypeID == "Wall") {
				return; // Stationary mechanisms do not have functions to trigger
			}
			if (_config.MechanismLevels[_curLevel].FuncType == FunctionType.Order) {
				var levelConfig = _config.MechanismLevels[_curLevel];
				var target = levelConfig.Order_Param_Target;
				var level = levelConfig.Order_Param_Level;
				for (int i = 0; i < target.Count; i++) {
					SetMechanismLevel(target[i], level[i]);
				}
			}
		}
		private void SetMechanismLevel(string target, string level) {
			if (GameMgr.Inst.Grid.Entities_ReadOnly.TryGetValue(target, out var entity)) {
				if (entity is IMechanism mechanism) {
					mechanism.SetLevel(level);
				} else {
					Debug.LogError($"Entity {target} is not a mechanism.");
				}
			} else {
				Debug.LogError($"Entity {target} not found.");
			}
		}

		public void SetLevel(string level) {
			Debug.Log($"Setting mechanism {GID} to level {level}");
			_curLevel = level;
			DetectRange = new(_config.MechanismLevels[_curLevel].DetectRange);
			DetectRange.Rotate(DirectionExtensions.GetRotation(Direction.Up, CurDrct));
		}

		public void LogicDestroy() {
			OnLogicDestroy?.Invoke();
			OnLogicDestroy = null;
			OnDirectionChanged = null;
			OnPositionChanged = null;
		}

		public void Tick() {
			if (TypeID == "Curtain" || TypeID == "Wall") {
				return;
			}
			var levelConfig = _config.MechanismLevels[_curLevel];

			var rotateAction = levelConfig.RotateActions[_rotateCnt];
			Rotate(rotateAction);
			_rotateCnt++;
			if (_rotateCnt >= levelConfig.RotateActions.Count) { _rotateCnt = 0; }

			var moveAction = levelConfig.MoveActions[_moveCnt];
			Move(moveAction);
			_moveCnt++;
			if (_moveCnt >= levelConfig.MoveActions.Count) { _moveCnt = 0; }

			var detectAction = levelConfig.DetectActions[_detectCnt];
			RangeActive = detectAction;
			_detectCnt++;
			if (_detectCnt >= levelConfig.DetectActions.Count) { _detectCnt = 0; }


			if (levelConfig.IsJump) {
				++_waitCnt;
				if (_waitCnt >= levelConfig.Jump_Param_Wait) {
					_waitCnt = 0;
					SetLevel(levelConfig.Jump_Param_TargetLevel);
				}
			}
		}

		private void Rotate(Rotation rotation) {
			if (rotation == Rotation.Zero) return;
			CurDrct = CurDrct.Rotate(rotation);
			DetectRange.Rotate(rotation);
			OnDirectionChanged?.Invoke(CurDrct);
		}
		private bool Move(GridPosition delta) {
			if (delta.X == 0 && delta.Y == 0) return true;
			Position += delta;
			OnPositionChanged?.Invoke(Position);
			return true;
		}

		private int _rotateCnt;
		private int _waitCnt;
		private int _detectCnt;
		private int _moveCnt;
		private string _curLevel = "1";
		private MechanismConfig _config;

	}
}