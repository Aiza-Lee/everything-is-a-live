namespace GameLogic {
	public interface IPrefabFactory {
		MechanismView CreateMechanismView(IMechanism mechanism);
		PlayerView CreatePlayerView(IPlayer player);
		GroundView CreateGroundView(IGround ground);
	}
}