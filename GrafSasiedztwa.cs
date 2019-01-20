using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agh.algorytmy
{
    class GrafSasiedztwa : Graf
    {
        public List<object> wierzcholki;
        public List<List<object>> krawedzi;
        private List<int>[] sasiedztwa;

        public GrafSasiedztwa()
        {
            krawedzi = new List<List<object>>();
            wierzcholki = new List<object>();
            sasiedztwa = new List<int>[0];
        }

        public bool czyIstniejeKrawedz(int a, int b)
        {
            List<int> ls = sasiedztwa[a];

            return ls.IndexOf(b) != -1;
        }

        public void dodajKrawedz(object k, int a, int b)
        {
            krawedzi[a][b] = k;
            sasiedztwa[a].Add(b);
        }

        public void dodajWierzcholek(object w)
        {
            List<int>[] tempSasiedztwa = new List<int>[wierzcholki.Count + 1];

            for (int i = 0; i < podajIloscWierzcholkow(); i++)
            {
                tempSasiedztwa[i] = sasiedztwa[i];
            }
            sasiedztwa = tempSasiedztwa;
            sasiedztwa[wierzcholki.Count] = new List<int>();
            wierzcholki.Add(w);
            krawedzi.Add(new List<object>(new object[podajIloscWierzcholkow() - 1]));
            foreach (List<object> ls in krawedzi)
            {
                ls.Add(null);
            }
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
            List<int> koncowe = sasiedztwa[idx];

            // wychodzace
            for (int j = 0; j < koncowe.Count; j++)
            {
                incydentne.Add(krawedzi[idx][j]);
            }

            // wchodzace
            for (int i = 0; i < podajIloscWierzcholkow(); i++)
            {
                if (i != idx && sasiedztwa[i].Contains(idx))
                {
                    incydentne.Add(krawedzi[i][idx]);
                }
            }

            return incydentne;
        }

        public List<object> podajSasiadnieWierzcholki(int idx)
        {
            List<object> sasiedzi = new List<object>();
            
            foreach (int s in sasiedztwa[idx])
            {
                sasiedzi.Add(wierzcholki[s]);
            }

            for (int i = 0; i < podajIloscWierzcholkow(); i++)
            {
                if (i != idx && sasiedztwa[i].Contains(idx))
                {
                    sasiedzi.Add(wierzcholki[i]);
                }
            }

            return sasiedzi;
        }

        public void usunKrawedz(int a, int b)
        {
            usunKrawedz(krawedzi[a][b]);
        }

        private void usunKrawedz(object k)
        {
            for (int i = 0; i < podajIloscWierzcholkow(); i++)
            {
                int j = krawedzi[i].IndexOf(k);
                krawedzi[i].Remove(k);
                sasiedztwa[i].Remove(j);
            }
        }

        public void usunWierzcholek(int idx)
        {
            foreach (object krawedz in podajIncydentneKrawedzie(idx))
            {
                usunKrawedz(krawedz);
            }
            wierzcholki.RemoveAt(idx);
        }

        public List<Krawedz> podajListeKrawedzi()
        {
            List<Krawedz> lista = new List<Krawedz>();
            for (int i = 0; i < sasiedztwa.Length; i++)
            {
                foreach (int w in sasiedztwa[i])
                {
                    lista.Add(new Krawedz(i.ToString() + '.' + w.ToString()){ obj = krawedzi[i][w], v1 = i, v2 = w});
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
