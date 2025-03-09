using System;
using System.Numerics;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;

public enum JoystickHand {
	Left,
	Right
}


public enum JoystickInputDirection {
	Forward,
	Backward,
	Left,
	Right,
	Unknown
}

public sealed class ControllerVR : Component, ITeleportable
{
	[Property] public bool IsDebugMode = false;
	[Property] public ControllerDebugHUDVR DebugHUDVR;

	protected override void OnStart()
	{
		DebugHUDVR.Enabled = IsDebugMode;

		AddDebugInformation();
	}

	private void AddDebugInformation()
	{
		if (IsDebugMode) {
			DebugHUDVR.AddText("joystick_delta");
		}
	}

	public static JoystickInputDirection GetJoystickInputDirection(JoystickHand hand) {
		var value = hand == JoystickHand.Right ? Input.VR.RightHand.Joystick.Value : Input.VR.LeftHand.Joystick.Value;
		var angleRange = 0.7f;
		
		var dotForward = Vector3.Dot(value, Vector3.Left);
		var dotBackward = Vector3.Dot(value, Vector3.Right);
		var dotLeft = Vector3.Dot(value, Vector3.Backward);
		var dotRight = Vector3.Dot(value, Vector3.Forward);

		// DebugHUDVR.SetText("joystick_delta", $"Value:{value} Dot: {dotProduct} Forward: {isMovingForward}");

		if (dotForward > angleRange) {
			return JoystickInputDirection.Forward;
		} else if (dotBackward > angleRange) {
			return JoystickInputDirection.Backward;
		} else if (dotLeft > angleRange) {
			return JoystickInputDirection.Left;
		} else if (dotRight > angleRange) {
			return JoystickInputDirection.Right;
		} else {
			return JoystickInputDirection.Unknown;
		}
	}

	protected override void OnUpdate()
	{
		// Get joystick delta

		// For detecting forward movement, compare against the forward vector
		// Note: Depending on S&Box's coordinate system, this might be different

		// Check if movement is primarily forward
		// bool isMovingForward = dotProduct > 0.7f; // Threshold value between 0 and 1
		var inputDirection = GetJoystickInputDirection(JoystickHand.Right);
		DebugHUDVR.SetText("joystick_delta", $"Joystick: {Input.VR.RightHand.Joystick.Value} Direction: {inputDirection}");
	}

	public void Teleport( Vector3 coordinate )
	{
		WorldPosition = coordinate;
	}
}
