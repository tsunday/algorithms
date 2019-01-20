using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace agh.algorytmy
{
    class Program
    {
        static void Main(string[] args)
        {
            // MACIERZOWO
            Graf gm = new GrafMacierzowy();

            gm.dodajWierzcholek("w1");
            gm.dodajWierzcholek("w2");
            gm.dodajWierzcholek(3);

            gm.dodajKrawedz("k1", 0, 1);
            gm.dodajKrawedz(2, 0, 2);

            List<object> sasiedzi = gm.podajSasiadnieWierzcholki(0);
            List<object> incydentne = gm.podajIncydentneKrawedzie(1);

            gm.usunKrawedz(0, 1);
            sasiedzi = gm.podajSasiadnieWierzcholki(0);
            incydentne = gm.podajIncydentneKrawedzie(1);
            bool czy = gm.czyIstniejeKrawedz(0, 1);

            // SASIEDZTWA
            Graf gs = new GrafSasiedztwa();

            gs.dodajWierzcholek("w1");
            gs.dodajWierzcholek("w2");
            gs.dodajWierzcholek(3);

            gs.dodajKrawedz("k1", 0, 1);
            gs.dodajKrawedz(2, 0, 2);

            sasiedzi = gs.podajSasiadnieWierzcholki(0);
            incydentne = gs.podajIncydentneKrawedzie(1);

            gs.usunKrawedz(0, 1);
            sasiedzi = gs.podajSasiadnieWierzcholki(0);
            incydentne = gs.podajIncydentneKrawedzie(1);
            czy = gs.czyIstniejeKrawedz(0, 1);

            // CSV
            gm = new GrafMacierzowy();
            gs = new GrafSasiedztwa();
            var csv = new GrafCsv("c:\\Users\\qpt487\\Work\\Code\\agh.algorytmy\\graf.txt");
            csv.UzupelnijGraf(gm);
            int[,] wagi = csv.PodajWagi();
            csv.UzupelnijGraf(gs);

            // FLOYD
            TimeSpan tMacierzowo;
            TimeSpan tSasiedztwa;
            Stopwatch sw = new Stopwatch();

            sw.Start();
            Floyd floyd = new Floyd(gm, wagi);
            floyd.Inicjalizuj();
            sw.Stop();
            tMacierzowo = sw.Elapsed;
            System.Console.WriteLine("Dlugosc sciezki w grafie macierzowym od 1 do 20: " + floyd.d[0, 19]);

            sw.Start();
            floyd = new Floyd(gs, wagi);
            floyd.Inicjalizuj();
            sw.Stop();
            tSasiedztwa = sw.Elapsed;
            System.Console.WriteLine("Dlugosc sciezki w liscie sasiedztw od 1 do 20: " + floyd.d[0, 19]);

            System.Console.WriteLine("R=" + tSasiedztwa.Ticks + "/" + tMacierzowo.Ticks);

            //FORD
            csv = new GrafCsv("c:\\Users\\qpt487\\Work\\Code\\agh.algorytmy\\graf.txt", true);
            Ford ford = new Ford(csv.dane, csv.iloscWierzcholkow());
            ford.Start();
            Console.WriteLine("Max flow 109-609: " + ford.PodajPrzepustowosc(new Ford.Wierzcholek(){Id=1}, new Ford.Wierzcholek(){Id=16}));


            ford.FordFulkersonAlgo(new Ford.Wierzcholek() { Id =  7 }, new Ford.Wierzcholek() { Id = ford.PodajIloscWierzcholkow() - 1 });
            

            // HUFFMAN
            Huffman huffman = new Huffman();
            String filename = "test";
            huffman.huffman(2, "c:\\Users\\qpt487\\Work\\Code\\agh.algorytmy\\" + filename + ".txt", "c:\\Users\\qpt487\\Work\\Code\\agh.algorytmy\\" + filename + "_encoded.txt");
           
            // MACIERZE 
            Macierze m = new Macierze();
            var macierze = m.WczytajPlik("c:\\Users\\qpt487\\Work\\Code\\agh.algorytmy\\sample-matrices.txt", 100);
            var wynik = m.PomnozSekwencyjnie(macierze);
            System.Console.WriteLine("Wymnozona macierz: ");
            foreach (var w in wynik)
            {
                foreach (var k in w)
                {
                    Console.Write(k);
                }
                Console.WriteLine();
            }

            //macierze = m.WczytajPlik("c:\\Users\\qpt487\\Work\\Code\\agh.algorytmy\\sample-matrices.txt", 8);
            //wynik = m.PomnozRownolegle(macierze, Environment.ProcessorCount);

            // GRAHAM
            var punkty = Punkt.Wczytaj("c:\\Users\\qpt487\\Work\\Code\\agh.algorytmy\\punktyPrzykladowe.csv");
            var graham = new Graham(punkty);
            var omega = graham.Otoczka();
            System.Console.WriteLine("Otoczka Grahama:");
            foreach(var p in omega)
            {
                Console.WriteLine(p.ToString());
            }

            Console.ReadKey();
        }
    }
}
