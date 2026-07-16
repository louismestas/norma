using System;
using System.Collections.Generic;
using Norma.Core.Model;

namespace Norma.Core.Matching
{
    /// <summary>
    /// Matches points by exact <see cref="SurveyPoint.PointId"/> equality
    /// (ordinal, case-insensitive). The default matcher for survey workflows
    /// where both sets share a point numbering scheme.
    /// </summary>
    public sealed class PointIdMatcher : IPointMatcher
    {
        /// <inheritdoc />
        public IReadOnlyList<PointPair> Match(IReadOnlyList<SurveyPoint> asDesigned, IReadOnlyList<SurveyPoint> asBuilt)
        {
            if (asDesigned is null) throw new ArgumentNullException(nameof(asDesigned));
            if (asBuilt is null) throw new ArgumentNullException(nameof(asBuilt));

            // TODO: index asDesigned by PointId, pair with asBuilt, decide policy for
            //       duplicate ids (first-wins vs. error).
            throw new NotImplementedException("PointId matching is not implemented yet.");
        }
    }
}
