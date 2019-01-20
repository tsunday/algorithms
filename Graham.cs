using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;


namespace agh.algorytmy
{
    class Punkt : IComparable
    {
        public double X;
        public double Y;

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Punkt inny = obj as Punkt;
            if(inny != null)
            {
                return this.Y.CompareTo(inny.Y);
            }
            else
            {
                throw new ArgumentException("Object is not a Punkt");
            }
        }

        public override String ToString()
        {
            return "( " + X + ", " + Y + " )";
        }
        public static List<Punkt> Wczytaj(String sciezka)
        {
            var lista = new List<Punkt>();
            using(StreamReader sr = new StreamReader(sciezka))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var fields = line.Split(',');
                    double[] values;
                    values = Array.ConvertAll(fields, s => double.Parse(s, NumberStyles.Float, CultureInfo.InvariantCulture));
                    lista.Add(new Punkt() { X = values[0], Y = values[1] });
                }
            }
            return lista;
        }
    }
    class Graham
    {
        List<Punkt> punkty;
        Punkt P;
        public Graham(List<Punkt> punkty)
        {
            this.punkty = punkty;
            P = punkty[indeksSkrajnego()];
        }
        public Stack<Punkt> Otoczka()
        {
            Stack<Punkt> omega = new Stack<Punkt>();
            punkty.Sort(isLeftToP);
            omega.Push(punkty[0]);
            omega.Push(punkty[1]);

            int i = 0;
            while(i < punkty.Count)
            {
                if(omega.Peek() == punkty[0])
                {
                    omega.Push(punkty[i]);
                    i++;
                }

                if(isLeft(punkty[i], omega.ElementAt(1), omega.ElementAt(0)) < 0)
                {
                    omega.Push(punkty[i]);
                    i++;
                }
                else
                {
                    omega.Pop();
                }
            }
            return omega;
        }
        int indeksSkrajnego()
        {
            return punkty.IndexOf(punkty.Min());
        }

        private int isLeftToP(Punkt A, Punkt B)
        {
            return isLeft(A, B, P);
        }
        private int isLeft(Punkt A, Punkt B, Punkt P)
        {
            double d = (P.X - A.X) * (B.Y - A.Y) - (P.Y - A.Y) * (B.X - A.X);
            if (d < 0)
                return -1;
            else
                return 1;

        }
    }
}
