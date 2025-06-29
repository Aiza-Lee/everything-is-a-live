using System.Collections;
using System.Collections.Generic;
using NSFrame;
using UnityEngine;

namespace GameLogic {
	public sealed class GlobalMgr : MonoSingleton<GlobalMgr> {
		[SerializeField] private List<string> _levelNames;
		[SerializeField] private int _currentLevelIndex = 0;
		[SerializeField] private GridView _gridView;

		private void Start() {
			RestartLevel();
		}

		public void LevelLose() {
			PlayerCameraMgr.Inst.Focus(GameMgr.Inst.Player.Position.AsVector2());
			AudioSystem.PlaySFX("lose-light");
			PlayerController.Inst.Controllable = false;
			StartCoroutine(DelayCoroutine(1f, () => {
				RestartLevel();
			}));
		}

		public void LevelWin() {
			PlayerCameraMgr.Inst.Focus(GameMgr.Inst.Player.Position.AsVector2());
			AudioSystem.PlaySFX("win");
			PlayerController.Inst.Controllable = false;
			_currentLevelIndex++;
			StartCoroutine(DelayCoroutine(1f, () => {
				RestartLevel();
			}));
		}

		public void RestartLevel() {
			if (_currentLevelIndex >= _levelNames.Count) {
#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
				return;
			}
			PlayerController.Inst.Controllable = true;
			GameMgr.Inst.LoadLevel(_levelNames[_currentLevelIndex]);
			_gridView.Init(GameMgr.Inst.Grid);
		}

		private IEnumerator DelayCoroutine(float delaySeconds, System.Action callback) {
			yield return new WaitForSeconds(delaySeconds);
			callback?.Invoke();
		}
	}
}