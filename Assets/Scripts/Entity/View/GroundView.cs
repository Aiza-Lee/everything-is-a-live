using NSFrame;
using UnityEngine;

namespace GameLogic {
	public class GroundView : MonoBehaviour, IEntityView {
		private SpriteRenderer _spRndr;
		public IEntity Entity { get; private set; }

		private void Awake() {
			_spRndr = GetComponent<SpriteRenderer>();
		}

		public void SetEntity(IEntity entity) {
			if (entity is not IMechanism mechanism) {
				Debug.LogError("Entity is not a mechanism");
				return;
			}
			Entity = mechanism;
			Entity.OnLogicDestroy += OnLogicDestroy;
			transform.position = new Vector3(
				entity.Position.X,
				entity.Position.Y,
				0f
			);
			_spRndr.sortingOrder = entity.GetSortingOrder();
		}

		public void Tick() { }

		private void OnLogicDestroy() {
			Entity = null;
			PoolSystem.PushGO(this.gameObject);
		}
	}
}