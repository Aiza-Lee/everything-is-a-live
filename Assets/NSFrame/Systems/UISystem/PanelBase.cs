using UnityEngine;

namespace NSFrame 
{
	public abstract class PanelBase : MonoBehaviour {
		public UITypeEnum UIType;
		public int TypeIndex { get => (int)UIType; }
		public bool Opened;

		protected virtual void Awake() {
			this.AddToFrame();
		}
		public abstract void OnShow();
		public abstract void OnClose();
	}
}