using UnityEngine;

namespace GameLogic {
	public class PlayerController : MonoBehaviour {

		public float Countdown => Mathf.Clamp(_countdown, 0f, _timeLimit);

		[SerializeField] private float _moveInterval = 0.2f;
		[SerializeField] private float _timeLimit = 2f;
		private float _countdown = 0f;
		private float _lastMoveTime = 0f;

		private IPlayer _player;
		private void OnEnable() {
			_lastMoveTime = Time.time;
			_countdown = _timeLimit;

			_player = GameMgr.Inst.Player;
			if (_player == null) {
				Debug.LogError("Player is not initialized.");
				return;
			}
		}
		private void Update() {
			_countdown -= Time.deltaTime;
			bool moved = false;
			if (Input.GetKeyDown(KeyCode.A)) {
				moved = _player.Move(Direction.Left);
			} else if (Input.GetKeyDown(KeyCode.D)) {
				moved = _player.Move(Direction.Right);
			} else if (Input.GetKeyDown(KeyCode.W)) {
				moved = _player.Move(Direction.Up);
			} else if (Input.GetKeyDown(KeyCode.S)) {
				moved = _player.Move(Direction.Down);
			}

			if (moved) {
				_lastMoveTime = Time.time;
				_countdown = _timeLimit;
			} else {
				if (Time.time - _lastMoveTime > _timeLimit) {
					GlobalMgr.Inst.LevelLose();
				}
			}
		}
	}
}