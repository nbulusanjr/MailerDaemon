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
                FindItemResponse findItemResponse = service.FindItem(StandardFolder.Inbox);

                for (int i = 0; i < findItemResponse.Items.Count; i++)
                {
                    Console.WriteLine(findItemResponse.Items[i].Subject);
                    Console.WriteLine(findItemResponse.Items[i].ItemClass);
                    Console.WriteLine(findItemResponse.Items[i].ItemId);
                }

                Console.Read();
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
