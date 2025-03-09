using System.Collections.Generic;

public sealed class RaycastLine : Component
{
	[Property] public bool IsEnabled = true;

	LineRenderer lineRenderer;

	protected override void OnStart()
	{
		lineRenderer = GetComponent<LineRenderer>();

		if (lineRenderer != null) {
			lineRenderer.UseVectorPoints = true;
			lineRenderer.VectorPoints = new List<Vector3>();
			lineRenderer.VectorPoints.Add(Vector3.Zero);
			lineRenderer.VectorPoints.Add(Vector3.Zero);
		}
	}

	protected override void OnUpdate()
	{
		// Raycast();	
	}

	public void SetLine(Vector3 from, Vector3 to) {
		if (lineRenderer.VectorPoints.Count == 2) {
			lineRenderer.VectorPoints[0] = from;
			lineRenderer.VectorPoints[1] = to;
		}
	}
	
	public void SetColor(Gradient color) {
		lineRenderer.Color = color;
	}

	void Raycast() {
		// Log.Info("Raycasting!");

		// SceneTraceResult tr = Scene.Trace.Ray( GameObject.WorldPosition, GameObject.Transform.World.Forward * int.MaxValue ).Run();

        // if ( tr.Hit )
        // {
        //     Log.Info( $"Hit: {tr.GameObject} at {tr.EndPosition}" );
        // }
        // else { 
		// 	Log.Info( $"Nothing" ); 
		// 	}
	}
}
