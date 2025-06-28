using UnityEngine;

namespace GameLogic {
	public class MechanismView : MonoBehaviour, IEntityView {
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
			Entity = entity;
			Entity.OnLogicDestroy += OnLogicDestroy;
			mechanism.OnDirectionChanged += OnMechanismDirectionChanged;
			mechanism.OnPositionChanged += OnMechanismPositionChanged;
			transform.position = new Vector3(
				entity.Position.X,
				entity.Position.Y,
				0f
			);
			_spRndr.sortingOrder = entity.GetSortingOrder();
		}

		public void Tick() {
		}

		private void OnMechanismDirectionChanged(Direction direction) {

		}
		private void OnMechanismPositionChanged(GridPosition position) {
			if (Entity == null) return;
			transform.position = new Vector3(position.X, position.Y, 0f);
		}
		private void OnLogicDestroy() {
			if (Entity is IMechanism mechanism) {
				mechanism.OnDirectionChanged -= OnMechanismDirectionChanged;
				mechanism.OnPositionChanged -= OnMechanismPositionChanged;
			}
			Entity = null;
		}
	}
}