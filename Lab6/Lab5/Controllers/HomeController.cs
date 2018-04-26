using Lab5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab5.Controllers
{
    public class HomeController : Controller
    {
        GomoriHuSolver Solver { get; set; }
         
        public ActionResult Index(int? nodeCount, int? matrixSize, string matrixStr)
        {
            var input = new GomoriInputModel();
            if (nodeCount == null || matrixStr == null || matrixSize == null)
            {
                input.StartIdx = 1;
                input.NodeCount = 7;
                input.Matrix = new List<List<double?>>
                    {   //                 a     b     d     e     f     g     h
                        new List<double?> {null, 5,    5,    3,    8,    null, null }, //a
                        new List<double?> {5,    null, null, 9,    null, null, null }, //b
                        new List<double?> {5,    null, null, null, 7,    6,    6    }, //d
                        new List<double?> {3,    9,    null, null, 4,    null, 2    }, //e
                        new List<double?> {8,    null, 7,    4,    null, null, null }, //f
                        new List<double?> {null, null, 6,    null, null, null, 6  }, //g
                        new List<double?> {null, null, 6,    2,    null, 6,    null }  //h
                    };
            }
            else
            {
                var dataArr = matrixStr.Split('|');
                input.NodeCount = (int)matrixSize;
                input.Matrix = new List<List<double?>>();
                for (int i = 0; i < matrixSize; i++)
                {
                    var row = new List<double?>();
                    for (int j = 0; j < matrixSize; j++)
                        row.Add(
                            Double.TryParse(dataArr[i * (int)matrixSize + j], out double val) ? 
                            (double?)val : null);
                    input.Matrix.Add(row);
                }
            }

            if (nodeCount < 2)
                nodeCount = 2;
            else if (nodeCount > 30)
                nodeCount = 30;

            //update matrix
            if (nodeCount > input.NodeCount)
            {
                for (int i = 0; i < input.NodeCount; i++)
                    input.Matrix[i].AddRange(
                        Enumerable.Repeat((double?)null,
                        (int)nodeCount - input.NodeCount));

                input.Matrix.AddRange(
                    Enumerable.Repeat(
                        Enumerable.Repeat((double?)null,
                        (int)nodeCount).ToList(),
                    (int)nodeCount - input.NodeCount));
            }
            else if (nodeCount < input.NodeCount)
            {
                input.Matrix.RemoveRange((int)nodeCount,
                         input.NodeCount - (int)nodeCount);
                for (int i = 0; i < input.Matrix.Count; i++)
                    input.Matrix[i].RemoveRange((int)nodeCount,
                        input.NodeCount - (int)nodeCount);
            }

            input.MatrixSize = input.NodeCount;
            return View(input);
        }
        
        
        public ActionResult Result(int? nodeCount, double?[][] matrix, int? currentStage)
        {
            if (Session["Gomori"] == null || currentStage == null)
            {
                if (nodeCount == null || matrix == null)
                    return RedirectToAction("Index");

                Solver = new GomoriHuSolver()
                {
                    Matrix = matrix.Select(row =>
                        row.Select(cell => cell ?? Double.NaN).ToArray()).
                        ToArray(),
                    NodeCount = (int)nodeCount
                };

                Solver.Solve();
                Session["Gomori"] = Solver;
            }
            Solver = Session["Gomori"] as GomoriHuSolver;
            var resultModel = new GomoriResultModel
            {
                CombinedVertexes = Solver.CombinedVertexes,
                MinCuts = Solver.MinCuts,
                Matrixes = Solver.Matrixes,
                FlowMarixesList = Solver.FlowMarixesList,
                FlowMarixNodesList = Solver.FlowMarixNodesList,
                TreeMarixesList = Solver.TreeMarixesList,
                TreeMarixNodesList = Solver.TreeMarixNodesList,

                StageCount = Solver.NodeCount - 1,
                CurrentStage = currentStage ?? 0,
                Matrix = matrix
            };

            return View(resultModel);
        }
    }
}