using System;

namespace ConnectFourResearch.Extensions
{
    public static class DoubleExtensions
    {
        public static double Distance(this double a, double b) => Math.Abs(a - b);

        public static double Squared(this double x) => x * x;
        
        public static double BoundTo(this double v, double left, double right)
        {
            if (v < left) 
                return left;
            if (v > right) 
                return right;
            return v;
        }
        
        public static double TruncateAbs(this double v, double maxAbs)
        {
            if (v < -maxAbs) 
                return -maxAbs;
            if (v > maxAbs) 
                return maxAbs;
            return v;
        }

        public static bool InRange(this double v, double min, double max) => v >= min && v <= max;

        public static double NormAngleInRadians(this double angle)
        {
            while (angle > Math.PI) angle -= 2 * Math.PI;
            while (angle <= -Math.PI) angle += 2 * Math.PI;
            return angle;
        }

        public static double NormDistance(this double value, double worldDiameter)
        {
            return value / worldDiameter;
        }
    }
}