using UnityEngine;

namespace GameLogic {
	public class GroundView : MonoBehaviour, IEntityView {
		public IEntity Entity { get; private set; }

		public void SetEntity(IEntity entity) {
			if (entity is not IMechanism mechanism) {
				Debug.LogError("Entity is not a mechanism");
				return;
			}
			Entity = entity;
			// Additional setup for the ground view can be done here
		}

		public void Tick() { }
	}
}