using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eurona.DAL.Entities {
    [Serializable]
    public static class Classifiers {
        public enum BonusovyKreditTyp : int {
            None = 0,
            RucneZadany = 1,
            OdeslanaObjednavka = 2,
            ProduktEmailPoslatPriteli = 3,
            ProduktEmailFacebook = 4,
            ProduktHodnoceni = 5,
            ProduktDetail = 6,

            RegistracePodrizenehoPrvniObjednavka = 7,
            ShareSpecialniNabidkyFacebook = 8,
            ShareAkcniNabidkyFacebook = 9,
            Eurosap = 10,
            MaximalniPocetBKzaMesic = 9999
        }
    }
}