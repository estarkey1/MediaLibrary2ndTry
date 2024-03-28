using System;
using System.IO;
using System.Linq;
using NLog;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        string path = Directory.GetCurrentDirectory() + "\\nlog.config";
        var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
        logger.Info("Program started");

        string scrubbedFile = "movies.scrubbed.csv";
        var movieFile = new MovieFile(scrubbedFile);

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("1) Add Movie");
            Console.WriteLine("2) Display All Movies");
            Console.WriteLine("3) Find Movie");
            Console.WriteLine("Enter 'quit' to exit");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddMovie(movieFile);
                    break;
                case "2":
                    DisplayAllMovies(movieFile.Movies);
                    break;
                case "3":
                    FindMovie(movieFile);
                    break;
                case "quit":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        logger.Info("Program ended");
    }

    static void AddMovie(MovieFile movieFile)
    {
        Console.WriteLine("Enter movie title:");
        string title = Console.ReadLine();

        Console.WriteLine("Enter genres separated by '|', e.g., Action|Adventure:");
        List<string> genres = Console.ReadLine().Split('|').ToList();

        Console.WriteLine("Enter movie director:");
        string director = Console.ReadLine();

        Console.WriteLine("Enter running time in format h:mm:ss:");
        TimeSpan runningTime;
        while (!TimeSpan.TryParseExact(Console.ReadLine(), @"h\:mm\:ss", null, out runningTime))
        {
            Console.WriteLine("Invalid input. Please enter running time in format h:mm:ss:");
        }

        var newMovie = new Movie()
        {
            title = title,
            genres = genres,
            director = director,
            runningTime = runningTime
        };

        movieFile.AddMovie(newMovie);
    }

    static void DisplayAllMovies(List<Movie> movies)
    {
        foreach (var movie in movies)
        {
            Console.WriteLine($"Id: {movie.mediaId}");
            Console.WriteLine($"Title: {movie.title}");
            Console.WriteLine($"Director: {movie.director}");
            Console.WriteLine($"Run time: {movie.runningTime}");
            Console.WriteLine($"Genres: {string.Join(", ", movie.genres)}\n");
        }
    }

    static void FindMovie(MovieFile movieFile)
    {
        Console.WriteLine("Enter the title of the movie to search:");
        string searchTitle = Console.ReadLine();
        List<Movie> searchResults = movieFile.SearchByTitle(searchTitle);
        
        Console.WriteLine($"Search Results ({searchResults.Count}):");
        DisplayAllMovies(searchResults);
    }
}