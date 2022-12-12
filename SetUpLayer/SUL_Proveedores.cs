using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;



namespace SetUpLayer
{
    public class SUL_Proveedores
    {
        SUL_Connection con = new SUL_Connection();

        #region Cargar lista de los proveedores disponibles
        /// <summary>
        /// En este metodo se cargan los nombres de proveedor disponibles
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> NombreProveedor()
        {
            List<string> nombres = new List<string>();

            SqlDataAdapter da = new SqlDataAdapter("SP_P_CargarProveedores", con.Connect()); da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            await Task.Run(() => con.CloseConnection());

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nombres.Add(dt.Rows[i][0].ToString());
            }
            return nombres;
        }
        #endregion

        #region Consultar nombre del proveedor
        public async Task<TypesLayer.TL_Proveedores> ConsultarProveedor(int IdProveedor)
        {
            TypesLayer.TL_Proveedores proveedor = new TypesLayer.TL_Proveedores();
            SqlDataAdapter da = new SqlDataAdapter("SP_P_ConsultarProveedor", con.Connect()); da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = IdProveedor;
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            da.SelectCommand.Parameters.Clear();
            await Task.Run(() => con.CloseConnection());

            #region Cargar datos del proveedor consultado
            proveedor.IdPRoveedor = Convert.ToInt32(dt.Rows[0][0]);
            proveedor.NombreProveedor = dt.Rows[0][1].ToString();
            proveedor.Telefono = Convert.ToInt32(dt.Rows[0][2]);
            proveedor.Email = dt.Rows[0][3].ToString();
            proveedor.Rif = dt.Rows[0][4].ToString();
            proveedor.Web = dt.Rows[0][5].ToString();
            #endregion

            return proveedor;
        }
        #endregion

    }
}
