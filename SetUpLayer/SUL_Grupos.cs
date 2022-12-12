using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypesLayer;

namespace SetUpLayer
{
    public class SUL_Grupos
    {
        SUL_Connection con = new SUL_Connection();

        #region Cargar nombres de Grupos en CRUD
        public List<string> NombreGrupo()
        {
            List<string> nombres = new List<string>();

            SqlDataAdapter da = new SqlDataAdapter("SP_G_CargarGrupos", con.Connect()); da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            con.CloseConnection();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nombres.Add(dt.Rows[i][0].ToString());
            }
            return nombres;
        }
        #endregion

        #region Consultar Nombre de Grupo
        public async Task<TL_Grupos> ConsultarGrupo(int IdGrupo)
        {
            TL_Grupos grupo = new TL_Grupos();
            SqlDataAdapter da = new SqlDataAdapter("SP_G_ConsultarGrupo", con.Connect()); da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.Add("@IdGrupo", SqlDbType.Int).Value = IdGrupo;
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            await Task.Run(() => con.CloseConnection());

            #region Cargar datos del grupo consultado
            grupo.IdGrupo = Convert.ToInt32(dt.Rows[0][0]);
            grupo.NombreGrupo = dt.Rows[0][1].ToString();
            #endregion

            return grupo;
        }
        #endregion

    }
}
