using InveComv1.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using TypesLayer;
using MessageBox = System.Windows.Forms.MessageBox;
using iTextSharp.text;
using iTextSharp.tool.xml;
using iTextSharp.text.pdf;
using System.IO;

namespace InveComv1.ViewModels
{
    public class PagosViewModel : Bases.BindableBase
    {
        public PagosViewModel()
        {
            isBusy = true;

            ViewCommand = new Bases.DelegateCommand<string>(ViewCommandExecute, ViewCommandCanExecute);
        }

        /// <summary>
        /// Generacion de una propiedad tipo Ventana para manejar los objetos dentro de la misma
        /// </summary>
        private Views.EditWindow _Window;
        public Views.EditWindow Window { get => _Window; set => _Window = value; }

        /// <summary>
        /// Propiedad de deteccion del estado ocupado de la ventana
        /// </summary>
        private bool isbusy;
        public bool isBusy { get => isbusy; set => SetProperty(ref isbusy, value); }

        /// <summary>
        /// Propiedad de declaracion de las propiedades que permiten ejecutar los comandos para manejo de botones
        /// </summary>
        private Bases.DelegateCommand<string> viewCommand;
        public Bases.DelegateCommand<string> ViewCommand { get { return viewCommand; } set { SetProperty(ref viewCommand, value); } }

        /// <summary>
        /// Propiedad de almacenamiento de los valores del user control actual
        /// </summary>
        private EditWindow pagoActual;
        public EditWindow PagoActual { get => pagoActual; set => pagoActual = value; }

        /// <summary>
        /// Propiedad de regreso a la ventana de venta
        /// </summary>
        private Views.Ventas regresar;
        public Views.Ventas Regresar { get => regresar; set => regresar = value; }

        /// <summary>
        /// Propiedad de deteccion del total de la venta actual
        /// </summary>
        private string total;
        public string Total { get => total; set => SetProperty(ref total, value); }

        /// <summary>
        /// Propiedad de deteccion del valor por pagar de la cuenta actual
        /// </summary>
        private string porPagar;
        public string PorPagar { get => porPagar; set { SetProperty(ref porPagar, value); } }

        /// <summary>
        /// Propiedad de deteccion del cambio en la venta actual
        /// </summary>
        private object cambio;
        public object Cambio { get => cambio; set => SetProperty(ref cambio, value); }

        /// <summary>
        /// Propiedad de deteccion del valor en USD de la venta
        /// </summary>
        private object porPagarUSD;
        public object PorPagarUSD { get => porPagarUSD; set => SetProperty(ref porPagarUSD, value); }

        /// <summary>
        /// Formas de Pago
        /// </summary>
        private string otras = "0.00";
        public string Otras { get => otras; set { SetProperty(ref otras, value); ActPagoActual(); } }

        private string tarjeta = "0.00";
        public string Tarjeta { get => tarjeta; set { SetProperty(ref tarjeta, value); ActPagoActual(); } }

        private string efectivoUSD = "0.00";
        public string EfectivoUSD { get => efectivoUSD; set { SetProperty(ref efectivoUSD, value); ActPagoActual(); } }

        private string efectivoBsF = "0.00";
        public string EfectivoBsF { get => efectivoBsF; set { SetProperty(ref efectivoBsF, value); ActPagoActual(); } }

        /// <summary>
        /// Propiedad de almacenamiento de los detalles de la venta actual
        /// </summary>
        private TL_Detalle_Ventas detalleVenta;
        public TL_Detalle_Ventas DetalleVenta { get => detalleVenta; set => detalleVenta = value; }

        /// <summary>
        /// Propiedad de almacenamiento de la venta actual
        /// </summary>
        private TypesLayer.TL_Ventas venta;
        public TL_Ventas Venta { get => venta; set => venta = value; }

        /// <summary>
        /// Propiedad de almacenamiento de la tasa de cambio actual
        /// </summary>
        private object tasaUSD;
        public object TasaUSD { get => tasaUSD; set => SetProperty(ref tasaUSD, value); }

        #region Actualizar pago y cambio de la venta actual
        private void ActPagoActual()
        {
            try
            {
                PorPagar = Math.Round((Convert.ToDouble(Total) - Convert.ToDouble(EfectivoUSD) * Convert.ToDouble(TasaUSD) - Convert.ToDouble(EfectivoBsF) - Convert.ToDouble(Tarjeta) - Convert.ToDouble(Otras) * Convert.ToDouble(TasaUSD)), 2).ToString();
                PorPagarUSD = Math.Round(Convert.ToDouble(PorPagar) / Convert.ToDouble(TasaUSD), 2).ToString();
                Cambio = Math.Round((Convert.ToDouble(EfectivoUSD) * Convert.ToDouble(TasaUSD)  + Convert.ToDouble(EfectivoBsF) + Convert.ToDouble(Tarjeta) + Convert.ToDouble(Otras) * Convert.ToDouble(TasaUSD) - Convert.ToDouble(Total)), 2).ToString();
            }
            catch (Exception ex)
            {
                if (String.IsNullOrEmpty(EfectivoUSD))
                {
                    PorPagar = Math.Round((Convert.ToDouble(Total) - Convert.ToDouble(EfectivoBsF) - Convert.ToDouble(Tarjeta) - Convert.ToDouble(Otras) * Convert.ToDouble(TasaUSD)), 2).ToString();
                }
                if (String.IsNullOrEmpty(EfectivoBsF))
                {
                    PorPagar = Math.Round((Convert.ToDouble(Total) - Convert.ToDouble(EfectivoUSD) * Convert.ToDouble(TasaUSD) - Convert.ToDouble(Tarjeta) - Convert.ToDouble(Otras) * Convert.ToDouble(TasaUSD)), 2).ToString();
                }
                if (String.IsNullOrEmpty(Tarjeta))
                {
                    PorPagar = Math.Round((Convert.ToDouble(Total) - Convert.ToDouble(EfectivoUSD) * Convert.ToDouble(TasaUSD) - Convert.ToDouble(EfectivoBsF) - Convert.ToDouble(Otras) * Convert.ToDouble(TasaUSD)), 2).ToString();
                }
                if (String.IsNullOrEmpty(Otras))
                {
                    PorPagar = Math.Round((Convert.ToDouble(Total) - Convert.ToDouble(EfectivoUSD) * Convert.ToDouble(TasaUSD) - Convert.ToDouble(EfectivoBsF) - Convert.ToDouble(Tarjeta)), 2).ToString();
                }
            }
        }
        #endregion

