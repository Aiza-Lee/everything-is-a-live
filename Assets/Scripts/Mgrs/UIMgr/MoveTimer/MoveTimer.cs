using NSFrame;
using TMPro;
using UnityEngine;

namespace GameLogic {
	public class MoveTimer : PanelBase {

		[SerializeField] private TextMeshProUGUI _timerText;

		private PlayerController Controller => PlayerController.Inst;

		private void Update() {
			if (Controller == null) {
				_timerText.text = "X";
			} else {
				_timerText.text = Controller.Countdown.ToString("F1");
			}
		}

		public override void OnClose() {
			_timerText.text = "X";
		}

		public override void OnShow() {}
	}
}