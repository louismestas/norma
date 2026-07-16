using System;
using System.Collections.Generic;

namespace Norma.Core.Geometry
{
    /// <summary>
    /// Robust plane fitting: RANSAC outlier rejection around an inner
    /// least-squares (SVD) fit. Random 3-point candidate planes are scored by
    /// inlier count; the best consensus set is re-fit with <see cref="SvdPlaneFitter"/>.
    /// </summary>
    /// <remarks>
    /// See <c>docs/plane-fitting-algorithm.md</c> for the algorithm and defaults
    /// (ported from https://github.com/htcr/plane-fitting).
    /// </remarks>
    public sealed class RansacPlaneFitter : IPlaneFitter
    {
        /// <summary>Creates a RANSAC plane fitter.</summary>
        /// <param name="options">Iteration count and inlier threshold; defaults are used when null.</param>
        public RansacPlaneFitter(RansacOptions? options = null)
        {
            Options = options ?? new RansacOptions();
        }

        /// <summary>The RANSAC parameters in effect for this fitter.</summary>
        public RansacOptions Options { get; }

        /// <inheritdoc />
        public PlaneFit Fit(IReadOnlyList<Vector3> points)
        {
            if (points is null) throw new ArgumentNullException(nameof(points));
            if (points.Count < 3) throw new ArgumentException("Plane fitting requires at least 3 points.", nameof(points));

            // TODO: for Options.Iterations rounds: sample 3 distinct points, fit a
            //       candidate plane, count points within Options.InlierThresholdMm;
            //       keep the candidate with the most inliers, then re-fit the winning
            //       inlier set with SvdPlaneFitter and reclassify inliers.
            throw new NotImplementedException("RANSAC plane fitting is not implemented yet.");
        }
    }

    /// <summary>
    /// Parameters for <see cref="RansacPlaneFitter"/>.
    /// </summary>
    public sealed class RansacOptions
    {
        /// <summary>Number of random-sample iterations. Default 1000.</summary>
        public int Iterations { get; set; } = 1000;

        /// <summary>
        /// Maximum orthogonal distance (mm) from the candidate plane for a point
        /// to count as an inlier. Default 5 mm.
        /// </summary>
        public double InlierThresholdMm { get; set; } = 5.0;

        /// <summary>
        /// Optional seed for the random sampler, for reproducible fits (e.g. in tests).
        /// Null uses a time-based seed.
        /// </summary>
        public int? RandomSeed { get; set; }
    }
}
