using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHP.Entities {
    [Serializable]
    public class AccountVote : CMS.Entities.AccountVote {
        public new enum ObjectType : int {
            Product = 1000,
            ProductComment = 1001
        }
    }
}
