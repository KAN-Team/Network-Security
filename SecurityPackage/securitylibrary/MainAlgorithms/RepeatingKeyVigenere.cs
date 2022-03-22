using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Encrypt(string plainText, string key)
        {
            plainText = plainText.ToLower();                            // the below code works only 
            key = key.ToLower();                                        // with the small case letters
            char[,] tableau = getTableau();
            int cipherLength = plainText.Length;

            StringBuilder cipherText = new StringBuilder("");
            for (int i = 0; i < cipherLength; ++i)
            {
                char c = key[i%key.Length];                             // key stream letter by letter
                cipherText.Append(tableau[plainText[i]-'a', c-'a']);    // the intersection between the two letters
            }

            return cipherText.ToString();
        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();                                  // the below code works only
            key = key.ToLower();                                                // with the small case letters
            char[,] tableau = getTableau();

            StringBuilder plainText = new StringBuilder("");
            for (int i = 0; i < cipherText.Length; ++i)
            {
                char c = key[i % key.Length];                                   // key stream letter by letter
                int colToSearchIn = c - 'a';
                int row = 0;
                for (; row < 26; ++row)
                    if (tableau[row, colToSearchIn] == cipherText[i])
                    {
                        plainText.Append((char)('a' + row));                    // same as tableau[row, 0]
                        break;
                    }
            }

            return plainText.ToString();
        }

        public string Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();                                    // the below code works only
            cipherText = cipherText.ToLower();                                  // with the small case letters
            char[,] tableau = getTableau();

            StringBuilder keyStream = new StringBuilder("");
            for (int i = 0; i < plainText.Length; ++i)
            {
                int colToSearchIn = plainText[i] - 'a';
                int row = 0;
                for (; row < 26; ++row)
                    if (tableau[row, colToSearchIn] == cipherText[i])
                    {
                        keyStream.Append((char)('a' + row));                    // same as tableau[row, 0]
                        break;
                    }
            }

            string key = getKeyFromKeyStream(keyStream.ToString());

            return key;
        }

        #region HELPERS
        private char[,] getTableau()
        {
            char[,] tableau = new char[26, 26];
            for (int i = 0; i < 26; ++i)
            {
                char currentChar = (char)('a' + i);
                for (int j = 0; j < 26; ++j)
                {
                    int letterIdx = (currentChar + j);
                    if (letterIdx > 'z')
                        letterIdx = letterIdx % ('z' + 1) + 'a';
                    tableau[i, j] = (char)letterIdx;
                }
            }
            return tableau;
        }

        private string getKeyFromKeyStream(string keyStream)
        {
            int splitAt = 1;
            while(true)
            {
                string a = keyStream.Substring(0, splitAt);
                string b = keyStream.Substring(splitAt, keyStream.Length - splitAt);
                if (a.Length < b.Length)
                {
                    if (b.StartsWith(a))
                        return a;
                }
                else
                {
                    if (a.StartsWith(b))
                        return a;
                }
                splitAt++;
            }
        }
        #endregion

    }
}