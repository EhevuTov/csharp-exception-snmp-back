using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;

namespace csharp_exception_snmp
{
    class Program
    {
        static void testFunc()
        {
            int i = 0;
            if (i == 0)
            {
               // throw new System.ArgumentException("Parameter cannot be 0", "original");
                throw new System.Exception("Something real bad happen!");
            }

        }

        static void sendTrap(IPAddress address)
        {

            Messenger.SendTrapV2(0, VersionCode.V2, new IPEndPoint(address, 162),
                new OctetString("public"),
                new ObjectIdentifier(new uint[] { 1, 3, 6 }),
                0,
                new List<Variable>());
        }
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello, SNMP!");
            IPAddress address = args.Length == 1 ? IPAddress.Parse(args[0]) : IPAddress.Loopback;
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
            try
            {
                testFunc();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                sendTrap(address);
                Console.Write("Press any key to continue . . . ");
                Console.ReadKey(true);
            }
        }
    }
}
