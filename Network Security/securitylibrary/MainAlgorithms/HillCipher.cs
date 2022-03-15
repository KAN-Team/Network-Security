using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher :  ICryptographicTechnique<List<int>, List<int>>
    {
        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            // **** return cipher as an indexed letters **** //
            return doEncryption(plainText, key);
        }

        public string Encrypt(string plainText, string key)
        {
            // **** this method returns cipher as a string **** //
            // =============================================== //
            List<int> plainNumeric = new List<int>(new int[plainText.Length]);
            List<int> keyNumeric = new List<int>(new int[key.Length]);
            plainText = plainText.ToLower();
            key = key.ToLower();
            // =============================================== //

            // ======= Convert from letters to numbers ======= //
            for (int i = 0; i < plainText.Length; ++i)
                plainNumeric[i] = plainText[i] - 'a';

            for (int i = 0; i < key.Length; ++i)
                keyNumeric[i] = key[i] - 'a';
            // =============================================== //

            List<int> cipherNumeric = doEncryption(plainNumeric, keyNumeric);

            // ======= Convert from numbers to letters ======= //
            StringBuilder cipherText = new StringBuilder(cipherNumeric.Count + 1);
            for (int i = 0; i < cipherNumeric.Count; ++i)
                cipherText.Append((char)('a' + cipherNumeric[i]));
            // =============================================== //

            return cipherText.ToString();
        }

        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            // **** return plain as an indexed letters **** //
            return doDecryption(cipherText, key);
        }

        public string Decrypt(string cipherText, string key)
        {
            // **** this method returns plain as a string **** //
            // =============================================== //
            List<int> cipherNumeric = new List<int>(new int[cipherText.Length]);
            List<int> keyNumeric = new List<int>(new int[key.Length]);
            cipherText = cipherText.ToLower();
            key = key.ToLower();
            // =============================================== //

            // ======= Convert from letters to numbers ======= //
            for (int i = 0; i < cipherText.Length; ++i)
                cipherNumeric[i] = cipherText[i] - 'a';

            for (int i = 0; i < key.Length; ++i)
                keyNumeric[i] = key[i] - 'a';
            // =============================================== //

            List<int> plainNumeric = doDecryption(cipherNumeric, keyNumeric);

            // ======= Convert from numbers to letters ======= //
            StringBuilder plainText = new StringBuilder(plainNumeric.Count + 1);
            for (int i = 0; i < plainNumeric.Count; ++i)
                plainText.Append((char)('a' + plainNumeric[i]));
            // =============================================== //

            return plainText.ToString();
        }

        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();
        }

        public string Analyse(string plainText, string key)
        {
            throw new NotImplementedException();
        }

        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();
        }

        public string Analyse3By3Key(string plainText, string key)
        {
            throw new NotImplementedException();
        }

        #region HELPERS
        private List<int> doEncryption(List<int> plainText, List<int> key)
        {
            /*
             * Instead of converting to 2D matrices and appling dot product, 
             * we worked on the row based arrays as they are.
             */
            List<int> cipherText = new List<int>(new int[plainText.Count]);
            int transition = (int)Math.Sqrt(key.Count);
            int keyIt = 0, plaintIt = 0;

            for (int m = 0; m < plainText.Count; ++m)
            {
                for (int tr = 0; tr < transition; ++tr)
                {
                    cipherText[m] += key[keyIt] * plainText[tr + plaintIt]; // element x element in 2 mat
                    keyIt = (keyIt + 1) % key.Count; // grantees circular shift
                }
                cipherText[m] %= 26;
                if ((m + 1) % transition == 0) // on getting m successive letters (one vector)
                    plaintIt += transition;
            }

            return cipherText;
        }

        private List<int> doDecryption(List<int> cipherText, List<int> key)
        {
            // =============================================== //
            // Convert row based key into a 2D matrix
            int N = (int)Math.Sqrt(key.Count);
            int[,] keyMat = new int[N, N];
            int c = 0;
            for (int i = 0; i < N; ++i)
                for (int j = 0; j < N; ++j)
                    keyMat[i, j] = key[c++];
            // =============================================== //

            int[,] keyInverseMat = getKeyInverse(keyMat);

            // =============================================== //
            // Multiply the Key Inverse Matrix
            // with the row based cipher List
            List<int> numericalPlain = new List<int>(new int[cipherText.Count]);
            int p = 0;
            for (int m = 0; m < cipherText.Count;)
            {
                for (int i = 0; i < keyInverseMat.GetLength(0); ++i)
                {
                    for (int j = 0; j < keyInverseMat.GetLength(1); ++j)
                        numericalPlain[p] += keyInverseMat[i, j] * cipherText[m + j];

                    numericalPlain[p] %= 26;
                    if (numericalPlain[p] < 0)
                        numericalPlain[p] += 26;

                    p++;
                }
                m += N;
            }
            // =============================================== //

            return numericalPlain;
        }

        private int[,] getKeyInverse(int[,] keyMat)
        {
            if (keyMat.GetLength(0) == 2) // if the key is a 2D matrix
            {
                int a = keyMat[0, 0], b = keyMat[0, 1], c = keyMat[1, 0], d = keyMat[1, 1];
                int delta = a * d - b * c; // determinant
                int detInverse = getDeterminantInverse(delta);
                a = (a * detInverse) % 26; b = (b * detInverse) % 26;
                c = (c * detInverse) % 26; d = (d * detInverse) % 26;
                keyMat[0, 0] = d; keyMat[1, 1] = a; keyMat[0, 1] = -b; keyMat[1, 0] = -c;

                return keyMat;
            }

            // ================================================== //
            // Get the Key Inverse of a 3D Matrix
            int det = getKeyDeterminant(keyMat);

            int detInv = getDeterminantInverse(det);

            int[,] keyInverseMat = new int[keyMat.GetLength(0), keyMat.GetLength(1)];
            for (int i = 0; i < keyMat.GetLength(0); ++i)
            {
                for (int j = 0; j < keyMat.GetLength(1); ++j)
                {
                    int temp1 = detInv * (int)Math.Pow(-1, i + j);
                    int temp2 = getSubDeterminant(keyMat, i, j) % 26; // may be a negative value
                    keyInverseMat[i, j] = ((temp1 * temp2) % 26 + 26) % 26;
                }
            }

            // transpose the matrix
            for (int i = 0; i < keyMat.GetLength(0); ++i)
                for (int j = 0; j < keyMat.GetLength(1); ++j)
                    keyMat[i, j] = keyInverseMat[j, i];
            // ================================================== //

            return keyMat;
        }

        private int getKeyDeterminant(int[,] keyMat)
        {
            int firstPart = keyMat[0, 0] * (keyMat[1, 1] * keyMat[2, 2] - keyMat[1, 2] * keyMat[2, 1]);
            int secondPart = keyMat[0, 1] * (keyMat[1, 0] * keyMat[2, 2] - keyMat[1, 2] * keyMat[2, 0]);
            int thirdPart = keyMat[0, 2] * (keyMat[1, 0] * keyMat[2, 1] - keyMat[1, 1] * keyMat[2, 0]);

            return ((firstPart - secondPart + thirdPart) % 26 + 26) % 26;
        }

        private int getDeterminantInverse(int det)
        {
            // calculate the determinant multiplicative inverse
            int b = 1, R = 26 - det;
            while (true)
                if (b % R == 0) break;
                else b += 26;
            return 26 - (b / R);
        }

        private int getSubDeterminant(int[,] mat, int r, int c)
        {
            List<int> region = new List<int>();
            for (int i = 0; i < 3; ++i)
            {
                if (i == r) continue;
                for (int j = 0; j < 3; ++j)
                {
                    if (j == c) continue;
                    region.Add(mat[i, j]);
                }
            }
            return region[0] * region[3] - region[1] * region[2];
        }
        #endregion
    }
}
