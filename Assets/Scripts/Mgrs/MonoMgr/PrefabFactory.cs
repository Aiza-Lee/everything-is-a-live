using System.Collections.Generic;
using NSFrame;
using UnityEngine;

namespace GameLogic {
	[System.Serializable]
	public struct KeyValuePair<TKey, TValue> {
		public TKey Key;
		public TValue Value;

		public KeyValuePair(TKey key, TValue value) {
			Key = key;
			Value = value;
		}
	}
	public sealed class PrefabFactory : MonoSingleton<PrefabFactory>, IPrefabFactory {
		[SerializeField] private List<KeyValuePair<string, Sprite>> _mechanismSprites;
		[SerializeField] private List<KeyValuePair<string, Animator>> _mechanismAnimators;
		[SerializeField] private GameObject _mechanismPrefab;
		[SerializeField] private GameObject _playerPrefab;
		[SerializeField] private GameObject _groundPrefab;
		[SerializeField] private GameObject _lightCellPrefab;
		[SerializeField] private GameObject _terminalPrefab;


		private void Start() {
			if (_mechanismPrefab == null || _playerPrefab == null || _groundPrefab == null) {
				Debug.LogError("PrefabFactory: One or more prefabs are not assigned.");
			}
			PoolSystem.InitPrefabPool(_mechanismPrefab);
			PoolSystem.InitPrefabPool(_playerPrefab);
			PoolSystem.InitPrefabPool(_groundPrefab);
			PoolSystem.InitPrefabPool(_lightCellPrefab);
			PoolSystem.InitPrefabPool(_terminalPrefab);
		}

		public GroundView CreateGroundView(IGround ground) {
			var entity = PoolSystem.PopGO<GroundView>(_groundPrefab);
			entity.SetEntity(ground);
			return entity;
		}

		public MechanismView CreateMechanismView(IMechanism mechanism) {
			var entity = PoolSystem.PopGO<MechanismView>(_mechanismPrefab);
			entity.SetEntity(mechanism);
			if (mechanism.TypeID != "Wall" && mechanism.TypeID != "Curtain") {
				entity.GetComponent<SpriteRenderer>().sprite = GetSprite(mechanism);
			}
			return entity;
		}

		private Sprite GetSprite(IMechanism mechanism) {
			var tarType = mechanism.TypeID.Trim().Split('_')[0];
			var pair = _mechanismSprites.Find(x => x.Key == tarType);
			if (pair.Key == null) {
				return _mechanismSprites.Find(x => x.Key == "temp").Value;
			} else {
				return pair.Value;
			}
		}

		public PlayerView CreatePlayerView(IPlayer player) {
			var entity = PoolSystem.PopGO<PlayerView>(_playerPrefab);
			entity.SetEntity(player);
			return entity;
		}

		public GameObject CreateLightCell() {
			return PoolSystem.PopGO(_lightCellPrefab);
		}

		public GameObject CreateTerminal() {
			return PoolSystem.PopGO(_terminalPrefab);
		}

	}
}