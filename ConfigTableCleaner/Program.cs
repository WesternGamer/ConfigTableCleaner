using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace ConfigTableCleaner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            foreach (string arg in args)
            {
                try
                {
                    string fileContent = File.ReadAllText(arg);
                    string modifiedfileContent = RemoveCommentsAndSpaces(fileContent);

                    // Write the modified file content back to the file
                    File.WriteAllText(arg, modifiedfileContent);

                    Console.WriteLine("Comments removed, spaces replaced successfully, and empty lines removed.");
                    Console.WriteLine($"For: {arg}");
                    Console.WriteLine($"SHA256 Checksum: {SHA256CheckSum(arg)}");
                    Console.WriteLine("");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred when parsing: {arg}");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("");
                }
            }

            Console.ReadLine();
        }

        public static string RemoveCommentsAndSpaces(string fileContent)
        {
            // Split input into lines
            string[] lines = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            // Process each line
            for (int i = 0; i < lines.Length; i++)
            {
                // Remove leading and trailing whitespace
                lines[i] = lines[i].Trim();

                // Remove comments starting with #
                lines[i] = Regex.Replace(lines[i], @"#.*", "");

                // Remove comments starting with //
                lines[i] = Regex.Replace(lines[i], @"//.*", "");

                // Replace multiple spaces with a single space
                lines[i] = Regex.Replace(lines[i], @"\s+", " ");
            }

            // Remove null or empty lines
            lines = Array.FindAll(lines, line => !string.IsNullOrEmpty(line));

            // Join processed lines into a single string
            string processedText = string.Join(Environment.NewLine, lines);

            return processedText;
        }

        public static string SHA256CheckSum(string filePath)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    return BitConverter.ToString(sha256.ComputeHash(fileStream)).Replace("-", "");
                }
            }
        }
    }
}
