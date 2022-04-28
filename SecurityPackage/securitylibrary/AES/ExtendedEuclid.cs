using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class ExtendedEuclid 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="baseN"></param>
        /// <returns>Mul inverse, -1 if no inv</returns>
        public int GetMultiplicativeInverse(int number, int baseN)
        {
            List<int> p = new List<int>();
            List<int> helperList = new List<int>();
            List<List<int>> aTable = new List<List<int>>();
            List<List<int>> bTable = new List<List<int>>();

            helperList.Add(1);
            helperList.Add(0);
            helperList.Add(baseN);
            aTable.Add(new List<int>(helperList));
            helperList.Clear();

            helperList.Add(0);
            helperList.Add(1);
            helperList.Add(number);
            bTable.Add(new List<int>(helperList));

            int k = 0;
            while (bTable[k][2] != 0 && bTable[k][2] != 1)
            {
                p.Add(aTable[k][2] / bTable[k][2]);

                for (int i = 0; i < 3; i++)
                {
                    helperList.Add(bTable[k][i]);
                }
                aTable.Add(new List<int>(helperList));
                helperList.Clear();

                for (int i = 0; i < 3; i++)
                {
                    helperList.Add(aTable[k][i] - p[k] * bTable[k][i]);
                }
                bTable.Add(new List<int>(helperList));
                helperList.Clear();
                k++;
            }
            if (bTable[k][2] == 1)
            {
                if (bTable[k][1] < 0)
                {
                    while (bTable[k][1] < 0)
                    {
                        bTable[k][1] += baseN;
                    }
                }
                return bTable[k][1];
            }

            return -1;
        }

        /*public static void Main()
        {
        }*/
    }
}
