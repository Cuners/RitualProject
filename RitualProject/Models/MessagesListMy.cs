using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RitualProject
{
    public class MessagesListMy 
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string FileURL {  get; set; }
        public byte[] ImagePho { get; set; }
        public Nullable<System.DateTime> Created_at { get; set; }
        public Nullable<System.DateTime> Deleted_at { get; set; }
        public Nullable<int> Razgovor_id { get; set; }
        public Nullable<int> Sender_id { get; set; }
        public Nullable<int> Type_id { get; set; }
        public bool IsSender { get; set; }
    }
}
