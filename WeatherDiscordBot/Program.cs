using Discord;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using WeatherDiscordBot.CommandHandlers;
using WeatherDiscordBot.Models;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services.AddHttpClient("GeocodingAPIClient", httpClient =>
        {
            httpClient.BaseAddress = new Uri("http://api.openweathermap.org/geo/1.0/direct?");
            httpClient.DefaultRequestHeaders.Add(API_KEY_HEADER, Environment.GetEnvironmentVariable("APIKey"));
        }))
    .Build();

namespace WeatherDiscordBot
{
    public class Program
    {
        private DiscordSocketClient _discordClient;
        private const string API_KEY_HEADER = "appid";

        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync()
        {
            SetupDiscordClient();
            var token = Environment.GetEnvironmentVariable("TokenName");
            await InitDiscordClient(token);
            await Task.Delay(-1);
        }

        private void SetupDiscordClient()
        {
            DiscordSocketConfig config = new()
            {
                UseInteractionSnowflakeDate = false
            };

            _discordClient = new DiscordSocketClient(config);

            _discordClient.Log += Log;
            _discordClient.Ready += AddAllGlobalSlashCommands;
            _discordClient.SlashCommandExecuted += SlashCommandHandler;
        }

        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        public async Task AddAllGlobalSlashCommands()
        {
            
        }

        private static SlashCommandBuilder SetupGlobalSlashCommand(SlashCommandModel commandModel)
        {
            var globalCommand = new SlashCommandBuilder();
            globalCommand.WithName(commandModel.Name);
            globalCommand.WithDescription(commandModel.Description);
            if (commandModel.Options != null)
            {
                globalCommand.AddOptions(commandModel.Options.ToArray());
            }
            return globalCommand;
        }

        private async Task CreateGlobalSlashCommand(SlashCommandBuilder globalCommand)
        {
            try
            {
                await _discordClient.CreateGlobalApplicationCommandAsync(globalCommand.Build());
            }
            catch (HttpException exception)
            {
                LogErrorToConsole(exception);
            }
        }

        private static void LogErrorToConsole(HttpException exception)
        {
            var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
            Console.WriteLine(json);
        }

        private async Task SlashCommandHandler(SocketSlashCommand slashCommand)
        {
            var handler = CommandHandlerFactory.GetCommandHandler(slashCommand.Data.Name);
            await handler.Handle(slashCommand);
        }

        private async Task InitDiscordClient(string? token)
        {
            await _discordClient.LoginAsync(TokenType.Bot, token);
            await _discordClient.StartAsync();
        }
    }
}