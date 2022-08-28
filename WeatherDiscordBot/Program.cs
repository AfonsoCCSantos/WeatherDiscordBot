using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using WeatherDiscordBot.CommandHandlers;
using WeatherDiscordBot.Models;

namespace WeatherDiscordBot
{
    public class Program
    {   
        private DiscordSocketClient _discordClient;
        private HttpClient _httpClient;

        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync()
        {
            SetupDiscordClient();
            var token = Environment.GetEnvironmentVariable("TokenName");
            await InitDiscordClient(token);

            _httpClient = new HttpClient();

            await Task.Delay(-1);
            _httpClient.Dispose();
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
            var cityParameter = CreateSlashCommandOptions("city", "The city of which we want the weather",
                                                            ApplicationCommandOptionType.String);
            await CreateGlobalSlashCommand(SetupGlobalSlashCommand(new SlashCommandModel("weather-in",
                                                                   "Gives information about the weather in the given city",
                                                                   cityParameter.Options)));

        }

        private static SlashCommandOptionBuilder CreateSlashCommandOptions(string name, string description,
                                                                           ApplicationCommandOptionType type)
        {
            var userInfoOption = new SlashCommandOptionBuilder();
            userInfoOption.AddOption(name, type, description, isRequired: true);
            return userInfoOption;
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
            await handler.Handle(slashCommand, _httpClient);
        }

        private async Task InitDiscordClient(string? token)
        {
            await _discordClient.LoginAsync(TokenType.Bot, token);
            await _discordClient.StartAsync();
        }
    }
}