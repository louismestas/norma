using System.Collections.Generic;
using Norma.Core.Model;

namespace Norma.Core.Matching
{
    /// <summary>
    /// An as-built point paired with its as-designed counterpart.
    /// </summary>
    public sealed class PointPair
    {
        /// <summary>Creates a matched pair.</summary>
        public PointPair(SurveyPoint asDesigned, SurveyPoint asBuilt)
        {
            AsDesigned = asDesigned;
            AsBuilt = asBuilt;
        }

        /// <summary>The design (nominal) point.</summary>
        public SurveyPoint AsDesigned { get; }

        /// <summary>The measured (as-built) point.</summary>
        public SurveyPoint AsBuilt { get; }
    }

    /// <summary>
    /// Pairs as-built points with as-designed points prior to deviation analysis.
    /// </summary>
    public interface IPointMatcher
    {
        /// <summary>
        /// Matches the two point sets. Points with no counterpart are omitted from
        /// the result (implementations should expose unmatched points separately
        /// if callers need them).
        /// </summary>
        IReadOnlyList<PointPair> Match(IReadOnlyList<SurveyPoint> asDesigned, IReadOnlyList<SurveyPoint> asBuilt);
    }
}
