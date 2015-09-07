using System;
using System.Diagnostics;
using System.IO;
using KattisSolution.IO;

namespace KattisSolution
{
    class Program
    {
        private static int _n;
        private static int _k;
        private static int[,] _matrix;
        private static char[] _stringToEncode;

        static void Main(string[] args)
        {
            Solve(Console.OpenStandardInput(), Console.OpenStandardOutput());
        }

        public unsafe static void Solve(Stream stdin, Stream stdout)
        {
            IScanner scanner = new OptimizedIntReader(stdin);
            BufferedStdoutWriter writer = new BufferedStdoutWriter(stdout);

            var testCasesNo = scanner.NextInt();

            for (int testCase = 0; testCase < testCasesNo; testCase++)
            {
                Debug.WriteLine("Working on test case {0}", testCase);

                _n = scanner.NextInt();
                _k = scanner.NextInt();

                _matrix = new int[_n, _k];
                _stringToEncode = new char[_k];

                fixed (int* intPtrMatrixFixed = _matrix)
                fixed (char* charPtrStringToEncode = _stringToEncode)
                {
                    for (int i = 0; i < _k * _n; i++)
                    {
                        intPtrMatrixFixed[i] = scanner.NextInt();
                    }

                    int result = SolveCodes(intPtrMatrixFixed, charPtrStringToEncode);
                    writer.Write(result);
                    writer.Write("\n");
                }
            }

            writer.Flush();
        }


        private unsafe static int SolveCodes(int* fixedMatrixPtr, char* fixedStringPtr)
        {
            int nonZeroMin = _n;
            int zeroCount = 0;
            double stringsCount = Math.Pow(2, _k);

            for (int i = 0; i < stringsCount; i++)
            {
                Convert.ToString(i, 2).PadLeft(_k, '0').CopyTo(0, _stringToEncode, 0, _k);

                int sum = MultiplyMatrix(fixedMatrixPtr, fixedStringPtr, nonZeroMin);

                if (sum == 0)
                {
                    zeroCount++;
                    if (zeroCount > 1)
                        return 0;
                }
                else if (sum < nonZeroMin)
                {
                    nonZeroMin = sum;
                }
            }

            return zeroCount > 1 ? 0 : nonZeroMin;
        }

        public unsafe static int MultiplyMatrix(int* fixedMatrixPtr, char* fixedStringPtr, int currentMin)
        {
            Debug.Assert(_matrix.GetLength(1) == _stringToEncode.GetLength(0), "Number of columns in First Matrix should be equal to Number of rows in Second Matrix.");

            int sum = 0, c;
            int* mPtr = fixedMatrixPtr;
            char* bPtr = fixedStringPtr;

            for (int i = 0; i < _n; i++)
            {
                c = 0;
                for (int j = 0; j < _k; j++)
                {
                    if (*mPtr == 1 && *bPtr == '1')
                    {
                        c = (c + 1) % 2;
                    }

                    mPtr++;
                    bPtr++;
                }
                bPtr = fixedStringPtr;

                sum += c;
                if (sum > currentMin)
                    return sum;
            }
            return sum;
        }
    }
}
