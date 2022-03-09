using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        char[] alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        public string Encrypt(string plainText, int key)
        {
            char[] input = plainText.ToUpper().ToCharArray();
            string encrypted = "";
            int index = 0;
            foreach (char alphabet in input)
            {
                for (int i = 0; i < alphabets.Length; i++)
                {
                    if (alphabet.Equals(alphabets[i]))
                    {
                        index = i; break;
                    }
                }
                index += key;
                if (index < 0) index = 26 + index;
                index %= 26;

                encrypted += alphabets[index];
            }
            return encrypted;
        }

        public string Decrypt(string cipherText, int key)
        {
            char[] input = cipherText.ToCharArray();
            string decrypted = "";
            int index = 0;
            foreach (char alphabet in input)
            {
                for (int i = 0; i < alphabets.Length; i++)
                {
                    if (alphabet.Equals(alphabets[i]))
                    {
                        index = i; break;
                    }
                }
                index -= key;
                if (index < 0) index = 26 + index; 
                index %= 26;
                decrypted += alphabets[index];
            }
            return decrypted.ToLower();
        }

        public int Analyse(string plainText, string cipherText)
        {
            char PT = plainText.ToUpper().ToCharArray()[0];
            char CT = cipherText.ToUpper().ToCharArray()[0];
            int index_PT = 0, index_CT = 0;
            int key;
            for (int i = 0; i < alphabets.Length; i++)
            {
                if (PT.Equals(alphabets[i]))
                {
                    index_PT = i; break;
                }
            }
            for (int i = 0; i < alphabets.Length; i++)
            {
                if (CT.Equals(alphabets[i]))
                {
                    index_CT = i; break;
                }
            }
            key = index_CT - index_PT;
            if (key < 0) key = 26 + key;
            key %= 26;
            return key;
        }
    }
}
