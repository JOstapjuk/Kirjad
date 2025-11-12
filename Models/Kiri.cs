using System.Text.Json.Serialization;

namespace Kirjad.Models
{
    public class Kiri
    {

        public int Id { get; set; }

        public string Pealkiri { get; set; } = string.Empty;


        public string Sisu { get; set; } = string.Empty;

        public string UnikaalsekKood { get; set; } = string.Empty;

        public DateTime LoodudKuupaev { get; set; } = DateTime.Now;

        [JsonIgnore]
        public virtual ICollection<Kiri> SeotudKirjad { get; set; } = new List<Kiri>();

        [JsonIgnore]
        public virtual ICollection<Kiri> TagasiViited { get; set; } = new List<Kiri>();

        public virtual string Kirjuta()
        {
            return $"Pealkiri: {Pealkiri}\nSisu: {Sisu}\nKood: {UnikaalsekKood}";
        }

        public int ViideteArv()
        {
            return TagasiViited?.Count ?? 0;
        }
    }
}
