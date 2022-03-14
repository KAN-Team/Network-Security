using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        List<char> alphabets = new List<char>();
        int row1, row2, col1, col2;

        public string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            char[,] matrix = new char[5, 5];
            matrix = CreateMatrix(key.ToUpper());
            List<string> splitted = new List<string>();
            string encrypted = "";
            splitted = splitString(plainText.ToUpper());
            int type;
            foreach (string part in splitted)
            {
                type = search(part, matrix);
                switch (type)
                {
                    case 1: //same row
                        encrypted += matrix[row1, (col1 + 1) % 5];
                        encrypted += matrix[row2, (col2 + 1) % 5];
                        break;
                    case 2: // same col
                        encrypted += matrix[(row1 + 1) % 5, col1];
                        encrypted += matrix[(row2 + 1) % 5, col2];
                        break;
                    case 3: // diagonal
                        encrypted += matrix[row1, col2];
                        encrypted += matrix[row2, col1];
                        break;
                }


            }

            return encrypted;
        }

        public char[,] CreateMatrix(string k)
        {
            alphabets.Clear();
            alphabets.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            string key = k.ToUpper();
            char[,] matrix = new char[5, 5];

            int x = 0, y = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (x < key.Length)
                    {
                        if (!ContainElement(matrix, key[x]))
                        {
                            if (key[x] == 'J' && !ContainElement(matrix, 'I'))
                            {
                                matrix[i, j] = 'I';
                            }
                            else if (alphabets[y] == 'J' && ContainElement(matrix, 'I'))
                            {
                                j--;
                            }
                            else
                                matrix[i, j] = key[x];
                        }
                        else
                        {
                            j--;
                        }
                        x++;
                    }
                    else
                    {
                        if (!ContainElement(matrix, alphabets[y]))
                        {
                            if (alphabets[y] == 'J' && !ContainElement(matrix, 'I'))
                            {
                                matrix[i, j] = 'I';
                            }
                            else if (alphabets[y] == 'J' && ContainElement(matrix, 'I'))
                            {
                                j--;
                            }
                            else
                                matrix[i, j] = alphabets[y];
                        }
                        else
                        {
                            j--;
                        }
                        y++;
                    }

                }
            }
            return matrix;
        }
        public bool ContainElement(char[,] matrix, char element)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (matrix[i, j].Equals(element))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public List<string> splitString(string text)
        {
            int count = 0;
            int j = 0;
            List<string> splitted = new List<string>();
            splitted.Add("");
            for (int i = 0; i < text.Length; i++)
            {
                if (count != 2)
                {
                    if (splitted[j].Length > 0)
                    {
                        if (splitted[j][0].Equals(text[i]))
                        {
                            splitted[j] += 'X';
                            i--;
                        }
                        else
                        {
                            splitted[j] += text[i];
                        }
                    }
                    else
                    {
                        splitted[j] += text[i];
                    }
                    count++;
                }
                else
                {
                    count = 0;
                    j++;
                    i--;
                    splitted.Add("");
                }
            }
            if (splitted[splitted.Count - 1].Length == 1)
            {
                splitted[splitted.Count - 1] += 'X';
            }
            return splitted;
        }
        public int search(string part, char[,] matrix)
        {
            int type = 0;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (matrix[i, j].Equals(part[0]))
                    {
                        row1 = i;
                        col1 = j;
                    }
                    if (matrix[i, j].Equals(part[1]))
                    {
                        row2 = i;
                        col2 = j;
                    }
                }
            }
            if (row1 == row2)
            {
                type = 1;
            }
            else if (col1 == col2)
            {
                type = 2;
            }
            else
            {
                type = 3;
            }

            return type;
        }
    }
}
