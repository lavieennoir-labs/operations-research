using Lab1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Model
{
    class CanonicalFormConverter
    {
        public CanonicalFormViewModel Convert(InputViewModel input)
        {
            //create matrix for holding equastion members
            double[,] matrix = new double[input.RawCount + 1, input.RawCount + input.ProductCount  + 1];

            //fill coefs
            for (int i = 0; i < input.RawCount; i++)
                for (int j = 0; j < input.ProductCount; j++)
                    matrix[i, j] = input.ProductCost[i][j];
            //fill extra variables
            for (int i = 0; i < input.RawCount; i++)
                matrix[i, i + input.ProductCount] = 1;
            //fill profit coefs
            for (int i = 0; i < input.ProductCount; i++)
                matrix[input.RawCount, i] = -input.Products[i].Value;
            //take last koef as F
            matrix[input.RawCount, input.RawCount + input.ProductCount] = 1;



            //create matrix for holding free members
            double[] freeMembers = new double[input.RawCount + 1];
            //fill free members
            for (int i = 0; i < input.RawCount; i++)
                freeMembers[i] = input.Raw[i].Value;


            return new CanonicalFormViewModel
            {
                Matrix = matrix,
                FreeMembers = freeMembers,
                Equastions = GenerateEquastions(matrix, freeMembers)
            };
        }

        List<string> GenerateEquastions(double[,] a, double[] b)
        {
            List<string> equastions = new List<string>();

            for(int i = 0; i < a.GetLength(0); i++)
            {
                StringBuilder sb = new StringBuilder();
                //process last coef F separatly
                if (a[i, a.GetLength(1) - 1] != 0)
                {
                    if(a[i, a.GetLength(1) - 1] != 1)
                        sb.Append(a[i, a.GetLength(1) - 1]).Append("*");
                    sb.Append("F ");
                }

                for (int j = 0; j < a.GetLength(1)-1; j++)
                {
                    if (a[i, j] == 0) continue;

                    //first sign check
                    if (sb.Length != 0 && a[i, j] > 0)
                        sb.Append("+");
                    //check member equals 1 and append members
                    if (a[i, j] != 1)
                        sb.Append(a[i, j]).Append("*");
                    sb.Append("X").Append(j + 1).Append(" ");
                }

                if (sb.Length == 0)
                    sb.Append("0");
                sb.Append(" = ").Append(b[i]);

                equastions.Add(sb.ToString());
            }

            return equastions;
        }
    }
}
