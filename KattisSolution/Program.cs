using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using KattisSolution.IO;

namespace KattisSolution
{
    class Program
    {
        static void Main(string[] args)
        {
            Solve(Console.OpenStandardInput(), Console.OpenStandardOutput());
        }

        public static void Solve(Stream stdin, Stream stdout)
        {
            IScanner scanner = new OptimizedIntReader(stdin);
            // uncomment when you need more advanced reader
            // scanner = new Scanner(stdin);
            BufferedStdoutWriter writer = new BufferedStdoutWriter(stdout);

            var testCasesNo = scanner.NextInt();

            for (int testCase = 0; testCase < testCasesNo; testCase++)
            {
                Debug.WriteLine("Working on test case {0}", testCase);

                var n = scanner.NextInt();
                var k = scanner.NextInt();

                var matrix = new int[n, k];

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < k; j++)
                    {
                        matrix[i, j] = scanner.NextInt();
                    }
                }

                int result = SolveCodes(ref matrix);
                writer.Write(result);
                writer.Write("\n");
            }


            writer.Flush();
        }

        private static int SolveCodes(ref int[,] matrix)
        {
            char[] stringToEncode = new char[matrix.GetLength(1)];
            int nonZeroMin = matrix.GetLength(0);
            int zeroCount = 0;
            double stringsCount = Math.Pow(2, matrix.GetLength(1));

            for (int i = 0; i < stringsCount; i++)
            {
                Convert.ToString(i, 2).PadLeft(matrix.GetLength(1), '0').CopyTo(0, stringToEncode, 0, stringToEncode.Length);
                for (int s = 0; s < stringToEncode.Length; s++)
                {
                    stringToEncode[s] -= '0';
                }

                int sum = MultiplyMatrix(ref matrix, ref stringToEncode, nonZeroMin);

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

        public static int MultiplyMatrix(ref int[,] a, ref char[] b, int currentMin)
        {
            Debug.Assert(a.GetLength(1) == b.GetLength(0), "Number of columns in First Matrix should be equal to Number of rows in Second Matrix.");

            int sum = 0, c;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                c = 0;
                for (int k = 0; k < a.GetLength(1); k++)
                {
                    if (a[i, k] == 1 && b[k] == 1)
                    {
                        c = (c + 1) % 2;
                    }
                }
                sum += c;
                if (sum > currentMin)
                    return sum;
            }

            return sum;
        }
    }
}
