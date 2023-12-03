using AutoMapper;

namespace Service.AuthAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                //config.CreateMap<Coupon, CouponDto>();
                //config.CreateMap<CouponDto, Coupon>();
            });

            return mappingConfig;
        }
    }
}
