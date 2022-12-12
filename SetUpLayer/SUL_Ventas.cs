using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SetUpLayer
{
    public class SUL_Ventas
    {
        readonly SUL_Connection obj_sul_connection = new SUL_Connection();

        #region Agragar producto a la venta actual
        public async Task<DataRow> AgregarProductoVenta(string codigo)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SP_A_AgregarAVenta", obj_sul_connection.Connect());

            try
            {
                DataRow productoAgregado;
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.AddWithValue("@Codigo", codigo);
                DataSet ds = new DataSet();
                ds.Clear();
                adapter.Fill(ds);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                adapter.SelectCommand.Parameters.Clear();
                await Task.Run(() => obj_sul_connection.CloseConnection());


                DataColumn Cantidad = new DataColumn();
                Cantidad.ColumnName = "Cantidad";
                DataColumn PrecioTotal = new DataColumn();
                PrecioTotal.ColumnName = "PrecioTotal";

                dt.Columns.Add(Cantidad);
                dt.Columns.Add(PrecioTotal);

                dt.Rows[0]["Cantidad"] = 1;
                dt.Rows[0]["PrecioTotal"] = Convert.ToDouble(dt.Rows[0]["Cantidad"]) * Convert.ToDouble(dt.Rows[0][3]);

                productoAgregado = dt.Rows[0];
                return productoAgregado;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Registrar Venta Actual
        public async Task<bool> RegistrarVenta (TypesLayer.TL_Ventas venta, TypesLayer.TL_Detalle_Ventas detalleVenta, int IdVendedor)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_V_RegistrarVenta", obj_sul_connection.Connect());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CedulaCliente", venta.CedulaCliente);
                cmd.Parameters.AddWithValue("@Fecha", venta.Fecha);
                cmd.Parameters.AddWithValue("@FacturaNo", venta.FacturaNo);
                cmd.Parameters.AddWithValue("@CantidadProductos", venta.CantidadProductos);
                cmd.Parameters.AddWithValue("@IVA", venta.IVA);
                cmd.Parameters.AddWithValue("@TotalVenta", venta.TotalVenta);
                cmd.Parameters.AddWithValue("@TasaDia", venta.TasaDia);
                cmd.Parameters.AddWithValue("@IdVendedor", IdVendedor);
                await Task.Run(() => cmd.ExecuteNonQuery());
                cmd.Parameters.Clear();
                await Task.Run(() => RegistrarProductosVenta(detalleVenta, IdVendedor));
                await Task.Run(() => RegistrarDetallePagoVenta(venta, IdVendedor));
                obj_sul_connection.CloseConnection();

                MessageBox.Show("Venta registrada correctamente");
                return true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un error al registrar la venta, intente de nuevo");
                return false;
            }
        }
        #endregion

        #region Registrar detalle de venta actual
        public async Task RegistrarProductosVenta (TypesLayer.TL_Detalle_Ventas detalleVenta, int IdVendedor)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_V_RegistrarProductosVenta", obj_sul_connection.Connect());
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (var producto in detalleVenta.CurrentSale)
                {
                    cmd.Parameters.AddWithValue("@CedulaCliente", detalleVenta.CedulaCliente);
                    cmd.Parameters.AddWithValue("@FacturaNo", detalleVenta.FacturaNo);
                    cmd.Parameters.AddWithValue("@Nombre", producto.NombreProductoVenta);
                    cmd.Parameters.AddWithValue("@Codigo", producto.CodigoProductoVenta);
                    cmd.Parameters.AddWithValue("@CantidadVendida", producto.CantidadProductoVenta);
                    cmd.Parameters.AddWithValue("@Precio", producto.PrecioUnitProductoVente);
                    cmd.Parameters.AddWithValue("@PrecioTotal", producto.TotalProductoVenta);
                    cmd.Parameters.AddWithValue("@IdVendedor", IdVendedor);
                    await Task.Run(() => cmd.ExecuteNonQuery());
                    cmd.Parameters.Clear();
                }
                obj_sul_connection.CloseConnection();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un error al registrar la venta, intente de nuevo");
            }
        }
        #endregion

        #region Registrar formas de pago de venta actual SP_V_RegistrarDetallePagoVenta
        public async Task RegistrarDetallePagoVenta(TypesLayer.TL_Ventas venta, int IdVendedor)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_V_RegistrarDetallePagoVenta", obj_sul_connection.Connect());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CedulaCliente", venta.CedulaCliente);
                cmd.Parameters.AddWithValue("@Fecha", venta.Fecha);
                cmd.Parameters.AddWithValue("@FacturaNo", venta.FacturaNo);
                cmd.Parameters.AddWithValue("@TotalEfectivoBsF", venta.TotalEfectivoBsF);
                cmd.Parameters.AddWithValue("@TotalEfectivoUSD", venta.TotalEfectivoUSD);
                cmd.Parameters.AddWithValue("@TotalTarjeta", venta.TotalTarjeta);
                cmd.Parameters.AddWithValue("@TotalOtras", venta.TotalOtras);
                cmd.Parameters.AddWithValue("@TotalVenta", venta.TotalVenta);
                cmd.Parameters.AddWithValue("@TasaDia", venta.TasaDia);
                cmd.Parameters.AddWithValue("@IdVendedor", IdVendedor);
                await Task.Run(() => cmd.ExecuteNonQuery());
                cmd.Parameters.Clear();
                obj_sul_connection.CloseConnection();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un error al registrar la venta, intente de nuevo");
            }
        }
        #endregion

        #region Consultar Detalle productos de Venta
        public async Task<TypesLayer.TL_Detalle_Ventas> ConsultarDetalleProductosVentas (string FacturaNo)
        {
            try
            {
                TypesLayer.TL_Detalle_Ventas ventaconsultada = new TypesLayer.TL_Detalle_Ventas();

                SqlDataAdapter adapter = new SqlDataAdapter("SP_M_BuscarDetalleVenta", obj_sul_connection.Connect());
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.AddWithValue("@FacturaNo", FacturaNo);
                DataSet ds = new DataSet();
                ds.Clear();
                adapter.Fill(ds);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                await Task.Run(() => obj_sul_connection.CloseConnection());

                ventaconsultada.CedulaCliente = dt.Rows[0][2].ToString();
                ventaconsultada.FacturaNo = dt.Rows[0][3].ToString();
                ventaconsultada.Fecha = dt.Rows[0][9].ToString();

                /// Almacenamiento de los productos de la venta consultada
                foreach (DataRow products in dt.Rows)
                {
                    ventaconsultada.CurrentSale.Add(new TypesLayer.TL_Producto_Vendido
                    {
                        CantidadProductoVenta = Convert.ToInt32(products[6]),
                        CodigoProductoVenta = products[5].ToString(),
                        NombreProductoVenta = products[4].ToString(),
                        PrecioUnitProductoVente = Convert.ToDouble(products[7]),
                        TotalProductoVenta = Convert.ToDouble(products[8]),
                    }
                );
                }
                return ventaconsultada;
            }
            catch
            {
                MessageBox.Show("Revise el numero de factura e intente de nuevo");
                return null;
            }

        }
        #endregion

        #region Consulta general de ventas

        public async Task<DataTable> ConsultaGeneralVenta(string fechaInicio = null, string fechafinal = null, string cedulaCliente = null, string reciboNo = null)
        {
            if (fechaInicio != null && fechafinal == null && cedulaCliente == null && reciboNo == null)
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SP_M_ConsultarVentasFecha",obj_sul_connection.Connect());
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.AddWithValue("@FechaInicio",Convert.ToDateTime(fechaInicio));
                adapter.SelectCommand.Parameters.AddWithValue("@FechaFinal", "XXXX");
                DataSet ds = new DataSet();
                ds.Clear();
                adapter.Fill(ds);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                await Task.Run(() => obj_sul_connection.CloseConnection());
                return dt;

            }
            if (fechaInicio != null && fechafinal != null && cedulaCliente == null && reciboNo == null)
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SP_M_ConsultarVentasFecha", obj_sul_connection.Connect());
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.AddWithValue("@FechaInicio", Convert.ToDateTime(fechaInicio));
                adapter.SelectCommand.Parameters.AddWithValue("@FechaFinal", Convert.ToDateTime(fechafinal));
                DataSet ds = new DataSet();
                ds.Clear();
                adapter.Fill(ds);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                await Task.Run(() => obj_sul_connection.CloseConnection());
                return dt;
            }
            if (cedulaCliente != null && reciboNo == null)
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SP_M_BuscarVentasCliente", obj_sul_connection.Connect());
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.AddWithValue("@CedulaCliente", cedulaCliente);
                DataSet ds = new DataSet();
                ds.Clear();
                adapter.Fill(ds);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                await Task.Run(() => obj_sul_connection.CloseConnection());
                return dt;
            }
            if (cedulaCliente == null && reciboNo != null)
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SP_M_BuscarFacturaVenta", obj_sul_connection.Connect());
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.AddWithValue("@FacturaNo", reciboNo);
                DataSet ds = new DataSet();
                ds.Clear();
                adapter.Fill(ds);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                await Task.Run(() => obj_sul_connection.CloseConnection());
                return dt;
            }
            return null;
        }

        #endregion

        #region Consultar Detalles de Venta
        public async Task<TypesLayer.TL_Detalle_Completo_Venta> ConsultarDetalleVenta (string ReciboNo)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SP_M_DetalleDeVenta", obj_sul_connection.Connect());
            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            adapter.SelectCommand.Parameters.AddWithValue("@FacturaNo", ReciboNo);
            DataSet ds = new DataSet();
            ds.Clear();
            adapter.Fill(ds);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            await Task.Run(() => obj_sul_connection.CloseConnection());

            #region Almacenamiento de los detalles del pago de la venta consultada
            TypesLayer.TL_Ventas DetallePagoVenta = new TypesLayer.TL_Ventas();
            DetallePagoVenta.CedulaCliente = dt.Rows[0][2].ToString();
            DetallePagoVenta.FacturaNo = dt.Rows[0][4].ToString();
            DetallePagoVenta.Fecha = dt.Rows[0][3].ToString();
            DetallePagoVenta.TotalVenta = Math.Round(Convert.ToDouble(dt.Rows[0][9]), 2);
            DetallePagoVenta.TotalUSD = Math.Round(DetallePagoVenta.TotalVenta / Convert.ToDouble(dt.Rows[0][10]), 2);
            DetallePagoVenta.IVA = Convert.ToDouble(dt.Rows[0][16]);
            DetallePagoVenta.TasaDia = Convert.ToDouble(dt.Rows[0][10]);
            DetallePagoVenta.TotalEfectivoUSD = dt.Rows[0][6].ToString();
            DetallePagoVenta.TotalEfectivoBsF = dt.Rows[0][5].ToString();
            DetallePagoVenta.TotalTarjeta = dt.Rows[0][7].ToString();
            DetallePagoVenta.TotalOtras = dt.Rows[0][8].ToString();
            #endregion

            #region Almacenamniento de los detalles de productos de la venta actual

            TypesLayer.TL_Detalle_Ventas DetalleProductosVenta = new TypesLayer.TL_Detalle_Ventas();

            foreach (DataRow product in dt.Rows)
            {
                DetalleProductosVenta.CurrentSale.Add(new TypesLayer.TL_Producto_Vendido
                {
                    CantidadProductoVenta = Convert.ToInt32(product[27]),
                    CodigoProductoVenta = product[26].ToString(),
                    NombreProductoVenta = product[25].ToString(),
                    PrecioUnitProductoVente = Convert.ToDouble(product[28]),
                    TotalProductoVenta = Convert.ToDouble(product[29]),
                }
                );
            }
            #endregion


            TypesLayer.TL_Detalle_Completo_Venta DetalleVenta = new TypesLayer.TL_Detalle_Completo_Venta();
            DetalleVenta.DetallePago = DetallePagoVenta;
            DetalleVenta.DetalleProductos = DetalleProductosVenta;

            return DetalleVenta;
        }
        #endregion

    }
}
