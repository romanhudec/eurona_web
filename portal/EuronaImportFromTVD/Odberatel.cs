using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuronaImportFromTVD {
    public class Odberatel {

        public Odberatel(int idOdberatele, string password) {
            this.IdOdberatele = idOdberatele;
            this.Password = password;
        }

        public int IdOdberatele { get; set; }
        public string Password { get; set; }
    }
}
