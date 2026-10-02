using System.Text;
using CoordinateUtilityServices;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

if (args.Length != 1)
{
    throw new ArgumentException(
        "Exactly one OCR diagnostic image path is required.");
}

string imagePath = args[0];

if (!File.Exists(imagePath))
{
    throw new FileNotFoundException(
        "The OCR diagnostic image was not found.",
        imagePath);
}

byte[] imageBuffer =
    await File.ReadAllBytesAsync(imagePath);

IOCRUtilityService ocrService =
    new OCRUtilityService();

OcrResult result =
    await ocrService.RecognizeAsync(
        imageBuffer,
        "zh-TW");

Console.WriteLine($"Image: {imagePath}");
Console.WriteLine($"Buffer length: {imageBuffer.Length}");
Console.WriteLine();

Console.WriteLine("=== Recognized Text ===");
Console.WriteLine(result.Text);
Console.WriteLine();

Console.WriteLine("=== OCR Geometry ===");

for (int lineIndex = 0;
     lineIndex < result.Lines.Count;
     lineIndex++)
{
    OcrTextLine line = result.Lines[lineIndex];

    Console.WriteLine(
        $"LINE[{lineIndex}] " +
        $"Text=\"{line.Text}\" " +
        $"Bounds={FormatBounds(line.Bounds)} " +
        $"Center={FormatCenter(line.Bounds)}");

    for (int wordIndex = 0;
         wordIndex < line.Words.Count;
         wordIndex++)
    {
        OcrTextWord word = line.Words[wordIndex];

        Console.WriteLine(
            $"    WORD[{wordIndex}] " +
            $"Text=\"{word.Text}\" " +
            $"Bounds={FormatBounds(word.Bounds)} " +
            $"Center={FormatCenter(word.Bounds)}");
    }
}

/// <summary>
/// Formats an OCR rectangle for diagnostic output.
/// </summary>
/// <param name="bounds">The rectangle to format.</param>
/// <returns>The formatted rectangle coordinates.</returns>
static string FormatBounds(Rectangle bounds)
{
    return
        $"({bounds.TopLeft.X}, {bounds.TopLeft.Y})-" +
        $"({bounds.BottomRight.X}, {bounds.BottomRight.Y})";
}

/// <summary>
/// Formats the center point of an OCR rectangle.
/// </summary>
/// <param name="bounds">The rectangle whose center is formatted.</param>
/// <returns>The formatted center coordinates.</returns>
static string FormatCenter(Rectangle bounds)
{
    return
        $"({bounds.Center.X}, {bounds.Center.Y})";
}