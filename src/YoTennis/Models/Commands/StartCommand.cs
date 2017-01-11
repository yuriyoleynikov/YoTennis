using System.ComponentModel.DataAnnotations;

namespace YoTennis.Models.Commands
{
    public class StartCommand
    {
        [Required, Display(Name = "First Player Name")]
        public string FirstPlayer { get; set; }

        [Required, Display(Name = "Second Player Name")]
        public string SecondPlayer { get; set; }

        [Required, Display(Name = "Sets for win")]
        public int SetsForWin { get; set; } = 2;

        [Required, Display(Name = "Tiebreak")]
        public bool TiebreakFinal { get; set; } = false;

        [Required, Display(Name = "Games in a set")]
        public int GamesInSet { get; set; } = 6;
        public int PointsInGame { get; set; } = 4;
        public int PointsInTiebreak { get; set; } = 7;

    }
}
