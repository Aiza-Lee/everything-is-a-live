using System;

namespace GameLogic {
	public sealed class TickMgr {
		private TickMgr() { }
		public static TickMgr Inst { get; } = new();
		public event Action BeforeTick;
		/// <summary>
		/// 每tick清空
		/// </summary>
		public event Action AfterTick;

		public void Tick() {
			BeforeTick?.Invoke();
			foreach (var entity in GameMgr.Inst.Grid.Entities_ReadOnly.Values) {
				entity.Tick();
			}
			GameMgr.Inst.Grid.CalculateMaps();
			if (GameMgr.Inst.CheckPlayerWin()) {
				GlobalMgr.Inst.LevelWin();
			}
			if (GameMgr.Inst.CheckGameOver()) {
				GlobalMgr.Inst.LevelLose();
			}
			foreach (var entity in GameMgr.Inst.Grid.Entities_ReadOnly.Values) {
				if (entity is IMechanism mechanism) {
					if (!GameMgr.Inst.Grid.IsWalkable(mechanism.Position)) {
						mechanism.TriggerFunc();
					}
				}
			}
			AfterTick?.Invoke();
			AfterTick = null;
		}
	}
}