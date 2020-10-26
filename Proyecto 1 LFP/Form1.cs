using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_1_LFP {
    public partial class Form1 : Form {
        Analizador analizador = new Analizador();
        int codigoPagina = 0;
        string rutaGeneral = @"C:\prograPruebas";
        Boolean siguiente = false;
        public Form1() {
            InitializeComponent();
            if (!Directory.Exists(rutaGeneral)) {
                Directory.CreateDirectory(rutaGeneral);
            }
            figura.SizeMode = PictureBoxSizeMode.StretchImage;
            figura.Image = null;
            pBandera.SizeMode = PictureBoxSizeMode.StretchImage;
            pBandera.Image = null;

        }

        private void NuevaPestañaToolStripMenuItem_Click(object sender, EventArgs e) {
            siguiente = true;
            codigoPagina += 1;
            pCodigo.TabPages.Add("Pestaña " + codigoPagina);
        }

        private void Button1_Click(object sender, EventArgs e) {

            if (siguiente == false) {
                MessageBox.Show("Primero Crea una Ventana y Carga un Archivo");
            } else {
                analizador.revisarCaja(figura, lPais, lHabitantes, pBandera);
            }

        }

        private void Button2_Click(object sender, EventArgs e) {
            string rimg = rutaGeneral + "\\imagen.png";
            if (File.Exists(rimg)) {
                analizador.crearPDF();
            } else {
                MessageBox.Show("Primero Genera una Imagen");
            }
        }

        private void CargarArchivoToolStripMenuItem_Click(object sender, EventArgs e) {
            if (pCodigo.TabPages.Count == 0) {
                MessageBox.Show("Inserta Primero una Pestaña");

            } else {
                analizador.cargarArchivo(pCodigo);

            }
        }

        private void MenuToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void GuardarArchivoToolStripMenuItem_Click(object sender, EventArgs e) {
            analizador.guardarArchivo();

        }

        private void BImagen_Click(object sender, EventArgs e) {


        }

        private void Label1_Click(object sender, EventArgs e) {

        }

        private void SalirToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        private void AcerdaDeToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Raul Quiñonez - 201503903");
        }
    }
}
