using GameLogic.View;
using NSFrame;
using UnityEngine;

namespace GameLogic {
	public enum CamSize {
		Focus = 3,
		Normal = 5,
	}
	[RequireComponent(typeof(SmoothMove), typeof(SmoothZRotate))]
	public class PlayerCameraMgr : MonoSingleton<PlayerCameraMgr> {
		private SmoothMove _smoothMove;
		private SmoothZRotate _smoothZRotate;
		private Camera _camera;
		public Vector2 DeadZoneSize = new(3f, 6f);

		[SerializeField] private float _maxShakeAngle = 25f; // 最大抖动角度
		[SerializeField] private float _shakeInterval = 1.0f; // 抖动间隔（秒）
		private float _shakeTimer = 0f;

		protected override void Awake() {
			base.Awake();
			_smoothMove = GetComponent<SmoothMove>();
			_smoothZRotate = GetComponent<SmoothZRotate>();
			_camera = GetComponent<Camera>();
		}

		private void Update() {
			_shakeTimer += Time.deltaTime;
			if (_shakeTimer >= _shakeInterval) {
				_shakeTimer = 0f;
				float randomAngle = Random.Range(-_maxShakeAngle, _maxShakeAngle);
				_smoothZRotate.SetTarget(randomAngle);
				_shakeInterval = Random.Range(0.5f, 1.5f);
			}
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

			_camera.orthographicSize = (int) CamSize.Normal;
		}
		public void SetCurPosition(Vector2 position) {
			_smoothMove.SetCurVal(new Vector3(position.x, position.y, -10f));
			_camera.orthographicSize = (int) CamSize.Normal;
		}

		public void Focus(Vector2 position) {
			_camera.orthographicSize = (int) CamSize.Focus;
			if (_smoothMove != null) {
				_smoothMove.SetTarget(new Vector3(position.x, position.y, -10f));
			} else {
				Debug.LogError("SmoothMove component is not found on PlayerCameraMgr.");
			}
		}
	}
}