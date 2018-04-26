using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Lab5.Models.GomoriTree;

namespace Lab5.Models
{
    /// <summary>
    /// Deprecated
    /// </summary>
    public class GomoriSolver
    {
        public List<GomoriTree> TreeList;
        public List<int> InNodeList;
        public List<int> OutNodeList;

        public double[][] Matrix;
        public int NodeCount;


        public int InNode
        {
            get => InNodeList.Last();
            set => InNodeList[InNodeList.Count - 1] = value;
        }
        public int OutNode
        {
            get => OutNodeList.Last();
            set => OutNodeList[OutNodeList.Count - 1] = value;
        }

        public GomoriTree CurrentTree
        {
            get => TreeList.Last();
            set => TreeList[TreeList.Count - 1] = value;
        }

        public double[,] CapacityTable { get; set; }
        

        public void Solve()
        {
            Init();


            for(int l = 0; l < NodeCount - 1; l++) //for each vertex
            {
                SplitGraph();
                SetNodePair();

                TreeList.Add(CurrentTree.Clone());
            }
            CapacityTable = CurrentTree.GetResult();
            
        }

        void SplitGraph()
        {
            //get out node capacity
            var outCap = Matrix[OutNode].Where(i => !Double.IsNaN(i)).Sum();

            //if first iteration
            if (CurrentTree.Nodes.Count == 0)
            {
                //create combined node
                List<int> vert = Enumerable.Range(0, NodeCount).ToList();
                vert.Remove(OutNode);
                CurrentTree.Nodes.Add(new GomoriTree.Node
                {
                    Vertexes = vert
                });
            }
            else
            {
                //edit combined node
                CurrentTree.Nodes[0].Vertexes.Remove(OutNode);
            }
            if(OutNodeList.Count < 2)
            {
                //Add new single node
                CurrentTree.Insert(new GomoriTree.Node
                {
                    Vertexes = new List<int> { OutNode } //new node
                },
                CurrentTree.Nodes[0], outCap);// combined node
            }
            else
            {
                var prevOutIdx = OutNodeList[OutNodeList.Count - 2];
                if (GetBranchCapacity(prevOutIdx) + GetCapacity(OutNode) //TODO: Count as branch
                        - 2 * Matrix[prevOutIdx][OutNode] < GetBranchCapacity(prevOutIdx))
                    //if can insert node to current branch
                {
                    //Add new single node
                    CurrentTree.Insert(new GomoriTree.Node
                    {
                        Vertexes = new List<int> { OutNode } //new node
                    },
                    CurrentTree.Nodes[0], outCap);// combined node
                }
                else
                {
                    CurrentTree.InsertBetween(new GomoriTree.Node
                    {
                        Vertexes = new List<int> { OutNode }//new node
                    },
                    CurrentTree.Nodes[CurrentTree.Nodes.Count - 1], //last added node
                    CurrentTree.Nodes[0],
                    GetBranchCapacity(prevOutIdx) + GetCapacity(OutNode)
                        - 2 * Matrix[prevOutIdx][OutNode]); // combined node
                }
            }
            
        }
        
        void Init()
        {
            TreeList = new List<GomoriTree>();
            TreeList.Add(new GomoriTree());

            //select fisrt nodes
            InNodeList = new List<int>();
            InNodeList.Add(1);

            OutNodeList = new List<int>();
            OutNodeList.Add(0);
        }

        void SetNodePair()
        {
            InNodeList.Add(InNode);
            OutNodeList.Add(OutNode);

            OutNode = InNode;

            double maxVal = Double.NegativeInfinity;
            int maxIdx = -1;
            for(int i = 0; i < NodeCount; i++)
                if(!Double.IsNaN(Matrix[OutNode][i]) &&
                    CurrentTree.Nodes[0].Vertexes.Contains(i) &&
                    Matrix[OutNode][i] > maxVal)
                {
                    maxVal = Matrix[OutNode][i];
                    maxIdx = i;
                }
            InNode = maxIdx; //val from combined node where edge weight is max
        }

        public double GetCapacity(int vertIdx)
        {
            double sum = 0;
                for(int j = 0; j < NodeCount; j++)
                    if(!Double.IsNaN(Matrix[vertIdx][j]))
                        sum += Matrix[vertIdx][j];
            return sum;
        }

        public double GetBranchCapacity(int lastVertIdx)
        {
            double sum = 0;

            //get all connections of branch
            var branchNodes = new List<Node>();
            var currentNode = CurrentTree.Nodes.
                       Where(n => n.Vertexes.Contains(lastVertIdx)).FirstOrDefault();
            while(currentNode != null)
            {
                branchNodes.Add(currentNode);

                var currentCon = CurrentTree.Connections.Where(con =>
                       con.NodeIdTo == currentNode.Id).FirstOrDefault();
                if (currentCon == null)
                    break; //end of branch

                currentNode = CurrentTree.Nodes.
                       Where(n => n.Id == currentCon.NodeIdFrom).FirstOrDefault();
            }
            //enumerate through branch
            var vertexes = branchNodes.Select(n => n.Vertexes[0]);
            foreach (var idx in vertexes)
            {
                for (int i = 0; i < NodeCount; i++)
                    if (!Double.IsNaN(Matrix[idx][i]) && !vertexes.Contains(i))
                        sum += Matrix[idx][i];
            }

            return sum;
        }
    }
}