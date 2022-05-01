using SecurityLibrary.AES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
        public int Encrypt(int p, int q, int M, int e)
        {
            int n = p * q;

            return getPowMod(M, e, n);
        }

        public int Decrypt(int p, int q, int C, int e)
        {
            int n = p * q;
            int phayN = (p - 1) * (q - 1);
            ExtendedEuclid obj = new ExtendedEuclid();
            int d = obj.GetMultiplicativeInverse(e, phayN);

            return getPowMod(C, d, n);
        }


        #region HELPERS
        private int getPowMod(int msg, int k, int n)
        {
            BigInteger newMsg = 1;
            while (k > 3)
            {
                newMsg *= (int)(Math.Pow(msg, 3) % n);
                k -= 3;
            }
            newMsg *= (int)(Math.Pow(msg, k) % n);
            newMsg %= n;

            return (int)newMsg;
        }
        #endregion
    }
}
