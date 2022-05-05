using SecurityLibrary.AES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.ElGamal
{
    public class ElGamal
    {
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="q"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        /// <returns>list[0] = C1, List[1] = C2</returns>
        public List<long> Encrypt(int q, int alpha, int y, int k, int m)
        {
            List<long> msgs = new List<long>();

            long c1 = getPowMod(alpha, k, q);
            long K = getPowMod(y, k, q);
            long c2 = (K * m) % q;
            msgs.Add(c1);
            msgs.Add(c2);

            return msgs;
        }

        public int Decrypt(int c1, int c2, int x, int q)
        {
            int  K = getPowMod(c1, x, q);
            ExtendedEuclid obj = new ExtendedEuclid();
            int KInverse = obj.GetMultiplicativeInverse(K, q);
            int m = (c2 * KInverse) % q;

            return m;
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
