using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RitualProject
{
    public class ConservationsMy
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> Created_at { get; set; }
        public Nullable<System.DateTime> Updated_at { get; set; }
        public Nullable<System.DateTime> Deleted_at { get; set; }
        public Nullable<System.DateTime> DateOfLastMessage { get; set; }
        public string LastMessage { get; set; }
        public Nullable<int> Creator_id { get; set; }
        public Nullable<int> lastMessageId { get; set; }
        public Nullable<bool> Is_Pinned { get; set; }
        public Nullable<bool> Is_Archived { get; set; }
        public string Login { get; set; }
        public byte[] Image { get; set; }
    }
}
