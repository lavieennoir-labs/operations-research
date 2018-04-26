using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab5.Models
{
    public class GomoriResultModel
    {
        public int StageCount { get; set; }
        public int CurrentStage { get; set; }
        
        public double?[][] Matrix { get; set; }

        public List<Vertex> CombinedVertexes;
        public Vertex CurrentCombinedVertex
        {
            get => CombinedVertexes[CurrentStage];
            set => CombinedVertexes[CurrentStage] = value;
        }

        public List<MinCut> MinCuts;
        public MinCut CurrentMinCut
        {
            get => MinCuts[CurrentStage];
            set => MinCuts[CurrentStage] = value;
        }

        public List<double[][]> Matrixes;
        public double[][] CurrentMatrix
        {
            get => Matrixes[CurrentStage];
            set => Matrixes[CurrentStage] = value;
        }

        public List<List<Vertex>> FlowMarixNodesList;
        public List<Vertex> CurrentFlowMarixNodes
        {
            get => FlowMarixNodesList[CurrentStage];
            set => FlowMarixNodesList[CurrentStage] = value;
        }
        public List<double[,]> FlowMarixesList;
        public double[,] CurrentFlowMarixes
        {
            get => FlowMarixesList[CurrentStage];
            set => FlowMarixesList[CurrentStage] = value;
        }
        public List<List<Vertex>> TreeMarixNodesList;
        public List<Vertex> CurrentTreeMarixNodes
        {
            get => TreeMarixNodesList[CurrentStage];
            set => TreeMarixNodesList[CurrentStage] = value;
        }
        public List<double[,]> TreeMarixesList;
        public double[,] CurrentTreeMarixes
        {
            get => TreeMarixesList[CurrentStage];
            set => TreeMarixesList[CurrentStage] = value;
        }

    }
}