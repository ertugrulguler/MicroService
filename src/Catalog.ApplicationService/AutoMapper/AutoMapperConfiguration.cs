using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.ApplicationService.AutoMapper
{
    public class AutoMapperConfiguration : IAutoMapperConfiguration
    {
        private readonly IMapper mapper;
        private readonly MapperConfiguration configuration;

        public AutoMapperConfiguration()
        {
            configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps("Catalog.ApplicationService");
            });

            mapper = new Mapper(configuration);
        }


        public TReturn MapObject<TMap, TReturn>(TMap obj) where TMap : class where TReturn : class
        {
            return mapper.Map<TMap, TReturn>(obj);
        }

        public List<TReturn> MapCollection<TMap, TReturn>(IEnumerable<TMap> expression) where TMap : class where TReturn : class
        {
            return mapper.Map<IEnumerable<TMap>, IEnumerable<TReturn>>(expression).ToList();
        }
    }
}
