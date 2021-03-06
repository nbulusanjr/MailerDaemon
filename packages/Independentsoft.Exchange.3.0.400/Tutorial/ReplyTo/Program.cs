using System;
using System.Net;
using Independentsoft.Exchange;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworkCredential credential = new NetworkCredential("username", "password");
            Service service = new Service("https://myserver/ews/Exchange.asmx", credential);

            try
            {
                IsEqualTo restriction = new IsEqualTo(MessagePropertyPath.Subject, "Test");

                FindItemResponse inboxResponse = service.FindItem(StandardFolder.Inbox, restriction);

                for (int i = 0; i < inboxResponse.Items.Count; i++)
                {
                    ItemId currentMessageId = inboxResponse.Items[i].ItemId;

                    ReplyItem replyItem = new ReplyItem(currentMessageId);
                    replyItem.NewBody = new Body("This is reply message body text.");

                    ItemInfoResponse response = service.Reply(replyItem);
                }
            }
            catch (ServiceRequestException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("Error: " + ex.XmlMessage);
                Console.Read();
            }
            catch (WebException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.Read();
            }
        }
    }
}
