using System.Collections.Generic;

namespace Norma.Core.Geometry
{
    /// <summary>
    /// The result of fitting a plane to a point set.
    /// The plane is represented in point–normal form: all points p satisfying
    /// <c>Dot(Normal, p − Centroid) = 0</c>.
    /// </summary>
    public sealed class PlaneFit
    {
        /// <summary>Creates a plane-fit result.</summary>
        public PlaneFit(Vector3 normal, Vector3 centroid, IReadOnlyList<int> inlierIndices, double rmsDistance)
        {
            Normal = normal;
            Centroid = centroid;
            InlierIndices = inlierIndices;
            RmsDistance = rmsDistance;
        }

        /// <summary>Unit normal of the fitted plane.</summary>
        public Vector3 Normal { get; }

        /// <summary>Centroid of the inlier points; a point on the plane.</summary>
        public Vector3 Centroid { get; }

        /// <summary>
        /// Indices (into the input point list) of the points classified as inliers.
        /// For a plain least-squares fit this is every input index.
        /// </summary>
        public IReadOnlyList<int> InlierIndices { get; }

        /// <summary>Root-mean-square orthogonal distance of the inliers to the plane, in mm.</summary>
        public double RmsDistance { get; }
    }
}
