using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Lab5.Models
{
    /// <summary>
    /// Gomori tree vertex
    /// </summary>
    public class Vertex : ICloneable
    {
        public int Id = -1;

        public List<Vertex> Group = new List<Vertex>();
        public List<Edge> Edges = new List<Edge>();

        /// <summary>
        /// Indicates wether vertex is used in current path
        /// </summary>
        public bool PathFlag;
        public Vertex Parent;
        
        public object Clone()
        {
            Vertex[] group = new Vertex[Group.Count];
            Edge[] edges = new Edge[Edges.Count];
            Group.CopyTo(group);
            Edges.CopyTo(edges);
            return new Vertex
            {
                Id = Id,
                PathFlag = PathFlag,
                Parent = Parent == null ? null : (Vertex)Parent.Clone(),
                Group = group.ToList(),
                Edges = edges.ToList()
            };
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Group.Count == 0)
            {
                sb.Append(Id);
            }
            else
            {
                sb.Append("{ ");
                foreach (var v in Group)
                    sb.Append(v.ToString()).Append(", ");
                sb.Remove(sb.Length - 2, 1);
                sb.Append("}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns substract of list2 from list1
        /// </summary>
        public static List<Vertex> SetSubstract(List<Vertex> list1, List<Vertex> list2)
        {
            return list1.Where(v => !list2.Contains(v)).ToList();
        }

        /// <summary>
        /// Returns intersection of list1 and list2
        /// </summary>
        public static List<Vertex> SetIntersect(List<Vertex> list1, List<Vertex> list2)
        {
            return list2.Where(v => list1.Contains(v)).ToList();
        }

        /// <summary>
        /// Removes vertex rem from set
        /// </summary>
        public static void Remove(List<Vertex> set, Vertex rem)
        {
            int i = set.IndexOf(rem);
            if (i == -1)
                return;
            foreach (var e in set[i].Edges)
                Edge.Remove(e.ToVertex, set[i]);
            set.RemoveAt(i);
        }

        /// <summary>
        /// Removes all vertexes from set
        /// </summary>
        public static void RemoveAll(List<Vertex> set)
        {
            while (set.Count > 0)
                Vertex.Remove(set, set.First());
        }


        /// <summary>
        /// Returns list of vertexes where 
        /// Group of each vertex is inserted before it
        /// </summary>
        public static List<Vertex> GetGroups(List<Vertex> set)
        {
            List<Vertex> result = new List<Vertex>();
            foreach (Vertex v in set)
            {
                if (v.Group.Count == 0)
                    result.Add(v);
                else
                    foreach (Vertex inner in v.Group)
                        result.Add(inner);
            }
            return result;
        }
    }


    /// <summary>
    /// Gomori tree edge
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// Vertex to which end of Edge is connected
        /// </summary>
        public Vertex ToVertex;
        /// <summary>
        /// Edge weight
        /// </summary>
        public double C;
        public double Flow;

        /// <summary>
        /// Search for edge which connects v1 and v2 vertexes
        /// </summary>
        public static Edge Find(Vertex v1, Vertex v2)
        {
            return v1.Edges.Where(e => e.ToVertex == v2).FirstOrDefault();
        }

        /// <summary>
        /// Search for idx of edge in v1 edge list, which connects v1 and v2 vertexes
        /// </summary>
        public static int FindIdx(Vertex v1, Vertex v2)
        {
            return v1.Edges.IndexOf(v1.Edges.Where(e => e.ToVertex == v2)
                .FirstOrDefault() ?? new Edge());
        }

        /// <summary>
        /// Connects a pair of vertexes with new edge
        /// </summary>
        /// <param name="c">
        /// Weight of new edge
        /// </param>
        /// <param name="TwoWay">
        /// Indicates should be created edge 
        /// from v1 to v2 and back
        /// or only from v1 to v2
        /// </param>
        public static void Connect(Vertex v1, Vertex v2, double c, bool TwoWay = true)
        {
            int i = Edge.FindIdx(v1, v2);
            if (i == -1)
                v1.Edges.Add(new Edge { ToVertex = v2, C = c });
            else
                v1.Edges[i].C += c;

            if (TwoWay)
                Edge.Connect(v2, v1, c, TwoWay: false);
        }


        /// <summary>
        /// Removes edge between v1 and v2 vertexes
        /// </summary>
        public static void Remove(Vertex v1, Vertex v2)
        {
            int i = Edge.FindIdx(v1, v2);
            if (i != -1)
                v1.Edges.RemoveAt(i);
        }

    };

    /// <summary>
    /// Minimal cut
    /// </summary>
    public class MinCut : ICloneable
    {
        public double Flow;
        public Vertex S;
        public Vertex T;
        public List<Vertex> X = new List<Vertex>();
        public List<Vertex> Y = new List<Vertex>();

        public object Clone()
        {
            Vertex[] x = new Vertex[X.Count];
            Vertex[] y = new Vertex[Y.Count];
            X.CopyTo(x);
            Y.CopyTo(y);
            return new MinCut
            {
                Flow = Flow,
                S = S == null ? null : (Vertex)S.Clone(),
                T = T == null ? null : (Vertex)T.Clone(),
                X = x.ToList(),
                Y = y.ToList()
            };
        }
    };

    public class GomoriHuSolver
    {
        public List<Vertex> CombinedVertexes;
        public List<MinCut> MinCuts;
        public List<double[][]> Matrixes;

        public double[][] CurrentMatrix
        {
            get => Matrixes.Last();
            set => Matrixes[Matrixes.Count - 1] = value;
        }
        
        public List<List<Vertex>> FlowMarixNodesList;
        public List<double[,]> FlowMarixesList
        {
            get => FlowMarixNodesList.Select(
                nodeList => GetMatrix(nodeList)).ToList();
        }
        public List<List<Vertex>> TreeMarixNodesList;
        public List<double[,]> TreeMarixesList
        {
            get => TreeMarixNodesList.Select(
                nodeList => GetMatrix(nodeList)).ToList();
        }

        public double[][] Matrix; //input matrix
        public int NodeCount;
        

        public void Init()
        {
            Matrixes = new List<double[][]>();
            Matrixes.Add(Matrix);

            CombinedVertexes = new List<Vertex>();
            FlowMarixNodesList = new List<List<Vertex>>();
            MinCuts = new List<MinCut>();
            TreeMarixNodesList = new List<List<Vertex>>();
        }

        public void Solve()
        {
            Init();

            Vertex[] temp;
            List<Vertex> vertTree = new List<Vertex>();
            List<Vertex> vertGroup = new List<Vertex>();
            for (int i = 0; i < NodeCount; i++)
            {
                vertGroup.Add(new Vertex { Id = i });
                for (int j = 0; j < i; j++)
                    if (!Double.IsNaN(CurrentMatrix[i][j]))
                        Edge.Connect(vertGroup[i], vertGroup[j], CurrentMatrix[i][j]);
            }
            vertTree.Add(new Vertex { Group = vertGroup, Id = -1 });



            for (int i = 0; i < NodeCount - 1; i++)
            {
                //finish when no more combined vertexes left
                int combinedVertexIdx = 0;
                while (combinedVertexIdx < vertTree.Count 
                    && vertTree[combinedVertexIdx].Group.Count < 2)
                        combinedVertexIdx++;
                if (combinedVertexIdx == vertTree.Count) break;

                Vertex combinedVertex = vertTree[combinedVertexIdx];
                CombinedVertexes.Add((Vertex)combinedVertex.Clone());

                List<Vertex> FlowMatrixNodes = new List<Vertex>();
                foreach (Vertex v in combinedVertex.Group)
                {
                    v.Parent = new Vertex { Group = new List<Vertex> { v }, Id = -1 };
                    FlowMatrixNodes.Add(v.Parent);
                }

                //update connections with combinedVertex
                // take branch of vertexes together
                foreach (Edge e in combinedVertex.Edges)
                {
                    Edge.Remove(e.ToVertex, combinedVertex);
                    Vertex branch = new Vertex
                    {
                        Group = Vertex.GetGroups(
                            FindPath(vertTree, e.ToVertex, combinedVertex)),
                        Id = -1
                    };

                    Edge.Connect(e.ToVertex, combinedVertex, e.C, TwoWay: false);

                    foreach (Vertex v in branch.Group)
                        v.Parent = branch;
                    FlowMatrixNodes.Add(branch);
                }

                //check connections between nodes in flow matrix
                foreach (Vertex vertex in FlowMatrixNodes)
                    foreach (Vertex subVertex in vertex.Group)
                        foreach (Edge e in subVertex.Edges)
                        {
                            if (vertex != e.ToVertex.Parent)
                                Edge.Connect(vertex, e.ToVertex.Parent, e.C, TwoWay: false);
                        }

                //save FlowMatrixNodes
                temp = new Vertex[FlowMatrixNodes.Count];
                FlowMatrixNodes.CopyTo(temp);
                FlowMarixNodesList.Add(temp.ToList());

                // get minimal cut and expand vertex groups
                MinCut cut = GetMinCut(FlowMatrixNodes);
                cut.X = Vertex.GetGroups(cut.X);
                cut.Y = Vertex.GetGroups(cut.Y);
                MinCuts.Add((MinCut)cut.Clone());

                Vertex.RemoveAll(FlowMatrixNodes);

                //update max flow through cut
                Vertex XCombined = new Vertex {
                    Group = Vertex.SetIntersect(combinedVertex.Group, cut.X),
                    Id = -1
                };
                Vertex YCombined = new Vertex {
                    Group = Vertex.SetIntersect(combinedVertex.Group, cut.Y),
                    Id = -1
                };
                Edge.Connect(XCombined, YCombined, cut.Flow);
                //copy edges for both X and Y part of combined vertex
                foreach(Edge e in combinedVertex.Edges)        
                {
                    Vertex v = e.ToVertex;
                    Edge.Connect(
                        (cut.X.Contains(v.Group[0]) ? XCombined : YCombined),
                        v, 
                        e.C);
                }
                //replace combined veretex in tree by 2 sub vertexes
                vertTree.Insert(combinedVertexIdx, YCombined);
                vertTree.Insert(combinedVertexIdx, XCombined);
                Vertex.Remove(vertTree, combinedVertex);

                //save vertex tree
                temp = new Vertex[vertTree.Count];
                vertTree.CopyTo(temp);
                TreeMarixNodesList.Add(temp.ToList());
            }

            //show result

            foreach(Vertex v in vertTree)
            {
                v.Id = v.Group[0].Id;
                v.Group.Clear();
            }

            OrderListForResult(vertTree);
            //save vertex tree
            temp = new Vertex[vertTree.Count];
            vertTree.CopyTo(temp);
            TreeMarixNodesList.Add(temp.ToList());
            
            //Vertex.RemoveAll(vertGroup);
            //Vertex.RemoveAll(vertTree);
        }


        /// <summary>
        /// Search for path from s to t
        /// </summary>
        /// <param name="s">
        /// Start vertex
        /// </param>
        /// <param name="t">
        /// End vertex
        /// </param>
        /// <returns>
        /// List of vertexes of path
        /// </returns>
        public static List<Vertex> FindPath(List<Vertex> set, Vertex s, Vertex t)
        {
            foreach (Vertex v in set)
            {
                v.Parent = null;
                v.PathFlag = false;
            }
            List<Vertex> path = new List<Vertex>();
            s.PathFlag = true;
	        path.Add(s);

	        for(int i = 0; i < path.Count; i++)
		        foreach(Edge e in path[i].Edges)
                {
                    Vertex v = e.ToVertex;
                    if (v.PathFlag == false && e.C > 0)
                    {
                        v.Parent = path[i];
                        v.PathFlag = true;
                        path.Add(v);
                        if (v == t)
                            return path;
                    }
                }
	        return path;
        }

        public MinCut GetMinCut(List<Vertex> set)
        {
            MinCut result = new MinCut
            {
                S = set[0],
                T = set[1]
            };

            while(true)
            {
                List<Vertex> path = FindPath(set, result.S, result.T);
                if (result.T.Parent == null)
                    break;

                double minEdgeC = int.MaxValue;
                List<Edge> pathEdges = new List<Edge>();

                //go through path from t to s 
                //find min C to increase Flow
                Vertex v = result.T;
                while (v.Parent != null)
                {
                    //remember passed edges
                    pathEdges.Add(v.Edges[Edge.FindIdx(v, v.Parent)]);
                    Edge edgeBack = Edge.Find(v.Parent, v);
                    pathEdges.Add(edgeBack);

                    if (edgeBack.C < minEdgeC)
                        minEdgeC = edgeBack.C;
                    v = v.Parent;
                }
                result.Flow += minEdgeC;
                //increase Flow and decrease weight
                foreach (Edge e in pathEdges)
                {
                    e.Flow += minEdgeC;
                    e.C -= minEdgeC;
                }
            }

            //get sets are cutted fom input set
            result.Y = FindPath(set, result.T, result.S);
            result.X = Vertex.SetSubstract(set, result.Y);
            //restore C back
            foreach (Vertex v in set)
                foreach (Edge e in v.Edges)
                {
                    e.C += e.Flow;
                    e.Flow = 0;
                }
            return result;
        }


        public static double[,] GetMatrix(List<Vertex> set)
        {
            double[,] result = new double[set.Count, set.Count];

            for (int i = 0; i < set.Count; i++)
            {
                foreach (Edge e in set[i].Edges)
                {
                    int toVertIdx = set.IndexOf(e.ToVertex);
                    if(toVertIdx == -1)
                    {
                        toVertIdx = set.IndexOf(
                            set.Where(v => v.Group.
                            Select(sv => sv.Id).Contains(e.ToVertex.Id)).
                            FirstOrDefault());
                    }
                    result[i, toVertIdx] = e.C;
                    result[toVertIdx, i] = e.C;
                }
            }
            //search other path
            for(int i = 0; i < set.Count; i++)
                for(int j = 0; j < set.Count; j++)
                    if(result[i, j].Equals(0) && i != j)
                    {
                        var path = FindPath(set, set[i], set[j]);
                        var v = (Vertex)set[j].Clone();
                        var flow = Double.PositiveInfinity;
                        while (v.Parent != null)
                        {
                            flow = Math.Min(flow, 
                                v.Parent.Edges.SingleOrDefault(e => e.ToVertex.ToString() == v.ToString()).C);

                            v = v.Parent;
                        }
                        result[i, j] = flow;
                    }

            return result;

        }

        /// <summary>
        /// Sorts set of vertexes by their id value
        /// </summary>
        public static void OrderListForResult(List<Vertex> set)
        {
            for (int i = 1; i < set.Count; i++)
                for (int j = 1; j <= i; j++)
                    if (set[j].Id < set[j - 1].Id)
                    {
                        var tmp = set[j];
                        set[j] = set[j - 1];
                        set[j - 1] = tmp;
                    }
        }

    }
}