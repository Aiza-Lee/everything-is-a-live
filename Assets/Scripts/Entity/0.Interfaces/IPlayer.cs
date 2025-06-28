namespace GameLogic {
	public interface IPlayer : IEntity {
		bool Move(Direction direction);
	}
}