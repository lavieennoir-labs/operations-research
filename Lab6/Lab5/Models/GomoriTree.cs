using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Lab5.Models
{
    public class GomoriTree
    {
        public class Connection
        {
            public int NodeIdFrom { get; set; }
            public int NodeIdTo { get; set; }
            public double Weight { get; set; }
        }

        public class Node
        {
            static int instanceCount = 0;

            public Node()
            {
                Id = ++instanceCount;
                Vertexes = new List<int>();
            }

            public int Id { get; set; }

            public List<int> Vertexes { get; set; }

            public override string ToString()
            {
                if (Vertexes == null)
                    return "{ }";

                StringBuilder sb = new StringBuilder();
                sb.Append("{ ");

                foreach (var v in Vertexes)
                    sb.Append(v).Append(", ");

                sb.Remove(sb.Length - 2, 1);
                sb.Append("}");

                return sb.ToString();
            }
            public override bool Equals(object obj)
            {
                if (obj is Node node)
                    return List<int>.Equals(Vertexes, node.Vertexes);
                return false;
            }
        }

        public GomoriTree()
        {
            Nodes = new List<Node>();
            Connections = new List<Connection>();
        }

        public List<Node> Nodes { get; set; }
        public List<Connection> Connections { get; set; }


        public string[] NodesNames
        {
            get => Nodes.Select(n => n.ToString()).ToArray();
        }

        public double[,] Capacity
        {
            get
            {
                var cap = new double[Nodes.Count, Nodes.Count];

                for (int i = 0; i < Nodes.Count; i++)
                    for (int j = 0; j < Nodes.Count; j++)
                    {
                        var connection = Connections.Where(con =>
                            con.NodeIdFrom == Nodes[i].Id &&
                            con.NodeIdTo == Nodes[j].Id).FirstOrDefault();
                        if(connection == null)
                            cap[i, j] = Double.NaN;
                        else
                            cap[i, j] = connection.Weight;
                    }
                return cap;
            }
        }
        public double[,] GetResult()
        {
            var cap = Capacity;

            for (int i = 0; i < Nodes.Count; i++)
                for (int j = 0; j < Nodes.Count; j++)
                    if(i != j && Double.IsNaN(cap[i, j]))
                        cap[i, j] = GetResultCapacity(i, j);
            return cap;
        }

        public double GetResultCapacity(int fromIdx, int toIdx)
        {
            //if (fromIdx == toIdx)
            //    return Double.NaN;

            //usedNodes = new List<Node>();
            //Node fromNode1 = Data.Keys.Where(k => k.Item1.Vertexes[0] == fromIdx).
            //    FirstOrDefault()?.Item1;
            //Node fromNode2 = Data.Keys.Where(k => k.Item2.Vertexes[0] == fromIdx).
            //    FirstOrDefault()?.Item2;
            //Node toNode1 = Data.Keys.Where(k => k.Item1.Vertexes[0] == toIdx).
            //    FirstOrDefault()?.Item1;
            //Node toNode2 = Data.Keys.Where(k => k.Item2.Vertexes[0] == toIdx).
            //    FirstOrDefault()?.Item2;

            //double currentCap = FoundCapacity(fromNode1 ?? fromNode2, toNode1 ?? toNode2, 0);
            //return currentCap;
            return -1;
        }

        //List<Node> usedNodes;

        //double FoundCapacity(Node curr, Node to, double currentCap)
        //{
        //    usedNodes.Add(curr);
        //    if (Data.TryGetValue(new Tuple<Node, Node>(curr, to), out double val))
        //    {
        //        return currentCap + val;
        //    }

        //    return Double.NaN;

        //    usedNodes.Remove(curr);
        //}


        public void InsertBetween(Node newNode, Node NodeFrom, Node NodeTo, double connectionWeight)
        {
            Nodes.Add(newNode);
            Connections.Add(new Connection
            {
                NodeIdFrom = newNode.Id,
                NodeIdTo = NodeTo.Id,
                Weight = connectionWeight
            });
            var con1 = Connections.Where(
                c => c.NodeIdFrom == NodeFrom.Id && c.NodeIdTo == NodeTo.Id).
                FirstOrDefault().NodeIdTo = newNode.Id;
        }

        public void Insert(Node newNode, Node NodeTo, double connectionWeight)
        {
            Nodes.Add(newNode);
            Connections.Add(new Connection
            {
                NodeIdFrom = newNode.Id,
                NodeIdTo = NodeTo.Id,
                Weight = connectionWeight
            });
        }



        public GomoriTree Clone()
        {
            //Connection[] c = new Connection[Connections.Count];
            var n = Nodes.Select(node => node.Vertexes).
                Select(v => new Node { Vertexes = v }).ToList();
            var conTmp = Connections.Select(con => 
                new
                {
                    FromNode = Nodes.Where(nFrom => nFrom.Id == con.NodeIdFrom).FirstOrDefault().ToString(),
                    ToNode = Nodes.Where(nTo => nTo.Id == con.NodeIdTo).FirstOrDefault().ToString(),
                    Con = con.Weight
                });
            var c = conTmp.Select(con =>
                new Connection {
                   NodeIdFrom = n.Where(nFrom => String.Equals(con.FromNode, nFrom.ToString())).FirstOrDefault().Id,
                   NodeIdTo = n.Where(nTo => String.Equals(con.ToNode, nTo.ToString())).FirstOrDefault().Id,
                   Weight = con.Con
                });

            return new GomoriTree()
            {
                Nodes = n,
                Connections = c.ToList()
            };
        }
    }
}