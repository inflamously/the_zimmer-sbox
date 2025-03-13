using System;
using System.Diagnostics;
using Sandbox;
using Sandbox.UI;

class GrabbableFinder {
	// This always stores the latest grabbable until another one is found.
	public Grabbable Grabbable;
	public Vector3? EndPosition;
}

public sealed class SimulatorHand : Component, IHandType, IControllerMode, IDebugCasts
{
	[Property] public RaycastLine Line;
	[Property] public bool ControllerMode;
	[Property] public int Range = 50;
	[Property] public Gradient BaseColor;
	[Property] public Gradient TargetColor;
	[Property] public TagSet RaycastTags = new TagSet();
	[Property] public int PickupColliderScaleMultiplier = 2;
	[Property] public bool IsDebugMode = false;
	[Property] public SimulatorHandType HandType = SimulatorHandType.None;
	bool _pickupMode = false;
	GrabbableFinder _raycastFind = new GrabbableFinder();

	protected override void OnStart()
	{
		if (Line != null) {
			Line.IsEnabled = true;
		}
	}

	protected override void OnUpdate()
	{
		ToggleControllerMode();
		TogglePickupMode();
		DragEntityOnPickupMode();
		ToggleGravityOnPickupMode();
		UpdateRotation();
		RaycastFindObject();
		DrawRaycast();
	}

	private void ToggleGravityOnPickupMode()
	{
		if (HasRaycastTarget() && _pickupMode) {
			_raycastFind.Grabbable.Rigidbody.Gravity = false;
		} 
		
		if (_raycastFind.Grabbable != null && !_raycastFind.Grabbable.Rigidbody.Gravity) {
			_raycastFind.Grabbable.Rigidbody.Gravity = true;
		}
	}

	private void DragEntityOnPickupMode()
	{
		if (HasRaycastTarget() && _pickupMode) {
			_raycastFind.Grabbable.Rigidbody.SmoothMove(GetRaycastEndPosition(), 1e-4f, Time.Delta);
			_raycastFind.Grabbable.Rigidbody.AngularVelocity *= 0.9f;
		}
	}

	private void TogglePickupMode()
	{
		_pickupMode = Input.Down("attack1");
	}

	private void ToggleControllerMode()
	{
		if (IsRightHand() && GetControllerModeKeyPressed()) {
			ControllerMode = !ControllerMode;
		}
	}

	private bool HasRaycastTarget() {
		return _raycastFind.EndPosition.HasValue && _raycastFind.Grabbable != null;
	}

	private Vector3 GetRaycastEndPosition() {
		return WorldPosition + Transform.World.Forward * Range;
	}

	private void DrawRaycast()
	{
		Vector3 rayLineEnd = HasRaycastTarget() ? _raycastFind.EndPosition.Value : GetRaycastEndPosition();
		Line.SetLine(WorldPosition + Transform.World.Forward, rayLineEnd);
		Line.SetColor(HasRaycastTarget() ? TargetColor : BaseColor);
	}

	private GrabbableFinder RaycastFindObject()
	{
		var debugSpherePosition = HasRaycastTarget() ? _raycastFind.EndPosition.Value : GetRaycastEndPosition();

		// SceneTraceResult tr = Scene.Trace.Ray(WorldPosition + Transform.World.Forward, GetRaycastEndPosition() ).Run();
		SceneTraceResult tr = Scene.Trace.Sphere(3f, WorldPosition + Transform.World.Forward, GetRaycastEndPosition()).IgnoreGameObject(GameObject).Run();

		if (IsDebugMode) {
			DebugOverlay.Sphere(new Sphere(debugSpherePosition, 3f), Color.Red);
		}

		if ( tr.Hit && tr.GameObject.GetComponent<Grabbable>() != null)
		{
			_raycastFind.Grabbable = tr.GameObject.GetComponent<Grabbable>();
			_raycastFind.EndPosition = tr.EndPosition;
			return _raycastFind;
		} else {
			_raycastFind.EndPosition = null;
		}
		
		return _raycastFind;
	}

	private void UpdateRotation()
	{
		if ( ControllerMode )
		{
			var mouseDelta = Input.MouseDelta * 2; // Times Rotation Speed
			LocalRotation = Rotation.FromAxis( Vector3.Up, -mouseDelta.x * Time.Delta ) * LocalRotation * Rotation.FromAxis( Vector3.Left, mouseDelta.y * Time.Delta );
		}
	}

	public bool IsRightHand()
	{
		return HandType == SimulatorHandType.Right;
	}

	public bool IsLeftHand()
	{
		return HandType == SimulatorHandType.Right;
	}

	public bool GetControllerMode()
	{
		return ControllerMode;
	}

	public bool GetControllerModeKeyPressed()
	{
		return Input.Pressed("Drop");
	}

	public bool GetDebugMode()
	{
		return IsDebugMode;
	}
}