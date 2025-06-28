using System.Collections.Generic;
using UnityEngine;

namespace GameLogic {
	public class MechanismConfigMgr {
		private MechanismConfigMgr() { }
		public static MechanismConfigMgr Inst { get; } = new();

		private readonly Dictionary<string, MechanismConfig> _mechanismConfigs = new();

		void LoadMechanismConfigs() {
			var files = Resources.LoadAll<TextAsset>("Configs/Mechanism");
			foreach (var file in files) {
				if (!file.name.EndsWith(".xml")) continue;
				var doc = new System.Xml.XmlDocument();
				doc.LoadXml(file.text);
				var root = doc.DocumentElement;
				if (root == null) {
					Debug.LogError($"MechanismConfigMgr: Failed to load XML from {file.name}, root element is null.");
					continue;
				}
				var ID = root.GetAttribute("ID");
				_mechanismConfigs.Add(ID, new(doc));
			}
		}
	}		
}