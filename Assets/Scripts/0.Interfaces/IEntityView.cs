namespace GameLogic {
	public interface IEntityView {
		void Tick();
		void SetEntity(IEntity entity);
		IEntity Entity { get; }
	}
}