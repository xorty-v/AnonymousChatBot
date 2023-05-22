using AnonymousChatBot.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Chat = AnonymousChatBot.Models.Chat;

namespace AnonymousChatBot.Resourses;

internal static class DataBase
{
    private static IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsetting.json").Build();
    private static string connectionString = config["DataBase:PostgreSqlConnection"];


    // ------ Queue ------
    internal static async Task AddUserToQueue(long chatId)
    {
        using (var db = new NpgsqlConnection(connectionString))
        {
            if (!await IsUserExists(db, chatId))
            {
                var queue = new Queue { Id = Guid.NewGuid(), ChatId = chatId };
                var sql = @"INSERT INTO queues (id, chat_id) VALUES (@Id, @ChatId)";
                await db.ExecuteAsync(sql, queue);
            }
        }
    }

    internal static async Task DeleteUserFromQueue(long chatId)
    {
        using (var db = new NpgsqlConnection(connectionString))
        {
            if (await IsUserExists(db, chatId))
            {
                var sql = @"DELETE FROM queues WHERE chat_id = @ChatId";
                await db.ExecuteAsync(sql, new Queue { ChatId = chatId });
            }
        }
    }

    internal static async Task<long> GetUserFromQueue()
    {
        using (var db = new NpgsqlConnection(connectionString))
        {
            var sql = @"SELECT chat_id AS ChatId FROM queues";
            var response = await db.QueryFirstOrDefaultAsync<Queue>(sql);

            if (response != null)
                return response.ChatId;

            return 0;
        }
    }

    private static async Task<bool> IsUserExists(NpgsqlConnection db, long chatId)
    {
        var sqlExist = @"SELECT COUNT(*) FROM queues WHERE chat_id = @ChatId";
        var existingCount = await db.ExecuteScalarAsync<int>(sqlExist, new Queue { ChatId = chatId });
        return existingCount > 0;
    }

    // ------ Chat ------
    internal static async Task CreateChat(long chatOne, long chatTwo)
    {
        using (var db = new NpgsqlConnection(connectionString))
        {
            var chat = new Chat { ChatOne = chatOne, ChatTwo = chatTwo };
            var sql = @"INSERT INTO chats (chat_one, chat_two) VALUES (@ChatOne, @ChatTwo)";
            await db.ExecuteAsync(sql, chat);
        }
    }

    internal static async Task DeleteChat(long chatId)
    {
        using (var db = new NpgsqlConnection(connectionString))
        {
            var sqlSelect = @"SELECT chat_one AS ChatOne, chat_two AS ChatTwo 
                              FROM chats 
                              WHERE chat_one = @ChatOne OR chat_two = @ChatOne";
            var chat = await db.QuerySingleOrDefaultAsync<Chat>(sqlSelect, new Chat { ChatOne = chatId });

            if (chat != null)
            {
                var sql = @"DELETE FROM chats WHERE chat_one = @ChatOne OR chat_two = @ChatOne";
                await db.ExecuteAsync(sql, chat);
            }
        }
    }

    internal static async Task<long> GetActiveChat(long chatId)
    {
        using (var db = new NpgsqlConnection(connectionString))
        {
            var sql = @"SELECT chat_one AS ChatOne, chat_two AS ChatTwo 
                        FROM chats
                        WHERE chat_one = @ChatOne OR chat_two = @ChatTwo";
            var response =
                await db.QueryFirstOrDefaultAsync<Chat>(sql, new Chat { ChatOne = chatId, ChatTwo = chatId });

            if (response != null)
            {
                if (response.ChatTwo == chatId)
                {
                    return response.ChatOne;
                }
                else if (response.ChatOne == chatId)
                {
                    return response.ChatTwo;
                }
            }

            return 0;
        }
    }
}