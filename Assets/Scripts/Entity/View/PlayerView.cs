using NSFrame;
using UnityEngine;

namespace GameLogic {
	public class PlayerView : MonoBehaviour, IEntityView {
		private SpriteRenderer _spRndr;
		public IEntity Entity { get; private set; }

		private void Awake() {
			_spRndr = GetComponent<SpriteRenderer>();
		}

		public void SetEntity(IEntity entity) {
			if (entity is not IPlayer player) {
				Debug.LogError("Entity is not a player");
				return;
			}
			Entity = player;
			player.OnLogicDestroy += OnLogicDestroy;
			transform.position = new Vector3(
				entity.Position.X,
				entity.Position.Y,
				0f
			);
			player.OnMove += OnMove;
			_spRndr.sortingOrder = entity.GetSortingOrder();
		}

		public void Tick() { }

		void OnLogicDestroy() {
			Entity = null;
			PoolSystem.PushGO(this.gameObject);
		}
		void OnMove(Direction direction) {
			if (Entity is IPlayer player) {
				transform.position = new Vector3(
					player.Position.X,
					player.Position.Y,
					0f
				);
				_spRndr.sortingOrder = player.GetSortingOrder();
			} else {
				Debug.LogError("Entity is not a player");
			}
		}
	}
}