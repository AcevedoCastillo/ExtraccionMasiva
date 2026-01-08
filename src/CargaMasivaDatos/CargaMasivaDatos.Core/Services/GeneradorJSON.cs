using CargaMasivaDatos.Core.Interfaces;
using CargaMasivaDatos.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CargaMasivaDatos.Core.Services
{
    public class GeneradorJSON : IGeneradorJSON
    {
        public string GenerarJSON(Pedido pedido, int loteId)
        {
            if (pedido == null)
                throw new ArgumentNullException(nameof(pedido));

            var jsonObject = new
            {
                pedidoId = pedido.PedidoID,
                cliente = new
                {
                    clienteId = pedido.Cliente.ClienteID,
                    nombre = pedido.Cliente.NombreCompleto,
                    email = pedido.Cliente.Email,
                    telefono = pedido.Cliente.Telefono
                },
                fechaPedido = pedido.FechaPedido,
                total = pedido.Total,
                estado = pedido.Estado,
                detalles = pedido.Detalles.Select(d => new
                {
                    producto = d.Producto.NombreProducto,
                    categoria = d.Producto.Categoria,
                    cantidad = d.Cantidad,
                    precioUnitario = d.PrecioUnitario,
                    subtotal = d.Subtotal
                }).ToList(),
                procesamiento = new
                {
                    loteId = loteId,
                    fechaProcesamiento = DateTime.Now
                }
            };

            return JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
        }

        public bool ValidarJSON(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return false;

            try
            {
                JToken.Parse(json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
