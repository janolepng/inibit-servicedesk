namespace InibitServiceDesk.Models
{
    public static class StatusWerte
    {
        public const byte Neu = 1;
        public const byte InBearbeitung = 2;
        public const byte Wartet = 3;
        public const byte Erledigt = 4;
    }

    public static class PrioritaetsWerte
    {
        public const byte Kritisch = 1;
        public const byte Hoch = 2;
        public const byte Mittel = 3;
        public const byte Niedrig = 4;

        public static string Name(byte p)
        {
            switch (p)
            {
                case 1: return "Kritisch";
                case 2: return "Hoch";
                case 3: return "Mittel";
                default: return "Niedrig";
            }
        }
    }

    public static class StatusNamen
    {
        public static string Name(byte s)
        {
            switch (s)
            {
                case 1: return "Neu";
                case 2: return "In Bearbeitung";
                case 3: return "Wartet auf Kunde";
                case 4: return "Erledigt";
                default: return "Unbekannt";
            }
        }
    }
}