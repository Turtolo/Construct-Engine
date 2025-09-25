using System;
using System.Threading.Tasks;

namespace ConstructEngine.Util
{
    public static class Timer
    {
        public static async void Wait(float seconds, Action callback)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));
            callback?.Invoke();
        }

        public static async Task WaitAsync(float seconds)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));
        }
    }
}