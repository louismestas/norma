using System;

namespace Norma.Core.Geometry
{
    /// <summary>
    /// An immutable 3D vector. Used for both positions and directions.
    /// Coordinate values are in millimetres when the vector represents a position.
    /// </summary>
    public readonly struct Vector3 : IEquatable<Vector3>
    {
        /// <summary>The X component (mm for positions).</summary>
        public double X { get; }

        /// <summary>The Y component (mm for positions).</summary>
        public double Y { get; }

        /// <summary>The Z component (mm for positions).</summary>
        public double Z { get; }

        /// <summary>Creates a vector from its three components.</summary>
        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>The Euclidean length of the vector.</summary>
        public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);

        /// <summary>Returns this vector scaled to unit length.</summary>
        /// <exception cref="InvalidOperationException">The vector has zero length.</exception>
        public Vector3 Normalized()
        {
            var len = Length;
            if (len <= 0.0)
            {
                throw new InvalidOperationException("Cannot normalize a zero-length vector.");
            }

            return new Vector3(X / len, Y / len, Z / len);
        }

        /// <summary>Dot product of two vectors.</summary>
        public static double Dot(Vector3 a, Vector3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

        /// <summary>Cross product of two vectors.</summary>
        public static Vector3 Cross(Vector3 a, Vector3 b) => new Vector3(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X);

        /// <summary>Component-wise difference <paramref name="a"/> − <paramref name="b"/>.</summary>
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        /// <summary>Component-wise sum.</summary>
        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        /// <summary>Scales the vector by a scalar.</summary>
        public static Vector3 operator *(Vector3 v, double s) => new Vector3(v.X * s, v.Y * s, v.Z * s);

        /// <inheritdoc />
        public bool Equals(Vector3 other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is Vector3 other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = X.GetHashCode();
                hash = (hash * 397) ^ Y.GetHashCode();
                hash = (hash * 397) ^ Z.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc />
        public override string ToString() => $"({X:0.###}, {Y:0.###}, {Z:0.###})";
    }
}
