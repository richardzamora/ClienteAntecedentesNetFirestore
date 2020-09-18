using System;
using PlayerUI.Controller;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PlayerUI.Estructurales;
using PlayerUI.GUI.Consultas;
using PlayerUI.GUI.Otras_Consultas;
using System.Runtime.InteropServices;



namespace PlayerUI.GUI.Antecedentes
{
    public partial class GUIAddAntecedente : Form, IBuscarCiudadano, IBuscarDelito
    {
        private ControllerAntecedentesPenales controller;
        

        public GUIAddAntecedente()
        {
            InitializeComponent();
            controller = ControllerAntecedentesPenales.getInstance();
            
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            //btn buscar
             
            string cedula = textBox1.Text.Trim();
            try
            {
                Dictionary<string, object> ciudadano = await this.controller.FindClient(cedula);

                MessageBox.Show("El ciudadano identificado con: "+ Convert.ToString(ciudadano["cedula"].ToString()) + " Con nombre de: " + ciudadano["nombre"].ToString() + " " + ciudadano["apellido"].ToString() + " Ha sido seleccionado");
                txtCiudad.Enabled = true;
                txtID.Enabled = true;
                txtCod.Enabled = true;
                txtEstado.Enabled = true;
                txtSentencia.Enabled = true;
                dateTimePicker1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advertencia");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //btn Añadir antecedente
            try
            {
                int id = Int32.Parse(txtID.Text.Trim());
                String ciudadanoDI = textBox1.Text.Trim();
                int delitoCod = Int32.Parse(txtCod.Text.Trim());
                String ciudad = txtCiudad.Text.Trim();
                DateTime date = dateTimePicker1.Value.ToUniversalTime();
                int sentencia = Int32.Parse(txtSentencia.Text.Trim());
                String estado = txtEstado.Text.Trim();
                if (controller.insert_antecedente(id, ciudadanoDI, delitoCod, ciudad, date, sentencia, estado))
                {
                    MessageBox.Show("El ciudadano con el DI: " + ciudadanoDI + " se le añadió correctamente el antecedente");
                    limpiar();
                }
                else
                {
                    MessageBox.Show("El ciudadano con el DI: " + ciudadanoDI + " no se le añadió el antecedente");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error! " + ex);
            }
        }

        private void btnLista_Click(object sender, EventArgs e)
        {
            GUITablaCiudadanos gui = new GUITablaCiudadanos(this);
            gui.ShowDialog();
        }

        public void CambiarTxtDelito(int codigoDelito)
        {
            txtCod.Text = "" + codigoDelito;
        }

        public void CambiarTxtCiudadano(string ciudadanoDI)
        {
            textBox1.Text = ciudadanoDI;
        }

        private void limpiar()
        {
            txtCiudad.Text = "";
            txtCod.Text = "";
            txtEstado.Text = "";
            txtSentencia.Text = "";
            textBox1.Text = "";
            txtID.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GUIDelitos gui = new GUIDelitos(this);
            gui.ShowDialog();
        }

        private void GUIAddAntecedente_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
