using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace InveComv1.Bases
{
    public static class TestInternetConnection
    {
        /// <summary>
        /// Chequeo de conexion
        /// </summary>
        /// <returns> true si hay conexion, false si no hoy conexion</returns>
        public static bool TestInternet ()
        {
            Ping p = new Ping();
            PingReply reply = p.Send("www.bcv.org.ve", 5000);
            if (reply.Status == IPStatus.Success)
            {
                return true;
            }
            else return false;
        }

            
    }
}
