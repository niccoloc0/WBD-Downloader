using System;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        using (HttpClient client = new HttpClient())
        {
            while (true)
            {
                Console.Write("Please enter the image URL (or '.' to exit): ");
                string url = Console.ReadLine();

                if (url == ".")
                {
                    Console.WriteLine("Exiting the program.");
                    break;
                }

                string processedUrl = ProcessUrl(url);
                if (processedUrl != null)
                {
                    await DownloadImageAsync(client, processedUrl);
                }
                else
                {
                    Console.WriteLine("Invalid URL format.");
                }
            }
        }
    }

    static string ProcessUrl(string url)
    {
        const string baseUrl = "https://static.cdn.turner.com/";
        int startIndex = url.IndexOf("images/");
        
        if (startIndex >= 0)
        {
            int endIndex = url.IndexOf(".jpg", startIndex) + 4;

            if (endIndex > startIndex)
            {
                string imagePath = url.Substring(startIndex, endIndex - startIndex);
                return $"{baseUrl}{imagePath}";
            }
        }

        return null;
    }

    static async Task DownloadImageAsync(HttpClient client, string imageUrl)
    {
        try
        {
            byte[] imageBytes = await client.GetByteArrayAsync(imageUrl);
            string fileName = Path.GetFileName(imageUrl);
            
            string downloadsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string filePath = Path.Combine(downloadsFolder, fileName);
            
            await File.WriteAllBytesAsync(filePath, imageBytes);
            Console.WriteLine($"Image downloaded and saved as: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading image: {ex.Message}");
        }
    }
}