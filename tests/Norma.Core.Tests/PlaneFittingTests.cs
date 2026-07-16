using System;
using System.Collections.Generic;
using Norma.Core.Geometry;
using Xunit;

namespace Norma.Core.Tests
{
    /// <summary>
    /// Plane-fitting tests against synthetic data: points sampled on a known plane,
    /// perturbed with Gaussian noise, optionally contaminated with gross outliers.
    /// The tests are skipped until the fitters are implemented — remove the Skip
    /// reasons as implementations land.
    /// </summary>
    public class PlaneFittingTests
    {
        private const string NotImplemented = "SVD/RANSAC plane fitting not implemented yet.";

        [Fact(Skip = NotImplemented)]
        public void SvdFit_ExactPlane_RecoversNormal()
        {
            var normal = new Vector3(0, 0, 1);
            var points = SampleOnPlane(normal, origin: new Vector3(10, 20, 30), count: 50, noiseSigmaMm: 0, seed: 1);

            var fit = new SvdPlaneFitter().Fit(points);

            Assert.True(AngleDegrees(fit.Normal, normal) < 1e-6);
            Assert.True(fit.RmsDistance < 1e-9);
        }

        [Fact(Skip = NotImplemented)]
        public void SvdFit_TiltedPlaneWithNoise_RecoversNormalWithinTolerance()
        {
            var normal = new Vector3(1, 2, 3).Normalized();
            var points = SampleOnPlane(normal, origin: new Vector3(0, 0, 0), count: 200, noiseSigmaMm: 0.5, seed: 2);

            var fit = new SvdPlaneFitter().Fit(points);

            Assert.True(AngleDegrees(fit.Normal, normal) < 0.5);
        }

        [Fact(Skip = NotImplemented)]
        public void RansacFit_PlaneWithOutliers_RejectsOutliers()
        {
            var normal = new Vector3(0, 1, 1).Normalized();
            var points = new List<Vector3>(SampleOnPlane(normal, origin: new Vector3(0, 0, 0), count: 100, noiseSigmaMm: 0.5, seed: 3));
            var outlierStart = points.Count;
            points.AddRange(GrossOutliers(count: 20, offsetMm: 500, seed: 4));

            var fitter = new RansacPlaneFitter(new RansacOptions { InlierThresholdMm = 3.0, RandomSeed = 5 });
            var fit = fitter.Fit(points);

            Assert.True(AngleDegrees(fit.Normal, normal) < 0.5);
            foreach (var idx in fit.InlierIndices)
            {
                Assert.True(idx < outlierStart, $"Gross outlier at index {idx} was classified as an inlier.");
            }
        }

        [Fact(Skip = NotImplemented)]
        public void NormalAngle_KnownVectors_MatchesExpected()
        {
            var a = new Vector3(0, 0, 1);
            var b = new Vector3(0, 1, 1).Normalized();

            Assert.Equal(45.0, OrientationDeviation.NormalAngleDegrees(a, b), precision: 6);
            // Sign-insensitive: an anti-parallel normal is the same plane.
            Assert.Equal(0.0, OrientationDeviation.NormalAngleDegrees(a, new Vector3(0, 0, -1)), precision: 6);
        }

        /// <summary>
        /// Samples <paramref name="count"/> points on the plane through
        /// <paramref name="origin"/> with unit <paramref name="normal"/>, then adds
        /// Gaussian noise (σ in mm) along the normal.
        /// </summary>
        private static IReadOnlyList<Vector3> SampleOnPlane(Vector3 normal, Vector3 origin, int count, double noiseSigmaMm, int seed)
        {
            var rng = new Random(seed);
            var u = ArbitraryPerpendicular(normal);
            var v = Vector3.Cross(normal, u);
            var points = new List<Vector3>(count);
            for (var i = 0; i < count; i++)
            {
                var p = origin
                    + u * ((rng.NextDouble() - 0.5) * 2000.0)
                    + v * ((rng.NextDouble() - 0.5) * 2000.0)
                    + normal * (Gaussian(rng) * noiseSigmaMm);
                points.Add(p);
            }

            return points;
        }

        /// <summary>Random points far from any plausible plane (gross blunders).</summary>
        private static IEnumerable<Vector3> GrossOutliers(int count, double offsetMm, int seed)
        {
            var rng = new Random(seed);
            for (var i = 0; i < count; i++)
            {
                yield return new Vector3(
                    (rng.NextDouble() - 0.5) * 2000.0,
                    (rng.NextDouble() - 0.5) * 2000.0,
                    offsetMm + rng.NextDouble() * offsetMm);
            }
        }

        private static Vector3 ArbitraryPerpendicular(Vector3 n)
        {
            var candidate = Math.Abs(n.X) < 0.9 ? new Vector3(1, 0, 0) : new Vector3(0, 1, 0);
            return Vector3.Cross(n, candidate).Normalized();
        }

        /// <summary>Standard normal deviate (Box–Muller).</summary>
        private static double Gaussian(Random rng)
        {
            var u1 = 1.0 - rng.NextDouble();
            var u2 = rng.NextDouble();
            return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        }

        private static double AngleDegrees(Vector3 a, Vector3 b)
        {
            var dot = Math.Abs(Vector3.Dot(a.Normalized(), b.Normalized()));
            return Math.Acos(Math.Min(1.0, dot)) * 180.0 / Math.PI;
        }
    }
}
