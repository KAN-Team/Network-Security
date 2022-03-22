using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Encrypt(string plainText, string key)
        {
            plainText = plainText.ToLower();
            key = key.ToLower();
            string keyStream = key + plainText.Substring(0, plainText.Length - key.Length);
            char[,] tableau = getTableau();

            StringBuilder cipherText = new StringBuilder("");
            for (int i = 0; i < plainText.Length; ++i)
                cipherText.Append(tableau[plainText[i] - 'a', keyStream[i] - 'a']);
            
            return cipherText.ToString();
        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            key = key.ToLower();
            char[,] tableau = getTableau();

            StringBuilder plainText = new StringBuilder("");
            int plainIt = 0;
            for (int i = 0; i < cipherText.Length; ++i)
            {
                int colToSearchIn = (i < key.Length) ? key[i] - 'a' : plainText[plainIt++] - 'a'; // if-else block
                for (int row = 0; row < 26; ++row)
                    if (tableau[row, colToSearchIn] == cipherText[i])
                        plainText.Append(tableau[row, 0]);
            }
            
            return plainText.ToString();
        }

        public string Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            char[,] tableau = getTableau();

            StringBuilder keyStream = new StringBuilder("");
            for (int i = 0; i < plainText.Length; ++i)
            {
                int colToSearchIn = plainText[i] - 'a';
                for (int row = 0; row < 26; ++row)
                {
                    if (tableau[row, colToSearchIn] == cipherText[i])
                        keyStream.Append(tableau[row, 0]);
                }
            }

            return getKeyFromKeyStream(keyStream.ToString(), plainText);
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

        private string getKeyFromKeyStream(string keyStream, string plainText)
        {
            int splitAt = 1;
            while (true)
            {
                string a = keyStream.Substring(0, splitAt);
                string b = keyStream.Substring(splitAt, keyStream.Length - splitAt);
                if (plainText.StartsWith(b))
                    return a;
                splitAt++;
            }
        }
        #endregion

    }
}
