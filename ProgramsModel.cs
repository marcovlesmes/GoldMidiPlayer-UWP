﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldMidiPlayer
{
    public class ProgramsModel
    {
        public int Index { get; }
        public string Name { get; }

        public ProgramsModel(int index, string name)
        {
            Index = index;
            Name = name;
        }
    }
}
