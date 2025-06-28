using System.Collections.Generic;
using UnityEngine;

namespace GameLogic {
	public class MechanismConfigMgr {
		private MechanismConfigMgr() { 
			LoadMechanismConfigs();
		}
		public static MechanismConfigMgr Inst { get; } = new();

		public MechanismConfig GetMechanismConfig(string typeID) {
			if (_mechanismConfigs.TryGetValue(typeID, out var config)) {
				return config;
			}
			Debug.LogError($"MechanismConfigMgr: No config found for ID {typeID}");
			return null;
		}

		private readonly Dictionary<string, MechanismConfig> _mechanismConfigs = new();

		void LoadMechanismConfigs() {
			var files = Resources.LoadAll<TextAsset>("Config/Mechanism/");
			foreach (var file in files) {
				// if (!file.name.EndsWith(".xml")) continue;
				Debug.Log($"MechanismConfigMgr: Loading XML from {file.name}");
				var doc = new System.Xml.XmlDocument();
				doc.LoadXml(file.text);
				var root = doc.DocumentElement;
				if (root == null) {
					Debug.LogError($"MechanismConfigMgr: Failed to load XML from {file.name}, root element is null.");
					continue;
				}
				var ID = root.GetAttribute("ID");
				_mechanismConfigs.Add(ID, new(root));
			}
		}
	}		
}