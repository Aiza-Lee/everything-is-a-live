using GameLogic.View;
using NSFrame;
using UnityEngine;

namespace GameLogic {

	[RequireComponent(typeof(SmoothMove))]
	public class PlayerView : MonoBehaviour, IEntityView {
		private SpriteRenderer _spRndr;
		private SmoothMove _smoothMove;
		public IEntity Entity { get; private set; }

		private void Awake() {
			_spRndr = GetComponent<SpriteRenderer>();
			_smoothMove = GetComponent<SmoothMove>();
		}

		public void SetEntity(IEntity entity) {
			if (entity is not IPlayer player) {
				Debug.LogError("Entity is not a player");
				return;
			}
			Entity = player;
			player.OnLogicDestroy += OnLogicDestroy;
			_smoothMove.SetTarget(new Vector3(
				entity.Position.X,
				entity.Position.Y,
				-0f
			));
			player.OnMove += OnMove;
			_spRndr.sortingOrder = entity.GetSortingOrder();
			PlayerCameraMgr.Inst.SetPosition(transform.position);
		}

		public void Tick() { }

		void OnLogicDestroy() {
			Entity = null;
			PoolSystem.PushGO(this.gameObject);
		}
		void OnMove(Direction direction) {
			if (Entity is IPlayer player) {
				_spRndr.sortingOrder = player.GetSortingOrder();
				var newPosition = new Vector2(
					player.Position.X,
					player.Position.Y
				);
				_smoothMove.SetTarget(newPosition);
				PlayerCameraMgr.Inst.SetPosition(newPosition);
			} else {
				Debug.LogError("Entity is not a player");
			}
		}
	}
}