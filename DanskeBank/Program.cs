using System;
using System.IO;
using System.Linq;

namespace DanskeBank
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine($"Answer: {Solve("Structure.txt")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }


        private static int Solve(string file)
        {
            (int Value, bool)[][] pyramid = Initialize(file);

            // Iterate from the bottom up, starting with 2nd to bottom row of the structure.
            for (int i = pyramid.Length - 2; i >= 0; i--)
            {
                for (int j = 0; j < pyramid[i].Length; j++)
                {
                    (int, bool IsEven) current = pyramid[i][j];
                    (int Value, bool IsEven) down = pyramid[i + 1][j];
                    (int Value, bool IsEven) diag = pyramid[i + 1][j + 1];

                    //Since the above are ValueTuples, original structure will not be affected.
                    down.Value = current.IsEven == down.IsEven ? 0 : down.Value;
                    diag.Value = current.IsEven == diag.IsEven ? 0 : diag.Value;

                    // Current node needs to be invalidated in the original structure whenever both sub-paths are 0'ed out.
                    if (down.Item1 == 0 && down.Item1 == diag.Item1)
                        pyramid[i][j].Value = 0;
                    else
                        pyramid[i][j].Value += Math.Max(down.Value, diag.Value);
                }
            }

            return pyramid[0][0].Value;
        }

        /// <summary>
        /// Attempts to parse a provided .txt file to a two-dimensional array of tuples, where Item1 is the actual value 
        /// and Item2 is a flag to indicate whether that number is an even one.
        /// </summary>
        /// <param name="file"></param>
        /// <exception cref="ArgumentNullException">Thrown when an invalid file name is provided.</exception>
        /// <returns>A two-dimensional array of tuples (int, bool).</returns>
        private static (int, bool)[][] Initialize(string file)
        {
            if (!File.Exists(file))
                throw new ArgumentNullException(nameof(file), $"File [{file}] could not be found.");

            return File.ReadAllLines(file)
                .Select(ln => ln.Trim().Split(" ").Select(i => (int.Parse(i)))
                .Select(x => (x, x % 2 == 0 ? true : false)).ToArray())
                .ToArray();
        }
    }
}
