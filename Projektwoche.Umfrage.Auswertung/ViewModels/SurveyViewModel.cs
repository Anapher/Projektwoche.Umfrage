using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Anapher.Wpf.Swan;
using Projektwoche.Umfrage.Auswertung.Core;

namespace Projektwoche.Umfrage.Auswertung.ViewModels
{
    public class SurveyViewModel : PropertyChangedBase
    {
        public bool IsMale { get; set; }
        public int? Birthyear { get; set; }

        public SchoolClass Lk1 { get; set; }
        public SchoolClass Lk2 { get; set; }

        public SchoolGrade? SchoolGrade { get; set; }
        public SchoolAchivement? SchoolAchivement { get; set; }
        public SportyCharacter? SportyCharacter { get; set; }
        public bool IsInRelationship { get; set; }
        public bool IsAlcoholic { get; set; } = true;
        public bool IsSmoker { get; set; }
        public ObservableCollection<FavoriteDrink> FavoriteDrinks { get; set; } = new ObservableCollection<FavoriteDrink>();

        public bool IsGamer { get; set; } = true;
        public ObservableCollection<GamingPlatform> GamingPlatforms { get; set; } = new ObservableCollection<GamingPlatform>();
        public int? TimeForComputerGamesSchoolWeek { get; set; }    
        public int? TimeForComputerGamesHolidayWeek { get; set; }
        public ObservableCollection<ComputerGame> FavoriteGames { get; set; } = new ObservableCollection<ComputerGame>();
        public ObservableCollection<GameGenre> FavoriteGameGenre { get; set; } = new ObservableCollection<GameGenre>();
        public int? FavoriteGamePlayingSince { get; set; }
        public int? FavoriteGameTotalPlaytime { get; set; }

        public int? ReserveComputerGamesImportant { get; set; }
        public int? ReserveComputerGamesImpact { get; set; }
        public int? ReserveComputerGamesHelpStress { get; set; }
        public int? ReserveComputerGamesLifeBoring { get; set; }
        public int? ReserveComputerGamesHighlight { get; set; }
        public int? ReserveComputerGamesParents { get; set; }
        public int? ReserveComputerGamesSuccess { get; set; }

        public ReasonForNotPlaying? ReasonForNotPlaying { get; set; }
        public ViewOfGamers? ViewOfGamers { get; set; }
    }

    public enum ViewOfGamers
    {
        Nerds,
        Normal
    }

    public enum ReasonForNotPlaying
    {
        NoInterest,
        NoTime,
        DidntTry,
        NoIdea
    }

    public enum GameGenre
    {
        Shooter,
        Survival,
        Strategie,
        Sports,
        Rennspiele,
        Rollenspiele,
        MOBA,
        MMORPG
    }

    public enum GamingPlatform
    {
        Computer,
        Playstation,
        Xbox,
        Nintendo,
        Smartphone
    }

    public enum FavoriteDrink
    {
        Wodka,
        Korn,
        Whiskey,
        Jägermeister,
        Wein,
        Sekt,
        Bier
    }

    public enum SchoolGrade
    {
        E1,
        Q1,
        Q3
    }
    
    public enum SchoolAchivement
    {
        [EnumMember(Value = "15to13")]
        A15to13,
        [EnumMember(Value = "12to10")]
        A12to10,
        [EnumMember(Value = "9to7")]
        A9to7,
        [EnumMember(Value = "6to5")]
        A6to5,
        [EnumMember(Value = "le5")]
        le5 //less or equal 5
    }

    public enum SportyCharacter
    {
        VeryAthletic,
        Athletic,
        Average,
        NoSport
    }
}