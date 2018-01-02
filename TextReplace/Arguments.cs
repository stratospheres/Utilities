using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerArgs;

namespace TextReplace
{
    public class Arguments
    {
        [ArgRequired(PromptIfMissing = true)]
        [ArgShortcut("replace")]
        [ArgDescription("Specifies the replacements CSV file.")]
        public string ReplacementsFile { get; set; }

        [ArgRequired(PromptIfMissing = true)]
        [ArgShortcut("input")]
        [ArgDescription("Specifies the input file to perform replacements on.")]
        public string InputFile { get; set; }

        [ArgShortcut("insensitive")]
        [ArgDescription("Specifies if the searches are to be case insensitive. Default is false.")]
        public bool CaseInsensitive { get; set; }
    }
}
