using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            int TextLenght = cipherText.Length;
            char[] CipherCharArray = (cipherText.ToUpper()).ToCharArray();
            char[] PlainCharArray = (plainText.ToUpper()).ToCharArray();
            int key = 0;
            for (int Lenght = 1; Lenght < TextLenght; Lenght++)
            {
                if (CipherCharArray[Lenght].Equals(PlainCharArray[1]))
                {
                    int MatrixLenght = Lenght;
                    key = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(TextLenght) / Convert.ToDouble(MatrixLenght)));
                    string plain = Decrypt(cipherText, key);
                    if (plain.Equals(plainText.ToUpper())) break;
                }
            }
            return key;

            ////or
            //Dictionary<int, int> Lenght_Divisors = new Dictionary<int, int>();
            //for (int i = 2; i <= (Text_lenght / 2); i++)
            //{
            //    if ((Text_lenght % i) == 0)
            //        Lenght_Divisors.Add(i, Text_lenght / i);
            //}
            //int matrix_height = 0;
            //int matrix_depth = 0;
            //char[,] plain_matrix = null;
            //char[,] cipher_matrix = null;

            //foreach (KeyValuePair<int, int> Divisors in Lenght_Divisors)
            //{

            //    plain_matrix = new char[Divisors.Key, Divisors.Value];
            //    cipher_matrix = new char[Divisors.Key, Divisors.Value];
            //    for (int height = 0, index_count = 0; height < Divisors.Value; height++)
            //    {
            //        for (int depth = 0; depth < Divisors.Key; depth++, index_count++)
            //        {
            //            cipher_matrix[depth, height] = cipher_tochar_Array[index_count];

            //        }
            //    }

            //    for (int depth = 0, index_count = 0; depth < Divisors.Key; depth++)
            //    {
            //        for (int height = 0; height < Divisors.Value; height++, index_count++)
            //        {
            //            plain_matrix[depth, height] = plain_tochar_Array[index_count];

            //        }
            //    }
            //    int check_counter = 0;
            //    for (int height = 0; height < Divisors.Value; height++)
            //    {
            //        for (int depth = 0; depth < Divisors.Key; depth++)
            //        {
            //            if (plain_matrix[depth, 0].Equals(cipher_matrix[depth, height]))
            //            {
            //                matrix_height = Divisors.Value;
            //                matrix_depth = Divisors.Key;
            //                check_counter++;
            //            }

            //        }
            //        if (check_counter == Divisors.Key) break;
            //        check_counter = 0;
            //    }
            //    if (check_counter == Divisors.Key) break;
            //}
            //return matrix_depth;
        }

        public string Decrypt(string cipherText, int key)
        {
            int TextLenght = cipherText.Length;
            int MatrixDepth = key;
            int MatrixLenght = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(TextLenght) / Convert.ToDouble(MatrixDepth)));
            char[,] Cipher2dMatrix = new char[MatrixDepth, MatrixLenght];
            char[] CipherCharArr = cipherText.ToCharArray();

            //convert cipher char arr to 2d matrix (row wise)
            for (int Depth = 0, IndexCounter = 0; Depth < MatrixDepth; Depth++)
            {
                int remaining_matrix_lenght = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(TextLenght) / Convert.ToDouble(MatrixDepth - Depth)));

                for (int Lenght = 0; Lenght < remaining_matrix_lenght; Lenght++, IndexCounter++)
                {
                    if (IndexCounter == CipherCharArr.Length)
                        break;
                    Cipher2dMatrix[Depth, Lenght] = CipherCharArr[IndexCounter];
                }
                TextLenght -= remaining_matrix_lenght;
            }

            char[] plain = new char[cipherText.Length];

            //convert cipher 2d matrix to plain char arr (column wise)
            for (int Lenght = 0, IndexCounter = 0; Lenght < MatrixLenght; Lenght++)
            {
                for (int Depth = 0; Depth < MatrixDepth; Depth++, IndexCounter++)
                {
                    if (IndexCounter == CipherCharArr.Length)
                        break;
                    plain[IndexCounter] = Cipher2dMatrix[Depth, Lenght];
                }
            }

            return string.Concat(plain);

        }

        public string Encrypt(string plainText, int key)
        {
            int MatrixDepth = key;
            int MatrixLenght = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(plainText.Length) / Convert.ToDouble(MatrixDepth)));
            char[,] Plain2dMatrix = new char[MatrixDepth, MatrixLenght];
            char[] PlainCharArray = plainText.ToCharArray();

            //convert plain char array to 2d matrix (column wise)
            for (int Lenght = 0, IndexCounter = 0; Lenght < MatrixLenght; Lenght++)
            {
                for (int Depth = 0; Depth < MatrixDepth; Depth++, IndexCounter++)
                {
                    if (IndexCounter >= PlainCharArray.Length)
                        Plain2dMatrix[Depth, Lenght] = 'x';
                    else
                        Plain2dMatrix[Depth, Lenght] = PlainCharArray[IndexCounter];

                }
            }

            char[] cipher = new char[MatrixLenght * MatrixDepth];

            //convert 2d plain matrix to cipher char arr (row wise)
            for (int Depth = 0, IndexCounter = 0; Depth < MatrixDepth; Depth++)
            {
                for (int Lenght = 0; Lenght < MatrixLenght; Lenght++, IndexCounter++)
                {
                    if (Plain2dMatrix[Depth, Lenght] == 0) continue;
                    cipher[IndexCounter] = Plain2dMatrix[Depth, Lenght];
                }
            }
            return string.Concat(cipher);
        }
    }
}
