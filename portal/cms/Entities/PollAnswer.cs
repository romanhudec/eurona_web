using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities {
    [Serializable]
    public class PollAnswer : Entity {
        public class ReadById {
            public int PollAnswerId { get; set; }
        }

        public class ReadByPollOptionId {
            public int PollOptionId { get; set; }
        }

        public class ReadByPollAndIP {
            public int PollId { get; set; }
            public string IP { get; set; }
        }

        public int InstanceId { get; set; }
        public string IP { get; set; }

        //FK PollOption
        public int PollOptionId { get; set; }
        public PollOption answer = null;
        public PollOption Answer {
            get {
                if (answer != null) return answer;
                answer = Storage<PollOption>.ReadFirst(new PollOption.ReadById { PollOptionId = this.PollOptionId });
                return answer;
            }
        }
    }
}
