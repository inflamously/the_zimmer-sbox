using Sandbox;

public sealed class Grabbable : Component
{
	public Rigidbody Rigidbody;

	public Collider Collider;

	protected override void OnStart()
	{
		Log.Info(Game.ActiveScene.GetComponentInChildren<ManagerVR>());

		Rigidbody = GetComponent<Rigidbody>();
		if (Rigidbody == null) {
			throw new System.Exception("Cannot use grabbable without rigidbody!");
		}

		Collider = GetComponent<Collider>();
	}


	protected override void OnUpdate()
	{

	}
}
