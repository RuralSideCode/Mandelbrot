using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Drawing;
using System.IO;

namespace Mandelbrotset {
	class Program {

		static int width = 1000;
		static int height = width;

		static int Maxiterations = 100;

		static double maxz = 2;
		static double minz = -2;

		static string filepath = Directory.GetCurrentDirectory();

		static void Main(string[] args) {

			Console.Title = "Mandelbrot Fractal Generator";

			getSettings();

			Console.Clear();

			Console.WriteLine("Starting...");

            Bitmap B1 = GenerateQuadrent(true, true);
            Bitmap B2 = GenerateQuadrent(false, true);
            Bitmap B3 = GenerateQuadrent(false, false);
            Bitmap B4 = GenerateQuadrent(true, false);

            Bitmap image = CombineQuadrents(B1, B2, B3, B4);

			try{
			image.Save(filepath + "/Mandelbrot" + width + "X" + height + "i" + Maxiterations + "min" + minz + "max" + maxz + ".png");
			} catch {
				Console.WriteLine("Filepath does not exist");
				Console.WriteLine("Saving at default path");
				image.Save(Directory.GetCurrentDirectory() + "/Mandelbrot" + width + "X" + height + "i" + Maxiterations + "min" + minz + "max" + maxz + ".png");
			}

			Console.Clear();

			Console.WriteLine("Done!");
		}

		static void getSettings() {

			Console.WriteLine("Create a Fractal! Press enter for defaults");

			Console.WriteLine("");

			Console.WriteLine("Enter size of image (pixels): ");
			string response = Console.ReadLine();
			if (response != String.Empty){
				width = Convert.ToInt32(response);
				height = Convert.ToInt32(response);
			}
			else
				return;

			Console.WriteLine("Enter interations: ");
			Maxiterations = Convert.ToInt32(Console.ReadLine());

			Console.WriteLine("Enter min offset/zoom (press enter for default): ");
			response = Console.ReadLine();
			if (response == String.Empty) {
				maxz = 2;
				minz = -2.5;
			} else {
				minz = Convert.ToDouble(response);

				Console.WriteLine("Enter max offset/zoom: ");
				maxz = Convert.ToDouble(Console.ReadLine());
			}
			Console.WriteLine("Paste a filepath (press enter for defalt): ");
			response = Console.ReadLine();
			if (response != String.Empty)
				filepath = response;
		}

        /// <summary>
        /// Generates one quadrent of the mandelbrot set
        /// </summary>
        /// <param name="isleft">Is Left half of quadrent</param>
        /// <param name="isup">Is Upper half of the quadrent</param>
        /// <returns></returns>
        static Bitmap GenerateQuadrent (bool isleft, bool isup) {

            Bitmap quadrent = new Bitmap(width / 2, height / 2);

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {

                    int _x = x;
                    int _y = y;

                    if (isleft == IsLeft(x) && isup == IsUp(y)) {

                        int iterations = CalculateIterations(x, y);

                        int color = (int)map(iterations, 0, Maxiterations, 0, 255);

                        if (!isleft) _x -= width / 2;
                        if (!isup) _y -= height / 2;

                        quadrent.SetPixel(_x, _y, Color.FromArgb(255, color, color, color));
                    }

                }
            }

            return quadrent;
        }


        /// <summary>
        /// Combines quadrents into one bitmap (I II III IV)
        /// </summary>
        /// <param name="B1">Bitmap 1</param>
        /// <param name="B2">Bitmap 2</param>
        /// <param name="B3">Bitmap 3</param>
        /// <param name="B4">Bitmap 4</param>
        /// <returns></returns>
        static Bitmap CombineQuadrents(Bitmap B1, Bitmap B2, Bitmap B3, Bitmap B4) {

            Bitmap image = new Bitmap(width, height);

            for(int x = 0; x < width/2; x++) {
                for(int y = 0; y < height/2; y++) {

                    image.SetPixel(x, y, B1.GetPixel(x, y));
                    image.SetPixel(x + width/2, y + height/2, B3.GetPixel(x, y));
                    image.SetPixel(x + width/2, y, B2.GetPixel(x, y));
                    image.SetPixel(x, y + height/2, B4.GetPixel(x, y));
                }
            }

            return image;
        }

        static bool IsLeft(int x) {

            if (x < width / 2) return true;
            else return false;
        }
        static bool IsUp(int y) {

            if (y < height / 2) return true;
            else return false;
        }


        static int CalculateIterations(double x, double y) {

            int w = width;
            int h = height;

            double x0 = map(x, 0, w, minz, maxz);
            double y0 = map(y, 0, h, minz, maxz);

            double cx = x0;
            double cy = y0;

            int iterations = 0;

            while (iterations < Maxiterations) {

                double _x0 = x0 * x0 - y0 * y0;
                double _y0 = 2 * x0 * y0;

                x0 = _x0 + cx;
                y0 = _y0 + cy;

                if (x0 * x0 + y0 * y0 > 2) {
                    break;
                }

                iterations += 1;
            }

            return iterations;
        }

		static double map(double value, double fromSource, double toSource, double fromTarget, double toTarget) {
			return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
		}


	}
}
