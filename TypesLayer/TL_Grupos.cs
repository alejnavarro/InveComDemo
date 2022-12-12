using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypesLayer
{
    public class TL_Grupos
    {
        private int idGrupo;
        private string nombreGrupo;

        public int IdGrupo { get => idGrupo; set => idGrupo = value; }
        public string NombreGrupo { get => nombreGrupo; set => nombreGrupo = value; }
    }
}
