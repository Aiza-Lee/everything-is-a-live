namespace GameLogic {
	public interface IPrefabFactory {
		MechanismView CreateMechanismView(string typeID, IMechanism mechanism);
		PlayerView CreatePlayerView(IPlayer player);
		GroundView CreateGroundView(IGround ground);
	}
}