using NSFrame;
using UnityEngine;

namespace GameLogic {
	public class PlayerController : MonoSingleton<PlayerController> {

		public bool Controllable { get; set; } = false;
		private Transform _playerTrans;
		public Transform PlayerTrans {
			get {
				return _playerTrans;
			}
			set {
				_playerTrans = value;
				_player = value.GetComponent<PlayerView>().Entity as IPlayer;
				_lastMoveTime = Time.time;
				_countdown = _timeLimit;
			}
		}

		public float Countdown => Mathf.Clamp(_countdown, 0f, _timeLimit);

		[SerializeField] private float _moveInterval = 0.2f;
		[SerializeField] private float _timeLimit = 2f;
		private float _countdown = 0f;
		private float _lastMoveTime = 0f;

		private IPlayer _player;
		private void Update() {
			if (!Controllable) return;
			_countdown -= Time.deltaTime;
			bool moved = false;
			if (Input.GetKeyDown(KeyCode.A)) {
				AudioSystem.PlaySFX("move");
				moved = _player.Move(Direction.Left);
			} else if (Input.GetKeyDown(KeyCode.D)) {
				AudioSystem.PlaySFX("move");
				moved = _player.Move(Direction.Right);
			} else if (Input.GetKeyDown(KeyCode.W)) {
				AudioSystem.PlaySFX("move");
				moved = _player.Move(Direction.Up);
			} else if (Input.GetKeyDown(KeyCode.S)) {
				AudioSystem.PlaySFX("move");
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