using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConverter.CodeBuilder.File
{
    public sealed class CodeFileHeader : BaseCodeFile
    {
        public string? Namespace { get; set; }
        public List<string> Usings { get; set; }
        public CodeFileHeader()
        {
            Usings = new();
        }

        public override StringBuilder Save(bool lastBracket = false)
        {
            // Write using first
            foreach(var codeUsing in Usings){
                CodeWriter.AppendLine($"using {codeUsing};");
            }

            // Namespace
            if (!string.IsNullOrWhiteSpace(Namespace))
                CodeWriter.AppendLine($"namespace {Namespace}" + StartBracket);

            return CodeWriter;
        }
    }
}
