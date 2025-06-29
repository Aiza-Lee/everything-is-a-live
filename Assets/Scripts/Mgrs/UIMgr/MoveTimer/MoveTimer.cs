using NSFrame;
using TMPro;
using UnityEngine;

namespace GameLogic {
	public class MoveTimer : PanelBase {

		[SerializeField] private TextMeshProUGUI _timerText;

		private PlayerController _controller;

		public void SetController(PlayerController controller) {
			_controller = controller;
			Update();
		}

		private void Update() {
			if (_controller == null) {
				_timerText.text = "X";
			} else {
				_timerText.text = _controller.Countdown.ToString("F1");
			}
		}

		public override void OnClose() {
			_controller = null;
			_timerText.text = "X";
		}

		public override void OnShow() {}
	}
}