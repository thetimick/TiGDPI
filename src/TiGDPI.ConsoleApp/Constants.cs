using Spectre.Console;

// ReSharper disable MemberCanBePrivate.Global

namespace TiGDPI.ConsoleApp;

public static class Constants
{
    public static class Titles
    {
        /// <summary>
        /// *Версия программы* (v.1.0)
        /// </summary>
        public const string Version = "v.1.0";
        /// <summary>
        /// *Версия программы с датой* (v.1.0 (02.09.2022))
        /// </summary>
        public const string VersionWithDate = $"{Version} (21.09.2024)";
        /// <summary>
        /// *Название программы* (*Версия* (*дата*)) by *Разработчик*
        /// </summary>
        public const string FullTitle = $"TiGDPI ({VersionWithDate}) by the_timick";
        /// <summary>
        /// *Название программы* by *Разработчик*
        /// </summary>
        public const string ShortTitle = "TiGDPI by Timick";
        /// <summary>
        /// *Название программы*
        /// </summary>
        public const string VeryShortTitle = "TiGDPI";
        /// <summary>
        /// Имя лог-файла
        /// </summary>
        public const string LogFileName = $"{VeryShortTitle}.log";
        /// <summary>
        /// Имя файла конфигурации
        /// </summary>
        public const string ConfigFileName = $"{VeryShortTitle}.config";
    }

    public static class Colors
    {
        /// <summary>
        /// Основной цвет
        /// </summary>
        public static readonly Color MainColor = new(119, 141, 169);
        /// <summary>
        /// Второй цвет
        /// </summary>
        public static readonly Color SecondColor = new(65, 90, 119);
        /// <summary>
        /// Цвет успеха
        /// </summary>
        public static readonly Color SuccessColor = new(76, 149, 108);
        /// <summary>
        /// Цвет ошибки
        /// </summary>
        public static readonly Color ErrorColor = new(178, 58, 72);
    }
}