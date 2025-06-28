using System;

namespace GameLogic {
	public sealed class TickMgr {
		private TickMgr() { }
		public static TickMgr Inst { get; } = new();
		public event Action BeforeTick;
		public event Action AfterTick;

		public void Tick() {
			BeforeTick?.Invoke();
			foreach (var entity in GameMgr.Inst.Entities) {
				entity.Tick();
			}
			AfterTick?.Invoke();
		}
	}
}