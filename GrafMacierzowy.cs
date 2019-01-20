using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agh.algorytmy
{
    class GrafMacierzowy : Graf
    {
        public List<object> wierzcholki;
        public List<object> krawedzi;
        private object[,] macierz;

        public GrafMacierzowy()
        {
            krawedzi = new List<object>();
            wierzcholki = new List<object>();
            macierz = new object[0, 0];
        }

        public bool czyIstniejeKrawedz(int a, int b)
        {
            return macierz[a, b] != null;
        }

        public void dodajKrawedz(object k, int a, int b)
        {
            krawedzi.Add(k);
            macierz[a, b] = k;
        }

        public void dodajWierzcholek(object w)
        {
            object[,] tempMacierz = new object[wierzcholki.Count + 1, wierzcholki.Count + 1];

            for (int i = 0; i < podajIloscWierzcholkow(); i++)
            {
                for (int j = 0; j < podajIloscWierzcholkow(); j++)
                {
                    tempMacierz[i, j] = macierz[i, j];
                }
            }
            macierz = tempMacierz;
            wierzcholki.Add(w);
        }

        public int podajIloscKrawedzi()
        {
            return krawedzi.Count;
        }

        public int podajIloscWierzcholkow()
        {
            return wierzcholki.Count;
        }

        public List<object> podajIncydentneKrawedzie(int idx)
        {
            List<object> incydentne = new List<object>();

            for (int j = 0; j < podajIloscWierzcholkow(); j++)
            {
                if (macierz[idx, j] != null)
                {
                    incydentne.Add(macierz[idx, j]);
                }
                if (idx != j && macierz[j, idx] != null)
                {
                    incydentne.Add(macierz[j, idx]);
                }
            }
            return incydentne;
        }

        public List<object> podajSasiadnieWierzcholki(int idx)
        {
            List<object> sasiedzi = new List<object>();

            for (int j = 0; j < podajIloscWierzcholkow(); j++)
            {
                if (idx != j && macierz[idx, j] != null)
                {
                    sasiedzi.Add(wierzcholki[j]);
                }
                if (idx != j && macierz[j, idx] != null)
                {
                    sasiedzi.Add(wierzcholki[j]);
                }
            }


            return sasiedzi;
        }

        private void usunKrawedz(int idx)
        {
            for (int i = 0; i < podajIloscWierzcholkow(); i++)
            {
                for (int j = 0; j < podajIloscWierzcholkow(); j++)
                {
                    if (macierz[i, j] == krawedzi[idx])
                    {
                        macierz[i, j] = null;
                    }
                }
            }
            krawedzi.RemoveAt(idx);
        }

        public void usunKrawedz(int a, int b)
        {
            object krawedz = macierz[a, b];
            usunKrawedz(krawedzi.IndexOf(krawedz));
        }

        public void usunWierzcholek(int idx)
        {
            foreach (object krawedz in podajIncydentneKrawedzie(idx))
            {
                usunKrawedz(krawedzi.IndexOf(krawedz));
            }
            wierzcholki.RemoveAt(idx);
        }

        public List<Krawedz> podajListeKrawedzi()
        {
            List<Krawedz> lista = new List<Krawedz>();
            for (int i = 0; i < podajIloscWierzcholkow(); i++)
            {
                for (int j = 0; j < podajIloscWierzcholkow(); j++)
                {
                    if (macierz[i, j] != null)
                        lista.Add(new Krawedz(i.ToString() + '.' + j.ToString()) { obj = macierz[i, j], v1 = i, v2 = j });
                }
            }
            return lista;
        }

        public List<object> podajListeWierzcholkow()
        {
            return wierzcholki;
        }
    }
}
