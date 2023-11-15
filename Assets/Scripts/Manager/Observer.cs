using System;

namespace Manager
{
    public static class Observer
    {
        public static Action<bool> showHideAllInformation;
        public static Action<bool> showHideButtonVisit;
        public static Action rotatePlanet;
    }

    [Serializable]
    public enum CelestialType
    {
        Sun,
        Mercury,
        Venus,
        Mars,
        Earth,
        Moon,
        Jupiter,
        Saturn,
        Uranus,
        Neptune,
        Pluto
    }
}
