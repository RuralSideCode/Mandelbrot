using System;

using System.Drawing;
using System.IO;

namespace Mandelbrotset {
	class Program {

		static int width = 1000;
		static int height = width;

		static int Maxiterations = 100;

		static double maxz = 2;
		static double minz = -2.5;

		static string filepath = Directory.GetCurrentDirectory();

		static void Main(string[] args) {

			Console.Title = "Mandelbrot Fractal Generator";

			getSettings();

			Bitmap image = new Bitmap(width, height);

			Console.Clear();

			Console.WriteLine("Starting...");

			image = generateImage(image);

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

		static Bitmap generateImage(Bitmap img) {

			int w = img.Width;
			int h = img.Height;

			Console.WriteLine("Creating Fractal...");

			Console.WriteLine("Size: " + w +" X " + h);

			Console.WriteLine("");

			Console.WriteLine("Please leave me alone if you want me to run correctly!");

			int pixelsDone = 0;

			int pixelsTotal = w * h;

			for(int x = 0; x < w; x++) {
				for(int y = 0; y < h; y++) {

					double x0 = map(x, 0, w, minz, maxz);
					double y0 = map(y, 0, h, minz, maxz);

					double cx = x0;
					double cy = y0;

					int iterations = 0;

					while(iterations < Maxiterations) {

						double _x0 = x0 * x0 - y0 * y0;
						double _y0 = 2 * x0 * y0;

						x0 = _x0 + cx;
						y0 = _y0 + cy;

						if(x0 * x0 + y0 * y0 > 2) {
							break;
						}

						iterations += 1;
					}

					int color = (int)map(iterations, 0, Maxiterations, 0, 255);

					img.SetPixel(x, y, Color.FromArgb(255, color, color, color));

					pixelsDone += 1;

					Console.Title = "Generating... " + (pixelsDone*100 / pixelsTotal) + "%";

				}
			}

			return img;

		}



		static double map(double value, double fromSource, double toSource, double fromTarget, double toTarget) {
			return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
		}


	}
}
