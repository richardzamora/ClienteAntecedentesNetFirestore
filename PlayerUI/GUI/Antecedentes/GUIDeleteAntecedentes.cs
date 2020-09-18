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
using PlayerUI.ServicioAntecedentesPenalesSWJavita;
using PlayerUI.GUI.Otras_Consultas;
using PlayerUI.GUI.Consultas;
using System.Runtime.InteropServices;

namespace PlayerUI.GUI.Antecedentes
{
    public partial class GUIDeleteAntecedentes : Form, IBuscarDelito, IBuscarCiudadano
    {
        private ControllerAntecedentesPenales controller;

        public GUIDeleteAntecedentes()
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
            int id = Int32.Parse(textBox1.Text);
            try
            {
                Dictionary<string, object> ciudadano = await this.controller.FindId(id);

                txtDoc.Text = Convert.ToString(ciudadano["ciudadanoDi"].ToString());
                txtSentencia.Text = Convert.ToString(ciudadano["sentencia"].ToString());
                txtEstado.Text = ciudadano["estado"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advertencia");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //btn eliminar

            try
            {
                int id = Int32.Parse(textBox1.Text);
                controller.delete_antecedente(id);
                MessageBox.Show("Se ha eliminado correctamente");
                limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se elimina debido a: " + ex);
            }
        }

        private void textBox2_Validated(object sender, EventArgs e)
        {

        }

        private void textBox1_Validated(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            GUIDelitos gui = new GUIDelitos(this);
            gui.ShowDialog();
        }

        public void CambiarTxtCiudadano(string ciudadanoDI)
        {
            textBox1.Text = ciudadanoDI;
        }

        public void CambiarTxtDelito(int codigoDelito)
        {
            //textBox2.Text = "" + codigoDelito;
        }

        private void limpiar()
        {
            txtEstado.Text = "";
            txtDoc.Text = "";
            txtSentencia.Text = "";
            textBox1.Text = "";
          //  textBox2.Text = "";
        }

        private void btnLista_Click(object sender, EventArgs e)
        {
            GUITablaCiudadanos gui = new GUITablaCiudadanos(this);
            gui.ShowDialog();
        }

        private void GUIDeleteAntecedentes_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
