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

            // ======= Convert from letters to numbers ======= //
            List<int> plainNumeric = textToNumbers(plainText);
            List<int> keyNumeric = textToNumbers(key);
            // =============================================== //

            List<int> cipherNumeric = doEncryption(plainNumeric, keyNumeric);

            // ======= Convert from numbers to letters ======= //
            string cipherText = numbersToText(cipherNumeric);
            // =============================================== //

            return cipherText;
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

            // ======= Convert from letters to numbers ======= //
            List<int> cipherNumeric = textToNumbers(cipherText);
            List<int> keyNumeric = textToNumbers(key);
            // =============================================== //

            List<int> plainNumeric = doDecryption(cipherNumeric, keyNumeric);

            // ======= Convert from numbers to letters ======= //
            string plainText = numbersToText(plainNumeric);
            // =============================================== //

            return plainText;
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
            // Converting both lists to matrices
            // before multiplying them (as a matrix multiplication)
            // =============================================== //
            int transition = (int)Math.Sqrt(key.Count);
            int[,] keyMat = listToMatrix(key);
            int[,] plainMat = listToMatrix(plainText, transition);
            List<int> cipherText = mul2Matrices(keyMat, plainMat);
            return cipherText;
        }

        private List<int> doDecryption(List<int> cipherText, List<int> key)
        {
            // =============================================== //
            // Convert row based key into a 2D matrix
            int N = (int)Math.Sqrt(key.Count);
            int[,] keyMat = listToMatrix(key);
            // =============================================== //
            // Get the inverse key matrix 
            int[,] keyInverseMat = getKeyInverse(keyMat);
            // =============================================== //
            // Convert row based cipher into 2D mat and multiply it with the key inverse mat
            int[,] cipherMat = listToMatrix(cipherText, N);
            List<int> numericalPlain = mul2Matrices(keyInverseMat, cipherMat);
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
                for (int j = 0; j < keyMat.GetLength(1); ++j)
                {
                    int temp1 = detInv * (int)Math.Pow(-1, i + j);
                    int temp2 = getSubDeterminant(keyMat, i, j) % 26;
                    keyInverseMat[i, j] = ((temp1 * temp2) % 26 + 26) % 26;
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

        private List<int> textToNumbers(string text)
        {
            List<int> result = new List<int>(new int[text.Length]);
            text = text.ToLower();
            for (int i = 0; i < text.Length; ++i)
                result[i] = text[i] - 'a';

            return result;
        }

        private string numbersToText(List<int> numbers)
        {
            StringBuilder text = new StringBuilder(numbers.Count + 1);
            for (int i = 0; i < numbers.Count; ++i)
                text.Append((char)('a' + numbers[i]));

            return text.ToString();
        }

        private static int[,] listToMatrix(List<int> list)
        {
            // === Convert row based list (e.g. key) into a 2D matrix === //
            int N = (int)Math.Sqrt(list.Count);   // key mat dimensions
            int[,] matrix = new int[N, N];
            int c = 0;
            for (int i = 0; i < N; ++i)
                for (int j = 0; j < N; ++j)
                    matrix[i, j] = list[c++];

            return matrix;
        }

        private static int[,] listToMatrix(List<int> list, int keyDimen)
        {
            // === Convert row based list (e.g. plain/cipher) into a 2D matrix 
            // === with rows equals to the specified keyDimen and multiple cols
            int N = (list.Count + 1) / keyDimen; // number of cols
            list.Add(0); list.Add(0);
            int[,] matrix = new int[keyDimen, N];
            int c = 0;
            for (int i = 0; i < N; ++i)
                for (int j = 0; j < keyDimen; ++j)
                    matrix[j, i] = list[c++];

            return matrix;
        }

        private List<int> mul2Matrices(int[,] mat1, int[,] mat2)
        {
            int N = mat1.GetLength(0); // key dimension
            int M = mat2.GetLength(0) * mat2.GetLength(1); // cipher/plain size
            int K = mat2.GetLength(1); // max number of iterations 
            List<int> result = new List<int>(new int[M]); // row list of the same (plain/cipher) size
            int it = 0; // result index iterator

            for (int k = 0; k < K; ++k)
                for (int i = 0; i < N; ++i, result[it] = (result[it] % 26 + 26) % 26, ++it)
                    for (int j = 0; j < N; ++j)
                        result[it] += mat1[i, j] * mat2[j, k];

            return result;
        }
        #endregion
    }
}
