using Microsoft.Bot.Builder.Dialogs;

namespace MattSpeakman.Bot.Infrastructure
{
    /// <summary>
    /// Dialog factory
    /// </summary>
    public static class DialogFactory
    {
        /// <summary>
        /// Make an instance of a class that implements <see cref="IDialog"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T MakeInstance<T>() where T : IDialog<object>, new()
        {
            return new T();
        }
    }
}