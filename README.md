# DiscordManager
해당 DiscordManager 는 C#으로 디스코드 봇을 처음 만들어보는 "초심자"를 위해 만들어졌습니다.

현재 여러 편의 기능들을 구현중에 있습니다.

해당 ReadMe.md에 적혀있는 기본 사용법은 최신 버전이 아닙니다. 

추후 업데이트 예정이니 사용법이 궁금하신 분들은
 Magma_Dev#7247 (Discord) DM
 GPLNature (Twitter)
로 부탁드립니다.

# ENG
This DiscordManager Project target is C# beginner for develop Discord Bots!

Currently, several convenience functions are being Develop.

Let me know if there is a convenience feature you want to add

The basic usage writed in this `ReadMe.md` File is not the latest version.
To be updated later

[![NuGet](https://img.shields.io/nuget/vpre/DiscordManager.svg?maxAge=2592000?style=plastic)](https://www.nuget.org/packages/DiscordManager)

Designed as a motive of [Addons.Interactive](https://github.com/foxbot/Discord.Addons.Interactive)

### 기본 사용법

```cs
// Warning
// Do not reference this code
// Not latest version of this code
class Program {
 static void Main() {
  var discordManager = DiscordBuilder
   .SocketBuilder // 샤드용 ShardBuilder 도 있습니다.
   .WithActivity(new Game("Live For Test"))
   .WithLogLevel(LogLevel.ALL)
   .WithCommandModule() // 만약 DiscordManager가 제공하는 명령어모듈을 사용하고 싶다면 해당 메소드를 사용해주세요.
   .Build();

  discordManager.Log += Log;
  discordManager.Run("Bot Token");
 }

 public static Task Log(LogObject logObject) {
  Console.WriteLine(logObject.ToString());
  return Task.CompletedTask;
 }
}
```

### 명령어모듈

```cs
// Warning
// Do not reference this code
// Not latest version of this code
public class Test: CommandModule {
 [CommandName("Test")]
 public void TestMethod() {
  _ = Reply("Test");
 }
}
```

~~Builder에서 WithCommandModule을 사용해주시고 명령어 메소드들이 있을 클래스를 생성합니다.~~

~~이때 해당 클래스는 CommandModule을 무조건 상속받아야합니다.~~

~~DiscordManager의 명령어 시스템이 자동으로 명령어들을 찾기 위함이며~~

~~명령어 시스템은 CommandModule에 있는 public 접근제어자를 가진 메소드만 명령어 메소드로 인식합니다.~~

~~이때 명령어 메소드는 명령어이름이 필요하기때문에 CommandName이 필요합니다.~~

~~만약 로딩 중에 명령어 메소드에 CommandName 없다면 오류를 발생합니다.~~
