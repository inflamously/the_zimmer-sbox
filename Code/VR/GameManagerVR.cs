using Sandbox;

public sealed class GameManagerVR : Component, IVRInstance
{

	[Property] GameObject PlayerVR;
	[Property] GameObject PlayerNonVR;

	public bool GetIsNonVR()
	{
		return !Game.IsRunningInVR;
	}

	public bool GetIsVR()
	{
		return Game.IsRunningInVR;
	}

	protected override void OnStart()
	{
		PlayerNonVR.Enabled = GetIsNonVR();
		PlayerVR.Enabled = GetIsVR();
	}

	protected override void OnUpdate()
	{
		
	}
}
