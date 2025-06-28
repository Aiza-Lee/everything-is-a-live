using UnityEngine;

namespace GameLogic {
	public class MechanismView : MonoBehaviour, IEntityView {
		public IEntity Entity { get; private set; }

		public void SetEntity(IEntity entity) {
			if (entity is not IMechanism mechanism) {
				Debug.LogError("Entity is not a mechanism");
				return;
			}
			Entity = entity;
			Entity.OnDestroy += OnMechanismDestroy;
			mechanism.OnDirectionChanged += OnMechanismDirectionChanged;
			mechanism.OnPositionChanged += OnMechanismPositionChanged;
		}

		public void Tick() {
		}

		private void OnMechanismDirectionChanged(Direction direction) {
			
		}
		private void OnMechanismPositionChanged(GridPosition position) {
			if (Entity == null) return;
			transform.position = new Vector3(position.X, position.Y, 0f);
		}
		private void OnMechanismDestroy() {
			Entity.OnDestroy -= OnMechanismDestroy;
			if (Entity is IMechanism mechanism) {
				mechanism.OnDirectionChanged -= OnMechanismDirectionChanged;
				mechanism.OnPositionChanged -= OnMechanismPositionChanged;
			}
		}
	}
}