using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DiffieHellman
{
    public class DiffieHellman 
    {
        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            List<int> secretKeys = new List<int>();

            int Ya = getPowMod(alpha, xa, q);
            int Yb = getPowMod(alpha, xb, q);

            int K1 = getPowMod(Yb, xa, q);
            int K2 = getPowMod(Ya, xb, q);

            secretKeys.Add(K1);
            secretKeys.Add(K2);

            return secretKeys;
        }


        #region HELPERS
        private int getPowMod(int msg, int k, int n)
        {
            int newMsg = 1;
            while (k >= 1)
            {
                newMsg = (newMsg * msg) % n;
                k--;
            }
            newMsg %= n;

            return newMsg;
        }
        #endregion
    }
}
