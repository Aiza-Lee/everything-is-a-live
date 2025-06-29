using System.Collections.Generic;
using NSFrame;
using UnityEngine;

namespace GameLogic {
	public class GridView : MonoBehaviour {

		[SerializeField] private Transform _GroudsParent;
		[SerializeField] private Transform _MechanismsParent;
		[SerializeField] private Transform _lightCellParent;

		[SerializeField] private PlayerView playerView;

		private readonly List<GameObject> _lightCells = new();
		private GameObject _terminal;

		IGrid Grid { get; set; }

		public void Init(IGrid grid) {
			Grid = grid;
			grid.OnDestroy += OnModelDestroy;
			grid.OnReCalculateMaps += OnReCalculateMaps;
			OnReCalculateMaps();
			foreach (var entity in grid.Entities_ReadOnly.Values) {
				if (entity is IGround ground) {
					PrefabFactory.Inst.CreateGroundView(ground).transform.SetParent(_GroudsParent, false);
				} else if (entity is IMechanism mechanism) {
					var view = PrefabFactory.Inst.CreateMechanismView(mechanism);
					view.transform.SetParent(_MechanismsParent, false);
				} else if (entity is IPlayer player) {
					playerView = PrefabFactory.Inst.CreatePlayerView(player);
					playerView.transform.SetParent(this.transform, false);
					PlayerController.Inst.PlayerTrans = playerView.transform;
				}
			}

			_terminal = PrefabFactory.Inst.CreateTerminal();
			_terminal.transform.SetParent(this.transform, false);
			_terminal.transform.localPosition = new Vector3(GameMgr.Inst.PlayerWinPosition.X, GameMgr.Inst.PlayerWinPosition.Y, 0);
		}

		private void OnReCalculateMaps() {
			for (int i = 0; i < _lightCells.Count; i++) {
				PoolSystem.PushGO(_lightCells[i]);
			}
			_lightCells.Clear();
			for (int x = 1; x <= Grid.Width; x++) {
				for (int y = 1; y <= Grid.Height; y++) {
					var pos = new GridPosition(x, y);
					int lightCount = Grid.CountLight(pos);
					while (lightCount > 0) {
						var lightCell = PrefabFactory.Inst.CreateLightCell();
						lightCell.transform.SetParent(_lightCellParent, false);
						lightCell.transform.localPosition = new Vector3(x, y, 0);
						_lightCells.Add(lightCell);
						lightCount--;
					}
				}
			}
		}

		private void OnModelDestroy() {
			Grid = null;
			for (int i = 0; i < _lightCells.Count; i++) {
				PoolSystem.PushGO(_lightCells[i]);
			}
			_lightCells.Clear();
			PoolSystem.PushGO(_terminal);
		}
	}
}