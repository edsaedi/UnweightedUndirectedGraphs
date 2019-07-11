using System;
using System.Collections.Generic;
using System.Text;

namespace Unweighted_and_Undirected_Graphs
{
    public class Vertex<T>
    {
        public T Value { get; set; }
        public List<Vertex<T>> Neighbors { get; set; }
        public bool IsVisited { get; set; }
        public int Count => Neighbors.Count;

        internal Vertex(T value)
        {
            Value = value;
            Neighbors = new List<Vertex<T>>();
        }
    }

    public class Graph<T>
    {
        public List<Vertex<T>> Vertices { get; set; }

        public int Count => Vertices.Count;

        public Graph()
        {
            Vertices = new List<Vertex<T>>();
        }

        public Vertex<T> this[int index]
        {
            get { return Vertices[index]; }
            set { Vertices[index] = value; }
        }

        public void AddVertex(Vertex<T> vertex)
        {
            if (vertex == null)
            {
                throw new ArgumentNullException(nameof(vertex));
            }

            if (vertex.Neighbors.Count != 0)
            {
                throw new Exception("You can not add a vertex that has edges!");
            }

            if (Vertices.Contains(vertex))
            {
                throw new Exception("Item already exists!");
            }

            Vertices.Add(vertex);
        }

        public void AddVertex(T value)
        {
            AddVertex(new Vertex<T>(value));
        }

        public bool RemoveVertex(Vertex<T> vertex)
        {
            if (vertex == null)
            {
                throw new ArgumentException(nameof(vertex));
            }

            for (int i = 0; i < vertex.Count; i++)
            {
                RemoveEdge(vertex, Vertices[i]);
            }

            return Vertices.Remove(vertex);
        }

        public bool RemoveVertex(T value)
        {
            var vertex = Search(value);

            return RemoveVertex(vertex);
        }

        public bool AddEdge(Vertex<T> a, Vertex<T> b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }
            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            if (!(Vertices.Contains(a) && Vertices.Contains(b)))
            {
                return false;
            }

            a.Neighbors.Add(b);
            b.Neighbors.Add(a);

            return true;
        }

        public bool AddEdge(T a, T b)
        {
            return AddEdge(Search(a), Search(b));
        }

        public bool RemoveEdge(Vertex<T> a, Vertex<T> b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            if (!((Vertices.Contains(a) && Vertices.Contains(b)) && a.Neighbors.Contains(b)))
            {
                return false;
            }

            a.Neighbors.Remove(b);
            b.Neighbors.Remove(a);

            return true;
        }

        public bool RemoveEdge(T a, T b)
        {
            return RemoveEdge(Search(a), Search(b));
        }

        public Vertex<T> Search(T value)
        {
            foreach (var item in Vertices)
            {
                if (item.Value.Equals(value))
                {
                    return item;
                }
            }
            return null;
        }

        public bool IfPathExists(Vertex<T> start, Vertex<T> end)
        {
            Stack<Vertex<T>> search = new Stack<Vertex<T>>();

            Vertices.ForEach(a => a.IsVisited = false);

            search.Push(start);

            Vertex<T> curr = null;

            while (curr != end)
            {
                curr = search.Pop();
                curr.IsVisited = true;

                for (int i = 0; i < curr.Neighbors.Count; i++)
                {
                    if (curr.Neighbors[i].IsVisited)
                    {
                        search.Push(curr.Neighbors[i]);
                    }
                }

                if (search.Count == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IfCycleExists()
        {
            bool[] cycles = new bool[Vertices.Count];

            for (int i = 0; i < cycles.Length; i++)
            {
                Queue<Vertex<T>> search = new Queue<Vertex<T>>();

                Vertices.ForEach(a => a.IsVisited = false);
                search.Enqueue(Vertices[i]);

                Vertex<T> current = null;

                while (search.Count != 0)
                {
                    current = search.Dequeue();
                    current.IsVisited = false;

                    for (int j = 0; j < current.Neighbors.Count; j++)
                    {
                        if (!current.Neighbors[j].IsVisited)
                        {
                            search.Enqueue(current.Neighbors[j]);
                        }
                        else if (search.Contains(current))
                        {
                            cycles[i] = true;
                            break;
                        }
                    }

                    if (cycles[i])
                    {
                        break;
                    }
                }

                return false;
            }

            return true;
        }
    }
}
