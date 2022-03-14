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
            // return cipher as an indexed letters.
            return doEncryption(plainText, key);
        }

        public string Encrypt(string plainText, string key)
        {
            // **** return cipher as a string **** //

            List<int> plainNumeric = new List<int>(new int[plainText.Length]);
            List<int> keyNumeric = new List<int>(new int[key.Length]);
            plainText.ToLower();
            key.ToLower();

            for (int i = 0; i < plainText.Length; ++i)
                plainNumeric[i] = plainText[i] - 'a';

            for (int i = 0; i < key.Length; ++i)
                keyNumeric[i] = key[i] - 'a';

            List<int> cipherNumeric = doEncryption(plainNumeric, keyNumeric);

            StringBuilder cipherText = new StringBuilder(cipherNumeric.Count + 1);
            for (int i = 0; i < cipherNumeric.Count; ++i)
                cipherText.Append((char)('a' + cipherNumeric[i]));

            return cipherText.ToString();
        }

        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string plainText, string key)
        {
            throw new NotImplementedException();
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
        #endregion
    }
}
