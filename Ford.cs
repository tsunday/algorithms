using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace agh.algorytmy
{
    public class Ford
    {
        static Dictionary<int, Wierzcholek> wierzcholki { get; set; }
        static Dictionary<string, Krawedz> krawedzi { get; set; }
        private const float MaxValue = float.MaxValue;
        List<int[]> dane;
        int ilosc;

        public Ford(List<int[]> data, int count)
        {
            wierzcholki = new Dictionary<int, Wierzcholek>();
            krawedzi = new Dictionary<string, Krawedz>();
            this.dane = data;
            this.ilosc = count;
        }

        //public static void Main(string[] args)
        //{
        //    new FordFulkerson().Run();
        //    PrintLn("Press key to exit ...");
        //    Console.ReadKey();
        //}

        void Parsuj()
        {
            Reset();

            for (int i = 0; i < ilosc; i++ )
            {
                wierzcholki.Add(i, new Wierzcholek() { Name = "w" + i.ToString() });
            }

            foreach (var edge in dane)
            {
                Wierzcholek node1 = wierzcholki[edge[0]];
                Wierzcholek node2 = wierzcholki[edge[1]];
                float capacity = edge[2];

                DodajKrawedz(node1, node2, capacity);
            }
        }

        public void Start()
        {
            Parsuj();
            Algo();
        }

        void Algo()
        {
            var nodeSource = wierzcholki[1];
            var nodeTerminal = wierzcholki[wierzcholki.Count - 1];

            WyswietlWierzcholki();

            FordFulkersonAlgo(nodeSource, nodeTerminal);
        }


        public void FordFulkersonAlgo(Wierzcholek nodeSource, Wierzcholek nodeTerminal)
        {
            PrintLn("\n** FordFulkerson");
            var flow = 0f;

            var path = Bfs(nodeSource, nodeTerminal);

            while (path != null && path.Count > 0)
            {
                var minCapacity = MaxValue;
                foreach (var edge in path)
                {
                    if (edge.Pojemnosc < minCapacity)
                        minCapacity = edge.Pojemnosc; // update
                }

                if (minCapacity == MaxValue || minCapacity < 0)
                    throw new Exception("minCapacity " + minCapacity);

                Sciezka(path, minCapacity);
                flow += minCapacity;

                path = Bfs(nodeSource, nodeTerminal);
            }

            // max flow
            PrintLn("\n** Max flow = " + flow);

            // min cut
            PrintLn("\n** Min cut");
            ZnajdzMin(nodeSource);
        }


        static void Sciezka(IEnumerable<Krawedz> path, float minCapacity)
        {
            foreach (var edge in path)
            {
                var keyResidual = PodajKlucz(edge.Prawy.Id, edge.Lewy.Id);

                edge.Pojemnosc -= minCapacity;
            }
        }

        void ZnajdzMin(Wierzcholek root)
        {
            var queue = new Queue<Wierzcholek>();
            var discovered = new HashSet<Wierzcholek>();
            var minCutNodes = new List<Wierzcholek>();
            var minCutEdges = new List<Krawedz>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (discovered.Contains(current))
                    continue;

                minCutNodes.Add(current);
                discovered.Add(current);

                var edges = current.NodeEdges;
                foreach (var edge in edges)
                {
                    var next = edge.Prawy;
                    if (edge.Pojemnosc <= 0 || discovered.Contains(next))
                        continue;
                    queue.Enqueue(next);
                    minCutEdges.Add(edge);
                }
            }

            // bottleneck as a list of arcs
            var minCutResult = new List<Krawedz>();
            List<int> nodeIds = minCutNodes.Select(node => node.Id).ToList();

            var nodeKeys = new HashSet<int>();
            foreach (var node in minCutNodes)
                nodeKeys.Add(node.Id);

            var edgeKeys = new HashSet<string>();
            foreach (var edge in minCutEdges)
                edgeKeys.Add(edge.Nazwa);


            Parsuj(); // reset the graph

            // finding by comparing residual and original graph

            foreach (var id in nodeIds)
            {
                var node = wierzcholki[id];
                var edges = node.NodeEdges;
                foreach (var edge in edges)
                {
                    if (nodeKeys.Contains(edge.Prawy.Id))
                        continue;

                    if (edge.Pojemnosc > 0 && !edgeKeys.Contains(edge.Nazwa))
                        minCutResult.Add(edge);
                }
            }

            float maxflow = 0;
            foreach (var edge in minCutResult)
            {
                maxflow += edge.Pojemnosc;
                PrintLn(edge.Info());
            }
            PrintLn("min-cut total maxflow = " + maxflow);
        }

        List<Krawedz> Bfs(Wierzcholek root, Wierzcholek target)
        {
            root.TraverseParent = null;
            target.TraverseParent = null; //reset

            var queue = new Queue<Wierzcholek>();
            var discovered = new HashSet<Wierzcholek>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                Wierzcholek current = queue.Dequeue();
                discovered.Add(current);

                if (current.Id == target.Id)
                    return PodajSciezke(current);

                var nodeEdges = current.NodeEdges;
                foreach (var edge in nodeEdges)
                {
                    var next = edge.Prawy;
                    var c = PodajPrzepustowosc(current, next);
                    if (c > 0 && !discovered.Contains(next))
                    {
                        next.TraverseParent = current;
                        queue.Enqueue(next);
                    }
                }
            }
            return null;
        }


        static List<Krawedz> PodajSciezke(Wierzcholek node)
        {
            var path = new List<Krawedz>();
            var current = node;
            while (current.TraverseParent != null)
            {
                var key = PodajKlucz(current.TraverseParent.Id, current.Id);
                var edge = krawedzi[key];
                path.Add(edge);
                current = current.TraverseParent;
            }
            return path;
        }

        public static string PodajKlucz(int id1, int id2)
        {
            return id1 + "|" + id2;
        }

        public float PodajPrzepustowosc(Wierzcholek node1, Wierzcholek node2)
        {
            var edge = krawedzi[PodajKlucz(node1.Id, node2.Id)];
            return edge.Pojemnosc;
        }

        public void DodajKrawedz(Wierzcholek nodeFrom, Wierzcholek nodeTo, float capacity)
        {
            var key = PodajKlucz(nodeFrom.Id, nodeTo.Id);
            var edge = new Krawedz() { Lewy = nodeFrom, Prawy = nodeTo, Pojemnosc = capacity, Nazwa = key };
            krawedzi.Add(key, edge);
            nodeFrom.NodeEdges.Add(edge);
        }


        static void WyswietlWierzcholki()
        {
            for (int i = 0; i < wierzcholki.Count; i++)
            {
                var node = wierzcholki[i];
                PrintLn(node.ToString() + " outnodes=" + node.Info());
            }
        }

        static void Reset()
        {
            wierzcholki.Clear();
            krawedzi.Clear();
            Wierzcholek.Reset();
        }

        public class Wierzcholek
        {
            private static int _counter;
            public int Id {get; set;}
            public string Name { get; set; }
            public List<Krawedz> NodeEdges { get; set; }
            public Wierzcholek TraverseParent { get; set; }

            public Wierzcholek()
            {
                Id = _counter++;
                NodeEdges = new List<Krawedz>();
            }

            public static void Reset()
            {
                _counter = 0;
            }

            public string Info()
            {
                var sb = new StringBuilder();
                foreach (var edge in NodeEdges)
                {
                    var node = edge.Prawy;
                    if (edge.Pojemnosc > 0)
                        sb.Append(node.Name + "C" + edge.Pojemnosc + " ");
                }
                return sb.ToString();
            }

            public override string ToString()
            {
                return string.Format("Id={0}, Name={1}", Id, Name);
            }
        }
        public class Krawedz
        {
            public Wierzcholek Lewy { get; set; }
            public Wierzcholek Prawy { get; set; }
            public float Pojemnosc { get; set; }
            public string Nazwa { get; set; }

            public override string ToString()
            {
                return
                    string.Format("NodeFrom={0}, NodeTo={1}, C={2}", Lewy.Name, Prawy.Name, Pojemnosc);
            }

            public string Info()
            {
                return string.Format("NodeFrom=({0}), NodeTo=({1}), C={2}", Lewy, Prawy, Pojemnosc);
            }
        }

        public int PodajIloscWierzcholkow()
        {
            return wierzcholki.Count;
        }
        public static void PrintLn(object o) { 
            Console.WriteLine(o); 
        } //alias
        public static void PrintLn() { 
            Console.WriteLine(); 
        } //alias
        public static void Print(object o) { 
            Console.Write(o); 
        } //alias
    }
}
