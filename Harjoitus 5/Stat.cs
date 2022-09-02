using System.ComponentModel.DataAnnotations;

namespace Harjoitus_5
{
    /// <summary>
    /// Malliluokka (Entity), joka edustaa tietoja, joita sovelluksella hallinnoidaan.
    /// </summary>
    public class Stat
    {
        [Key]
        public int Id { get; set; }
        public int CurrentHitPoints { get; set; }
        public int MaxHitPoints { get; set; }
        public int Gold { get; set; }
        public int Exp { get; set; }
        public int CurrentLocationID { get; set; }
    }
}
