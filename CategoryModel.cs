using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldMidiPlayer
{
    class CategoryModel
    {
        public int Index { get; }
        public string Name { get; }
        public int StartIndex { get; }
        public int Count { get;  }

        public CategoryModel(int index, string name, int startIndex, int count)
        {
            Index = index;
            Name = name;
            StartIndex = startIndex;
            Count = count;
        }
    }
}
