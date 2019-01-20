using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace agh.algorytmy
{
    class GrafCsv
    {
        public List<int[]> dane;

        public GrafCsv(String sciezka, bool fromZero = false)
        {
            dane = new List<int[]>();
            try
            {
                using (StreamReader sr = new StreamReader(sciezka))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        var fields = line.Split(';');
                        int[] values;
                        if(!fromZero)
                            values = Array.ConvertAll(fields, s => int.Parse(s) - 1);
                        else
                            values = Array.ConvertAll(fields, s => int.Parse(s));
                        dane.Add(values);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        public int iloscWierzcholkow()
        {
            var max = new List<int>();
            for (int i = 0; i < dane.Count; i++)
            {
                max.Add(Math.Max(dane[i][0], dane[i][1]));
            }
            return max.Max() + 1;
        }

        public void UzupelnijGraf(Graf graf)
        {
            for (int i = 0; i < iloscWierzcholkow(); i++)
            {
                graf.dodajWierzcholek(new Wierzcholek(i.ToString()));
            }
            for (int i = 0; i < dane.Count; i++)
            {
                graf.dodajKrawedz(new Krawedz(dane[i][0].ToString() + '.' + dane[i][1].ToString()), dane[i][0], dane[i][1]);
            }
        }

        public int[,] PodajWagi()
        {
            int[,] wagi = new int[iloscWierzcholkow(), iloscWierzcholkow()];

            foreach(var val in dane)
            {
                wagi[val[0], val[1]] = val[2];
            }

            return wagi;
        }
    }
}
