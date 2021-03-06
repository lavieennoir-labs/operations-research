﻿using Lab5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab5.Controllers
{
    public class FloydController : Controller
    {
        FloydSolver Solver { get; set; }

        public ActionResult Index(int? nodeCount, int? matrixSize, string matrixStr)
        {
            var input = new FloydInputModel();
            if (nodeCount == null || matrixStr == null || matrixSize == null)
            {
                input.NodeCount = 7;
                input.Matrix = new List<List<double?>>
                    {
                        new List<double?> {null, 0.8, 0.3, 0.65, null, null, null },
                        new List<double?> {null, null, null, 0.9, 0.5, null, null },
                        new List<double?> {null, null, null, null, null, 0.95, null },
                        new List<double?> {null, null, 0.85, null, 0.7, 0.6, null },
                        new List<double?> {null, null, null, null, null, 0.5, 0.8 },
                        new List<double?> {null, null, null, null, null, null, 0.9 },
                        new List<double?> {null, null, null, null, null, null, null }
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
            if (Session["Floyd"] == null || currentStage == null)
            {
                if (nodeCount == null || matrix == null)
                    return RedirectToAction("Index");

                Solver = new FloydSolver()
                {
                    Matrix = matrix.Select(row =>
                        row.Select(cell => cell ?? Double.NaN).ToArray()).
                        ToArray(),
                    NodeCount = (int)nodeCount,
                };

                Solver.Solve();
                Session["Floyd"] = Solver;
            }
            Solver = Session["Floyd"] as FloydSolver;
            var resultModel = new FloydResultModel
            {
                CurrentStage = currentStage ?? 0,
                PathTables = Solver.PathTables,
                DistTables = Solver.DistTables,
                StageCount = Solver.DistTables.Count,
                Matrix = matrix
            };

            return View(resultModel);
        }
    }
}