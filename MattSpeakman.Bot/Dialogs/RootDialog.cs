using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using MattSpeakman.Bot.Infrastructure;

namespace MattSpeakman.Bot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.PostAsync("Hello");
            context.Wait(this.MessageReceivedAsync);

            return Task.CompletedTask;
        }

        public void GiveUserChoices(IDialogContext context)
        {
            PromptDialog.Choice<Triggers>(context,
            this.UserMadeSelection,
            new[] { Triggers.Root, Triggers.Calculator },
            "Please select from these functions",
            "Invalid selection",
            promptStyle: PromptStyle.Auto);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            this.GiveUserChoices(context);
        }

        private async Task UserMadeSelection(IDialogContext context, IAwaitable<Triggers> argument)
        {
            var arg = await argument;

            switch (arg)
            {
                case Triggers.Root:
                    await context.PostAsync("Returning to the root menu");
                    this.GiveUserChoices(context);

                    break;

                case Triggers.Calculator:
                    await context.PostAsync("Welcome to the calculator");
                    await context.PostAsync("Please post your calculation in the form X + Y");

                    var calculatorDialog = DialogFactory.MakeInstance<CalculatorDialog>();
                    context.Wait(calculatorDialog.PerformCalculation);

                    break;
                default:
                    break;
            }
        }
    }

    public enum Triggers
    {
        Root = 0,
        Calculator
    }

}