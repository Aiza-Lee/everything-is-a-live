using GameLogic.View;
using NSFrame;
using UnityEngine;

namespace GameLogic {
	[RequireComponent(typeof(SmoothMove))]
	public class PlayerCameraMgr : MonoSingleton<PlayerCameraMgr> {
		private SmoothMove _smoothMove;
		public Vector2 DeadZoneSize = new(3f, 6f);

		protected override void Awake() {
			base.Awake();
			_smoothMove = GetComponent<SmoothMove>();
		}

		public void SetTargetPosition(Vector2 position) {
			Vector2 CamPos = transform.position;
			Vector2 delta = position - CamPos;
			var dx = Mathf.Max(0, Mathf.Abs(delta.x) - DeadZoneSize.x / 2f);
			var dy = Mathf.Max(0, Mathf.Abs(delta.y) - DeadZoneSize.y / 2f);
			if (dx > 0 || dy > 0) {
				CamPos.x += Mathf.Sign(delta.x) * dx;
				CamPos.y += Mathf.Sign(delta.y) * dy;
				if (_smoothMove != null) {
					_smoothMove.SetTarget(new Vector3(CamPos.x, CamPos.y, -10f));
				} else {
					Debug.LogError("SmoothMove component is not found on PlayerCameraMgr.");
				}
			}
		}
		public void SetCurPosition(Vector2 position) {
			_smoothMove.SetCurVal(new Vector3(position.x, position.y, -10f));
		}
	}
}