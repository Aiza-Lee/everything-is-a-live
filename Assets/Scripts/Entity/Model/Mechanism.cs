using System;
using System.Xml;
using UnityEngine;

namespace GameLogic {
	public class Mechanism : IMechanism {
		public Range DetectRange { get; private set; } = new();
		public Direction CurDrct { get; set; }

		public bool IsBlock { get; private set; }
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
			_curLevel = int.Parse(root.SelectSingleNode("InitLevel").InnerText);

			var overrideConfig = root.SelectSingleNode("LevelConfigs").SelectNodes("LevelConfig");
			if (overrideConfig != null) {
				foreach (XmlNode config in overrideConfig) {
					var res = new MechanismLevelConfig(config);
					_config.OverrideLevelConfig(res);
				}
			}

			Rotate(initRotate);
			DetectRange = new(_config.MechanismLevels[_curLevel].DetectRange);
		}
		/// <summary>
		/// 加载默认配置
		/// 该方法会从配置管理器中获取默认的机关配置，并设置
		/// </summary>
		private void LoadDefaulConfig() {
			_config = MechanismConfigMgr.Inst.GetMechanismConfig(TypeID);
			IsBlock = _config.IsBlock;
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
			TypeID = typeID;
		}

		public void TriggerFunc() {
			if (_config.MechanismLevels[_curLevel].FuncType == FunctionType.TriggerLevel) {
				var levelConfig = _config.MechanismLevels[_curLevel];
				var target = levelConfig.Function_Param_Target;
				var level = levelConfig.Function_Param_Level;
				GameMgr.Inst.Grid.Entities_ReadOnly.TryGetValue(target, out var entity);
				if (entity is IMechanism mechanism) {
					mechanism.SetLevel(level);
					Debug.Log($"Mechanism {target} set to level {level}");
				} else {
					Debug.LogError($"Mechanism {target} not found or is not a mechanism.");
				}
			}
		}

		public void SetLevel(int level) {
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
			var levelConfig = _config.MechanismLevels[_curLevel];

			var rotateAction = levelConfig.RotateActions[_rotateCnt];
			Rotate(rotateAction);
			_rotateCnt++;
			if (_rotateCnt >= levelConfig.RotateActions.Count) { _rotateCnt = 0; }

			var moveAction = levelConfig.MoveActions[_moveCnt];
			Move(moveAction);
			_moveCnt++;
			if (_moveCnt >= levelConfig.MoveActions.Count) { _moveCnt = 0; }

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
		private int _moveCnt;
		private int _curLevel = 1;
		private MechanismConfig _config;

	}
}