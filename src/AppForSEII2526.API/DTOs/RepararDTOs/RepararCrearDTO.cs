using AppForSEII2526.API.DTOs;
using static AppForSEII2526.API.Models.Reparación;

namespace AppForSEII2526.API.DTOs
{
    public class RepararCrearDTO
    {
        private DateTime dateTime;
        private string v1;
        private string v2;
        private List<RepararItemDTO> repararItemDTOs;
        private metodoDePago efectivo;
        private object value;

        public RepararCrearDTO(DateTime dateTime, string v1, string v2, List<RepararItemDTO> repararItemDTOs, metodoDePago efectivo, object value)
        {
            this.dateTime = dateTime;
            this.v1 = v1;
            this.v2 = v2;
            this.repararItemDTOs = repararItemDTOs;
            this.efectivo = efectivo;
            this.value = value;
        }

        public RepararCrearDTO(DateTime fechaEntrega, DateTime fechaRecogida,
            float precioTotal, string name, string surname, IList<RepararItemDTO> repararItem, metodoDePago tiposMetodoPago, string phone)
        {
            FechaEntrega = fechaEntrega;
            FechaRecogida = fechaRecogida;
            PrecioTotal = precioTotal;
            Name = name;
            Surname = surname;
            RepararItem = repararItem;
            TiposMetodoPago = tiposMetodoPago;
            Phone = phone;
        }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaEntrega { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaRecogida { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, 100, ErrorMessage = "Minimo es 0.5 y maximo 100")]
        [Display(Name = "Precio Total de Alquiler")]
        [Precision(10, 2)]
        public float PrecioTotal { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Apellidos")]
        public string Surname { get; set; }

        [Display(Name = "Telefono")]
        public string Phone { get; set; }

        public metodoDePago TiposMetodoPago { get; set; }

        public IList<RepararItemDTO> RepararItem { get; set; }

    }
}