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
    public class SUL_Totalizacion
    {

        #region Preparar cierre de caja
        public async Task<TypesLayer.TL_CierreCaja> PrepararCierre()
        {

            TypesLayer.TL_CierreCaja cajacerrada = new TypesLayer.TL_CierreCaja();
            SUL_Ventas obj_sul_ventas = new SUL_Ventas();
            DataTable ventasdia = await Task.Run(() => obj_sul_ventas.ConsultaGeneralVenta(DateTime.Now.ToString("dd/MM/yyyy")));
            TypesLayer.TL_Detalle_Completo_Venta DetalleVenta = new TypesLayer.TL_Detalle_Completo_Venta();

            foreach (DataRow venta in ventasdia.Rows)
            {
                DetalleVenta = await obj_sul_ventas.ConsultarDetalleVenta(venta[3].ToString());
                cajacerrada.EfectivoBsF += Math.Round(Convert.ToDouble(DetalleVenta.DetallePago.TotalEfectivoBsF), 2);
                cajacerrada.EfectivoUSD += Math.Round(Convert.ToDouble(DetalleVenta.DetallePago.TotalEfectivoUSD), 2);
                cajacerrada.Tarjeta += Math.Round(Convert.ToDouble(DetalleVenta.DetallePago.TotalTarjeta), 2);
                cajacerrada.Otras += Math.Round(Convert.ToDouble(DetalleVenta.DetallePago.TotalOtras), 2);
                cajacerrada.TasaDia = Math.Round(Convert.ToDouble(DetalleVenta.DetallePago.TasaDia), 2);
                cajacerrada.TotalVentas += Math.Round(Convert.ToDouble(DetalleVenta.DetallePago.TotalVenta), 2);
            }
            cajacerrada.Fecha = DateTime.Now.ToString("dd/MM/yyyy");
            return cajacerrada;
        }

        #endregion

        #region cerrar caja y alamacenar informacion del cierre
        public async Task<bool> CerrarCaja (TypesLayer.TL_CierreCaja caja)
        {
            try
            {
                SUL_Connection con = new SUL_Connection();
                SqlCommand command = new SqlCommand("SP_C_CierreCaja", con.Connect());

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdUsuario", caja.IdUsuario);
                command.Parameters.AddWithValue("@Fecha", caja.Fecha);
                command.Parameters.AddWithValue("@TasaDia", caja.TasaDia);
                command.Parameters.AddWithValue("@TotalEfectivoUSD", caja.EfectivoUSD);
                command.Parameters.AddWithValue("@TotalEfectivoBsF", caja.EfectivoBsF);
                command.Parameters.AddWithValue("@TotalTarjeta", caja.Tarjeta);
                command.Parameters.AddWithValue("@TotalOtra", caja.Otras);
                command.Parameters.AddWithValue("@TotalVentas", caja.TotalVentas);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                await Task.Run(() => con.CloseConnection());
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado, intente de nuevo");
                return false;
            }
            }

        #endregion
    }
}
