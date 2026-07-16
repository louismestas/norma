using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.Runtime;

[assembly: CommandClass(typeof(Norma.Plugin.Commands.CompareCommands))]

namespace Norma.Plugin.Commands
{
    /// <summary>
    /// Deviation-analysis commands: compare as-built points against as-designed
    /// points and report positional and orientation deviations.
    /// </summary>
    public sealed class CompareCommands
    {
        /// <summary>
        /// NRMCOMPARE — loads (or selects) an as-designed and an as-built point set,
        /// matches pairs by point id, and reports:
        /// per-axis and total positional deviation (mm) per pair;
        /// best-fit plane normal vs. design plane normal (degrees, SVD + RANSAC);
        /// axis/centerline rotational misalignment (degrees).
        /// Results are written to the command line and a CSV report via
        /// <c>Norma.Core.Deviation.DeviationAnalyzer</c>.
        /// </summary>
        [CommandMethod("NRMCOMPARE")]
        public void ComparePoints()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc is null) return;

            // TODO: prompt for design CSV + as-built selection (or two CSVs),
            //       PointIdMatcher → DeviationAnalyzer → RansacPlaneFitter /
            //       OrientationDeviation, then write report.
            doc.Editor.WriteMessage("\nNRMCOMPARE: not implemented yet.\n");
        }
    }
}
