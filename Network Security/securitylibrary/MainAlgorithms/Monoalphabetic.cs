using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        List<char> alphabets = new List<char>();

        public string Analyse(string plainText, string cipherText)
        {
            alphabets.Clear();
            alphabets.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ");

            List<char> PT = new List<char>();
            PT.AddRange(plainText.ToUpper());

            List<char> CT = new List<char>();
            CT.AddRange(cipherText);

            int index;
            string key = "";
            char nextChar;
            foreach (char alphabet in alphabets)
            {
                if (PT.Contains(alphabet))
                {
                    index = PT.IndexOf(alphabet);
                    key += CT[index];
                }
                else
                {
                    if (key.Length > 0)
                    {
                        index = alphabets.IndexOf(key[key.Length - 1]);
                        nextChar = alphabets[(index + 1) % 26];
                        while (key.Length != 26)
                        {
                            if (!key.Contains(nextChar) && !CT.Contains(nextChar))
                            {
                                key += nextChar;
                                break;
                            }
                            else
                            {
                                index++;
                                nextChar = alphabets[(index + 1) % 26];
                            }
                        }
                    }
                    else
                    {
                        key += alphabets[0];
                    }

                }
            }
            return key.ToLower();
        }

        public string Decrypt(string cipherText, string key)
        {
            alphabets.Clear();
            alphabets.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ");

            List<char> input = new List<char>();
            input.AddRange(cipherText);
            List<char> mainkey = new List<char>();
            mainkey.AddRange(key.ToUpper());
            int index;
            string decrypted = "";

            foreach (char alphabet in input)
            {
                index = mainkey.IndexOf(alphabet);
                decrypted += alphabets[index];
            }
            return decrypted.ToLower();
        }

        public string Encrypt(string plainText, string key)
        {
            alphabets.Clear();
            alphabets.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            List<char> input = new List<char>();
            input.AddRange(plainText.ToUpper());
            List<char> mainkey = new List<char>();
            mainkey.AddRange(key.ToUpper());
            int index;
            string encrypted = "";
            foreach (char alphabet in input)
            {
                index = alphabets.IndexOf(alphabet);
                encrypted += mainkey[index];
            }
            return encrypted.ToUpper();
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            string output;
            alphabets.Clear();
            alphabets.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ");

            List<char> freq = new List<char>();
            freq.AddRange("ETAOINSRHLDCUMFPGWYBVKXJQZ");

            Dictionary<char, int> CT = new Dictionary<char, int>();
            CT = countChar(cipher.ToLower());
            output = cipher;
            for (int i = 0; i < CT.Count ; i++)
            {
                output = output.Replace(CT.Keys.ElementAt(i), freq[i]);
            }
            return output;
        }
        public Dictionary<char, int> countChar(string cipher)
        {
            Dictionary<char, int> CT = new Dictionary<char, int>();
            foreach (char c in cipher)
            {
                if (!CT.ContainsKey(c))
                    CT.Add(c, 1);
                else
                {
                    CT[c] += 1;
                }
            }
            var ordered = CT.OrderByDescending(key => key.Value).ToDictionary(x => x.Key, x => x.Value);
            return ordered;
        }
    }
}
