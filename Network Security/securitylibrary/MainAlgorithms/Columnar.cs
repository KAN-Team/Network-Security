using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public string Encrypt(string plainText, List<int> key)
        {
            List<int> NewKey = SortingNewKey(key);

            int TextLenght = plainText.Length;
            int MatrixLenght = key.Count;
            int MatrixDepth = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(TextLenght) / Convert.ToDouble(MatrixLenght)));

            char[,] Plain2dMatrix = new char[MatrixDepth, MatrixLenght];
            char[] PlainCharArr = plainText.ToCharArray();

            //convert plain char arr to 2d matrix (row wise)
            for (int Depth = 0, IndexCount = 0; Depth < MatrixDepth; Depth++)
            {
                for (int Lenght = 0; Lenght < MatrixLenght; Lenght++, IndexCount++)
                {
                    if (IndexCount >= TextLenght)
                        Plain2dMatrix[Depth, Lenght] = 'X';
                    else
                        Plain2dMatrix[Depth, Lenght] = PlainCharArr[IndexCount];
                }
            }

            char[] Cipher = new char[MatrixDepth * MatrixLenght];
            char[,] SwapedCipher2dMatrix = new char[MatrixDepth, MatrixLenght];

            //generate cipher matrix using the new key and convert cipher 2d matrix to char arr (column wise)
            for (int Lenght = 0, IndexCount = 0; Lenght < MatrixLenght; Lenght++)
            {
                for (int Depth = 0; Depth < MatrixDepth; Depth++, IndexCount++)
                {

                    if (Plain2dMatrix[Depth, NewKey[Lenght]] == 0) continue;
                    SwapedCipher2dMatrix[Depth, Lenght] = Plain2dMatrix[Depth, NewKey[Lenght]];
                    if (SwapedCipher2dMatrix[Depth, Lenght] == 0) continue;
                    Cipher[IndexCount] = SwapedCipher2dMatrix[Depth, Lenght];
                }
            }

            return string.Concat(Cipher);
        }

        public string Decrypt(string cipherText, List<int> key)
        {

            List<int> NewKey = SortingNewKey(key);

            int Text_lenght = cipherText.Length;
            int MatrixLenght = key.Count;
            int MatrixDepth = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Text_lenght) / Convert.ToDouble(MatrixLenght)));

            char[] CipherCharArr = cipherText.ToCharArray();
            char[] PlainCharArr = new char[cipherText.Length];
            char[,] SwapedCipher2dMatrix = new char[MatrixDepth, MatrixLenght];
            char[,] Cipher2dMatrix = new char[MatrixDepth, MatrixLenght];

            //convert cipher char arr to 2d matrix (column wise) and swap it using key list
            for (int Lenght = 0, IndexCount = 0; Lenght < MatrixLenght; Lenght++)
            {
                for (int Depth = 0; Depth < MatrixDepth; Depth++, IndexCount++)
                {
                    if (IndexCount == Text_lenght) break;
                    Cipher2dMatrix[Depth, Lenght] = CipherCharArr[IndexCount];
                    if (Cipher2dMatrix[Depth, Lenght] == 0) continue;
                    SwapedCipher2dMatrix[Depth, NewKey[Lenght]] = Cipher2dMatrix[Depth, Lenght];
                }
            }

            //convert swaped 2d cipher matrix to char array (row wise)
            for (int Depth = 0, IndexCount = 0; Depth < MatrixDepth; Depth++)
            {
                for (int Lenght = 0; Lenght < MatrixLenght; Lenght++, IndexCount++)
                {
                    if (IndexCount == Text_lenght) break;
                    if (SwapedCipher2dMatrix[Depth, Lenght] == 0) continue;
                    PlainCharArr[IndexCount] = SwapedCipher2dMatrix[Depth, Lenght];
                }
            }
            return string.Concat(PlainCharArr).ToLower();
        }

        public List<int> Analyse(string plainText, string cipherText)
        {
            int TextLenght = cipherText.Length;
            int MatrixLenght = 0;
            int MatrixDepth = 0;
            char[,] Plain2dMatrix = null;
            char[,] Cipher2dMatrix = null;
            Dictionary<int, int> MatrixLenghtDivisors = new Dictionary<int, int>();
            char[] CipherCharArr = (cipherText.ToUpper()).ToCharArray();
            char[] PlainCharArr = (plainText.ToUpper()).ToCharArray();

            //get all divisors of Text lenght and put them into row, col (key,value)
            for (int Divisors = 2; Divisors <= (TextLenght / 2); Divisors++)
            {
                if ((TextLenght % Divisors) == 0)
                    MatrixLenghtDivisors.Add(Divisors, TextLenght / Divisors);
            }
            //search in all divisors combinations 
            foreach (KeyValuePair<int, int> Divisors in MatrixLenghtDivisors)
            {
                MatrixLenght = Divisors.Value;
                MatrixDepth = Divisors.Key;
                Plain2dMatrix = new char[MatrixDepth, MatrixLenght];
                Cipher2dMatrix = new char[MatrixDepth, MatrixLenght];

                //transfer cipher char array into 2d array (column wise)
                for (int Lenght = 0, IndexCount = 0; Lenght < MatrixLenght; Lenght++)
                {
                    for (int Depth = 0; Depth < MatrixDepth; Depth++, IndexCount++)
                    {
                        Cipher2dMatrix[Depth, Lenght] = CipherCharArr[IndexCount];
                    }
                }
                //transfer plain char array into 2d array (row wise)
                for (int Depth = 0, IndexCount = 0; Depth < MatrixDepth; Depth++)
                {
                    for (int Lenght = 0; Lenght < MatrixLenght; Lenght++, IndexCount++)
                    {
                        Plain2dMatrix[Depth, Lenght] = PlainCharArr[IndexCount];

                    }
                }
                int CheckRightChar = 0;
                //check the right depth and lenght
                for (int height = 0; height < MatrixLenght; height++)
                {
                    for (int depth = 0; depth < MatrixDepth; depth++)
                    {
                        if (Plain2dMatrix[depth, 0].Equals(Cipher2dMatrix[depth, height]))
                            CheckRightChar++;
                    }
                    if (CheckRightChar == Divisors.Key) goto EndOfLoop;
                    CheckRightChar = 0;
                }
            }
        EndOfLoop:

            //get key of each swaped column
            List<int> Key = new List<int>();
            for (int PlainLenght = 0; PlainLenght < MatrixLenght; PlainLenght++)
            {
                char[] plainColumn = Enumerable.Range(0, Plain2dMatrix.GetLength(0))
                .Select(x => Plain2dMatrix[x, PlainLenght])
                .ToArray();
                for (int CipherLenght = 0; CipherLenght < MatrixLenght; CipherLenght++)
                {
                    char[] CipherColumn = Enumerable.Range(0, Cipher2dMatrix.GetLength(0))
                    .Select(x => Cipher2dMatrix[x, CipherLenght])
                    .ToArray();
                    if (plainColumn.SequenceEqual(CipherColumn))
                    {
                        Key.Add(CipherLenght + 1);
                        break;
                    }
                }
            }
            if (Key.Count == 0)
                Key = new List<int>(new int[TextLenght]);

            return Key;
        }
        
        #region HELPERS
        private List<int> SortingNewKey(List<int> key)
        {
            List<int> newkey = key.ToList();
            var key_dec = new Dictionary<int, int>();

            for (int i = 0; i < key.Count; i++)
            {
                key_dec.Add(newkey[i], i);
            }

            var key_sorted_list = key_dec.Keys.ToList();
            key_sorted_list.Sort();
            for (int i = 0; i < newkey.Count; i++)
            {
                newkey[i] = key_dec[key_sorted_list[i]];
            }
            return newkey;
        }
        #endregion
    }
}
