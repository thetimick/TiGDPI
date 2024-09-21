namespace TiGDPI.ConsoleApp.Entities;

public record DataEntity
{
    public record UrlsEntity
    {
        public string BlackListUrl { get; set; } = "https://p.thenewone.lol/domains-export.txt";
        public string YouTubeListUrl { get; set; } = "";
    }

    public record StartupEntity
    {
        public string Default { get; set; } = "-9";
        public List<string> DefaultWithYouTubeFix { get; set; } = [
            "-9 --fake-gen 5 --fake-from-hex 160301FFFF01FFFFFF0303594F5552204144564552544953454D454E542048455245202D202431302F6D6F000000000009000000050003000000",
            "-5 -e1 -q --fake-gen 5 --fake-from-hex 160301FFFF01FFFFFF0303594F5552204144564552544953454D454E542048455245202D202431302F6D6F000000000009000000050003000000"
        ];

        public string StartupAsApp = """
                                     @ECHO OFF
                                     PUSHD "%~dp0"
                                     set _arch=x86
                                     IF "%PROCESSOR_ARCHITECTURE%"=="AMD64" (set _arch=x86_64)
                                     IF DEFINED PROCESSOR_ARCHITEW6432 (set _arch=x86_64)
                                     PUSHD "%_arch%"
                                     
                                     start "" goodbyedpi.exe %%commands%% --blacklist ..\russia-blacklist.txt --blacklist ..\russia-youtube.txt
                                     
                                     POPD
                                     POPD
                                     """;

        public string StartupAsService = """
                                         @ECHO OFF
                                         PUSHD "%~dp0"
                                         set _arch=x86
                                         IF "%PROCESSOR_ARCHITECTURE%"=="AMD64" (set _arch=x86_64)
                                         IF DEFINED PROCESSOR_ARCHITEW6432 (set _arch=x86_64)
                                         
                                         echo This script should be run with administrator privileges.
                                         echo Right click - run as administrator.
                                         echo Press any key if you're running it as administrator.
                                         
                                         sc stop "GoodbyeDPI"
                                         sc delete "GoodbyeDPI"
                                         sc create "GoodbyeDPI" binPath= "\"%CD%\%_arch%\goodbyedpi.exe\" %%commands%% --blacklist \"%CD%\russia-blacklist.txt\" --blacklist \"%CD%\russia-youtube.txt\"" start= "auto"
                                         sc description "GoodbyeDPI" "Passive Deep Packet Inspection blocker and Active DPI circumvention utility"
                                         sc start "GoodbyeDPI"
                                         
                                         POPD
                                         """;

        public string RemoveService = """
                                      @ECHO OFF
                                      echo This script should be run with administrator privileges.
                                      echo Right click - run as administrator.
                                      echo Press any key if you're running it as administrator.
                                      
                                      sc stop "GoodbyeDPI"
                                      sc delete "GoodbyeDPI"
                                      sc stop "WinDivert"
                                      sc delete "WinDivert"
                                      sc stop "WinDivert14"
                                      sc delete "WinDivert14"
                                      """;
    }
    
    public UrlsEntity Urls { get; set; } = new();
    public StartupEntity Startup { get; set; } = new();
}

