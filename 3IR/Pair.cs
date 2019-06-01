using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3IR
{
    public class Pair<T, G>
    {
        public T first;
        public G second;

        public Pair(T first, G second)
        {
            this.first = first;
            this.second = second;
        }

    }
}
