using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TypesLayer;

namespace SetUpLayer
{
    public class SUL_Products
    {
        TL_Products obk_tL_Products = new TL_Products();
        SUL_Connection obj_sul_Connection = new SUL_Connection();

        #region Cargar productos en inventario

        public async Task<DataTable> Buscar(string buscar)
        {
            SqlDataAdapter da = new SqlDataAdapter("SP_A_CargarArticulos", obj_sul_Connection.Connect());
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            await Task.Run(() => obj_sul_Connection.CloseConnection());

            return dt;
        }
        #endregion

        #region Agregar productos en Inventario

        public async Task Insertar (TL_Products producto)
        {
            try
            {
                if (await BuscarProducto(producto.Codigo))
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = obj_sul_Connection.Connect(),
                        CommandText = "SP_A_Insertar_Producto",
                        CommandType = CommandType.StoredProcedure,
                    };

                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre.ToLower());
                    cmd.Parameters.AddWithValue("@Grupo", producto.Grupo);
                    cmd.Parameters.AddWithValue("@Codigo", producto.Codigo.ToLower());
                    cmd.Parameters.AddWithValue("@Costo", producto.Costo);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Cantidad", producto.Cantidad);
                    cmd.Parameters.AddWithValue("@Img", producto.Imagen);
                    cmd.Parameters.AddWithValue("@IdProveedor", producto.Proveedor);
                    cmd.Parameters.AddWithValue("@Activo", producto.Activo1);
                    cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion.ToLower());
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    await Task.Run(() => obj_sul_Connection.CloseConnection());
                    MessageBox.Show("Producto guardado correctamente");
                }
                else
                {
                    MessageBox.Show("El producto " + producto.Nombre.ToUpper() + " con codigo " + producto.Codigo.ToUpper() + "ya existe");
                }
                

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Consultar producto del inventario
        public async Task<TL_Products> Consultar (int IdProducto)
        {
            TL_Products producto = new TL_Products();
            SqlDataAdapter da = new SqlDataAdapter("SP_A_Consultar", obj_sul_Connection.Connect());
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.Add("@IdProducto", SqlDbType.Int).Value = IdProducto;
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            await Task.Run(() => obj_sul_Connection.CloseConnection());

            #region Cargar datos de producto consultado
            producto.IdArticulo = Convert.ToInt32(dt.Rows[0][0]);
            producto.Nombre = dt.Rows[0][1].ToString();
            producto.Grupo = Convert.ToInt32(dt.Rows[0][2]);
            producto.Codigo = dt.Rows[0][3].ToString();
            producto.Costo = Convert.ToDouble(dt.Rows[0][4]);
            producto.Precio = Convert.ToDouble(dt.Rows[0][5]);
            producto.Activo1 = Convert.ToBoolean(dt.Rows[0][6]);
            producto.Cantidad = Convert.ToInt32(dt.Rows[0][7]);
            producto.Proveedor = Convert.ToInt32(dt.Rows[0][9]);
            producto.Descripcion = dt.Rows[0][10].ToString();
            #endregion

            return producto;
        }

        #endregion

        #region Modificar producto del inventario

        public async Task Modificar(TL_Products producto)
        {
            try
            {
                SqlCommand cmd = new SqlCommand()
                {
                    Connection = obj_sul_Connection.Connect(),
                    CommandText = "SP_A_ModificarProducto",
                    CommandType = CommandType.StoredProcedure,
                };

                cmd.Parameters.AddWithValue("@IdProducto", producto.IdArticulo);
                cmd.Parameters.AddWithValue("@Nombre", producto.Nombre.ToLower());
                cmd.Parameters.AddWithValue("@Grupo", producto.Grupo);
                cmd.Parameters.AddWithValue("@Codigo", producto.Codigo.ToLower());
                cmd.Parameters.AddWithValue("@Costo", producto.Costo);
                cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                cmd.Parameters.AddWithValue("@Cantidad", producto.Cantidad);
                cmd.Parameters.AddWithValue("@Img", producto.Imagen);
                cmd.Parameters.AddWithValue("@IdProveedor", producto.Proveedor);
                cmd.Parameters.AddWithValue("@Activo", producto.Activo1);
                cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion.ToLower());
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                await Task.Run(() => obj_sul_Connection.CloseConnection());
                MessageBox.Show("Producto Modificado correctamente");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Eliminar producto del inventario
        public async Task<bool> EliminarProducto( int IdProducto)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_A_EliminarArticulo", obj_sul_Connection.Connect());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdArticulo", IdProducto);
                //cmd.Parameters.Add("@IdArticulo", SqlDbType.Int).Value = IdProducto;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                await Task.Run(() => obj_sul_Connection.CloseConnection());
                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }

        }
        #endregion

        #region Buscar Producto
        public async Task<bool> BuscarProducto(string CodProducto)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_A_Buscar_Producto", obj_sul_Connection.Connect());
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@Nombre", CodProducto);
                DataSet ds = new DataSet();
                ds.Clear();
                da.Fill(ds);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                await Task.Run(() => obj_sul_Connection.CloseConnection());
                if (dt.Rows.Count == 1)
                {
                    return false;
                }
                else return true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        #endregion

        #region Consultar producto en Venta Actual

        public async Task<DataTable> ConsultarProducto(string codigo)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SP_A_Buscar_Producto", obj_sul_Connection.Connect());
            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            adapter.SelectCommand.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = codigo;
            DataSet ds = new DataSet();
            ds.Clear();
            adapter.Fill(ds);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            await Task.Run(() => obj_sul_Connection.CloseConnection());

            return dt;
        }

        #endregion
    }
}
