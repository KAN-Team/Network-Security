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
            int Text_lenght = cipherText.Length;
            char[] cipher_tochar_Array = (cipherText.ToUpper()).ToCharArray();
            char[] plain_tochar_Array = (plainText.ToUpper()).ToCharArray();
            int key = 0;
            for (int i = 1; i < Text_lenght; i++)
            {
                if (cipher_tochar_Array[i].Equals(plain_tochar_Array[1]))
                {
                    int matrix_lenght = i;
                    key = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Text_lenght) / Convert.ToDouble(matrix_lenght)));
                    string dec_cipher = Decrypt(cipherText, key);
                    if (dec_cipher.Equals(plainText.ToUpper())) break;
                }
            }
            return key;
        }

        public string Decrypt(string cipherText, int key)
        {
            int lenght = cipherText.Length;
            int matrix_lenght = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(lenght) / Convert.ToDouble(key)));
            int Index_counter = 0;
            char[,] cipher_matrix = new char[key, matrix_lenght];
            char[] cipher_tochar_Array = cipherText.ToCharArray();
            for (int i = 0; i < key; i++)
            {
                int remaining_matrix_lenght = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(lenght) / Convert.ToDouble(key - i)));
                for (int j = 0; j < remaining_matrix_lenght; j++)
                {
                    if (Index_counter == cipher_tochar_Array.Length)
                        break;
                    cipher_matrix[i, j] = cipher_tochar_Array[Index_counter];
                    Index_counter++;
                }
                lenght -= remaining_matrix_lenght;
            }

            char[] plain = new char[cipherText.Length];
            Index_counter = 0;
            for (int i = 0; i < matrix_lenght; i++)
            {
                for (int j = 0; j < key; j++)
                {
                    if (Index_counter == cipher_tochar_Array.Length)
                        break;
                    plain[Index_counter] = cipher_matrix[j, i];
                    Index_counter++;
                }
            }
            return string.Concat(plain);

        }
        public string Encrypt(string plainText, int key)
        {

            int matrix_lenght = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(plainText.Length) / Convert.ToDouble(key)));

            int Index_counter = 0;
            char[,] plain_matrix = new char[key, matrix_lenght];
            char[] plain_tochar_Array = plainText.ToCharArray();

            for (int i = 0; i < matrix_lenght; i++)
            {
                for (int j = 0; j < key; j++)
                {
                    if (Index_counter == plain_tochar_Array.Length)
                        break;
                    plain_matrix[j, i] = plain_tochar_Array[Index_counter];
                    Index_counter++;
                }
            }

            char[] cipher = new char[plainText.Length];
            Index_counter = 0;
            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < matrix_lenght; j++)
                {
                    if (Index_counter == plain_tochar_Array.Length)
                        break;
                    if (plain_matrix[i, j] == 0) continue;
                    cipher[Index_counter] = plain_matrix[i, j];
                    Index_counter++;
                }
            }

            return string.Concat(cipher);
        }
    }
}
