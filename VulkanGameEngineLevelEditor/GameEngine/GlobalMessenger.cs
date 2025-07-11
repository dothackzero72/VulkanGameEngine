using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public static class GlobalMessenger
    {
        static public List<MessengerModel> messenger = new List<MessengerModel>();
        private static readonly object _lockObject = new object();
        private static readonly ManualResetEvent _messageAvailable = new ManualResetEvent(false);
        private static List<string> UnloggedMessages = new List<string>();

        static public void AddMessenger(MessengerModel model)
        {
            lock (_lockObject)
            {
                messenger.Add(model);
                _messageAvailable.Set();
            }
        }

        public static MessengerModel WaitForMessenger()
        {
            var messengerModel = messenger.First();
            messenger.Remove(messengerModel);

            if (messenger.Count == 0)
            {
                _messageAvailable.Reset();
            }

            return messengerModel;
        }
    }
}
