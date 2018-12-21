using System;

namespace GetVersionByReleaseNotes
{
    class Program
    {
        static void Main(string[] args)
        {
var version=            VersionTools.ExtractFromReleaseNotesExtended(args[0]);
Console.WriteLine(version);
        }
    }
}
