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
using System.Runtime.InteropServices;

namespace PlayerUI.GUI
{
    public partial class GUIDeleteCiudadano : Form
    {
        private ControllerAntecedentesPenales controller;

        public GUIDeleteCiudadano()
        {
            InitializeComponent();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void GUIDeleteCiudadano_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        #region botones
        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            //btn buscar
            string cedula = textBox1.Text.Trim();
            try
            {
                Dictionary<string, object> ciudadano = await this.controller.FindClient(cedula);

                txtName.Text = ciudadano["nombre"].ToString();
                txtAp.Text = ciudadano["apellido"].ToString();
                String cad = Convert.ToString(ciudadano["fecha"]);
                String[] cad2 = cad.Split(' ');
                String[] cad3 = cad2[1].Split('T');
                dateTimePicker1.Value = Convert.ToDateTime(cad3[0]);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advertencia");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                String cedula = textBox1.Text.Trim();
                try
                {
                    controller.delete_ciudadano(cedula);
                    MessageBox.Show("Ciudadano eliminado");
                }
                catch(Exception ex)
                {
                    MessageBox.Show("El ciudadano no se elimina debido a: " + ex);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error! " + ex);
            }
        }
        #endregion

        #region validaciones
        private void textBox1_Validated(object sender, EventArgs e)
        {
            if(textBox1.Text.Trim() == "")
            {
                errorProv.SetError(textBox1, "Por favor ingrese el DI ");
                textBox1.Focus();
            }
            else
            {
                errorProv.Clear();
            }
        }

        private void limpiar()
        {
            textBox1.Text = "";
            txtName.Text = "";
            txtAp.Text = "";
        }
        #endregion
      
    }
}
