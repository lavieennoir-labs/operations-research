using Lab8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab8.Services
{
    public class SymplexSolver
    {
        public List<SymplexTable> SymplexTables { get; set; } = new List<SymplexTable>();
        
        /// <summary>
        /// Iterates through plans looking for the optimal one
        /// </summary>
        /// <returns>Wether task can be solved</returns>
        public bool Solve(CanonicalFormSymplexInput canonicalForm)
        {
            try
            {
                var firstTable = SymplexTable.GetFromCanonicalForm(canonicalForm);
                SymplexTables.Clear();

                var table = firstTable.Clone();
                while (!table.IsOptimalPlan())
                {
                    SymplexTables.Add(table);
                    table = table.GetNextPlan();
                }
                //add optimal table to end
                for (int i = 0; i < table.MarkingRelations.Length; i++)
                    table.MarkingRelations[i] = Double.PositiveInfinity;
                SymplexTables.Add(table);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            return true;
        }

        public List<SymplexTableModel> GetSymplexTableModels()
        {
            return SymplexTables.Select((st, i) =>
            {
                var model = st.GetTableModel("X");
                model.Num = i + 1;
                return model;
            }).ToList();
        }
    }
}