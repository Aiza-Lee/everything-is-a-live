using System;

namespace GameLogic {
	public interface IPlayer : IEntity {
		bool Move(Direction direction);
		event Action<Direction> OnMove;
	}
}