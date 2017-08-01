using Assent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace YoTennis.Tests.Test
{
    static class Approvals
    {
        public static void Verify(string text, [CallerFilePath] string filepath = null, [CallerMemberName] string membername = null)
        {
            var filename = Path.GetFileNameWithoutExtension(filepath);
            var filedir = Path.GetDirectoryName(filepath);
            var config = new Configuration().UsingNamer(new Assent.Namers.FixedNamer(Path.Combine(filedir, "__snap__", filename + "." + membername)))
                //.UsingReporter(new Assent.Reporters.DiffReporter(new[] { new Assent.Reporters.DiffPrograms.VsCodeDiffProgram() }))
                ;
            string.Empty.Assent(text, config, membername, filepath);
        }
    }
}
