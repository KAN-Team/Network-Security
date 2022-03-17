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
            // Brute Force the Key and encrypt the plain with it 
            // and compare the result with the given cypher
            // =============================================== //
            List<int> key = get2x2Key(plainText, cipherText);
            if (key == null)
                throw new InvalidAnlysisException();
            return key;
        }

        public string Analyse(string plainText, string cipherText)
        {
            // **** this method returns key as a string **** //
            // =============================================== //

            // ======= Convert from letters to numbers ======= //
            List<int> plainNumeric = textToNumbers(plainText);
            List<int> cipherNumeric = textToNumbers(cipherText);
            // =============================================== //

            List<int> keyNumeric = get2x2Key(plainNumeric, cipherNumeric);

            // ======= Convert from numbers to letters ======= //
            string keyText = numbersToText(keyNumeric);
            // =============================================== //

            return keyText;
        }

        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            // Brute Force the Key and encrypt the plain with it 
            // and compare the result with the given cypher
            // =============================================== //
            List<int> key = get3x3Key(plainText, cipherText);
            // if (key != null)
            return key;
        }

        public string Analyse3By3Key(string plainText, string cipherText)
        {
            // **** this method returns key as a string **** //
            // =============================================== //

            // ======= Convert from letters to numbers ======= //
            List<int> plainNumeric = textToNumbers(plainText);
            List<int> cipherNumeric = textToNumbers(cipherText);
            // =============================================== //

            List<int> keyNumeric = get3x3Key(plainNumeric, cipherNumeric);

            // ======= Convert from numbers to letters ======= //
            string keyText = numbersToText(keyNumeric);
            // =============================================== //

            return keyText;
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
            int[,] keyInverseMat = getMatInverse(keyMat);
            // =============================================== //
            // Convert row based cipher into 2D mat and multiply it with the key inverse mat
            int[,] cipherMat = listToMatrix(cipherText, N);
            List<int> numericalPlain = mul2Matrices(keyInverseMat, cipherMat);
            // =============================================== //

            return numericalPlain;
        }

        private int[,] getMatInverse(int[,] matrix)
        {
            if (matrix.GetLength(0) == 2) // if the key is a 2D matrix
            {
                int a = matrix[0, 0], b = matrix[0, 1], c = matrix[1, 0], d = matrix[1, 1];
                int delta = a * d - b * c; // determinant
                int detInverse = getDeterminantInverse(delta);
                a = (a * detInverse) % 26; b = (b * detInverse) % 26;
                c = (c * detInverse) % 26; d = (d * detInverse) % 26;
                matrix[0, 0] = d; matrix[1, 1] = a; matrix[0, 1] = -b; matrix[1, 0] = -c;

                return matrix;
            }

            // ================================================== //
            // Get the Key Inverse of a 3D Matrix
            int det = getKeyDeterminant(matrix);

            int detInv = getDeterminantInverse(det);

            int[,] keyInverseMat = new int[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); ++i)
                for (int j = 0; j < matrix.GetLength(1); ++j)
                {
                    int temp1 = detInv * (int)Math.Pow(-1, i + j);
                    int temp2 = getSubDeterminant(matrix, i, j) % 26;
                    keyInverseMat[i, j] = ((temp1 * temp2) % 26 + 26) % 26;
                }

            // transpose the matrix
            for (int i = 0; i < matrix.GetLength(0); ++i)
                for (int j = 0; j < matrix.GetLength(1); ++j)
                    matrix[i, j] = keyInverseMat[j, i];
            // ================================================== //

            return matrix;
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

        private bool isKeyFound = false;
        private List<int> getKey(List<int> key, int index, List<int> plain, List<int> cypher)
        {
            if (isKeyFound) return key;
            if (index == key.Count)
                return key;

            for (int i = 0; i < 26; ++i)
            {
                key[index] = i;
                List<int> currentKey = getKey(key, index + 1, plain, cypher);
                List<int> cypherTest = doEncryption(plain, currentKey);
                if (isEqual(cypher, cypherTest))
                {
                    isKeyFound = true;
                    return currentKey;
                }
            }

            return key;
        }
        private List<int> get2x2Key(List<int> plain, List<int> cypher)
        {
            for(int i = 0; i < 26; ++i)
                for (int j = 0; j < 26; ++j)
                    for (int k = 0; k < 26; ++k)
                        for (int l = 0; l < 26; ++l)
                        {
                            List<int> currentKey = new List<int>() { i, j, k, l };
                            List<int> cypher_test = doEncryption(plain, currentKey);
                            if (isEqual(cypher, cypher_test)) return currentKey;
                        }
            return null;
        }
        private List<int> get3x3Key(List<int> plain, List<int> cypher)
        {
            for (int a = 0; a < 26; ++a)
                for (int b = 0; b < 26; ++b)
                    for (int c = 0; c < 26; ++c)
                        for (int d = 0; d < 26; ++d)
                            for (int e = 0; e < 26; ++e)
                                for (int f = 0; f < 26; ++f)
                                    for (int g = 0; g < 26; ++g)
                                        for (int h = 0; h < 26; ++h)
                                            for (int i = 0; i < 26; ++i)
                                            {
                                                List<int> currentKey = new List<int>() { a, b, c, 
                                                                                         d, e, f,
                                                                                         g, h, i};
                                                List<int> cypher_test = doEncryption(plain, currentKey);
                                                if (isEqual(cypher, cypher_test)) return currentKey;
                                            }
            return null;
        }

        // === HELPERS OF HELPERS === //
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

        private int[,] listToMatrix(List<int> list)
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

        private int[,] listToMatrix(List<int> list, int keyDimen)
        {
            // === Convert row based list (e.g. plain/cipher) into a 2D matrix 
            // === with rows equals to the specified keyDimen and multiple cols
            int N = (list.Count + 1) / keyDimen;    // number of cols
            list.Add(0); list.Add(0);               // for equalizing with matrix
            int[,] matrix = new int[keyDimen, N];
            int c = 0;                              // List iterator

            for (int i = 0; i < N; ++i)
                for (int j = 0; j < keyDimen; ++j)
                    matrix[j, i] = list[c++];

            list.RemoveRange(list.Count - 2, 2);    // to handle reference passing
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

        private bool isEqual(List<int> L1, List<int> L2)
        {
            int N = L1.Count, M = L2.Count;
            if (N != M) return false;
            for (int i = 0; i < N; ++i)
                if (L1[i] != L2[i]) return false;
            return true;
        }
        #endregion
    }
}
