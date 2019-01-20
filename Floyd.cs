using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agh.algorytmy
{
    class Floyd
    {
        private Graf graf;
        private int[,] wagi;
        public int[,] d;
        public object[,] poprzednik;

        public Floyd(Graf g, int[,] w)
        {
            graf = g;
            wagi = w;
            d = new int[g.podajIloscWierzcholkow(), g.podajIloscWierzcholkow()];
            poprzednik = new object[g.podajIloscWierzcholkow(), g.podajIloscWierzcholkow()];
        }

        public void Inicjalizuj()
        {
            List<object> wierzcholki = graf.podajListeWierzcholkow();
            for (int i = 0; i < graf.podajIloscWierzcholkow(); i++)
            {
                for (int j = 0; j < graf.podajIloscWierzcholkow(); j++)
                {
                    if (i == j)
                    {
                        d[i, j] = 0;
                    }
                    else
                    {
                        d[i, j] = 9999;
                    }
                    poprzednik[i, j] = null;
                }
            }

            List<Krawedz> krawedzi = graf.podajListeKrawedzi();
            foreach (Krawedz k in graf.podajListeKrawedzi())
            {
                d[k.v1, k.v2] = wagi[k.v1, k.v2];
                poprzednik[k.v1, k.v2] = wierzcholki[k.v1];
            }

            for(int u = 0; u < wierzcholki.Count; u++)
            {
                for(int v1 = 0; v1 < wierzcholki.Count; v1++)
                {
                    for (int v2 = 0; v2 < wierzcholki.Count; v2++)
                    {
                        if (d[v1, v2] > d[v1, u] + d[u, v2])
                        {
                            d[v1, v2] = d[v1, u] + d[u, v2];
                            poprzednik[v1, v2] = poprzednik[u, v2];
                        }
                    }
                }
            }
        }
    }
}
