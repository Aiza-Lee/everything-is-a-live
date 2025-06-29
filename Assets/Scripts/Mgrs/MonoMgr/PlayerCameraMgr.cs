using GameLogic.View;
using NSFrame;
using UnityEngine;

namespace GameLogic {
	[RequireComponent(typeof(SmoothMove), typeof(SmoothZRotate))]
	public class PlayerCameraMgr : MonoSingleton<PlayerCameraMgr> {
		private SmoothMove _smoothMove;
		private SmoothZRotate _smoothZRotate;
		public Vector2 DeadZoneSize = new(3f, 6f);

		[SerializeField] private float _maxShakeAngle = 25f; // 最大抖动角度
		[SerializeField] private float _shakeInterval = 1.0f; // 抖动间隔（秒）
		private float _shakeTimer = 0f;

		protected override void Awake() {
			base.Awake();
			_smoothMove = GetComponent<SmoothMove>();
			_smoothZRotate = GetComponent<SmoothZRotate>();
		}

		private void Update() {
			_shakeTimer += Time.deltaTime;
			if (_shakeTimer >= _shakeInterval) {
				_shakeTimer = 0f;
				float randomAngle = Random.Range(-_maxShakeAngle, _maxShakeAngle);
				_smoothZRotate.SetTarget(randomAngle);
				// 可选：_shakeInterval = Random.Range(0.5f, 1.5f); // 随机间隔更自然
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
		}
		public void SetCurPosition(Vector2 position) {
			_smoothMove.SetCurVal(new Vector3(position.x, position.y, -10f));
		}
	}
}