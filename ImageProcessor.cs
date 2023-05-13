using System;
using System.Collections.Generic;
using System.IO;
using SkiaSharp;
namespace aiExplainer;
public class ImageService
{
    private int _squares = 0;
    public ImageService()
    {
    }

    public SKBitmap LoadImage(string filePath)
    {
        return SKBitmap.Decode(filePath);
    }

    public List<SKBitmap> SplitImage(SKBitmap image, int squares)
    {
        _squares = squares;
        var width = image.Width / squares;
        var height = image.Height / squares;

        var list_of_squares = new List<SKBitmap>();
        for (var y = 0; y < image.Height; y += height)
        {
            for (var x = 0; x < image.Width; x += width)
            {
                var square = new SKBitmap(width, height);
                var srcRect = new SKRectI(x, y, x + width, y + height);
                image.ExtractSubset(square, srcRect);
                list_of_squares.Add(square);
            }
        }

        return list_of_squares;
    }

    public List<SKBitmap> RearrangeSquares(List<SKBitmap> squares, int rearrangements)
    {
        var random = new Random();
        for (int i = 0; i < rearrangements; i++)
        {
            var index1 = random.Next(squares.Count);
            var index2 = random.Next(squares.Count);
            var temp = squares[index1];
            squares[index1] = squares[index2];
            squares[index2] = temp;
        }

        return squares;
    }

    public SKBitmap MergeSquares(List<SKBitmap> squares)
    {
        var image = new SKBitmap(squares[0].Width * _squares, squares[0].Height * _squares);
        using var canvas = new SKCanvas(image);

        int i = 0;
        for (var y = 0; y < image.Height; y += squares[0].Height)
        {
            for (var x = 0; x < image.Width; x += squares[0].Width)
            {
                var destRect = new SKRectI(x, y, x + squares[i].Width, y + squares[i].Height);
                canvas.DrawBitmap(squares[i++], destRect);
            }
        }

        return image;
    }

    // method to save SKBitmap to file
    public void SaveImage(SKBitmap image, string filePath)
    {
        using var imageStream = File.OpenWrite(filePath);
        image.Encode(imageStream, SKEncodedImageFormat.Png, 100);
    }
}
