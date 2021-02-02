using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldMidiPlayer
{
    public class ProgramsModel
    {
        public int Program { get; }
        public string Name { get; }

        public ProgramsModel(int program, string name)
        {
            Program = program;
            Name = name;
        }
    }
}
