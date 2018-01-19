using System;
using System.Collections.Generic;
using System.Linq;

namespace EvenTree
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] n = new int[2];
            n = Array.ConvertAll(Console.ReadLine().Split(' '), Int32.Parse);
            Node[] nodes = new Node[n[0]];
            for (int i = 0; i < nodes.Length; i++) nodes[i] = new Node();
            for (int i = 0; i < n[1]; i++)
            {
                int[] e = Array.ConvertAll(Console.ReadLine().Split(' '), Int32.Parse);
                nodes[e[0] - 1].AddEdge(nodes[e[1] - 1]);
            }

            int maxCut = 0;
            for(int i = 0; i < nodes.Length; i++)
            {
                if(nodes[i].GetParentID() != 0)
                {
                    int t = nodes[i].GetParentID();
                    nodes[i].RemoveEdge();
                    if (nodes[i].ForestSize % 2 == 0 && nodes[t - 1].ForestSize % 2 == 0) maxCut++;
                    else nodes[i].AddEdge(nodes[t - 1]);
                }
            }
            Console.WriteLine(maxCut);
        }
    }

    class Node
    {
        static int nodes = 0;
        int id { get; set; }
        int forestSize { get; set; }
        Node parent = null;
        List<Node> children = new List<Node>();

        public Node() : this(null) { }

        public Node(Node _parent)
        {
            parent = _parent;
            nodes++;
            ID = nodes;
            ForestSize = 1;
        }

        public int ID
        {
            get { return id;  }
            set { id = value; }
        }

        public int ForestSize
        {
            get { return forestSize;  }
            set
            {
                forestSize = value;
                if(parent != null)
                {
                    parent.ForestSize = value;
                }
                foreach(Node i in children)
                {
                    i.forestSize = value;
                }
            }
        }

        public int GetParentID()
        {
            if (parent != null) return parent.ID;
            else return 0;
        }

        public void AddEdge(Node _parent)
        {
            parent = _parent;
            parent.AddChildren(this);
            CalculateForest();
        }

        public void RemoveEdge()
        {
            Node op = parent;
            parent.RemoveChildren(this);
            parent = null;
            CalculateForest();
            op.CalculateForest();
        }

        void AddChildren(Node child)
        {
            if(children.All(x => x.ID != child.ID)) children.Add(child);
            foreach(Node i in child.children)
            {
                if (children.All(x => x.ID != i.ID)) children.Add(i);
            }
            if(parent != null)
            {
                parent.AddChildren(this);
            }
        }

        void RemoveChildren(Node child)
        {
            children.Remove(child);
            foreach(Node i in child.children)
            {
                children.Remove(i);
            }
            if(parent != null) parent.RemoveChildren(child);
        }

        void CalculateForest()
        {
            if (parent == null) ForestSize = children.Count + 1;
            else parent.CalculateForest();
        }
    }
}
