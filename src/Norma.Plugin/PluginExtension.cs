using Autodesk.AutoCAD.Runtime;

[assembly: ExtensionApplication(typeof(Norma.Plugin.PluginExtension))]

namespace Norma.Plugin
{
    /// <summary>
    /// Entry point AutoCAD calls when the assembly is NETLOADed.
    /// Keep <see cref="Initialize"/> fast — it runs on the UI thread during load.
    /// </summary>
    public sealed class PluginExtension : IExtensionApplication
    {
        /// <summary>Called once when AutoCAD loads the assembly.</summary>
        public void Initialize()
        {
            var doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            doc?.Editor.WriteMessage("\nNorma loaded. Commands: NRMEXPORT, NRMCOMPARE\n");
        }

        /// <summary>Called once when AutoCAD shuts down. No cleanup needed yet.</summary>
        public void Terminate()
        {
        }
    }
}
