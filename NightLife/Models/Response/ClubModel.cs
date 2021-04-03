using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightLife.Models.Response
{
    public class ClubModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Rating { get; set; }
        public ISet<EventModel> Events { get; set; }
    }
}
