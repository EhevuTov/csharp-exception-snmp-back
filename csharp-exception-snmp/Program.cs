using System;
using System.Diagnostics;           // for system event logging
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

using Lextm.SharpSnmpLib;           // for SNMP
using Lextm.SharpSnmpLib.Messaging; // for SNMP traps

namespace csharp_exception_snmp
{
    public class ExceptionEvent
    {
        private string sSource;
		private string sLog;
		private string sEvent;
        public ExceptionEvent(){
            sSource = "dotNETSampleApp";
            sLog = "Application";
            sEvent = "Sample Event";
            // Create the source, if it does not already exist. 
           if (!EventLog.SourceExists(sSource))
            {
                //An event log source should not be created and immediately used. 
                //There is a latency time to enable the source, it should be created 
                //prior to executing the application that uses the source. 
                //Execute this sample a second time to use the new source.
                EventLog.CreateEventSource(sSource, sLog);
                Console.WriteLine("CreatedEventSource");
                Console.WriteLine("Exiting, execute the application a second time to use the source.");
                // The source is created.  Exit the application to allow it to be registered. 
                return;
            }

            // Create an EventLog instance and assign its source.
            EventLog myLog = new EventLog();
            myLog.Source = sSource;

            // Write an informational entry to the event log.    
            myLog.WriteEntry("Writing to event log.");

        }
        public void sendEvent(){
		if (!EventLog.SourceExists(sSource))
            EventLog.CreateEventSource(sSource,sLog);

            EventLog.WriteEntry(sSource,sEvent);
            EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 234);
        }
    }

    class Program
    {
        // function that throws the test throw
        static void testFunc()
        {
            int i = 0;
            if (i == 0)
            {
                throw new System.Exception("Something real bad happen!");
            }

        }

        // SNMP send function
        static void sendTrap(IPAddress address)
        {
            List<Variable> list = new List<Variable>();
            //new Variable(new uint[] { 1, 3, 6, 1 }, 
            //list.Add();
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
                ExceptionEvent exception = new ExceptionEvent();
                exception.sendEvent();
                Console.WriteLine(e.ToString()); //write to console
                sendTrap(address); //write to SNMP
                Console.Write("Press any key to continue . . . ");
                Console.ReadKey(true);
            }
        }
    }
}
