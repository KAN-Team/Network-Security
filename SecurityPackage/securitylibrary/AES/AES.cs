﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {
        byte[,] sbox = new byte[16, 16] {{0x63, 0x7C, 0x77, 0x7B, 0xF2, 0x6B, 0x6F, 0xC5, 0x30, 0x01, 0x67, 0x2B, 0xFE, 0xD7, 0xAB, 0x76},
                                         {0xCA, 0x82, 0xC9, 0x7D, 0xFA, 0x59, 0x47, 0xF0, 0xAD, 0xD4, 0xA2, 0xAF, 0x9C, 0xA4, 0x72, 0xC0},
                                         {0xB7, 0xFD, 0x93, 0x26, 0x36, 0x3F, 0xF7, 0xCC, 0x34, 0xA5, 0xE5, 0xF1, 0x71, 0xD8, 0x31, 0x15},
                                         {0x04, 0xC7, 0x23, 0xC3, 0x18, 0x96, 0x05, 0x9A, 0x07, 0x12, 0x80, 0xE2, 0xEB, 0x27, 0xB2, 0x75},
                                         {0x09, 0x83, 0x2C, 0x1A, 0x1B, 0x6E, 0x5A, 0xA0, 0x52, 0x3B, 0xD6, 0xB3, 0x29, 0xE3, 0x2F, 0x84},
                                         {0x53, 0xD1, 0x00, 0xED, 0x20, 0xFC, 0xB1, 0x5B, 0x6A, 0xCB, 0xBE, 0x39, 0x4A, 0x4C, 0x58, 0xCF},
                                         {0xD0, 0xEF, 0xAA, 0xFB, 0x43, 0x4D, 0x33, 0x85, 0x45, 0xF9, 0x02, 0x7F, 0x50, 0x3C, 0x9F, 0xA8},
                                         {0x51, 0xA3, 0x40, 0x8F, 0x92, 0x9D, 0x38, 0xF5, 0xBC, 0xB6, 0xDA, 0x21, 0x10, 0xFF, 0xF3, 0xD2},
                                         {0xCD, 0x0C, 0x13, 0xEC, 0x5F, 0x97, 0x44, 0x17, 0xC4, 0xA7, 0x7E, 0x3D, 0x64, 0x5D, 0x19, 0x73},
                                         {0x60, 0x81, 0x4F, 0xDC, 0x22, 0x2A, 0x90, 0x88, 0x46, 0xEE, 0xB8, 0x14, 0xDE, 0x5E, 0x0B, 0xDB},
                                         {0xE0, 0x32, 0x3A, 0x0A, 0x49, 0x06, 0x24, 0x5C, 0xC2, 0xD3, 0xAC, 0x62, 0x91, 0x95, 0xE4, 0x79},
                                         {0xE7, 0xC8, 0x37, 0x6D, 0x8D, 0xD5, 0x4E, 0xA9, 0x6C, 0x56, 0xF4, 0xEA, 0x65, 0x7A, 0xAE, 0x08},
                                         {0xBA, 0x78, 0x25, 0x2E, 0x1C, 0xA6, 0xB4, 0xC6, 0xE8, 0xDD, 0x74, 0x1F, 0x4B, 0xBD, 0x8B, 0x8A},
                                         {0x70, 0x3E, 0xB5, 0x66, 0x48, 0x03, 0xF6, 0x0E, 0x61, 0x35, 0x57, 0xB9, 0x86, 0xC1, 0x1D, 0x9E},
                                         {0xE1, 0xF8, 0x98, 0x11, 0x69, 0xD9, 0x8E, 0x94, 0x9B, 0x1E, 0x87, 0xE9, 0xCE, 0x55, 0x28, 0xDF},
                                         {0x8C, 0xA1, 0x89, 0x0D, 0xBF, 0xE6, 0x42, 0x68, 0x41, 0x99, 0x2D, 0x0F, 0xB0, 0x54, 0xBB, 0x16}};

        static byte[,] sboxInverse = new byte[16, 16] {{ 0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb },
                                                       { 0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb },
                                                       { 0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e },
                                                       { 0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25 },
                                                       { 0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92 },
                                                       { 0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84 },
                                                       { 0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06 },
                                                       { 0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b },
                                                       { 0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73 },
                                                       { 0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e },
                                                       { 0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b },
                                                       { 0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4 },
                                                       { 0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f },
                                                       { 0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef },
                                                       { 0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61 },
                                                       { 0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d }};

        byte[,] galoisField = new byte[4, 4] {{0x02, 0x03, 0x01, 0x01},
                                              {0x01, 0x02, 0x03, 0x01},
                                              {0x01, 0x01, 0x02, 0x03},
                                              {0x03, 0x01, 0x01, 0x02}};

        byte[,] galoisFieldInverse = new byte[4, 4] {{0x0e, 0x0b, 0x0d, 0x09},
                                                     {0x09, 0x0e, 0x0b, 0x0d},
                                                     {0x0d, 0x09, 0x0e, 0x0b},
                                                     {0x0b, 0x0d, 0x09, 0x0e}};
        int RconIndex = 0;
        byte[,] Rcon = new byte[4, 10] {{0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36 },
                                        {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                                        {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                                        {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }};

        byte[,] keyExpansionMat = new byte[44, 4];

        string ConvertMatrixToString(byte[,] matrix)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var cell = Convert.ToString(matrix[j, i], 16);
                    if (cell.Length < 2)
                        str.Append("0" + cell);
                    else
                        str.Append(cell);
                }
            }
            return str.ToString().ToUpper().Insert(0, "0x");
        }

        byte[,] MakeStateByteMatrix(string str)
        {
            byte[,] matrix = new byte[4, 4];

            int k = 2;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string cell = "0x" + str[k] + str[k + 1];
                    matrix[j, i] = Convert.ToByte(cell, 16);
                    k += 2;
                }
            }
            return matrix;
        }

        byte[,] MakeKeyByteMatrix(string str)
        {
            byte[,] matrix = new byte[4, 4];

            int k = 2;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string cell = "0x" + str[k] + str[k + 1];
                    matrix[i, j] = Convert.ToByte(cell, 16);
                    k += 2;
                }
            }
            return matrix;
        }

        byte[] Rotword(byte[] word)
        {
            byte first = word[0];
            for (int i = 0; i < 3; i++)
                word[i] = word[i + 1];
            word[3] = first;
            return word;
        }
        byte[] SubByte(byte[] word)
        {
            byte[] arr = new byte[4];
            int newI;
            int newJ;
            for (int i = 0; i < 4; i++)
            {
                string cell = Convert.ToString(word[i], 16);
                if (cell.Length == 1)
                {
                    newI = 0;
                    newJ = Convert.ToInt32(cell[0].ToString(), 16);
                }
                else
                {
                    newI = Convert.ToInt32(cell[0].ToString(), 16);
                    newJ = Convert.ToInt32(cell[1].ToString(), 16);
                }
                arr[i] = sbox[newI, newJ];
            }

            return arr;
        }
        byte[] Xor(byte[] first, byte[] second, byte[] third, int isMultipleOf4)
        {
            byte[] arr = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                string tmp;
                if (isMultipleOf4 == 0)
                    tmp = Convert.ToString(first[i] ^ second[i], 16);
                else
                    tmp = Convert.ToString(first[i] ^ second[i] ^ third[i], 16);

                arr[i] = Convert.ToByte(tmp, 16);
            }

            return arr;
        }
        void WorkOnKey(string key)
        {
            byte[,] keyArr = new byte[4, 4];
            keyArr = MakeKeyByteMatrix(key);
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    keyExpansionMat[i, j] = keyArr[i, j];
        }
        byte[,] GetKeyMatrix(int index)
        {
            byte[,] matrix = new byte[4, 4];
            int row = 0, col = 0;
            for (int i = index * 4; i < index * 4 + 4; i++)
            {
                col = 0;
                for (int j = 0; j < 4; j++)
                {
                    matrix[col, row] = keyExpansionMat[i, j];
                    col++;
                }
                row++;
            }
            return matrix;
        }
        byte[,] AddRoundKey(byte[,] matrix, int roundNum)
        {
            byte[,] keyRound = GetKeyMatrix(roundNum);

            PrintMatrix(keyRound);
            PrintMatrix(matrix);

            string cell;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    cell = Convert.ToString(keyRound[i, j] ^ matrix[i, j], 16);
                    keyRound[i, j] = Convert.ToByte(cell, 16);
                }
            }

            return keyRound;
        }
        void ImplementKeyExpansion()
        {
            byte[] first = new byte[4];
            byte[] second = new byte[4];
            byte[] third = new byte[4];
            byte[] final = new byte[4];
            for (int i = 4; i < 44; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    first[j] = keyExpansionMat[i - 1, j];
                    second[j] = keyExpansionMat[i - 4, j];
                    if (RconIndex < 10)
                        third[j] = Rcon[j, RconIndex];
                }
                if (i % 4 == 0)
                {
                    RconIndex++;
                    first = Rotword(first);
                    first = SubByte(first);
                    final = Xor(first, second, third, 1);
                }
                else
                    final = Xor(first, second, third, 0);

                for (int j = 0; j < 4; j++)
                {
                    // Console.Write(keyExpansionMat[i,j]);
                    keyExpansionMat[i, j] = final[j];
                }
            }
        }
        void PrintKeyMatrix()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 44; j++)
                {
                    Console.Write(string.Join(", ", keyExpansionMat[j, i].ToString("X2")));
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
        void PrintMatrix(byte[,] mat)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.Write(string.Join(", ", mat[i, j].ToString("X2")));
                    Console.Write(" ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("");
        }

        byte[,] MixCols(byte[,] shiftedMatrix)
        {
            byte[] xorArray = new byte[4];
            byte[,] mixedColsMatrix = new byte[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (galoisField[j, k] == 1)
                        {
                            xorArray[k] = shiftedMatrix[k, i];
                        }
                        else if (galoisField[j, k] == 2)
                        {
                            xorArray[k] = MultiplyByTwo(shiftedMatrix[k, i]);
                        }
                        else if (galoisField[j, k] == 3)
                        {
                            xorArray[k] = MltiplyByThree(shiftedMatrix[k, i]);
                        }
                    }
                    var cell = xorArray[0] ^ xorArray[1] ^ xorArray[2] ^ xorArray[3];
                    mixedColsMatrix[j, i] = Convert.ToByte(cell);
                }
            }
            return mixedColsMatrix;
        }

        //Credit of this code snippet: Stack Overflow
        byte MultiplyByTwo(byte x)
        {
            byte ans;
            UInt32 temp = Convert.ToUInt32(x << 1);
            ans = (byte)(temp & 0xFF);
            if (x > 127)
                ans = Convert.ToByte(ans ^ 27);
            return ans;
        }

        byte MltiplyByThree(byte x)
        {
            //rule is: x*3 = (x*2) xor (x);
            byte multiplyedByTwo = MultiplyByTwo(x);
            return (Convert.ToByte(multiplyedByTwo ^ x));
        }

        byte[,] MixColsInverse(byte[,] shiftedmatrix)
        {
            List<byte> mixedMat = new List<byte>();
            byte[] arrayXor = new byte[4];
            byte[,] mixedColsMat = new byte[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (galoisFieldInverse[j, k] == 0x9)
                        {
                            byte x0 = shiftedmatrix[k, i];
                            byte x1 = MultiplyByTwo(x0);
                            byte x2 = MultiplyByTwo(x1);
                            byte x3 = MultiplyByTwo(x2);
                            arrayXor[k] = Convert.ToByte(x3 ^ x0);
                        }
                        if (galoisFieldInverse[j, k] == 0xB)
                        {
                            byte x0 = shiftedmatrix[k, i];
                            byte x1 = MultiplyByTwo(x0);
                            byte x2 = MultiplyByTwo(x1);
                            byte x3 = MultiplyByTwo(x2);
                            arrayXor[k] = Convert.ToByte(x3 ^ x0 ^ x1);
                        }
                        if (galoisFieldInverse[j, k] == 0xD)
                        {
                            byte x0 = shiftedmatrix[k, i];
                            byte x1 = MultiplyByTwo(x0);
                            byte x2 = MultiplyByTwo(x1);
                            byte x3 = MultiplyByTwo(x2);
                            arrayXor[k] = Convert.ToByte(x3 ^ x2 ^ x0);
                        }
                        if (galoisFieldInverse[j, k] == 0xE)
                        {
                            byte x0 = shiftedmatrix[k, i];
                            byte x1 = MultiplyByTwo(x0);
                            byte x2 = MultiplyByTwo(x1);
                            byte x3 = MultiplyByTwo(x2);
                            arrayXor[k] = Convert.ToByte(x3 ^ x2 ^ x1);
                        }
                    }
                    var cell = arrayXor[0] ^ arrayXor[1] ^ arrayXor[2] ^ arrayXor[3];
                    mixedColsMat[j, i] = Convert.ToByte(cell);
                }
            }
            return mixedColsMat;
        }

        byte[,] DoInitialRound(byte[,] state)
        {
            string cell;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    cell = Convert.ToString(state[j, i] ^ keyExpansionMat[i, j], 16);
                    state[j, i] = Convert.ToByte(cell, 16);
                }
            }
            return state;
        }
        byte[,] SubBytes(byte[,] matrix)
        {
            byte[,] resultMatrix = new byte[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string cell = Convert.ToString(matrix[i, j], 16);
                    int newI, newJ;
                    if (cell.Length == 1)
                    {
                        newI = 0;
                        newJ = Convert.ToInt32(cell[0].ToString(), 16);
                    }
                    else
                    {
                        newI = Convert.ToInt32(cell[0].ToString(), 16);
                        newJ = Convert.ToInt32(cell[1].ToString(), 16);
                    }

                    resultMatrix[i, j] = sbox[newI, newJ];
                }
            }

            return resultMatrix;
        }
        byte[,] SubBytesMatrixInverse(byte[,] matrix)
        {
            byte[,] newMatrix = new byte[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string cell = Convert.ToString(matrix[i, j], 16);
                    int newI, newJ;
                    if (cell.Length == 1)
                    {
                        newI = 0;
                        newJ = Convert.ToInt32(cell[0].ToString(), 16);
                    }
                    else
                    {
                        newI = Convert.ToInt32(cell[0].ToString(), 16);
                        newJ = Convert.ToInt32(cell[1].ToString(), 16);
                    }

                    newMatrix[i, j] = sboxInverse[newI, newJ];
                }
            }

            return newMatrix;
        }

        byte[] ShiftOneRow(byte[] row, int n)
        {
            byte[] newRow = new byte[4];
            int j = 0;
            for (int i = n; i < 4; i++)
                newRow[j++] = row[i];

            for (int i = 0; i < n; i++)
                newRow[j++] = row[i];

            return newRow;
        }
        byte[,] ShiftRows(byte[,] matrix)
        {
            byte[,] resultMatrix = new byte[4, 4];
            byte[] row = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    row[j] = matrix[i, j];
                }
                row = ShiftOneRow(row, i);
                for (int j = 0; j < 4; j++)
                {
                    resultMatrix[i, j] = row[j];
                }
            }
            return resultMatrix;
        }
        byte[] ShiftRowInverse(byte[] row, int n)
        {
            UInt32 number = 0;
            for (int i = 0; i < 4; i++)
            {
                number += Convert.ToUInt32(row[i]);
                if (i != 3) number = number << 8;
            }
            number = ((number >> (n * 8)) | (number) << (32 - (n * 8)));

            byte[] newRow = new byte[4];
            for (int i = 3; i >= 0; i--)
            {
                newRow[i] = (byte)(number & 0xFF);
                number = number >> 8;
            }
            return newRow;
        }
        byte[,] ShiftMatrixInverse(byte[,] matrix)
        {
            byte[,] newMatrix = new byte[4, 4];
            byte[] row = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    row[j] = matrix[i, j];
                }
                row = ShiftRowInverse(row, i);
                for (int j = 0; j < 4; j++)
                {
                    newMatrix[i, j] = row[j];
                }
            }
            return newMatrix;
        }
        byte[,] DoFinalRound(byte[,] state)
        {
            state = SubBytes(state);
            PrintMatrix(state);
            //PrintKeyMatrix();
            state = ShiftRows(state);
            PrintMatrix(state);
            //PrintKeyMatrix();
            state = AddRoundKey(state, 10);
            PrintMatrix(state);
            //PrintKeyMatrix();
            return state;
        }
        byte[,] DoFinalRoundDecrypt(byte[,] state)
        {
            state = AddRoundKey(state, 10);
            //PrintMatrix(state);
            //PrintKeyMatrix();
            state = ShiftMatrixInverse(state);
            //PrintMatrix(state);
            //PrintKeyMatrix();
            state = SubBytesMatrixInverse(state);
            //PrintMatrix(state);
            //PrintKeyMatrix();
            return state;
        }
        byte[,] DoMainRounds(byte[,] state, int round)
        {
            state = SubBytes(state);
            PrintMatrix(state);
            //PrintKeyMatrix();
            state = ShiftRows(state);
            PrintMatrix(state);
            //PrintKeyMatrix();
            state = MixCols(state);
            PrintMatrix(state);
            //PrintKeyMatrix();
            state = AddRoundKey(state, round);
            PrintMatrix(state);
            //PrintKeyMatrix();
            return state;
        }
        byte[,] DoMainRoundsDecrypt(byte[,] state, int round)
        {
            state = AddRoundKey(state, round);
            //PrintMatrix(state);
            state = MixColsInverse(state);
            //PrintMatrix(state);
            state = ShiftMatrixInverse(state);
            //PrintMatrix(state);
            state = SubBytesMatrixInverse(state);
            //PrintMatrix(state);
            return state;
        }
        byte[,] DoFirstRoundDecrypt(byte[,] state)
        {
            state = AddRoundKey(state, 0);
            return state;
        }
        public override string Decrypt(string cipherText, string key)
        {
            byte[,] state = MakeStateByteMatrix(cipherText);
            WorkOnKey(key);
            ImplementKeyExpansion();

            //phase 3
            state = DoFinalRoundDecrypt(state);
            PrintMatrix(state);
            //phase 2
            for (int i = 9; i > 0; i--)
            {
                state = DoMainRoundsDecrypt(state, i);
            }
            //phase 1
            state = DoFirstRoundDecrypt(state);
            PrintMatrix(state);

            var decryptedText = ConvertMatrixToString(state);
            return decryptedText;
        }

        public override string Encrypt(string plainText, string key)
        {
            byte[,] state = MakeStateByteMatrix(plainText);
            WorkOnKey(key);
            ImplementKeyExpansion();

            //phase 1
            state = DoInitialRound(state);
            //phase 2
            for (int roundNum = 1; roundNum <= 9; roundNum++)
            {
                state = DoMainRounds(state, roundNum);
            }
            //phase 3
            state = DoFinalRound(state);

            var encryptedText = ConvertMatrixToString(state);
            return encryptedText;
        }
        static void Main(string[] args)
        {
            var AES_obj = new AES();
            string output = AES_obj.Encrypt("0x54776F204F6E65204E696E652054776F", "0x5468617473206D79204B756E67204675");
            //Console.WriteLine();
            Console.WriteLine("-----------------------------------------");
        }
    }
}
