using System;
using Sandbox;

public sealed class InputDeltaTest : Component
{
	Vector2 _lineXY;
	Vector2 _lineXY2;
	Vector2 _point;

	int _radius = 50;

	protected override void OnStart()
	{
		_point = new Vector2(0, _radius);
	}

	private Vector2 GetMouseDeltaDirection() {
		var mouseDelta = Input.MouseDelta.Normal;
		if (mouseDelta.Length < 0.1f) {
			return Vector2.Zero;
		}

		var fieldOfView = MathF.Cos(45);

		if (Vector2.Dot(mouseDelta, Vector2.Up) > 0.707) {
			return -Vector2.Up;
		}

		if (Vector2.Dot(mouseDelta, Vector2.Down) > 0.707) {
			return -Vector2.Down;
		}
		
		if (Vector2.Dot(mouseDelta, Vector2.Right) > 0.707) {
			return -Vector2.Right;
		}
		
		if (Vector2.Dot(mouseDelta, Vector2.Left) > 0.707) {
			return -Vector2.Left;
		}

		return Vector2.Zero;
	}

	protected override void OnUpdate()
	{
		if ( Scene.Camera is null )
			return;

		// var mouseDelta = Input.MouseDelta;
		// if (mouseDelta.Length < 0.1f) {
		// 	return;
		// };

		// var angle = MathF.Atan2(mouseDelta.x, mouseDelta.y) * 180 / Math.PI;
		// Log.Info(angle);

		// var newX = MathF.Sin( (float)(angle * Math.PI / 180) ) * _radius;
		// var newY = MathF.Cos( (float)(angle * Math.PI / 180) ) * _radius;

		// Log.Info(newX);
		// Log.Info(newY);

		// DebugOverlay.Line(new Line(WorldPosition, WorldPosition + Transform.World.Up * 25f));

		// _point = new Vector2(newX, newY);
		// Log.Info(_point);

		var hud = Scene.Camera.Hud;
		_lineXY = Scene.Camera.ScreenRect.Center;
		
		if (GetMouseDeltaDirection() == Vector2.Zero) {
			_lineXY2 = _lineXY;
		}

		if (GetMouseDeltaDirection() == Vector2.Up) {
			_lineXY2 = _lineXY + new Vector2(0, -50);
		}

		else if (GetMouseDeltaDirection() == Vector2.Left) {
			_lineXY2 = _lineXY + new Vector2(-50, 0);
		}

		else if (GetMouseDeltaDirection() == Vector2.Right) {
			_lineXY2 = _lineXY + new Vector2(50, 0);
		}

		else if (GetMouseDeltaDirection() == Vector2.Down) {
			_lineXY2 = _lineXY + new Vector2(0, 50);
		}
		hud.DrawLine(_lineXY, _lineXY2, 2f, Color.White);
	}
}
