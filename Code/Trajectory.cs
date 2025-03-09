using System;
using System.Linq;

public class TrajectoryUtils {

    public static void StepTrajectory(
       GameTransform transform, 
        float angleDegrees, float xMaxDistance, float stepSize, Func<Vector3, Vector3, bool> handleTrajectoryStep) {
        int maxSteps = (int)(xMaxDistance / stepSize);

        Vector3 lastPosition = transform.World.Position + transform.Local.Forward;
        for (int stepIndex = 0; stepIndex < maxSteps; stepIndex++) {
            float x = stepSize*stepIndex;
            float y = GetTrajectory(angleDegrees, xMaxDistance, x);
            Vector3 last = lastPosition;
            Vector3 next = last + transform.World.Forward * x + transform.World.Up * y; // Times Direction makes it fluid
            bool shouldContinue = handleTrajectoryStep(last,next);
            if (!shouldContinue) {
                break;
            }
            lastPosition = next;
        }   
    }

     public static float GetTrajectory(float trajectoryAngle, float velocityLength, float x) {
        float trajectoryAngleRadians = (float)(trajectoryAngle * (Math.PI / 180.0));
        float cosAngle = (float)Math.Cos(trajectoryAngleRadians);
        float tanAngle = (float)Math.Tan(trajectoryAngleRadians);
        
        float distanceAngled = x * tanAngle;

        float gravity = 850;
        float gravityByDistance = gravity * x * x;

        float velocitySquared = velocityLength * velocityLength;
        float trajectorySquare = cosAngle * cosAngle;

        float velocityTrajectored = (float)(2.0 * velocitySquared * trajectorySquare);

        return distanceAngled - (gravityByDistance / velocityTrajectored);
    }

    public static void DebugDrawTrajectory(
        DebugOverlaySystem overlay, GameTransform transform, float angleDegrees, float xMaxDistance, float stepSize) {
        StepTrajectory(transform, angleDegrees, xMaxDistance, stepSize, (last, next) => {
            overlay.Line(new Line(last,next), Color.White);
            return true;
        });
    }

    public static SceneTraceResult RayTrajectory(DebugOverlaySystem overlay, SceneTrace trace, GameTransform transform, float angleDegrees, float xMaxDistance, float stepSize) {
        SceneTraceResult tr = new SceneTraceResult();

        StepTrajectory(transform, angleDegrees, xMaxDistance, stepSize, (from, to) => {
            var direction = to - from;
            tr = trace.Ray(new Ray(from, direction.Normal), stepSize).IgnoreGameObject(transform.GameObject).Run();

            if (overlay != null) {
                var fromRight = from + transform.World.Right * 5;
                var toRight = to + transform.World.Right * 5;
                overlay.Line(new Line(fromRight, toRight), tr.Hit ? Color.Red: Color.White);
            }

            if (tr.Hit && tr.GameObject.Tags.Contains("teleport")) {
                return false;
            };

            return true;
        });

        return tr;
    }

    public static SceneTraceResult RayTrajectory(SceneTrace trace, GameTransform transform, float angleDegrees, float xMaxDistance, float stepSize) {
        return RayTrajectory(null, trace, transform, angleDegrees, xMaxDistance, stepSize);
    }
}