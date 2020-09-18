using System;
using PlayerUI.Controller;
using PlayerUI.ServicioAntecedentesPenalesSWJavita;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PlayerUI.GUI.Otras_Consultas;
using PlayerUI.GUI.Consultas;
using System.Runtime.InteropServices;
using System.Threading;

namespace PlayerUI.GUI.Antecedentes
{
    public partial class GUISearchAntecedente : Form, IBuscarDelito, IBuscarCiudadano
    {
        private ControllerAntecedentesPenales controller;

        public GUISearchAntecedente()
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

        #region Botones
        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            //btn buscar
            try
            {
                int id = Int32.Parse(txtID.Text);
                try
                {
                    Dictionary<string, object> ciudadano = await this.controller.FindId(id);

                    txtDocumentoID.Text = Convert.ToString(ciudadano["ciudadanoDi"].ToString());
                    txtDelito.Text = ciudadano["delitoCodigo"].ToString();
                    txtCiudad.Text = ciudadano["ciudad"].ToString();
                    txtSentencia.Text = Convert.ToString(ciudadano["sentencia"].ToString());
                    txtEstado.Text = ciudadano["estado"].ToString();
                    String cad = Convert.ToString(ciudadano["fechaDelito"]);
                    String[] cad1 = cad.Split(' ');
                    String[] cad2 = cad1[1].Split('T');
                    dateTimePicker1.Value = Convert.ToDateTime(cad2[0]);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Advertencia");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GUIDelitos gui = new GUIDelitos(this);
            gui.ShowDialog();
        }

        private void btnLista_Click(object sender, EventArgs e)
        {
            GUITablaCiudadanos gui = new GUITablaCiudadanos(this);
            gui.ShowDialog();
        }

        private void GUISearchAntecedente_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        #endregion

        #region Validaciones

        private void textBox3_Validated(object sender, EventArgs e)
        {
            
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            
        }

        private void limpiar()
        {
            txtCiudad.Clear();
            txtDocumentoID.Clear();
            txtDelito.Clear();
            txtID.Clear();
            txtEstado.Clear();
            txtSentencia.Clear();
        }

        #endregion

        #region Cambios
        public void CambiarTxtCiudadano(string ciudadanoDI)
        {
            txtDocumentoID.Text = ciudadanoDI;
        }

        public void CambiarTxtDelito(int codigoDelito)
        {
            txtDelito.Text = codigoDelito + "";
        }
        #endregion
       
    }
}
