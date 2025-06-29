using System.Collections.Generic;
using NSFrame;
using UnityEngine;

namespace GameLogic {
	public sealed class GlobalMgr : MonoSingleton<GlobalMgr> {
		[SerializeField] private List<string> _levelNames;
		[SerializeField] private int _currentLevelIndex = 0;
		[SerializeField] private GridView _gridView;

		private void Start() {
			GameMgr.Inst.LoadLevel(_levelNames[_currentLevelIndex]);
			_gridView.Init(GameMgr.Inst.Grid);
		}

		public void LevelLose() {
			//todo: 结束动画转场
			GameMgr.Inst.LoadLevel(_levelNames[_currentLevelIndex]);
			_gridView.Init(GameMgr.Inst.Grid);
		}

		public void LevelWin() {
			//todo: 结束动画转场
			_currentLevelIndex++;
			GameMgr.Inst.LoadLevel(_levelNames[_currentLevelIndex]);
			_gridView.Init(GameMgr.Inst.Grid);
		}
	}
}