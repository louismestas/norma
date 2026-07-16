using Norma.Core.Matching;

namespace Norma.Core.Deviation
{
    /// <summary>
    /// Positional deviation of one matched pair: as-built minus as-designed,
    /// per axis and total. All values in mm.
    /// </summary>
    public sealed class PointDeviation
    {
        /// <summary>Creates a deviation record for a matched pair.</summary>
        public PointDeviation(PointPair pair, double dX, double dY, double dZ, double dTotal)
        {
            Pair = pair;
            DX = dX;
            DY = dY;
            DZ = dZ;
            DTotal = dTotal;
        }

        /// <summary>The matched pair this deviation was computed from.</summary>
        public PointPair Pair { get; }

        /// <summary>X deviation in mm (as-built − as-designed).</summary>
        public double DX { get; }

        /// <summary>Y deviation in mm (as-built − as-designed).</summary>
        public double DY { get; }

        /// <summary>Z deviation in mm (as-built − as-designed).</summary>
        public double DZ { get; }

        /// <summary>Total (Euclidean) deviation in mm: √(dX² + dY² + dZ²).</summary>
        public double DTotal { get; }
    }
}
