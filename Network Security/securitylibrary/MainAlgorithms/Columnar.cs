using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
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

            throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, List<int> key)
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

            int Text_lenght = cipherText.Length;
            int matrix_lenght = key.Count;
            int matrix_depth = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Text_lenght) / Convert.ToDouble(matrix_lenght)));
            char[,] cipher_matrix = new char[matrix_depth, matrix_lenght];
            char[] plain_to_char_Arr = cipherText.ToCharArray();
            int index_count = 0;

            char[] cipher = new char[cipherText.Length];
            char[,] swaped_cipher_matrix = new char[matrix_depth, matrix_lenght];

            for (int i = 0; i < matrix_lenght; i++)
            {
                for (int j = 0; j < matrix_depth; j++)
                {
                    if (index_count == Text_lenght) break;
                    // if (plain_to_char_Arr[index_count].Equals("X")) continue;
                    cipher_matrix[j, i] = plain_to_char_Arr[index_count];
                    index_count++;
                    if (cipher_matrix[j, i] == 0) continue;
                    swaped_cipher_matrix[j, newkey[i]] = cipher_matrix[j, i];
                }
            }

            index_count = 0;
            for (int i = 0; i < matrix_depth; i++)
            {
                for (int j = 0; j < matrix_lenght; j++)
                {
                    if (swaped_cipher_matrix[i, j] == 0) continue;
                    cipher[index_count] = swaped_cipher_matrix[i, j];
                    index_count++;
                }
            }

            return string.Concat(cipher).ToLower();
        }

        public string Encrypt(string plainText, List<int> key)
        {
            var key_dec = new Dictionary<int, int>();
            for (int i = 0; i < key.Count; i++)
            {
                key_dec.Add(key[i], i);
            }

            var key_sorted_list = key_dec.Keys.ToList();
            key_sorted_list.Sort();
            for (int i = 0; i < key.Count; i++)
            {
                key[i] = key_dec[key_sorted_list[i]];
            }

            int Text_lenght = plainText.Length;
            int matrix_lenght = key.Count;
            int matrix_depth = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Text_lenght) / Convert.ToDouble(matrix_lenght)));
            char[,] cipher_matrix = new char[matrix_depth, matrix_lenght];
            char[] plain_to_char_Arr = plainText.ToCharArray();
            int index_count = 0;

            for (int i = 0; i < matrix_depth; i++)
            {
                for (int j = 0; j < matrix_lenght; j++)
                {
                    if (index_count == Text_lenght) break;
                    cipher_matrix[i, j] = plain_to_char_Arr[index_count];
                    index_count++;
                }
            }

            index_count = 0;
            char[] cipher = new char[plainText.Length];
            char[,] swaped_cipher_matrix = new char[matrix_depth, matrix_lenght];

            for (int i = 0; i < matrix_lenght; i++)
            {
                for (int j = 0; j < matrix_depth; j++)
                {
                    if (cipher_matrix[j, key[i]] == 0) continue;
                    swaped_cipher_matrix[j, i] = cipher_matrix[j, key[i]];
                    if (swaped_cipher_matrix[j, i] == 0) continue;
                    cipher[index_count] = swaped_cipher_matrix[j, i];
                    index_count++;
                }
            }

            return string.Concat(cipher);
        }
    }
}
