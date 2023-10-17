﻿using AnonymousChatBot.Domain.Entities;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnonymousChatBot.Domain;

public class BotKeyboardResponses
{
    public static InlineKeyboardMarkup ChooseInterests(List<Interest> allInterests, List<Interest> userInterests)
    {
        var interestButtons = new List<InlineKeyboardButton[]>();
        var row = new List<InlineKeyboardButton>();

        foreach (var interest in allInterests)
        {
            var isSelected = userInterests.Any(i => i.Id == interest.Id);
            var checkmark = isSelected ? "✅" : "";

            var buttonText = $"{checkmark} {interest.Name}";
            var interestCallbackData = $"interest_{interest.Id}";

            row.Add(InlineKeyboardButton.WithCallbackData(buttonText, interestCallbackData));

            // Если в ряду уже 2 кнопки или это последний интерес, добавьте ряд в клавиатуру.
            if (row.Count == 2 || interest == allInterests.Last())
            {
                interestButtons.Add(row.ToArray());
                row.Clear();
            }
        }

        interestButtons.Add(new[] { InlineKeyboardButton.WithCallbackData("❌ Сбросить интересы", "reset") });
        return new InlineKeyboardMarkup(interestButtons);
    }
}