using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Model
{
    class PotentialCounter
    {
        int RawCount;
        int NeedCount;

        double[] rawPotential;
        double[] needPotential;
        double[,] cost;
        double[,] count;

        public void CountPotentials(
            out double[] RawPotential, out double[] NeedPotential,
            double[,] Cost, double[,] Count, int BaseRawPotentialIdx)
        {
            //init data
            RawCount = Cost.GetLength(0);
            NeedCount = Cost.GetLength(1);
            rawPotential = new double[RawCount];
            needPotential = new double[NeedCount];
            for (int i = 0; i < RawCount; i++)
                rawPotential[i] = Double.NaN;
            for (int i = 0; i < NeedCount; i++)
                needPotential[i] = Double.NaN;
            rawPotential[BaseRawPotentialIdx] = 0;
            cost = Cost;
            count = Count;


            while (rawPotential.Contains(Double.NaN) || needPotential.Contains(Double.NaN))
            {
                UpdateNeedPotential();
                UpdateRawPotential();
            }
            
            RawPotential = rawPotential;
            NeedPotential = needPotential;
        }

        void UpdateRawPotential()
        {
            for (int i = 0; i < RawCount; i++)
                for (int j = 0; j < NeedCount; j++)
                    if (!count[i, j].Equals(Double.NaN) && 
                        !needPotential[j].Equals(Double.NaN) &&
                        rawPotential[i].Equals(Double.NaN))
                        rawPotential[i] = cost[i, j] - needPotential[j];
        }

        void UpdateNeedPotential()
        {
            for(int i = 0; i < RawCount; i++)
                for(int j = 0; j < NeedCount; j++)
                    if (!count[i, j].Equals(Double.NaN) &&
                        !rawPotential[i].Equals(Double.NaN) &&
                        needPotential[j].Equals(Double.NaN))
                        needPotential[j] = cost[i, j] - rawPotential[i];
        }
    }
}
