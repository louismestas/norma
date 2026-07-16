using System.Collections.Generic;

namespace Norma.Core.Geometry
{
    /// <summary>
    /// The result of fitting a straight axis (centerline) to a point set,
    /// in point–direction form: <c>p(t) = Centroid + t · Direction</c>.
    /// </summary>
    public sealed class AxisFit
    {
        /// <summary>Creates an axis-fit result.</summary>
        public AxisFit(Vector3 direction, Vector3 centroid, IReadOnlyList<int> inlierIndices, double rmsDistance)
        {
            Direction = direction;
            Centroid = centroid;
            InlierIndices = inlierIndices;
            RmsDistance = rmsDistance;
        }

        /// <summary>Unit direction of the fitted axis. Sign is arbitrary.</summary>
        public Vector3 Direction { get; }

        /// <summary>Centroid of the inlier points; a point on the axis.</summary>
        public Vector3 Centroid { get; }

        /// <summary>Indices (into the input point list) of the points classified as inliers.</summary>
        public IReadOnlyList<int> InlierIndices { get; }

        /// <summary>Root-mean-square radial distance of the inliers to the axis, in mm.</summary>
        public double RmsDistance { get; }
    }

    /// <summary>
    /// Fits a straight axis/centerline to a set of 3D points.
    /// </summary>
    public interface IAxisFitter
    {
        /// <summary>
        /// Fits an axis to <paramref name="points"/> (positions in mm).
        /// The direction is the right singular vector of the LARGEST singular value
        /// of the centered point matrix (the direction of maximum variance).
        /// </summary>
        /// <param name="points">At least two distinct points.</param>
        AxisFit Fit(IReadOnlyList<Vector3> points);
    }
}
