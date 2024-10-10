using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            _messageAvailable.WaitOne();
            lock (_lockObject)
            {
                if (messenger.Count > 0)
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

            return null;
        }
    }
}
