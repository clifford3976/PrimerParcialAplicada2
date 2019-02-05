﻿using BLL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrimerParcial.Registros
{
    public partial class Depositos : System.Web.UI.Page
    {
        string condicion = "Select One";
        RepositorioBase<Deposito> repositorio = new RepositorioBase<Deposito>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {


                LlenaComboCuentaID();
                FechadateTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        public Deposito LlenaClase(Deposito deposito)
        {
            int id;
            bool result = int.TryParse(DepositoIDTextbox.Text, out id);
            if (result == true)
            {
                deposito.DepositoID = id;
            }
            else
            {
                deposito.DepositoID = 0;
            }

            deposito.CuentaID = Convert.ToInt32(DropDownList.SelectedValue);

            deposito.Concepto = ConceptoTextbox.Text;
            deposito.Fecha = Convert.ToDateTime(FechadateTime.Text);
            deposito.Monto = Convert.ToDecimal(MontoTexbox.Text.ToString());

            return deposito;
        }

        private void LlenaCampos(Deposito deposito)
        {
            DepositoIDTextbox.Text = deposito.DepositoID.ToString();
            LlenaComboCuentaID();
            ConceptoTextbox.Text = deposito.Concepto;
            MontoTexbox.Text = deposito.Monto.ToString();

        }
        private void Limpiar()
        {
            DepositoIDTextbox.Text = "";
            LlenaComboCuentaID();
            ConceptoTextbox.Text = "";
            MontoTexbox.Text = "";
            FechadateTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

        }

        void MostrarMensaje(TiposMensajes tipo, string mensaje)

        {

            ErrorLabel.Text = mensaje;

            if (tipo == TiposMensajes.Success)

                ErrorLabel.CssClass = "alert-success";

            else

                ErrorLabel.CssClass = "alert-danger";

        }

        private void LlenaComboCuentaID()
        {
            RepositorioBase<Cuenta> cuentas = new RepositorioBase<Cuenta>();
            DropDownList.Items.Clear();
            DropDownList.Items.Add(condicion);
            DropDownList.DataSource = cuentas.GetList(x => true);
            DropDownList.DataValueField = "CuentaID";
            DropDownList.DataTextField = "Nombre";
            DropDownList.DataBind();
        }

        protected void NuevoButton_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        protected void GuardarButton_Click(object sender, EventArgs e)
        {
            if (DropDownList.SelectedValue == condicion)
                return;


            DepositoBLL dep = new DepositoBLL();
            Deposito deposito = new Deposito();
            bool paso = false;

            dep.Guardar(LlenaClase(deposito));
            //Validacion
            if (deposito.DepositoID == 0)

                paso = repositorio.Guardar(deposito);
            else
                paso = repositorio.Modificar(deposito);
            if (paso)

            {
                Utilities.Utils.ShowToastr(this, "Guardo Con Exito", "Exito", "success");
                Limpiar();

            }
            else
                MostrarMensaje(TiposMensajes.Error, "No fue posible Guardar el Registro");

            Limpiar();
        }

        protected void EliminarButton_Click(object sender, EventArgs e)
        {
            RepositorioBase<Deposito> repositorio = new RepositorioBase<Deposito>();

            int id = Convert.ToInt32(DepositoIDTextbox.Text);



            var deposito = repositorio.Buscar(id);



            if (deposito == null)

                MostrarMensaje(TiposMensajes.Error, "Registro no encontrado");

            else

                repositorio.Eliminar(id);
        }

        protected void BuscarButton_Click(object sender, EventArgs e)
        {

            RepositorioBase<Deposito> repositorio = new RepositorioBase<Deposito>();


            Deposito deposito = repositorio.Buscar(Convert.ToInt32(DepositoIDTextbox.Text));
            if (deposito != null)
            {
                LlenaCampos(deposito);
            }
            else
            {
                Response.Write("<script>alert('Usuario no encontrado');</script>");

            }
        }
    }
}