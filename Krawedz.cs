using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agh.algorytmy
{
    class Krawedz
    {
        public string id;
        public object obj { get; set; }
        public int v1 { get; set; }
        public int v2 { get; set; }
        public Krawedz(string id) 
        {
            this.id = id;
        }
    }
}
