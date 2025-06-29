using UnityEngine;

namespace GameLogic {
	public class PlayerController : MonoBehaviour {
		private IPlayer _player;
		private void OnEnable() {
			_player = GameMgr.Inst.Player;
			if (_player == null) {
				Debug.LogError("Player is not initialized.");
				return;
			}
		}
		private void Update() {
			bool tryedMove = true;
			if (Input.GetKeyDown(KeyCode.A)) {
				_player.Move(Direction.Left);
			} else if (Input.GetKeyDown(KeyCode.D)) {
				_player.Move(Direction.Right);
			} else if (Input.GetKeyDown(KeyCode.W)) {
				_player.Move(Direction.Up);
			} else if (Input.GetKeyDown(KeyCode.S)) {
				_player.Move(Direction.Down);
			} else {
				tryedMove = false;
			}
			if (tryedMove) {
				PlayerCameraMgr.Inst.SetPosition(new Vector2(_player.Position.X, _player.Position.Y));
			}
		}
	}
}