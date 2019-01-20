using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agh.algorytmy
{
    interface Graf
    {
        void dodajWierzcholek(object w);
        void usunWierzcholek(int idx);
        void dodajKrawedz(object k, int a, int b);
        void usunKrawedz(int a, int b);
        List<object> podajSasiadnieWierzcholki(int idx);
        List<object> podajIncydentneKrawedzie(int idx);
        int podajIloscWierzcholkow();
        int podajIloscKrawedzi();
        List<Krawedz> podajListeKrawedzi();
        List<object> podajListeWierzcholkow();
        bool czyIstniejeKrawedz(int a, int b);
    }
}
