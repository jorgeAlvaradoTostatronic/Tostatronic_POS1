using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maxima_Distribuidores_VS
{
    public struct Servidor
    {
        public string Ip;
        public string Password;
        public string User;
        public int Puerto;

        public Servidor(string ip, string password, string user, int puerto)
        {
            Ip = ip;
            Password = password;
            User = user;
            Puerto = puerto;
        }
    }

    public class InformacionServidor
    {
        public Servidor Servidor { get; set; }

        public InformacionServidor() { }

        public InformacionServidor(Servidor servidor)
        {
            Servidor = servidor;
        }
    }
}
