using BLL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrimerParcial.Consultas
{
    public partial class Cuentas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                DesdeTextBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
                HastaTextBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }

        }

        RepositorioBase<Cuenta> repositorio = new RepositorioBase<Cuenta>();
        bool paso = false;
        Expression<Func<Cuenta, bool>> filtrar = x => true;

        protected void ButtonBuscar_Click1(object sender, EventArgs e)
        {
            var DesdeDateTime = Convert.ToDateTime(DesdeTextBox.Text);
            var HastaDateTime = Convert.ToDateTime(HastaTextBox.Text);
            int id = 0;
            if (TextCriterio.Text == string.Empty && FechaCheckBox.Checked == true)
            {
                filtrar = t => true && (t.Fecha >= DesdeDateTime.Date) && (t.Fecha <= HastaDateTime.Date);
            }
            else
            {
                filtrar = t => true;
            }
            switch (TipodeFiltro.SelectedIndex)
            {

                //Lista todo
                case 0:

                    if (FechaCheckBox.Checked == true)
                    {
                        filtrar = t => true && (t.Fecha >= DesdeDateTime.Date) && (t.Fecha <= HastaDateTime.Date);
                    }
                    else
                    {
                        filtrar = t => true;
                    }

                    break;

                case 1:
                    if (paso)
                        return;
                    id = int.Parse(TextCriterio.Text);
                    if (FechaCheckBox.Checked == true)
                    {
                        filtrar = t => t.CuentaID == id && (t.Fecha >= DesdeDateTime.Date) && (t.Fecha <= HastaDateTime.Date);
                    }
                    else
                    {
                        filtrar = t => t.CuentaID == id;
                    }

                    break;

                case 2:

                    if (FechaCheckBox.Checked == true)
                    {
                        filtrar = t => t.Nombre.Equals(TextCriterio.Text) && (t.Fecha >= DesdeDateTime.Date) && (t.Fecha <= HastaDateTime.Date);
                    }
                    else
                    {
                        filtrar = t => t.Nombre.Equals(TextCriterio.Text);

                    }
                    break;

                case 3:
                    if (paso)
                        return;
                    if (FechaCheckBox.Checked == true)
                    {
                        filtrar = t => t.Balance.Equals(TextCriterio.Text) && (t.Fecha >= DesdeDateTime.Date) && (t.Fecha <= HastaDateTime.Date);
                    }
                    else
                    {
                        filtrar = t => t.Balance.Equals(TextCriterio.Text);
                    }

                    break;

            }

            CuentaGridView.DataSource = repositorio.GetList(filtrar);
            CuentaGridView.DataBind();
        }
    }
}
