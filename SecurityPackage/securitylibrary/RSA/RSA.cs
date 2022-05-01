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
            BigInteger encrypted = 1;
            int n = p * q;

            while (e > 3)
            {
                encrypted *= (int)(Math.Pow(M, 3) % n);
                e -= 3;
            }

            encrypted *= (int)(Math.Pow(M, e) % n);
            encrypted %= n;

            return (int)encrypted;

        }

        public int Decrypt(int p, int q, int C, int e)
        {
            BigInteger decrypted = 1;
            int n = p * q;
            int phayN = (p - 1) * (q - 1);
            ExtendedEuclid obj = new ExtendedEuclid();
            int d = obj.GetMultiplicativeInverse(e, phayN);


            while (d > 3)
            {
                decrypted *= (int)(Math.Pow(C, 3) % n);
                d -= 3;
            }
            decrypted *= (int)(Math.Pow(C, d) % n);
            decrypted %= n;

            return (int)decrypted;
        }
    }
}
