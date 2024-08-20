namespace Backend.Models
{
    public class PaymentDataDTO
    {
        public string Duznik { get; set; }
        public string RacunDuznika { get; set; }
        public string Poverilac { get; set; }
        public string RacunPoverioca { get; set; }
        public string Sifra { get; set; }
        public string Valuta { get; set; }
        public decimal Iznos { get; set; }
        public string Svrha { get; set; }
        public string Model { get; set; }
        public string Poziv { get; set; }
        public string Email { get; set; }
    }
}
