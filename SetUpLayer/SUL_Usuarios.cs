using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SetUpLayer
{
    public class SUL_Usuarios
    {
        private TypesLayer.TL_Usuario userlogin = new TypesLayer.TL_Usuario();
        SUL_Connection connection = new SUL_Connection();

        #region Validar usuario en base de datos

        public TypesLayer.TL_Usuario LogIn (string usuario, string clave)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SP_U_LogIn", connection.Connect());
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.AddWithValue("@Usuario", usuario);
                adapter.SelectCommand.Parameters.AddWithValue("@Clave", clave);

                DataSet ds = new DataSet();
                ds.Clear();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    userlogin.IdUsuario = Convert.ToInt32(row[0]);
                    userlogin.Usuario = Convert.ToString(row[1]);
                    userlogin.Clave = Convert.ToString(row[2]);
                    userlogin.Tipo = Convert.ToInt32(row[3]);
                }
                return userlogin;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //MessageBox.Show("Usuario o Clave Invalida, Intente de nuevo");
                return null;
            }

        }

        #endregion
    }
}
