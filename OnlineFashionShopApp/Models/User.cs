using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineFashionShopApp.Models
{
    public partial class User
    {
        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Phonenumber { get; set; }
        public int ShipmentId { get; set; }

        public int Userrole { get; set; }

        public virtual ICollection<AccessLog> AccessLogs { get; set; } = new List<AccessLog>();

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
