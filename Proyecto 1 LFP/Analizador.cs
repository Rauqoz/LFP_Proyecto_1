using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_1_LFP {

    class Analizador {
        public List<lexemas> buenas = new List<lexemas>();
        public List<errores> malas = new List<errores>();
        public String[] palabrasReservadas = { "nombre", "grafica", "continente", "pais", "poblacion", "saturacion", "bandera" };
        OpenFileDialog archivo;
        SaveFileDialog guardar;
        RichTextBox caja;
        int filaE = 0;
        int columnaE = 0;
        string leer;
        Boolean acceder = false;
        string continente;
        StringBuilder archivoDot;

        public void cargarArchivo(TabControl pagina) {
            acceder = true;
            buenas.Clear();
            malas.Clear();
            archivo = new OpenFileDialog();
            archivo.DefaultExt = "txt";
            archivo.Filter = "Archivos de texto (*.txt)|*.txt";

            if (archivo.ShowDialog() == DialogResult.OK) {
                pagina.SelectedTab.BackColor = Color.White;
                caja = new RichTextBox();
                caja.Height = 551;
                caja.Width = 434;
                pagina.SelectedTab.Controls.Add(caja);
                StreamReader leer;
                leer = new StreamReader(archivo.FileName);
                while (leer.Peek() > -1) {
                    //peek revisa el siguiente caracter y tiene que ser mayor a -1 que representa que no hay nada
                    String linea = leer.ReadLine().Trim();

                    if (!String.IsNullOrEmpty(linea)) {
                        caja.AppendText(linea + "\n");

                    }
                }
                Console.WriteLine("Linea Versin Beta -----");
            } else {
                MessageBox.Show("Error Archivo no Seleccionado");

            }
        }

        public void analizarArchivo(PictureBox figura, Label lPais, Label lHabitantes, PictureBox pBandera) {

            buenas.Clear();
            malas.Clear();
            int opcion = 0;
            leer = caja.Text.Trim();
            int comprobarNumero = 0;
            String palabraArmada = "";
            Boolean sumoPalabra = false;
            filaE = 0;
            columnaE = 0;
            filaE += 1;
            caja.Clear();
            char[] letra = leer.Trim().ToArray();
            Console.WriteLine(letra[0]);
            for (int i = 0; i < letra.Length; i++) {
                //Console.WriteLine(palabraArmada + " " + letra[i] + " " + opcion);
                switch (opcion) {
                    case 0:
                        if (char.IsLetter(letra[i])) {
                            opcion = 1;
                            i--;
                        } else if (char.IsDigit(letra[i])) {
                            opcion = 2;
                            i--;
                        } else if (char.IsWhiteSpace(letra[i])) {
                            opcion = 0;

                        } else {
                            opcion = 3;
                            i--;
                        }
                        break;

                    case 1:
                        if (char.IsLetter(letra[i])) {
                            columnaE += 1;
                            palabraArmada += letra[i];
                            caja.SelectionColor = Color.Blue;
                            caja.AppendText(letra[i].ToString());
                            caja.SelectionColor = caja.ForeColor;
                            opcion = 1;

                        } else {
                            //revisar signo
                            opcion = 0;
                            i--;

                        }
                        break;

                    case 2:
                        if (char.IsDigit(letra[i])) {
                            columnaE += 1;
                            palabraArmada += letra[i];
                            caja.SelectionColor = Color.Green;
                            caja.AppendText(letra[i].ToString());
                            caja.SelectionColor = caja.ForeColor;
                            opcion = 2;

                        } else {
                            //revisar signo
                            opcion = 0;
                            i--;
                        }
                        break;

                    case 3:
                        if (letra[i].Equals(':')) {
                            caja.SelectionColor = Color.Gold;
                            caja.AppendText(letra[i].ToString());
                            caja.SelectionColor = caja.ForeColor;
                            for (int j = 0; j < palabrasReservadas.Length; j++) {
                                if (palabrasReservadas[j].Equals(palabraArmada.ToLower())) {
                                    buenas.Add(new lexemas(palabraArmada, "Reservada"));
                                    Console.WriteLine("Es Reservada");
                                    sumoPalabra = true;
                                    break;
                                } else {
                                    sumoPalabra = false;
                                }
                            }
                            buenas.Add(new lexemas(letra[i].ToString(), "Simbolo"));
                            columnaE += 1;
                            //revisar palabra
                            if (sumoPalabra == false) {
                                malas.Add(new errores(palabraArmada, filaE, columnaE));
                            }
                            sumoPalabra = false;
                            palabraArmada = "";
                            opcion = 0;

                        } else if (letra[i].Equals('{') | letra[i].Equals('}')) {
                            caja.SelectionColor = Color.Red;
                            caja.AppendText(letra[i].ToString() + "\n");
                            caja.SelectionColor = caja.ForeColor;
                            //empieza de nuevo
                            buenas.Add(new lexemas(letra[i].ToString(), "Simbolo"));
                            filaE += 1;
                            columnaE = 0;
                            palabraArmada = "";
                            opcion = 0;
                        } else if (letra[i].Equals('"')) {
                            caja.SelectionColor = Color.Gold;
                            caja.AppendText(letra[i].ToString());
                            caja.SelectionColor = caja.ForeColor;
                            //viene una cadena
                            columnaE += 1;
                            buenas.Add(new lexemas(letra[i].ToString(), "Simbolo"));
                            palabraArmada = "";
                            opcion = 4;
                        } else if (letra[i].Equals('%')) {
                            caja.SelectionColor = Color.Black;
                            caja.AppendText(letra[i].ToString());
                            caja.SelectionColor = caja.ForeColor;
                            columnaE += 1;
                            if (int.TryParse(palabraArmada, out comprobarNumero)) {
                                //guarda el numero
                                buenas.Add(new lexemas(palabraArmada, "Numero"));
                            } else {
                                //va a error
                                malas.Add(new errores(palabraArmada, filaE, columnaE));
                            }
                            buenas.Add(new lexemas(letra[i].ToString(), "Simbolo"));
                            palabraArmada = "";
                            opcion = 0;
                        } else if (letra[i].Equals(';')) {
                            caja.SelectionColor = Color.Orange;
                            caja.AppendText(letra[i].ToString() + "\n");
                            caja.SelectionColor = caja.ForeColor;
                            //termino la linea
                            if (palabraArmada.Length != 0 && int.TryParse(palabraArmada, out comprobarNumero)) {
                                buenas.Add(new lexemas(palabraArmada, "Numero"));
                            } else if (palabraArmada.Length != 0 && !int.TryParse(palabraArmada, out comprobarNumero)) {
                                buenas.Add(new lexemas(palabraArmada, "Cadena"));
                            }

                            buenas.Add(new lexemas(letra[i].ToString(), "Simbolo"));
                            palabraArmada = "";
                            filaE += 1;
                            columnaE = 0;
                            opcion = 0;
                        }
                        break;

                    case 4:
                        caja.SelectionColor = Color.Gold;
                        caja.AppendText(letra[i].ToString());
                        caja.SelectionColor = caja.ForeColor;
                        opcion = 4;
                        columnaE += 1;
                        if (letra[i].Equals('"')) {
                            //cierra la cadena
                            buenas.Add(new lexemas(palabraArmada, "Cadena"));
                            buenas.Add(new lexemas(letra[i].ToString(), "Simbolo"));
                            opcion = 0;
                            palabraArmada = "";
                        } else {
                            palabraArmada += letra[i];
                        }
                        break;
                    default:
                        break;

                }

            }

            generarHtml();
            armarDot(figura, lPais, lHabitantes, pBandera);

        }

        public void guardarArchivo() {
            if (acceder == true) {
                guardar = new SaveFileDialog();
                guardar.Filter = "Archivos de texto (*.txt)|*.txt";
                guardar.DefaultExt = "txt";
                guardar.ShowDialog();
                if (guardar.FileName != "") {
                    File.WriteAllText(guardar.FileName, caja.Text);
                }
            } else {
                MessageBox.Show("No hay Texto");
            }


        }

        public void generarHtml() {
            //string ruta = "C:\\PruebasGenerales";
            //if (Directory.Exists(ruta) == false) {
            //    Directory.CreateDirectory(ruta);
            //}

            string webB = Path.Combine(Application.StartupPath, "Practica1 Buenas.html");
            string webM = Path.Combine(Application.StartupPath, "Practica1 Malas.html");
            string inicio = "<html>" +
                "<head>" +
                "</head>" +
                "<body style='background-color:#34495E'>";
            string fin =
                "</table>" +
                "<body>" +
                "</html>";

            string medioB = "<h1 align=center><font color ='white'> Archivo de Salida </h1>" +
                "<table border=11 , align='center', bordercolor='orange'>" +
                "<tr align=center> <th><font color ='white'> Numero </th> <th><font color ='white'> Palabra </th> <th><font color ='white'> Tipo </th></tr>";

            for (int i = 0; i < buenas.Count; i++) {
                medioB += "\n<tr align=center> <td><font color ='white'> " + (i + 1) + " </td> <td><font color ='white'> " + buenas[i].palabras + " </td> <td><font color ='white'> " + buenas[i].tipoPalabra + " </td> </tr>";
            }

            string contenidoB = inicio + medioB + fin;

            File.WriteAllText(webB, contenidoB);

            string medioM = "<h1 align=center><font color ='white'> Archivo de Errores </h1>" +
                "<table border=11 , align='center', bordercolor='orange'>" +
                "<tr align=center> <th><font color ='white'> Numero </th> <th><font color ='white'> Fila </th> <th><font color ='white'> Columna </th> <th><font color ='white'> Palabra </th> <th><font color ='white'> Error </th></tr>";

            for (int i = 0; i < malas.Count; i++) {
                medioM += "\n<tr align=center> <td><font color ='white'> " + (i + 1) + " </td> <td><font color ='white'> " + malas[i].filaError + " </td> <td><font color ='white'> " + malas[i].columnaError + " </td> <td><font color ='white'> " + malas[i].palabrarError + " </td> <td><font color ='white'> Signo Error </td></tr>";
            }

            string contenidoM = inicio + medioM + fin;

            File.WriteAllText(webM, contenidoM);
            Process start = new Process();
            start.StartInfo.FileName = webM;
            start.Start();
            start.StartInfo.FileName = webB;
            start.Start();



        }

        public void revisarCaja(PictureBox figura, Label lPais, Label lHabitantes, PictureBox pBandera) {
            if (acceder == true) {
                analizarArchivo(figura, lPais, lHabitantes, pBandera);

            } else {
                MessageBox.Show("No hay Texto");
            }
        }

        public void armarDot(PictureBox figura, Label lPais, Label lHabitantes, PictureBox pBandera) {
            archivoDot = new StringBuilder();
            archivoDot.Clear();
            string rutaGeneral = @"C:\prograPruebas";/*Environment.GetFolderPath(Environment.SpecialFolder.Desktop);*/
            string rdot = rutaGeneral + "\\datos.dot";
            //string rimg = rutaGeneral + "\\imagen" + contador + ".png";
            string rimg = rutaGeneral + "\\imagen.png";
            int satTotal = 0;
            int satCount = 0;
            int menorSC = 1000;
            int menorSP = 1000;
            int menorPobla = 0;
            int satContinente;
            string menorSat = "", menorParth = "";

            if (figura.Image != null) {
                figura.Image.Dispose();
                File.Delete(rimg);
            }

            if (pBandera.Image != null) {
                pBandera.Image.Dispose();
            }

            archivoDot.Append("digraph G { \n");

            for (int i = 0; i < buenas.Count; i++) {
                if (palabrasReservadas[1].Equals(buenas[i].palabras.ToLower())) {
                    buscarGrafica(i);
                    Console.WriteLine("Agrego Grafica");

                } else if (palabrasReservadas[2].Equals(buenas[i].palabras.ToLower())) {
                    satTotal = 0;
                    satCount = 0;
                    buscarContinente(i);
                    Console.WriteLine("Agrego Continente");

                } else if (palabrasReservadas[3].Equals(buenas[i].palabras.ToLower())) {
                    buscarPais(i);
                    Console.WriteLine("Agrego Pais");

                }

            }

            archivoDot.Append("}");
            generarDot();
            //contador++;

            void buscarGrafica(int j) {
                archivoDot.Append("start; \n");
                for (int i = j; i < buenas.Count; i++) {
                    if (palabrasReservadas[0].Equals(buenas[i].palabras)) {
                        archivoDot.Append("start [shape = Mdiamond label=\"" + buenas[i + 3].palabras + "\"]; \n");
                        break;
                    }
                }

            }

            void buscarContinente(int j) {
                for (int i = j; i < buenas.Count; i++) {
                    if (palabrasReservadas[0].Equals(buenas[i].palabras)) {
                        continente = buenas[i + 3].palabras;
                        archivoDot.Append("start -> " + continente + "\n");
                        break;
                    }

                }

            }

            void buscarPais(int j) {
                string pais = "";
                int saturacion = 0;
                string color = "black";
                for (int i = j; i < buenas.Count; i++) {
                    if (palabrasReservadas[0].Equals(buenas[i].palabras)) {
                        pais = buenas[i + 3].palabras;
                        break;
                    }
                }

                for (int i = j; i < buenas.Count; i++) {
                    if (palabrasReservadas[5].Equals(buenas[i].palabras)) {
                        saturacion = int.Parse(buenas[i + 2].palabras);
                        if (saturacion >= 0 && saturacion <= 15) {
                            color = "white";
                        } else if (saturacion > 15 && saturacion <= 30) {
                            color = "blue";
                        } else if (saturacion > 30 && saturacion <= 45) {
                            color = "green";
                        } else if (saturacion > 45 && saturacion <= 60) {
                            color = "yellow";
                        } else if (saturacion > 60 && saturacion <= 75) {
                            color = "orange";
                        } else if (saturacion > 75 && saturacion <= 100) {
                            color = "red";
                        }
                        break;
                    }
                }
                archivoDot.Append(continente + " -> " + pais + "\n");
                archivoDot.Append(pais + " [shape = record label=\"{" + pais + "|" + saturacion + "}\" style = filled fillcolor = " + color + "];\n");
                satTotal += saturacion;
                satCount++;
                satContinente = satTotal / satCount;
                archivoDot.Append(continente + " [shape = record label=\"{" + continente + "|" + satContinente + "}\" style = filled]; \n");
                verPequeño();

                void verPequeño() {
                    if (saturacion < menorSP && menorSC >= satContinente) {
                        menorSC = satContinente;
                        menorSP = saturacion;
                        menorSat = pais;
                        for (int i = j; i < buenas.Count; i++) {
                            if (palabrasReservadas[6].Equals(buenas[i].palabras)) {
                                menorParth = buenas[i + 3].palabras;
                                break;
                            }
                        }
                        for (int i = j; i < buenas.Count; i++) {
                            if (palabrasReservadas[4].Equals(buenas[i].palabras)) {
                                menorPobla = int.Parse(buenas[i + 2].palabras);
                                break;
                            }
                        }
                    }
                }

            }

            void generarDot() {
                if (!File.Exists(rdot)) {
                    File.WriteAllText(rdot, archivoDot.ToString());

                } else {
                    File.Delete(rdot);
                    File.WriteAllText(rdot, archivoDot.ToString());

                }

                Console.WriteLine("Se Creo");
                string comandoDot = "dot -Tpng " + rdot + " -o " + rimg + " ";

                var proceso = new Process();
                var StartInfo = new ProcessStartInfo {
                    FileName = "cmd.exe",
                    Arguments = "/c " + comandoDot,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = false
                };
                proceso.StartInfo = StartInfo;

                proceso.Start();
                proceso.WaitForExit();
                abrirImagen();


            }

            void abrirImagen() {
                if (File.Exists(rimg)) {
                    figura.Image = System.Drawing.Image.FromFile(rimg);

                }
                if (File.Exists(menorParth)) {
                    figura.Image = System.Drawing.Image.FromFile(rimg);
                }
                lPais.Text = menorSat;
                lHabitantes.Text = menorPobla.ToString();
            }
        }

        public void crearPDF() {
            string rutaGeneral = @"C:\prograPruebas";
            string rpdf = rutaGeneral + "\\documento.pdf";
            string rimg = rutaGeneral + "\\imagen.png";
            Document doc = new Document();
            // Indicamos donde vamos a guardar el documento
            PdfWriter writer;


            try {
                writer = PdfWriter.GetInstance(doc, new FileStream(rpdf, FileMode.Create));

                // Le colocamos el título y el autor
                // **Nota: Esto no será visible en el documento
                doc.AddTitle("Diagrama");

                // Abrimos el archivo
                doc.Open();

                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(rimg);
                //png.ScaleToFit(250f, 250f);
                png.RotationDegrees = 270f;
                doc.Add(png);

            } catch (Exception ex) {


            } finally {

                doc.Close();

            }

            Process abrir = new Process();
            abrir.StartInfo.FileName = rpdf;
            abrir.Start();
            abrir.WaitForExit();
        }

    }
}
