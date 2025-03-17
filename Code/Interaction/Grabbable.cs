using System;
using Sandbox;

public sealed class Grabbable : Component
{
	[RequireComponent] public Rigidbody Rigidbody { get;set; }
	[RequireComponent] public Collider Collider { get;set; }

	protected override void OnStart()
	{
		// Log.Info(Game.ActiveScene.GetComponentInChildren<GameManagerVR>());

		Rigidbody = GetComponent<Rigidbody>();
		if (Rigidbody == null) {
			throw new System.Exception("Cannot use grabbable without rigidbody!");
		}

		Collider = GetComponent<Collider>();
		if (Collider == null) {
			throw new System.Exception("Cannot use grabbable without rigidbody!");
		}
	}


	protected override void OnUpdate()
	{
		
	}
}
