using System;
using Sandbox;

public sealed class SimulatorVR : Component, ITeleportable
{
	private float _MovementSpeed = 0f;

	[Property] float WalkSpeed = 10f;
	[Property] float RunSpeed = 10f;
	[Property] float Deacceleration = 10f;
	[Property] float MouseSensitivity = 2f;
	[Property] SimulatorHand LeftHand;
	[Property] SimulatorHand RightHand;

	private CharacterController controller;

	private bool HMDMove = false;

	private Vector3 _speedClampMin = new Vector3(-500f, -500f, -500f);
	private Vector3 _speedClampMax = new Vector3(500f, 500f, 500f);

	protected override void OnStart()
	{
		controller = GetComponent<CharacterController>();

		_MovementSpeed = WalkSpeed;
	}

	protected override void OnUpdate()
	{
		MoveHMDPlayer();
		MapWalkSpeed();
		MapHMDSwitch();
		MapHMDView();
		MapHandsToCamera();
		MapHeightChange();

		if (controller.Velocity.Length > 0) {
			controller.Velocity = controller.Velocity.Clamp(_speedClampMin, _speedClampMax);	
			controller.Velocity *= 1f - Deacceleration * Time.Delta;
		}

		controller.Move();
	}

	private void MapHeightChange()
	{
		if (Input.Down("Walk")) {
			controller.WorldPosition += controller.Transform.World.Up * 10f;
		}

		if (Input.Down("Duck")) {
			controller.WorldPosition += -controller.Transform.World.Up * 10f;
		}

		controller.Move();
	}

	private void MapHMDSwitch()
	{
		if (Input.Pressed("Reload")) {
			HMDMove = !HMDMove;

			if (!HMDMove) {
				ResetHandRotation();
			}
		}
	}

	private void ResetHandRotation()
	{
		LeftHand.WorldRotation = Scene.Camera.WorldRotation;
		RightHand.WorldRotation = Scene.Camera.WorldRotation;
	}

	void MapHMDView() {
		if (!HMDMove) {
			return;
		}

		var mouse = Input.MouseDelta;
		Rotation frameYaw = Rotation.FromAxis(Vector3.Up, -mouse.x * Time.Delta * MouseSensitivity);
        Rotation framePitch = Rotation.FromAxis(Vector3.Right, -mouse.y * Time.Delta * MouseSensitivity);
		Scene.Camera.LocalRotation = frameYaw * Scene.Camera.LocalRotation * framePitch;
	}

	void MoveHMDPlayer() {
		if (HMDMove) {
			return;
		}

		if (Input.Down("Forward")) {
			controller.Velocity += Scene.Camera.Transform.Local.Forward * _MovementSpeed;
		}

		if (Input.Down("Backward")) {
			controller.Velocity -= Scene.Camera.Transform.Local.Forward * _MovementSpeed;
		}

		if (Input.Down("Left")) {
			controller.Velocity += Scene.Camera.Transform.Local.Left * _MovementSpeed;
		}

		if (Input.Down("Right")) {
			controller.Velocity -= Scene.Camera.Transform.Local.Left * _MovementSpeed;
		}

		// Log.Info(Math.Sqrt(controller.Velocity.Length));
	}

	void MapHandsToCamera() {
		PlayerMoveHandLocation();
	}

	private void PlayerMoveHandLocation()
	{
		if (!HMDMove) {
			RightHand.LocalPosition = Scene.Camera.Transform.Local.Forward * 40 + Scene.Camera.Transform.Local.Up * 25 + Scene.Camera.Transform.Local.Left * -10;
			LeftHand.LocalPosition = Scene.Camera.Transform.Local.Forward * 40 + Scene.Camera.Transform.Local.Up * 25 + Scene.Camera.Transform.Local.Right * -10;
		}
	}

	void MapWalkSpeed() {
		_MovementSpeed = Input.Down("Run") ? RunSpeed : WalkSpeed; 
	}

	void ITeleportable.Teleport( Vector3 coordinate )
	{
		WorldPosition = coordinate;
	}
}
