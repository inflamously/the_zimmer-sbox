using Sandbox;

public sealed class ManagerVR : Component, IVRInstance
{

	[Property] GameObject PlayerVR;
	[Property] GameObject PlayerNonVR;

	protected override void OnStart()
	{
		if (Game.IsRunningInVR) {
			PlayerNonVR.Enabled = false;
		} else {
			PlayerVR.Enabled = false;
		}
	}

	protected override void OnUpdate()
	{
		
	}
}
