namespace GameLogic {
	public static class SortingLayerHelper {
		public static int GetSortingOrder(this IEntity entity) {
			if (entity is IMechanism) {
				return -entity.Position.Y * 2;
			} else if (entity is IPlayer) {
				return -entity.Position.Y * 2 + 1;
			} else {
				return -entity.Position.Y * 2;
			}
		}
	}
}