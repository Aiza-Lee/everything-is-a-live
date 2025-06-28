using NSFrame;
using UnityEngine;

namespace GameLogic {
	public class GridView : MonoBehaviour {

		[SerializeField] private Transform _GroudsParent;
		[SerializeField] private Transform _MechanismsParent;
		[SerializeField] private Transform _lightCellParent;

		[SerializeField] private PlayerView playerView;


		IGrid Grid { get; set; }

		public void Init(IGrid grid) {
			Grid = grid;
			grid.OnDestroy += OnModelDestroy;
		}

		private void OnModelDestroy() {
			Grid = null;
			PoolSystem.PushGO(this.gameObject);
		}
	}
}