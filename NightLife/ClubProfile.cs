using AutoMapper;
using NightLife.Models;
using NightLife.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightLife
{
    public class ClubProfile : Profile
    {
        public ClubProfile()
        {
            // may not work
            this.CreateMap<Club, ClubModel>()
                .ForMember(c => c.Events, o => o.MapFrom(m => m.Events))
                .ReverseMap();

            this.CreateMap<Club, ClubModelPost>()
                .ForMember(c => c.Events, o => o.MapFrom(m => m.Events))
                .ReverseMap();

            this.CreateMap<Event, EventModel>()
                .ReverseMap();
        }
    }
}
