using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using MattSpeakman.Bot.Infrastructure;
using MattSpeakman.Bot.Enumerations;

namespace MattSpeakman.Bot.Dialogs
{
    /// <summary>
    /// The <see cref="RootDialog"/> dialog. Responsible for asking the user to make a function selection and redirecting to that dialog
    /// </summary>
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        /// <summary>
        /// The base StartAsync method called when the object is instantiated
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task StartAsync(IDialogContext context)
        {
            // Display a friendly message
            context.PostAsync("Hello");

            // Add the MessageReceivedAsync to the top of the call stack
            context.Wait(this.MessageReceivedAsync);

            // Return the Task
            return Task.CompletedTask;
        }

        /// <summary>
        /// This method is responsible for giving the user a list of options to choose from
        /// </summary>
        /// <param name="context"></param>
        public void GiveUserChoices(IDialogContext context)
        {
            PromptDialog.Choice<Triggers>(context, // The current context
            this.UserMadeSelection, // The method to be added to the top of the call stack once the user has made a choice
            new[] { Triggers.Root, Triggers.Calculator }, // An array of options
            "Please select from these functions", // A friendly message
            "Invalid selection", // A message to display if the user attempts to do something invalid
            promptStyle: PromptStyle.Auto); // Default to the standard prompt message
        }

        /// <summary>
        /// This method is responsible for giving the user a list of options to choose from by calling <see cref="GiveUserChoices(IDialogContext)"/>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            this.GiveUserChoices(context);
        }

        /// <summary>
        /// Once the user has made a selection then direct the user to the relevant dialog
        /// </summary>
        /// <param name="context"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        private async Task UserMadeSelection(IDialogContext context, IAwaitable<Triggers> argument)
        {
            // Await the user's interaction
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



}