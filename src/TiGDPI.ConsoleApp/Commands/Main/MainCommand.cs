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
                new TaskDescriptionColumn
                {
                    Alignment = Justify.Right
                },
                new ProgressBarColumn
                {
                    IndeterminateStyle = new Style(Constants.Colors.SecondColor, Constants.Colors.MainColor)
                }
            )
            .StartAsync(
                async progress =>
                {
                    var color = Constants.Colors.MainColor.ToHex();
                    
                    progress.AddTask($"[#{color}]Загрузка данных...[/]")
                        .IsIndeterminate();
                    
                    var data = new DataEntity();
                    var str = JsonConvert.SerializeObject(data, Formatting.Indented);
                    await File.WriteAllTextAsync(Path.Combine(Environment.CurrentDirectory, "data.json"), str);
                    
                    await Task.Delay(2000);
                    
                    progress.AddTask($"[#{color}]Сравнение параметров...[/]")
                        .IsIndeterminate();
                    
                    await Task.Delay(2000);
                    
                    progress.AddTask($"[#{color}]Запуск GoodbyeDPI...[/]")
                        .IsIndeterminate();
                    
                    await Task.Delay(2000);
                }
            );
        
        AnsiConsole.WriteLine();
        AnsiConsoleLib.ShowRule("Успех! Нажмите любую клавишу, чтобы закрыть программму", Justify.Center, Constants.Colors.SuccessColor);
        
        await AnsiConsole.Console.Input.ReadKeyAsync(true, CancellationToken.None);
        return 0;
    }

    #region Private Methods
    private async Task<string> ObtainData()
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(DataUrl);
        return await response.Content.ReadAsStringAsync();
    }
    #endregion
}