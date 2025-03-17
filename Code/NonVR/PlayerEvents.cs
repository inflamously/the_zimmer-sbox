using Sandbox;

public sealed class PlayerEvents : Component, PlayerController.IEvents
{
	[RequireComponent] public PlayerController Player { get; set; }

	void PlayerController.IEvents.StartPressing( Component target )
	{
		// DO NOTHING
	}
}
