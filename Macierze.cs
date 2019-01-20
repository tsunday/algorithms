using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Globalization;
using Matrix = System.Collections.Generic.List<System.Collections.Generic.List<double>>;

namespace agh.algorytmy
{
    class Macierze
    {
        
        Matrix Pomnoz(Matrix a, Matrix b)
        {
            if (a == null && b != null)
                return b;
            if (b == null && a != null)
                return a;
            if(a == null && b == null)
                throw new Exception("nic do mnozenia");

            Matrix result = new Matrix();
            
            for(int i = 0; i < a.Count; i++)
            {
                result.Add(new List<double>());
                for (int j = 0; j < b[0].Count; j++)
                {
                    result[i].Add(0);
                    for(int k = 0; k < a[i].Count; k++)
                    {
                        result[i][j] += a[i][k] * b[k][j];
                    }
                }
            }
            return result;
        }

        public Matrix PomnozSekwencyjnie(Stack<Matrix> lista)
        {
            if (!lista.Any())
                return null;
            Matrix second = lista.Pop();
            return Pomnoz(PomnozSekwencyjnie(lista), second);
        }

        public void Mnozenie(object data)
        {
            Stack<Matrix> lista = (Stack<Matrix>)data;
            PomnozSekwencyjnie(lista);
        }

        public Matrix PomnozRownolegle(Stack<Matrix> lista, int iloscWatkow)
        {
            if (lista.Count != 0 && lista.Count / 2 < iloscWatkow)
                iloscWatkow = lista.Count / 2;
            int ilosc = lista.Count / iloscWatkow;
            int dlugosc = lista.Count;
            Thread[] watki = new Thread[iloscWatkow];
            int w = 0;
            Stack<Matrix> czesc;
            for (int i = 0; i < dlugosc; i+=ilosc)
            {
                czesc = new Stack<Matrix>();

                for (int j = 0; j < ilosc; j++)
                    czesc.Push(lista.Pop());
                
                if (lista.Count < ilosc)
                {
                    for (int k = 0; k < lista.Count; k++)
                        czesc.Push(lista.Pop());
                }

                czesc.Reverse();

                Macierze m = new Macierze();
                watki[w]= new Thread(m.Mnozenie);
                watki[w].Start(czesc);
                w++;
            }
            foreach (Thread t in watki)
                t.Join();
            return new Matrix();
        }

        public Stack<Matrix> WczytajPlik(String sciezka, int ilosc = -1)
        {
            int i = 0;
            Stack<Matrix> macierze = new Stack<Matrix>();

            using (StreamReader sr = new StreamReader(sciezka))
            {
                while (!sr.EndOfStream)
                {
                    if (ilosc >= 0 && i == ilosc)
                        break;
                    var line = sr.ReadLine();
                    if (!line.Any())
                    {
                        i++;
                        continue;
                    }
                    var fields = line.Split(';');
                    double[] values;
                    values = Array.ConvertAll(fields, s => double.Parse(s, NumberStyles.Float, CultureInfo.InvariantCulture));
                    if (macierze.Count <= i)
                        macierze.Push(new Matrix());
                    macierze.Peek().Add(new List<double>(values));
                }
            }
            return macierze;
        }
    }
}
