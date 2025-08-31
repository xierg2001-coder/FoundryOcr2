using System;
using System.IO;
using System.Threading.Tasks;

namespace FoundryOcr.Cli;

public static class Program
{
    [STAThread]
    public static async Task<int> Main()
    {
        try
        {
            // Open the standard input stream to read the binary image data.
            using var inputStream = Console.OpenStandardInput();
            using var memoryStream = new MemoryStream();
            
            // Copy the piped data into a memory stream.
            await inputStream.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();

            if (imageBytes.Length == 0)
            {
                Console.Error.WriteLine("Error: No image data was received via standard input.");
                return 1;
            }

            // Call the service method that handles a byte array.
            var jsonResult = await OcrService.RecognizeAsJsonFromBytesAsync(imageBytes, indented: true);

            // Write the JSON result to the standard output.
            Console.WriteLine(jsonResult);
            return 0; // Success
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An unexpected error occurred: {ex.Message}");
            return 1;
        }
    }
}