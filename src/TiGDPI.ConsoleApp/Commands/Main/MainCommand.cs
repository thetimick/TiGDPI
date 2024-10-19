using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using Spectre.Console;
using Spectre.Console.Cli;
using TiGDPI.ConsoleApp.Entities;
using TiGDPI.ConsoleApp.Helpers;

// ReSharper disable PossiblyImpureMethodCallOnReadonlyVariable
// ReSharper disable ClassNeverInstantiated.Global

namespace TiGDPI.ConsoleApp.Commands.Main;

public class MainCommand : AsyncCommand<MainCommandSettings>
{
    private const string DataUrl = "https://raw.githubusercontent.com/thetimick/TiGDPI/refs/heads/main/public/data.json";
    
    public override async Task<int> ExecuteAsync(CommandContext context, MainCommandSettings settings)
    {
        AnsiConsoleLib.ShowHeader();

        await AnsiConsole.Progress()
            .AutoRefresh(true)
            .Columns(
                new TaskDescriptionColumn { Alignment = Justify.Right },
                new ProgressBarColumn { IndeterminateStyle = new Style(Constants.Colors.SecondColor, Constants.Colors.MainColor) }
            )
            .StartAsync(
                async progress =>
                {
                    var color = Constants.Colors.MainColor.ToHex();
                    
                    progress.AddTask($"[#{color}]Загрузка данных...[/]")
                        .IsIndeterminate();

                    var data = await ObtainDataAsync();
                    
                    progress.AddTask($"[#{color}]Поиск лучшего обхода для YouTube...[/]")
                        .IsIndeterminate();

                    var urls = await File.ReadAllLinesAsync(Path.Combine(Environment.CurrentDirectory, "russia-youtube.txt"));
                    var result = await Check(data.Startup.DefaultWithYouTubeFix[0], urls);
                    AnsiConsole.WriteLine(result);
                    
                    progress.AddTask($"[#{color}]Запуск GoodbyeDPI...[/]")
                        .IsIndeterminate();
                }
            );
        
        AnsiConsole.WriteLine();
        AnsiConsoleLib.ShowRule("Успех! Нажмите любую клавишу, чтобы закрыть программму", Justify.Center, Constants.Colors.SuccessColor);
        
        await AnsiConsole.Console.Input.ReadKeyAsync(true, CancellationToken.None);
        return 0;
    }

    #region Private Methods
    private async Task<DataEntity> ObtainDataAsync()
    {
        using var client = new HttpClient();
        
        var dataString = await ObtainDataAsync(client, DataUrl);
        await File.WriteAllTextAsync(Path.Combine(Environment.CurrentDirectory, "current-data.json"), dataString);
        var data = JsonConvert.DeserializeObject<DataEntity>(dataString) ?? new DataEntity();
        
        var blackListString = await ObtainDataAsync(client, data.Urls.BlackListUrl);
        await File.WriteAllTextAsync(Path.Combine(Environment.CurrentDirectory, "russia-blacklist.txt"), blackListString);

        var youtubeListString = await ObtainDataAsync(client, data.Urls.YouTubeListUrl);
        await File.WriteAllTextAsync(Path.Combine(Environment.CurrentDirectory, "russia-youtube.txt"), youtubeListString);
        
        return data;
    }

    private async Task<bool> Check(string command, IList<string> urls)
    {
        var process = LaunchProcess(
            Path.Combine(Environment.CurrentDirectory, "x86_64", "goodbyedpi.exe"), 
            $@" {command} --blacklist ..\russia-blacklist.txt --blacklist ..\russia-youtube.txt"
        );

        foreach (var url in urls)
        {
            var reply = await PingAsync(url);
            if (reply?.Status != IPStatus.Success)
            {
                AnsiConsole.WriteLine("ERROR");
            }
        }

        await Task.Delay(5000);
        
        // process.Close();

        return true;
    }
    #endregion

    #region Private Methods (static)
    private static async Task<string> ObtainDataAsync(HttpClient client, string url)
    {
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
 
    private static Process LaunchProcess(string filename, string arguments)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = filename,
                Arguments = arguments,
                UseShellExecute = true
            }
        };
        process.Start();
        return process;
    }

    private static async Task<PingReply?> PingAsync(string url)
    {
        try
        {
            var reply = await new Ping().SendPingAsync(url);
            return reply;
        }
        catch
        {
            // ignored
        }

        return null;
    }
    #endregion
}