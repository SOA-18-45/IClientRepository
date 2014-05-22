using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IClientRepository.Domain
{
    public class ClientRepo2
    {
        public virtual System.Guid IdClient { set; get; }
        public virtual string Name { set; get; }
        public virtual string Lastname { set; get; }
        public virtual string PESEL { set; get; }
        public virtual string address { set; get; }
    }
}
