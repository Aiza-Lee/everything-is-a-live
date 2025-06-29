using GameLogic.View;
using NSFrame;
using UnityEngine;

namespace GameLogic {
	[RequireComponent(typeof(SmoothMove))]
	public class PlayerCameraMgr : MonoSingleton<PlayerCameraMgr> {
		private SmoothMove _smoothMove;

		protected override void Awake() {
			base.Awake();
			_smoothMove = GetComponent<SmoothMove>();
		}

		public void SetPosition(Vector2 position) {
			if (_smoothMove != null) {
				_smoothMove.SetTarget(new(position.x, position.y, -10f));
			} else {
				Debug.LogError("SmoothMove component is not found on PlayerCameraMgr.");
			}
		}
	}
}