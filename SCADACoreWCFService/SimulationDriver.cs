using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCADACoreWCFService
{
    // ovo je samo primer proste implementacjie sumulacionog drivera sa vrednostima koje se krecu od 0 do 100
    //ukoliko u sistemu postoji i RealTimeDriver, preporuka je da se koristi nasledjivanje ili implementacija interfejsa, zarad uniformnog pristupa ovim driverima

    public class SimulationDriver
    {
        public static double ReturnValue(string address)
        {
            // u ovoj implementaciji simulacionog drivera adrese su opisane (po uzoru na iFIX)
            // S - sine
            // C - cosine
            // R - ramp

            if(address == "S")
            {
                return Sine();
            }
            else if(address == "C")
            {
                return Cosine();
            }
            else if(address == "R")
            {
                return Ramp();
            }
            else
            {
                return -1000;
            }
        }

        private static double Sine()
        {
            return 100*Math.Sin((double)DateTime.Now.Second / 60 * Math.PI);
        }

        private static double Cosine()
        {
            return 100 * Math.Cos((double)DateTime.Now.Second / 60 * Math.PI);
        }

        private static double Ramp()
        {
            return 100 * DateTime.Now.Second / 60;
        }
    }
}