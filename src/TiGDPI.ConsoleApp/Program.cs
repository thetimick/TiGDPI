using Spectre.Console;
using Spectre.Console.Cli;
using System.Text;
using TiGDPI.ConsoleApp;
using TiGDPI.ConsoleApp.Commands.Main;
using TiGDPI.ConsoleApp.Helpers;

// ReSharper disable PossiblyImpureMethodCallOnReadonlyVariable

Console.OutputEncoding = Encoding.UTF8;
Console.Title = Constants.Titles.VeryShortTitle;
Console.BackgroundColor = ConsoleColor.Black;

var app = new CommandApp<MainCommand>();
app.Configure(conf =>
{
    // conf.AddCommand<OcherCommand>("main");

    conf.Settings.ApplicationName = $"{Constants.Titles.VeryShortTitle}.exe";
    conf.Settings.ApplicationVersion = Constants.Titles.VersionWithDate;

    // conf.AddExample("-f messages.iss", "-s rus", "-t eng");

    conf.Settings.ExceptionHandler = (exception, _) =>
    {
        var color = Constants.Colors.ErrorColor.ToHex();

        AnsiConsole.Clear();
        AnsiConsoleLib.ShowFiglet(Constants.Titles.VeryShortTitle, Justify.Center, Constants.Colors.ErrorColor);
        AnsiConsoleLib.ShowRule(Constants.Titles.FullTitle, Justify.Right, Constants.Colors.ErrorColor);

        AnsiConsole.MarkupLine($"\n> [bold #{color}]A fatal error has occurred in the operation of the program![/]\n");
        AnsiConsole.WriteException(exception, ExceptionFormats.ShortenEverything);

        AnsiConsole.Console.Input.ReadKey(true);
        return -1;
    };
});

return await app.RunAsync(args);