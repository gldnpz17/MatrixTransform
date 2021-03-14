using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace MatrixTransform
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Write identity and instructions
            Console.WriteLine("Tugas Program Transformasi Matrix 2D");
            Console.WriteLine("Nama : Firdaus Bisma Suryakusuma");
            Console.WriteLine("NIM  : 19/444051/TK/49247");
            Console.WriteLine("1. Mohon pastikan terdapat file bernama \'input.png\' di dalam direktori yang sama dengan program. File output akan bernama \'output.png\'.");
            Console.WriteLine("2. Program hanya akan membaca pixel dengan warna hitam sehingga mohon pastikan R=0, G=0, dan B=0.");
            Console.WriteLine("3. Di dalam \'output.png\', hasil transformasi berwarna merah dan ditindih di atas gambar aslinya.");
            Console.WriteLine();

            // Input transformation matrix
            var transformationMatrices = new List<double[,]>();
            while (true)
            {
                Console.WriteLine("Select a transformation :");
                Console.WriteLine("1. Translation");
                Console.WriteLine("2. Rotation");
                Console.WriteLine("3. Scaling");
                Console.WriteLine("4. Shearing");
                Console.WriteLine("5. Input custom matrix");
                Console.Write("Choice : ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Translation direction (x/y) : ");
                        var direction = Console.Read();
                        Console.Write("Translation distance : ");
                        var distance = Console.Read();
                        break;

                    case "2":
                        break;

                    case "3":
                        break;

                    case "4":
                        break;
                        
                    case "5":
                        Console.WriteLine("Input transformation matrix (3x3) :");
                        var matrix = new double[3, 3];
                        for (int row = 0; row < 3; row++)
                        {
                            var rowElements = Console.ReadLine().Split(' ');
                            for (int col = 0; col < 3; col++)
                            {
                                matrix[row, col] = Convert.ToDouble(rowElements[col]);
                            }
                        }
                        transformationMatrices.Add(matrix);
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        Console.WriteLine();
                        continue;
                }

                // Prompt
                Console.Write("Start calculation(y)? or Enter another matrix(n) : ");
                var ans = Console.ReadLine();

                if (ans == "Y")
                {
                    break;
                }
            }

            // Load image.
            var inputBitmap = new Bitmap("./input.png");

            // Convert image
            var imagePixels = new List<double[,]>();
            int totalPixels = inputBitmap.Width * inputBitmap.Height;
            for (int y = 0; y < inputBitmap.Height; y++)
            {
                for (int x = 0; x < inputBitmap.Width; x++)
                {
                    if (inputBitmap.GetPixel(x, y).R == 0 && inputBitmap.GetPixel(x, y).G == 0 && inputBitmap.GetPixel(x, y).B == 0)
                    {
                        imagePixels.Add(new double[3, 1] {
                            { x },
                            { y },
                            { 1 }
                        });
                    }

                    OverwriteConsoleLine($"Converting pixels...({y * inputBitmap.Width + x}/{totalPixels})");
                } 
            }
            ClearConsoleLine();
            Console.WriteLine("Pixel conversion complete.");

            // Transform image pixels.
            var outputPixels = imagePixels;
            for(int pixelIndex = 0; pixelIndex < imagePixels.Count; pixelIndex++)
            {
                var pixel = outputPixels[pixelIndex];

                foreach (var transformationMatrix in transformationMatrices)
                {
                    outputPixels[pixelIndex] = (CrossProduct(transformationMatrix, pixel));
                }
                OverwriteConsoleLine($"Converting pixels...({pixelIndex}/{imagePixels.Count})");
            }
            ClearConsoleLine();
            Console.WriteLine("Pixel transformation complete.");

            // Render pixels
            // Copy original image and overlay with transformed image
            var outputBitmap = (Bitmap)inputBitmap.Clone();
            for(int pixelIndex = 0; pixelIndex < outputPixels.Count; pixelIndex++)
            {
                var pixel = outputPixels[pixelIndex];

                if (Convert.ToInt32(pixel[0,0]) < inputBitmap.Width && Convert.ToInt32(pixel[1, 0]) < inputBitmap.Height) 
                {
                    outputBitmap.SetPixel(Convert.ToInt32(pixel[0, 0]), Convert.ToInt32(pixel[1, 0]), Color.Red);
                }

                OverwriteConsoleLine($"Rendering pixels...({pixelIndex}/{imagePixels.Count})");
            }
            ClearConsoleLine();
            Console.WriteLine("Pixel rendering complete.");
            inputBitmap.Dispose();
            outputBitmap.Save("./output.png");

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public static double[,] CrossProduct(double[,] matrix1, double[,] matrix2)
        {
            var output = new double[matrix2.GetLength(0), matrix2.GetLength(1)];

            for (int row = 0; row < matrix1.GetLength(0); row++)
            {
                for (int col = 0; col < matrix2.GetLength(1); col++)
                {
                    for (int element = 0; element < matrix1.GetLength(1); element++)
                    {
                        output[row, col] += matrix1[row, element] * matrix2[element, col];
                    }
                }
            }

            return output;
        }

        public static void WriteMatrix(double[,] matrix)
        {
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    Console.Write($"{matrix[row, col]} ");
                }
                Console.WriteLine();
            }
        }

        public static void OverwriteConsoleLine(string line)
        {
            int currentLine = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(line);
            Console.Write(new string(' ', Console.WindowWidth - line.Length));
            Console.SetCursorPosition(0, currentLine);
        }

        public static void ClearConsoleLine()
        {
            int currentLine = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLine);
        }
    }
}
