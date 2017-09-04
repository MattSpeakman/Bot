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
    /// <summary>
    /// The calculator dialog. Responsible for allowing the user to perform a calculation
    /// </summary>
    [Serializable]
    public class CalculatorDialog : IDialog<object>
    {
        /// <summary>
        /// Base StartAsync Method. Responsible for adding the <see cref="PerformCalculation(IDialogContext, IAwaitable{object})"/> to
        /// the top of the call stack
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task StartAsync(IDialogContext context)
        {
            // Display a friendly message
            context.PostAsync("Welcome to the Calculator");

            // Add the PerformCalculation to the top of the call stack
            context.Wait(this.PerformCalculation);

            // Return
            return Task.CompletedTask;
        }

        /// <summary>
        /// Allow the user to perform a calculation
        /// </summary>
        /// <param name="context"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public async Task PerformCalculation(IDialogContext context, IAwaitable<object> argument)
        {
            // Await the user's interaction
            var activity = await argument as Activity;

            // Split the data by spaces
            var data = activity.Text.Split(' ');

            // If the user has added a + symbol then they want to perform an addition
            // TODO: Parse the data using regex to create a pattern for performing more complicated calculations
            if (activity.Text.Contains("+"))
            {
                var count = 0;
                foreach (var item in data)
                {
                    int num;
                    // If the current item is a number then perform a calculation
                    if (int.TryParse(item, out num))
                    {
                        count += num;
                    }
                }

                // Send the calculation back to the user
                await context.PostAsync($"I calculated that as: {count}");

                // Add this method back to the call stack for the user to make another calculation
                context.Wait(this.PerformCalculation);
            }

            // If the user tries to do something that isn't supported in this Dialog then display the Choices available
            else
            {
                await context.PostAsync("I'm sorry. I haven't been taught how to do that yet. I will return you to the main menu.");
                DialogFactory.MakeInstance<RootDialog>().GiveUserChoices(context);
            }
        }
    }
}