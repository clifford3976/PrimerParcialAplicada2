using BLL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrimerParcial.Registros
{
    public partial class Cuentas : System.Web.UI.Page
    {
        RepositorioBase<Cuentas> repositorio = new RepositorioBase<Cuentas>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FechadateTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            }
        }

        public Cuenta LlenaClase(Cuenta cuentas)
        {
           
            int id;
            bool result = int.TryParse(CuentaIDTextbox.Text, out id);
            if (result == true)
            {
                cuentas.CuentaID = id;
            }
            else
            {
                cuentas.CuentaID = 0;
            }

            cuentas.Nombre = nombreTextbox.Text;
            cuentas.Balance = Convert.ToDecimal(BalanceTextbox.Text.ToString());

            return cuentas;
        }

        private void LlenaCampos(Cuenta cuentas)
        {
            CuentaIDTextbox.Text = cuentas.CuentaID.ToString();
            nombreTextbox.Text = cuentas.Nombre;
            BalanceTextbox.Text = cuentas.Balance.ToString();


        }

        private void Limpiar()
        {
            CuentaIDTextbox.Text = "";
            nombreTextbox.Text = "";
            BalanceTextbox.Text = "";

        }


        void MostrarMensaje(TiposMensajes tipo, string mensaje)

        {

            ErrorLabel1.Text = mensaje;

            if (tipo == TiposMensajes.Success)

                ErrorLabel1.CssClass = "alert-success";

            else

                ErrorLabel1.CssClass = "alert-danger";

        }


        protected void EliminarButton_Click(object sender, EventArgs e)
        {
            RepositorioBase<Cuenta> repositorio = new RepositorioBase<Cuenta>();



            int id = 0;
            if (CuentaIDTextbox.Text != null)
            {
                id = Convert.ToInt32(CuentaIDTextbox.Text);
            }

            else
                return;
            if (CuentaIDTextbox.Text != null)
            {

                //Si tiene algun prestamo o deposito enlazado no elimina
                RepositorioBase<Deposito> repositorios = new BLL.RepositorioBase<Deposito>();

                if (repositorios.GetList(x => x.CuentaID == id).Count() > 0)
                {
                    MostrarMensaje(TiposMensajes.Error, "No Fue Posible Eliminardo, Clntiene Depositos en esa Cuenta");

                }

                var usuario = repositorio.Buscar(id);



                if (usuario == null)

                    MostrarMensaje(TiposMensajes.Error, "Registro no encontrado");

                else

                    repositorio.Eliminar(id);
            }
        }


        protected void BuscarButton_Click(object sender, EventArgs e)
        {
            RepositorioBase<Cuenta> repositorio = new RepositorioBase<Cuenta>();


            Cuenta cuentas = repositorio.Buscar(Convert.ToInt32(CuentaIDTextbox.Text));
            if (cuentas != null)
            {
                LlenaCampos(cuentas);
            }
            else
            {
                Response.Write("<script>alert('Usuario no encontrado');</script>");

            }
        }

        protected void NuevoButton_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        protected void GuardarButton_Click(object sender, EventArgs e)
        {
            RepositorioBase<Cuenta> repositorio = new RepositorioBase<Cuenta>();
            Cuenta cuentas = new Cuenta();
            bool paso = false;

            LlenaClase(cuentas);
            //Validacion
            if (cuentas.CuentaID == 0)

                paso = repositorio.Guardar(cuentas);
            else
                paso = repositorio.Modificar(cuentas);
            if (paso)

            {
                MostrarMensaje(TiposMensajes.Success, "Registro Exitoso!");
                Limpiar();

            }
            else
                MostrarMensaje(TiposMensajes.Error, "No fue posible Guardar el Registro");

            Limpiar();
        }
    }
}