namespace Kirjad.Models
{
    public class Veebiuudis : Kiri
    {
        public string URL { get; set; } = string.Empty;

        public override string Kirjuta()
        {
            return $"{base.Kirjuta()}\nURL: {URL}";
        }
    }
}
