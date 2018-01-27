using Anapher.Wpf.Swan;

namespace Projektwoche.Umfrage.Auswertung.Core
{
    public class ComputerGame : PropertyChangedBase
    {
        private string _name;

        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set => SetProperty(value, ref _name);
        }

        protected bool Equals(ComputerGame other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ComputerGame) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}