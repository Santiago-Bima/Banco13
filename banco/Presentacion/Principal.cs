﻿using banco.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace banco.Presentacion
{
    public partial class frmPrincipal : Form
    {
        DbHelper oBD;
        List<Cliente> lClientes;

        public frmPrincipal()
        {
            InitializeComponent();
            oBD = new DbHelper();
            lClientes = new List<Cliente>();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            CargarListaClientes();
        }

        private void CargarListaClientes()
        {
            lstClientes.Items.Clear();
            lClientes.Clear();
            DataTable tabla = oBD.ConsultarDb("Select * from clientes");
            foreach (DataRow fila in tabla.Rows)
            {
                Cliente c = new Cliente();
                c.pNombre = Convert.ToString(fila["nombre"]);
                c.pApellido = Convert.ToString(fila["apellido"]);
                c.pDni = Convert.ToInt32(fila["dni"]);
                lClientes.Add(c);
                lstClientes.Items.Add(c.ToString());
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CargarGridCuentasClientes(lClientes[lstClientes.SelectedIndex]);
            }
            catch (Exception)
            {

            }
            dgvTransacciones.DataSource = null;
            dgvMovimientos.DataSource = null;
        }

        private void CargarGridCuentasClientes(Cliente c)
        {
            string nombre = c.pNombre + " " + c.pApellido;
            dgvCuentas.DataSource = null;
            dgvCuentas.Rows.Clear();
            dgvCuentas.Columns.Clear();
            DataTable tabla = oBD.ConsultarDb("Select id_cuenta ID,cbu CBU, t.nombre as 'Tipo de Cuenta', c.nombre + ' ' + c.apellido as Cliente, total Total, ultimo_mov 'Ultimo Movimiento'" +
                " from cuentas cu join clientes c on c.id_cliente=cu.id_cliente" +
                " join tiposCuentas t on t.id_tipoCuenta=cu.id_tipoCuenta" +
                " where c.nombre + ' ' + c.apellido = '" + nombre + "' and activo=1");
            dgvCuentas.DataSource = tabla;

        }


        private void dgvCuentas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvTransacciones.DataSource = null;
            dgvMovimientos.DataSource = null;
            dgvTransacciones.Rows.Clear();
            dgvTransacciones.Columns.Clear();
            int idCuenta= Convert.ToInt32(dgvCuentas.Rows[e.RowIndex].Cells[0].Value);
            DataTable tabla = oBD.ConsultarDb("Select nro_transaccion 'Numero de la Transaccion', total Total, fecha 'Fecha de realizacion' from transacciones where id_cuenta=" + idCuenta + " and activo=1");
            dgvTransacciones.DataSource = tabla;
        }

        private void dgvTransacciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvMovimientos.DataSource = null;
            dgvMovimientos.Rows.Clear();
            dgvMovimientos.Columns.Clear();
            int nroTransaccion = Convert.ToInt32(dgvTransacciones.Rows[e.RowIndex].Cells[0].Value);
            DataTable tabla = oBD.ConsultarDb("Select monto Monto, t.tipo from Movimientos m" +
                " join tiposMovimientos t on t.id_tipo=m.id_tipo " +
                " where nro_transaccion=" + nroTransaccion);
            dgvMovimientos.DataSource = tabla;
        }


        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmClientes Cliente = new frmClientes();
            Cliente.Show();
        }

        private void nuevaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCuentas  Cuenta = new frmCuentas();
            Cuenta.Show();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Seguro que desea salir del formulario?",
                "saliendo...",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes) Close();
        }

        private void nuevaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmTransacciones transaccion = new frmTransacciones();
            transaccion.Show();
        }


        

        private void dgvMovimientos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarListaClientes();
            dgvCuentas.DataSource = null;
            dgvTransacciones.DataSource = null;
            dgvMovimientos.DataSource = null;
        }

        private void darDeBajaAltaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAltaBajaCuentas AltaBaja = new frmAltaBajaCuentas();
            AltaBaja.Show();
        }

        private void darDeBajaAltaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAltaBajaTransacciones AltaBaja = new frmAltaBajaTransacciones();
            AltaBaja.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void btnReporteCuentas_Click(object sender, EventArgs e)
        {
            frmReporteTransacciones RepoTransaccion = new frmReporteTransacciones();
            RepoTransaccion.Show();
        }

        private void btnRepClientes_Click(object sender, EventArgs e)
        {
            FrmReporteClientes RepoCliente = new FrmReporteClientes();
            RepoCliente.Show();
        }

        private void btnRepoCuentas_Click(object sender, EventArgs e)
        {
            FrmReporteCuentas RepoCuenta = new FrmReporteCuentas();
            RepoCuenta.Show();
        }
    }
}
