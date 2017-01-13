using System.ComponentModel.DataAnnotations;

namespace YoTennis.Models.Commands
{
    public class StartCommand
    {
        [Required, Display(Name = "Add First Player Name")]
        public string FirstPlayer { get; set; }

        [Required, Display(Name = "Add Second Player Name")]
        public string SecondPlayer { get; set; }

        [Required, Display(Name = "Sets for win")]
        public int SetsForWin { get; set; } = 3;

        [Required, Display(Name = "Tiebreak for Final Set")]
        public bool TiebreakFinal { get; set; } = false;

        [Required, Display(Name = "Games in a set")]
        public int GamesInSet { get; set; } = 6;

        [Required, Display(Name = "Points in game")]
        public int PointsInGame { get; set; } = 4;

        [Required, Display(Name = "Points in Tiebreak")]
        public int PointsInTiebreak { get; set; } = 7;
    }
}
