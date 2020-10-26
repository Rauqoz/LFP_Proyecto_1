using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_1_LFP {
    class errores {

        public String palabrarError { get; set; }

        public int filaError { get; set; }

        public int columnaError { get; set; }

        public errores(string palabrarError, int filaError, int columnaError)
        {
            this.palabrarError = palabrarError;
            this.filaError = filaError;
            this.columnaError = columnaError;
        }
    }
}
