using Sandbox;

public sealed class GrabbableVR : Component
{
	[Property] public Rigidbody Rigidbody;

	[Property] public Collider Collider;

	protected override void OnStart()
	{
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
