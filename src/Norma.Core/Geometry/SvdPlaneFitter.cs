using System;
using System.Collections.Generic;

namespace Norma.Core.Geometry
{
    /// <summary>
    /// Least-squares plane fitting via singular value decomposition (Math.NET Numerics).
    /// The plane normal is the left singular vector associated with the smallest
    /// singular value of the centroid-subtracted point matrix.
    /// </summary>
    /// <remarks>
    /// See <c>docs/plane-fitting-algorithm.md</c> for the math and its provenance
    /// (ported from https://github.com/htcr/plane-fitting).
    /// </remarks>
    public sealed class SvdPlaneFitter : IPlaneFitter
    {
        /// <inheritdoc />
        public PlaneFit Fit(IReadOnlyList<Vector3> points)
        {
            if (points is null) throw new ArgumentNullException(nameof(points));
            if (points.Count < 3) throw new ArgumentException("Plane fitting requires at least 3 points.", nameof(points));

            // TODO: 1) compute centroid, 2) build N×3 matrix of centered points,
            //       3) SVD, 4) normal = right singular vector of smallest singular value,
            //       5) RMS distance over all points. All points are inliers.
            throw new NotImplementedException("SVD plane fitting is not implemented yet.");
        }
    }
}
