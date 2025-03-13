using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Sandbox;

struct TeleportInformation {
    public bool Allowed;
    public Vector3 Position;
}

public sealed class TeleportVR : Component, IHandType, IDebugCasts
{
    [Property] public SimulatorHandType handType;

    [Property] public bool IsDebugMode;

    float trajectoryAngle = 45f; // Starting angle
    float trajectoryDistance = 300f; // Starting distance
    float velocityDelta = 50f; // 50 is 1 Meter
    bool teleportMode = false;
    TeleportInformation teleportInformation;
    ITeleportable teleportable;
    Timer _avoidMouseJump = new Timer();

	protected override void OnStart()
	{
		if (teleportable == null) {
            Log.Info("SimulatorVR found!");
            teleportable = GetComponentInParent<SimulatorVR>();
        }
        else if (teleportable == null) {
            Log.Info("ControllerVR found!");
            teleportable = GetComponentInParent<ControllerVR>();
        } else {
            Log.Info("No teleportable found, teleportation deactivated!");
        }

        _avoidMouseJump.Start(1);
	}

	protected override void OnUpdate()
	{
        _avoidMouseJump.Update(Time.Delta);

        if (!IsRightHand()) {
            return;
        }

        if (!_avoidMouseJump.IsActive()) {
		    ProcessJump();
        }
	}

	private void ProcessJump()
	{
        ProcessTeleportPosition();
        ProcessTeleport();
	}

    private void ProcessTeleportPosition() {
		if (IsJumpPressed()) {
            teleportMode = true;

            float mouseY = Input.MouseDelta.y * Time.Delta * velocityDelta;
            trajectoryAngle -= mouseY;
            trajectoryAngle = Math.Clamp(trajectoryDistance, 15f, 45f);
            trajectoryDistance -= mouseY;
            trajectoryDistance = Math.Clamp(trajectoryDistance, 50f, 500f);
            // TrajectoryCalculator.DebugDrawTrajectory(DebugOverlay, Transform, trajectoryAngle, trajectoryDistance, 25);

            SceneTraceResult tr = TrajectoryUtils.RayTrajectory(DebugOverlay, Scene.Trace, Transform, 45f, trajectoryDistance, 25);
            if (tr.Hit) {
                teleportInformation = new TeleportInformation() {
                    Allowed = tr.GameObject.Tags.Contains("teleport"),
                    Position = tr.HitPosition,
                };
            }
            
            DebugOverlay.Sphere(new Sphere(tr.HitPosition, 25f), Color.Red);
		}
    }

    private void ProcessTeleport() {
        if (IsJumpReleased() && teleportInformation.Allowed) {
            if (teleportable != null) {
                Vector3 teleportTarget = teleportable.GetTransformForTeleporation().World.Forward * teleportInformation.Position.x + teleportable.GetTransformForTeleporation().World.Left * teleportInformation.Position.y; 
                teleportable.Teleport(teleportInformation.Position); // Add min(...) for height
            }
            DebugOverlay.Box(BBox.FromPositionAndSize(teleportInformation.Position, 50f), Color.Red, duration: 5);
            teleportMode = false;
            teleportInformation = new TeleportInformation() {
                Allowed = false,
                Position = Vector3.Zero
            };
        }
    }

    private bool IsJumpReleased() {
        return (Game.IsRunningInVR ? ControllerVR.GetJoystickInputDirection(JoystickHand.Right) == JoystickInputDirection.Unknown : Input.Released("attack2")) && teleportMode;
    }

	private bool IsJumpPressed()
	{
        return Game.IsRunningInVR ? ControllerVR.GetJoystickInputDirection(JoystickHand.Right) == JoystickInputDirection.Forward : Input.Down("attack2");
	}


	public bool IsRightHand()
	{
        return handType == SimulatorHandType.Right;
	}

	public bool IsLeftHand()
	{
        return handType == SimulatorHandType.Left;
	}

	public bool GetDebugMode()
	{
        return IsDebugMode;
    }
}
