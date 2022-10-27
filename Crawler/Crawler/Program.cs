using System;
using System.Text.RegularExpressions;

namespace Crawler // Note: actual namespace depends on the project name.
{
    public class Program
    {
        // ThisIsMethodName 
        public static async Task Main(string[] args)
        {
            if (args.Length == 0) throw new ArgumentNullException("Musisz podać poprawny adres URL");
            string url = args[0];
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new ArgumentException(url + " Nie jest poprawnym adresem URL");
            }
            using HttpClient client = new HttpClient();
            using HttpResponseMessage response = await client.GetAsync(url);
            Console.WriteLine("Trwa łączenie z: " + url);
            try
            {
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
                        Console.WriteLine("Następujące adresy email zostały znalezione:");
                        foreach (var match in matches)
                        {
                            Console.WriteLine(match);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nie znaleziono adresów email");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd w czasie pobierania strony");
            }
            finally
            {
                client.Dispose();
                response.Dispose();
            }
            
        }
    }
}