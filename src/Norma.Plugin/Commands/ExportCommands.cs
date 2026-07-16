using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.Runtime;

[assembly: CommandClass(typeof(Norma.Plugin.Commands.ExportCommands))]

namespace Norma.Plugin.Commands
{
    /// <summary>
    /// Point-extraction commands: pull XYZ coordinates out of the drawing and
    /// export them as CSV for external analysis.
    /// </summary>
    public sealed class ExportCommands
    {
        /// <summary>
        /// NRMEXPORT — prompts for a selection of point-bearing entities
        /// (COGO points, block inserts, DBPoint entities), extracts
        /// <c>PointID, X, Y, Z, Source</c> per entity (mm), and writes them to a
        /// user-chosen CSV file via <c>Norma.Core.Csv.PointCsvSerializer</c>.
        /// </summary>
        [CommandMethod("NRMEXPORT")]
        public void ExportPoints()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc is null) return;

            // TODO: selection filter (AECC_COGO_POINT, INSERT, POINT), extract
            //       id/position/source per entity, save-file dialog, serialize.
            doc.Editor.WriteMessage("\nNRMEXPORT: not implemented yet.\n");
        }
    }
}
