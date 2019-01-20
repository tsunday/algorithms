using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;
using System.IO;

namespace agh.algorytmy
{
    class Huffman
    {
        public void huffman(int dlugoscKodowania, String inputFile, String outputFile)
        {
            PriorityQueue<Node> drzewo = null;
            Dictionary<dynamic, dynamic> kodowanie = new Dictionary<dynamic, dynamic>();
            Dictionary<dynamic, dynamic> dekodowanie = new Dictionary<dynamic, dynamic>();
            Dictionary<dynamic, int> dict = new Dictionary<dynamic, int>();

            StreamReader fileIn = new StreamReader(inputFile, Encoding.UTF8);
            String text = fileIn.ReadToEnd();

            int licz = 1;
            String tekst = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (dlugoscKodowania == 1)
                {
                    char a = text.ElementAt(i);
                    if (dict.ContainsKey(a))
                        dict[a] = dict[a] + 1;
                    else
                        dict[a] = 1;
                }
                else
                {
                    tekst += text.ElementAt(i);
                    if (licz == dlugoscKodowania)
                    {
                        if (dict.ContainsKey(tekst))
                            dict[tekst] = dict[tekst] + 1;
                        else
                            dict[tekst] = 1;
                        tekst = "";
                        licz = 0;
                    }
                    licz++;
                }
            }


            drzewo = new PriorityQueue<Node>(dict.Count, new NodeComparator());
            int n = 0;

            if (dlugoscKodowania == 1)
            {
                Dictionary<dynamic, int> test = dict;
                foreach (Char c in test.Keys)
                {
                    drzewo.Add(new Node(c, dict[c]));
                    n++;
                }
            }
            else
            {
                Dictionary<dynamic, int> test = dict;
                foreach (String c in test.Keys)
                {
                    drzewo.Add(new Node(c, dict[c]));
                    n++;
                }
            }

            for (int i = 1; i <= n - 1; i++)
            {
                Node z = new Node();
                z.lewy = drzewo.Take();
                z.prawy = drzewo.Take();
                z.czestosc = z.lewy.czestosc + z.prawy.czestosc;
                drzewo.Add(z);
            }
            Node root = drzewo.Take();

            koduj(root, "", kodowanie, dekodowanie, dlugoscKodowania);
            String skompresowanyTekst = kompresja(text, kodowanie, dlugoscKodowania);

            StreamWriter fileOut = new StreamWriter(outputFile);
            fileOut.WriteLine(skompresowanyTekst);
            fileOut.Close();

            String zdekompresowanyTekst = dekompresja(skompresowanyTekst, dekodowanie, dlugoscKodowania);
            fileOut = new StreamWriter(inputFile + "_decmp");
            fileOut.WriteLine(zdekompresowanyTekst);
            fileOut.Close();

            FileInfo infoIn = new FileInfo(inputFile);
            FileInfo infoOut = new FileInfo(outputFile);
            double K = ((infoIn.Length * 8.0 - infoOut.Length)) / (infoIn.Length * 8.0);
            System.Console.WriteLine("Wejscie: " + infoIn.Length + " Wyjscie: " + infoOut.Length + " K = " + K);
        }


        public void koduj(Node n, String s, Dictionary<dynamic, dynamic> kodowanie, Dictionary<dynamic, dynamic> dekodowanie, int dlugoscKodowania)
        {
            if (n == null)
                return;
            koduj(n.lewy, s + "0", kodowanie, dekodowanie, dlugoscKodowania);
            koduj(n.prawy, s + "1", kodowanie, dekodowanie, dlugoscKodowania);

            if (dlugoscKodowania == 1)
            {
                if (!Char.ToString(n.znak).Equals('\0'))
                {
                    kodowanie[n.znak] = s;
                    dekodowanie[s] = n.znak;
                }
            }
            else
            {
                if (n.znaki != null)
                {
                    kodowanie[n.znaki] = s;
                    dekodowanie[s] = n.znaki;
                }
            }
        }



        public String kompresja(String s, Dictionary<dynamic, dynamic> kodowanie, int dlugoscKodowania)
        {
            String c = "";
            if (dlugoscKodowania == 1)
            {
                for (int i = 0; i < s.Length; i++)
                    c = c + kodowanie[s.ElementAt(i)];
            }
            else
            {
                String tekst = "";
                int licz = 1;
                for (int i = 0; i < s.Length; i++)
                {
                    tekst += s.ElementAt(i);
                    if (dlugoscKodowania == licz)
                    {
                        c = c + kodowanie[tekst];
                        tekst = "";
                        licz = 0;
                    }
                    licz++;
                }
            }
            return c;
        }

        public String dekompresja(String s, Dictionary<dynamic, dynamic> dekodowanie, int dlugoscKodowania)
        {
            String temp = "";
            String result = "";

            if (dlugoscKodowania == 1)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    temp = temp + s.ElementAt(i);
                    Char c = (Char)dekodowanie[temp];
                    if (c != null && c != 0)
                    {
                        result = result + c;
                        temp = "";
                    }
                }
            }
            else
            {
                for (int i = 0; i < s.Length; i++)
                {
                    temp = temp + s.ElementAt(i);
                    if (dekodowanie.ContainsKey(temp))
                    {
                        String c = (String)dekodowanie[temp];
                        if (c != null)
                        {
                            result = result + c;
                            temp = "";
                        }
                    }
                }
            }
            return result;
        }
    }


    class Node
    {
        public char znak;
        public int czestosc;
        public Node lewy, prawy;
        public String znaki;

        public Node() { }

        public Node(char znak, int czestosc)
        {
            this.znak = znak;
            this.czestosc = czestosc;
        }

        public Node(String znaki, int czestosc)
        {
            this.znaki = znaki;
            this.czestosc = czestosc;
        }
    }

    class NodeComparator : IComparer<Node>
    {
        public int Compare(Node a, Node b)
        {
            return a.czestosc - b.czestosc;
        }

    }
}