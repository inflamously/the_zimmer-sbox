using System;

public class LerpingUtils {

    // 10 50 0.5
    // 10 50 0.75
    public static float Lerp(float a, float b, float t) {
        return b * t + a * (1 - t);
    }

    public static float InverseLerp(float a, float b, float value) {
        return Math.Clamp((value - a) / (b - a), 0f, 1f);
    }

    public static float Remap(float value, float minRangeA, float maxRangeA, float minRangeB, float maxRangeB) {
        return minRangeB + (value - minRangeA) * (maxRangeB - minRangeB) / (maxRangeA - minRangeA);
    }
}