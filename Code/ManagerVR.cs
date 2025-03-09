using Sandbox;

public sealed class ManagerVR : Component
{

	[Property] GameObject PlayerVR;
	[Property] GameObject SimulatorVR;

	protected override void OnStart()
	{
		if (Game.IsRunningInVR) {
			SimulatorVR.Enabled = false;
		} else {
			PlayerVR.Enabled = false;
		}
	}

	protected override void OnUpdate()
	{
		
	}
}