        #region Reguion ejecuion de comandos para botones
        private bool ViewCommandCanExecute(string action)
        {
            return true;
        }

        private async void ViewCommandExecute(string action)
        {
            switch (action)
            {

                #region Cancelar Venta Actual
                case "cancelar":
                    MessageBox.Show("Se cancelara la venta");
                    Window.Close();
                    break;
                #endregion

                #region Facturar Venta acutal
                case "facturar":
                    if (Convert.ToDouble(PorPagar) > 0 || String.IsNullOrEmpty(PorPagar))
                    {
                        MessageBox.Show("Indique las formas de pago para continuar");
                    }
                    else
                    {
                        Venta.TotalEfectivoBsF = EfectivoBsF;
                        Venta.TotalEfectivoUSD = EfectivoUSD;
                        Venta.TotalTarjeta = Tarjeta;
                        Venta.TotalOtras = Otras;
                        Venta.TasaDia = Convert.ToDouble(TasaUSD);
                        DetalleVenta.TasaDia = Convert.ToDouble(TasaUSD);
                        double PagoTotal = Convert.ToDouble(EfectivoBsF) + Convert.ToDouble(EfectivoUSD) * Convert.ToDouble(TasaUSD) + Convert.ToDouble(Tarjeta) + Convert.ToDouble(Otras) * Convert.ToDouble(TasaUSD);
                        double Subtotal = Convert.ToDouble(Venta.TotalVenta) - Convert.ToDouble(Venta.IVA);
                        SetUpLayer.SUL_Ventas venta = new SetUpLayer.SUL_Ventas();
                        if(await Task.Run(() => venta.RegistrarVenta(Venta, DetalleVenta, Properties.Settings.Default.IdUsuario)))
                        {
                            #region Generacion de factura en pdf

                            SaveFileDialog savefile = new SaveFileDialog
                            {
                                FileName = Venta.FacturaNo + ".pdf"
                            };

                            string pagina = Properties.Resources.factura.ToString();
                            pagina = pagina.Replace("@FacturaNo", Venta.FacturaNo);
                            pagina = pagina.Replace("@CedulaRifCliente", Venta.CedulaCliente);
                            pagina = pagina.Replace("@efectivo", PagoTotal.ToString());
                            pagina = pagina.Replace("@cambio", Cambio.ToString());
                            pagina = pagina.Replace("@Usuario", Properties.Settings.Default.NameUsuario);
                            pagina = pagina.Replace("@Fecha", DateTime.Now.ToString("dd/MM/yyyy"));

                            string filasFactura = null;
                            foreach (var producto in DetalleVenta.CurrentSale)
                            {
                                filasFactura += "<tr>";
                                filasFactura += "<td align=\"center\">" + producto.CantidadProductoVenta.ToString() + "</td>";
                                filasFactura += "<td align=\"center\">" + producto.NombreProductoVenta.ToString() + "</td>";
                                filasFactura += "<td align=\"right\">" + producto.PrecioUnitProductoVente.ToString() + "</td>";
                                filasFactura += "<td align=\"right\">" + producto.TotalProductoVenta.ToString() + "</td>";
                                filasFactura += "</tr>";
                            }

                            pagina = pagina.Replace("@cant_articulos", Venta.CantidadProductos.ToString());
                            pagina = pagina.Replace("@grid", filasFactura);
                            pagina = pagina.Replace("@SUBTOTAL", Subtotal.ToString());
                            pagina = pagina.Replace("@IVA", Venta.IVA.ToString());
                            pagina = pagina.Replace("@TOTAL", Venta.TotalVenta.ToString());

                            if (savefile.ShowDialog() == DialogResult.OK)
                            {
                                using (FileStream stream = new FileStream(savefile.FileName, FileMode.Create))
                                {
                                    int artfilas = DetalleVenta.CurrentSale.Count;
                                    Rectangle pagesize = new Rectangle(298, 420 + (artfilas * 10));
                                    Document pdfdoc = new Document(pagesize, 25, 25, 25, 25);
                                    PdfWriter writer = PdfWriter.GetInstance(pdfdoc, stream);
                                    pdfdoc.Open();

                                    using (StringReader sr = new StringReader(pagina))
                                    {
                                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfdoc, sr);
                                    }
                                    pdfdoc.Close();
                                    stream.Close();
                                }
                                //MessageBox.Show("Venta exitosa");
                                isBusy = false;
                            }
                            #endregion
                            Venta = null;
                            DetalleVenta = null;
                            PagoActual.Close();
                        }
                    }
                    break;
                #endregion

                default:
                    break;
            }

        }
        #endregion
    }
}
