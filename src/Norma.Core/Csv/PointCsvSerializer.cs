using System;
using System.Collections.Generic;
using System.IO;
using Norma.Core.Model;

namespace Norma.Core.Csv
{
    /// <summary>
    /// Reads and writes point sets as CSV. The canonical column layout is
    /// <c>PointID, X (mm), Y (mm), Z (mm), Source</c> — the unit is always
    /// declared in the header so exported files are self-describing.
    /// </summary>
    public sealed class PointCsvSerializer
    {
        /// <summary>The header row written by <see cref="Write"/> and expected by <see cref="Read"/>.</summary>
        public const string Header = "PointID,X (mm),Y (mm),Z (mm),Source";

        /// <summary>
        /// Writes <paramref name="points"/> to <paramref name="writer"/>, header first,
        /// one point per row, invariant-culture number formatting.
        /// </summary>
        public void Write(TextWriter writer, IEnumerable<SurveyPoint> points)
        {
            if (writer is null) throw new ArgumentNullException(nameof(writer));
            if (points is null) throw new ArgumentNullException(nameof(points));

            // TODO: write Header, then rows; quote PointId/Source when they contain
            //       commas or quotes; use CultureInfo.InvariantCulture for coordinates.
            throw new NotImplementedException("CSV writing is not implemented yet.");
        }

        /// <summary>
        /// Reads points from <paramref name="reader"/>. The first row must be the
        /// canonical header; coordinate values are parsed invariant-culture.
        /// </summary>
        /// <exception cref="FormatException">The header or a row is malformed.</exception>
        public IReadOnlyList<SurveyPoint> Read(TextReader reader)
        {
            if (reader is null) throw new ArgumentNullException(nameof(reader));

            // TODO: validate header, parse rows, surface line numbers in errors.
            throw new NotImplementedException("CSV reading is not implemented yet.");
        }
    }
}
