using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using BitMiracle.LibTiff.Classic;
using System.Drawing.Imaging;

namespace Neural.Utilities
{
    public class TiffConverter
    {
        public static int Split(string tiffFilePath, int splitPixelSize = 100)
        {
            using (Tiff image = Tiff.Open(tiffFilePath, "r"))
            {
                // Find the width and height of the image
                FieldValue[] value = image.GetField(TiffTag.IMAGEWIDTH);
                int width = value[0].ToInt();

                value = image.GetField(TiffTag.IMAGELENGTH);
                int height = value[0].ToInt();

                int imageSize = height * width;
                int[] raster = new int[imageSize];
                image.ReadRGBAImage(width, height, raster);
                int total = imageSize / splitPixelSize;
                int rowCount = 0;
                int colCount = 0;
                // Read the image into the memory width 
                while (100 >= rowCount * colCount)
                {
                    try
                    {
                        using (Bitmap bmp = new Bitmap(splitPixelSize, splitPixelSize))
                        {
                            for (int i = 0; i < bmp.Width; ++i)
                                for (int j = 0; j < bmp.Height; ++j)
                                {
                                    int x = i + (colCount * splitPixelSize);
                                    int y = j + (rowCount * splitPixelSize);

                                    int offset = (height - y - 1) * width + x;
                                    int red = Tiff.GetA(raster[offset]);
                                    int green = Tiff.GetR(raster[offset]);
                                    int blue = Tiff.GetG(raster[offset]);
                                    bmp.SetPixel(i, j, Color.FromArgb(red, green, blue));
                                }
                            bmp.Save(String.Format("test{0}x{1}.bmp", rowCount, colCount));
                        }
                        using (Bitmap bmp = new Bitmap(splitPixelSize, splitPixelSize))
                        {
                            for (int i = 0; i < bmp.Width; ++i)
                                for (int j = 0; j < bmp.Height; ++j)
                                {
                                    int x = i + (colCount * splitPixelSize);
                                    int y = j + (rowCount * splitPixelSize);

                                    int offset = (height - y - 1) * width + x;
                                    int red = Tiff.GetR(raster[offset]);
                                    int green = Tiff.GetG(raster[offset]);
                                    int blue = Tiff.GetB(raster[offset]);
                                    bmp.SetPixel(i, j, Color.FromArgb(red, green, blue));
                                }
                            bmp.Save(String.Format("test{0}x{1}_real.bmp", rowCount, colCount));
                        }

                    }
                    catch { }
                    finally
                    {
                        colCount++;
                        if (colCount % 10 == 0) {
                            colCount = 0;
                            rowCount++; }
                    }
                }
                return rowCount;
            }
        }
    }
}
