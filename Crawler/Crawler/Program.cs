using System;
using System.Text.RegularExpressions;

namespace Crawler // Note: actual namespace depends on the project name.
{
    public class Program
    {
        // ThisIsMethodName 
        public static async Task Main(string[] args)
        {
            if (args.Length == 0) throw new ArgumentNullException("You need to provide a valid URL");
            string url = args[0];
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new ArgumentNullException(url + " is an invalid URL");
            }

            Console.WriteLine("Connecting to: " + url);
            using HttpClient client = new HttpClient();
            try
            {
                using HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string context = await response.Content.ReadAsStringAsync();
                    Regex emailRx = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
                    var matches = emailRx.Matches(context)
                        .OfType<Match>()
                        .Select(m => m.Value)
                        .Distinct()
                        .ToList();

                    if (matches.Count > 0)
                    {
                        Console.WriteLine("Following email adresses were found:");
                        foreach (var match in matches)
                        {
                            Console.WriteLine(match);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No emails found");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd podczas łączenia z podanym adresem");
            }
        }
    }
}