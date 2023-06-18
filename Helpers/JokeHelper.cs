using JokeApi.Models;

public class JokeHelper
{
    public static List<Joke> RetrieveFallbackJokesFromPropertiesFile()
    {

         var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
         var fallbackJokesFilePath = configuration["FallbackJokesFilePath"];

        var fallbackJokes = new List<Joke>();

        if (File.Exists(fallbackJokesFilePath))
        {
            var properties = File.ReadAllLines(fallbackJokesFilePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Split('='))
                .ToDictionary(parts => parts[0].Trim().Substring(4), parts => parts[1].Trim());

            foreach (var kvp in properties)
            {
                var joke = new Joke { Id = int.Parse(kvp.Key), Text = kvp.Value, Length = kvp.Value.Length };
                fallbackJokes.Add(joke);
            }
        }

        return fallbackJokes;
    }
}
