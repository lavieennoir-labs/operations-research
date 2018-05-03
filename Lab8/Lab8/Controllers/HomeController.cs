using Lab8.Models;
using Lab8.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab8.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            var input = MyInput;
            return View(input);
        }

        [HttpPost]
        public ActionResult Index(InputModel input)
        {
            if (input.Matrix == null)
            {
                return RedirectToAction("Index");
            }
            else
                input = new InputModelResizer().
                    Resize(
                    input, 
                    input.MatrixWidthUpdated, 
                    input.MatrixHeightUpdated);
            return View(input);            
        }

        [HttpPost]
        public ActionResult Result(InputModel input)
        {
            if (input.Matrix == null)
                return RedirectToAction("Index", new { input });
            var solver = new GameTaskSolver
            {
                CurrentInput = input
            };
            solver.Solve();

            return View(solver.Result);
        }


        InputModel MyInput = new InputModel
        {
            MatrixHeight = 3,
            MatrixHeightUpdated = 3,
            MatrixWidth = 4,
            MatrixWidthUpdated = 4,
            Matrix = new double?[][]
                    {
                        new double?[] { 11, 7, 8, 1 },
                        new double?[] { 6, 2, 11, 9 },
                        new double?[] { 4, 1, 3, 2 }
                    }
        };
        InputModel TestInput = new InputModel
        {
            MatrixHeight = 4,
            MatrixHeightUpdated = 4,
            MatrixWidth = 5,
            MatrixWidthUpdated = 5,
            Matrix = new double?[][]
                    {
                        new double?[] { 1, 4, 6, 3, 7 },
                        new double?[] { 3, 1, 2, 4, 3 },
                        new double?[] { 2, 3, 4, 3, 5 },
                        new double?[] { 0, 1, 5, 2, 6 }
                    }
        };
    }
}