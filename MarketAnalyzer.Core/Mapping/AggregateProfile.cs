using AutoMapper;
using MarketAnalyzer.Core.Model;
using MarketAnalyzer.Domain.Model;

namespace MarketAnalyzer.Core.Mapping
{
    public class AggregateProfile : Profile
    {
        public AggregateProfile()
        {
            CreateMap<ItemIndicator, ItemStatistic>();
            CreateMap<ItemAggregatedStatistic, ItemWeekIndicator>();
        }
    }
}
