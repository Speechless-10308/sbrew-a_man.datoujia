using System;

namespace StorybrewScripts
{
    public class CubicBezier
    {

        private readonly double ax;
        private readonly double bx;
        private readonly double cx;
        private readonly double ay;
        private readonly double by;
        private readonly double cy;
        /// <summary>
        /// Create a new cubic bezier curve.
        /// </summary>
        /// <param name="p1x">The x coordinate of the first control point.</param>
        /// <param name="p1y">The y coordinate of the first control point.</param>
        /// <param name="p2x">The x coordinate of the second control point.</param>
        /// <param name="p2y">The y coordinate of the second control point.</param>
        public CubicBezier(double p1x, double p1y, double p2x, double p2y)
        {
            cx = 3 * (p1x - 0);
            bx = 3 * (p2x - p1x) - cx;
            ax = 1 - cx - bx;
            cy = 3 * (p1y - 0);
            by = 3 * (p2y - p1y) - cy;
            ay = 1 - cy - by;
        }

        private double GetX(double t) => ((ax * t + bx) * t + cx) * t;
        private double GetY(double t) => ((ay * t + by) * t + cy) * t;

        /// <summary>
        /// Solve the curve for a given x value.
        /// </summary>
        /// <param name="x">The x value to solve for.</param>
        /// <returns>The y value of the curve at the given x value.</returns>
        /// <remarks>
        /// This method uses Newton's method to approximate the value.
        /// </remarks>
        public double Solve(double x)
        {
            if (x == 0) return 0;
            if (x == 1) return 1;

            var time = x;
            // Newton's method
            for (int i = 0; i < 8; i++)
            {
                var x2 = GetX(time) - x;
                if (Math.Abs(x2) < 1e-3) return GetY(time);
                var d = (3 * ax * time + 2 * bx) * time + cx; // dx/dt
                if (Math.Abs(d) < 1e-6) break; // If the derivative is too small, Newton's method won't work
                time -= x2 / d;
            }

            return GetY(time);
        }
    }
}