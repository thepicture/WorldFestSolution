using System;
using System.IO;
using System.Text;

namespace WorldFestSolution.WebAPI.Models.Filters
{
    public class SwearingFilter
    {
        /// <summary>
        /// Gets the swearing words from the root directory of the project 
        /// with the file name "SwearingAsBase64.txt". File content 
        /// must be encoded in Base64 using UTF-8, separated by \r\n. 
        /// If file is not found, returns null.
        /// </summary>
        private string[] SwearingWords
        {
            get
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                           "SwearingAsBase64.txt");
                if (!File.Exists(path))
                {
                    return null;
                }
                return Encoding.UTF8.GetString(
                    Convert.FromBase64String(
                        File.ReadAllText(path)))
                    .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public bool IsTextContainsSwearing(string text)
        {
            if (SwearingWords == null)
            {
                return false;
            }
            foreach (string swearingWord in SwearingWords)
            {
                if (text.IndexOf(swearingWord,
                                 StringComparison.OrdinalIgnoreCase) != -1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}