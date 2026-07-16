using Norma.Core.Geometry;

namespace Norma.Core.Model
{
    /// <summary>
    /// A named 3D point extracted from a drawing or survey. Coordinates are in millimetres.
    /// </summary>
    public sealed class SurveyPoint
    {
        /// <summary>Creates a survey point.</summary>
        public SurveyPoint(string pointId, Vector3 position, string source)
        {
            PointId = pointId;
            Position = position;
            Source = source;
        }

        /// <summary>
        /// Identifier used to match as-built points to as-designed points
        /// (COGO point number, block attribute, or a generated handle).
        /// </summary>
        public string PointId { get; }

        /// <summary>Position in mm.</summary>
        public Vector3 Position { get; }

        /// <summary>
        /// Where the point came from, e.g. <c>COGO</c>, <c>BLOCK</c>, <c>POINT</c>
        /// (DBPoint entity), or a file name for imported sets.
        /// </summary>
        public string Source { get; }
    }
}
