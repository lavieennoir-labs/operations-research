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
            double[,] matrix = new double[input.LimitCount + 1, input.LimitCount + input.VarCount  + 1];
            //check signs
            var inversedRows = new List<int>();
            for (int i = 0; i < input.LimitCount; i++)
                if(input.Limits[i].Sign == ">=")
                {
                    inversedRows.Add(i);
                    input.Limits[i].Value *= -1;
                    input.Limits[i].Sign = "<=";
                }

            //fill coefs
            for (int i = 0; i < input.LimitCount; i++)
                for (int j = 0; j < input.VarCount; j++)
                    if(inversedRows.Contains(i))
                        matrix[i, j] = -input.Coefs[i][j];
                    else
                        matrix[i, j] = input.Coefs[i][j];
            //fill extra variables
            for (int i = 0; i < input.LimitCount; i++)
                matrix[i, i + input.VarCount] = 1;//inversedRows.Contains(i) ? -1 : 1;
            //fill profit coefs
            for (int i = 0; i < input.VarCount; i++)
                matrix[input.LimitCount, i] = -input.Vars[i].Value;
            //take last koef as F
            matrix[input.LimitCount, input.LimitCount + input.VarCount] = 1;



            //create matrix for holding free members
            double[] freeMembers = new double[input.LimitCount + 1];
            //fill free members
            for (int i = 0; i < input.LimitCount; i++)
                freeMembers[i] = input.Limits[i].Value;
            
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
