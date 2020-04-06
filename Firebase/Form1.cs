using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Firebase
{
    public partial class Form1 : Form
    {
        //============================================
        //Desarrollado por Master Solutions
        //Ing. Luis Manuel Calderón Molina
        //Estelí, Nicaragua, Abril 2020
        //============================================

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "OhV47D1XnCsiShmgclSKbiER9YNaMJ4ISuMoDuh2",
            BasePath = "https://ms-test-3b9d6.firebaseio.com"
        };

        IFirebaseClient client;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var empleado = new clsEmpleados()
            {
                id = txtId.Text,
                nombre = txtNombre.Text,
                edad = txtEdad.Text
            };

            var set = client.Set(@"empleados/" + txtId.Text, empleado);

            MessageBox.Show("Registro guardado...", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLeer_Click(object sender, EventArgs e)
        {
            var res = client.Get("empleados/" + txtId.Text);
            clsEmpleados empleado = new clsEmpleados();
            empleado = res.ResultAs<clsEmpleados>();


            txtId.Text = empleado.id;
            txtNombre.Text = empleado.nombre;
            txtEdad.Text = empleado.edad.ToString();
        }

        private void btnObtenerRegistros_Click(object sender, EventArgs e)
        {
            List<clsEmpleados> lstEmpleados;
            FirebaseResponse res = client.Get("empleados/");
            lstEmpleados = res.ResultAs<List<clsEmpleados>>();

            //string bodyJson = res.Body;
            //MessageBox.Show(bodyJson);
            dataGridView1.DataSource = lstEmpleados;
            formatoDGV();
        }

        private void formatoDGV()
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Columns["nombre"].Width = 250;
            dataGridView1.ReadOnly = true;

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtId.Text = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();
            txtEdad.Text = dataGridView1.Rows[e.RowIndex].Cells["edad"].Value.ToString();
            txtNombre.Text = dataGridView1.Rows[e.RowIndex].Cells["nombre"].Value.ToString();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Está seguro que desea eliminar el registro seleccionado?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            var set = client.Delete(@"empleados/" + txtId.Text);
        }
    }
}
