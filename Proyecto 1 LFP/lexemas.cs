using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_1_LFP {
    class lexemas {

        public string palabras { get; set; }
        public string tipoPalabra { get; set; }

        public lexemas(string palabras, string tipoPalabra)
        {
            this.palabras = palabras;
            this.tipoPalabra = tipoPalabra;
        }
    }
}
