using UnityEngine;

namespace GameLogic {
	public class PlayerController : MonoBehaviour {
		private IPlayer _player;
		private void Update() {
			if (Input.GetKeyDown(KeyCode.A)) {
				_player.Move(Direction.Up);
			} else if (Input.GetKeyDown(KeyCode.D)) {
				_player.Move(Direction.Down);
			} else if (Input.GetKeyDown(KeyCode.W)) {
				_player.Move(Direction.Left);
			} else if (Input.GetKeyDown(KeyCode.S)) {
				_player.Move(Direction.Right);
			}
		}
	}
}