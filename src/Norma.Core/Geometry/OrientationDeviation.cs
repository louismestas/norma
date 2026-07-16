using System;

namespace Norma.Core.Geometry
{
    /// <summary>
    /// Angular deviation calculations between fitted (as-built) and design orientations.
    /// </summary>
    public static class OrientationDeviation
    {
        /// <summary>
        /// Angle in degrees between a fitted plane normal and the design plane normal.
        /// Sign-insensitive: normals are direction lines, so the result is in [0°, 90°].
        /// Computed as <c>acos(|n̂_fit · n̂_design|)</c>.
        /// </summary>
        /// <param name="fittedNormal">Normal of the best-fit (as-built) plane; need not be unit length.</param>
        /// <param name="designNormal">Normal of the design plane; need not be unit length.</param>
        public static double NormalAngleDegrees(Vector3 fittedNormal, Vector3 designNormal)
        {
            // TODO: normalize both, clamp |dot| to [0,1] before acos to guard rounding.
            throw new NotImplementedException("Plane-normal angle is not implemented yet.");
        }

        /// <summary>
        /// Rotational misalignment in degrees between an as-built axis/centerline and the
        /// design axis. Sign-insensitive, result in [0°, 90°].
        /// </summary>
        /// <param name="fittedAxis">Direction of the best-fit (as-built) axis.</param>
        /// <param name="designAxis">Direction of the design axis.</param>
        public static double AxisAngleDegrees(Vector3 fittedAxis, Vector3 designAxis)
        {
            // TODO: same |dot|-clamped acos as NormalAngleDegrees.
            throw new NotImplementedException("Axis misalignment angle is not implemented yet.");
        }
    }
}
