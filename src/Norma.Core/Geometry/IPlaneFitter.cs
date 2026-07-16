using System.Collections.Generic;

namespace Norma.Core.Geometry
{
    /// <summary>
    /// Fits a plane to a set of 3D points.
    /// </summary>
    public interface IPlaneFitter
    {
        /// <summary>
        /// Fits a plane to <paramref name="points"/> (positions in mm).
        /// </summary>
        /// <param name="points">At least three non-collinear points.</param>
        /// <returns>The fitted plane with inlier classification and fit quality.</returns>
        PlaneFit Fit(IReadOnlyList<Vector3> points);
    }
}
