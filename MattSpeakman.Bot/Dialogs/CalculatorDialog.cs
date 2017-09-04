using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using MattSpeakman.Bot.Infrastructure;

namespace MattSpeakman.Bot.Dialogs
{
    [Serializable]
    public class CalculatorDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.PostAsync("Welcome to the Calculator");
            context.Wait(this.PerformCalculation);
            return Task.CompletedTask;
        }

        public async Task CalculatorInit(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            context.Wait(this.CalculatorInit);
        }

        public async Task PerformCalculation(IDialogContext context, IAwaitable<object> argument)
        {
            var activity = await argument as Activity;
            var data = activity.Text.Split(' ');
            if (activity.Text.Contains("+"))
            {
                var count = 0;
                foreach (var item in data)
                {
                    int num;
                    if (int.TryParse(item, out num))
                    {
                        count += num;
                    }
                }

                await context.PostAsync($"I calculated that as: {count}");
                context.Wait(this.PerformCalculation);
            }
            else
            {
                await context.PostAsync("I'm sorry. I haven't been taught how to do that yet. I will return you to the main menu.");
                DialogFactory.MakeInstance<RootDialog>().GiveUserChoices(context);
            }
        }
    }
}