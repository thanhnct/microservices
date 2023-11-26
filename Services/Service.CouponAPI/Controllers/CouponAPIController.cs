using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Service.CouponAPI.Data;
using Service.CouponAPI.Models;
using Service.CouponAPI.Models.Dto;
using System.Diagnostics;

namespace Service.CouponAPI.Controllers
{
    [ApiController]
    [Route("/v1/api/coupon")]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<CouponAPIController> _logger;
        private ResponseDto _response;
        private IMapper _mapper;

        public CouponAPIController(AppDbContext db, ILogger<CouponAPIController> logger, IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> coupons = _db.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _logger.LogError(ex, "Internal error system. Can not get list coupon.");
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                var coupon = _db.Coupons.Find(id);
                if (coupon is not null)
                    _response.Result = _mapper.Map<CouponDto>(coupon);
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Coupon not found";
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _logger.LogError(ex, "Internal error system. CouponId: {id}", id);
            }
            return _response;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                var coupon = _db.Coupons.FirstOrDefault(x => x.CouponCode.ToLower().Equals(code.ToLower()));
                if (coupon is not null)
                    _response.Result = _mapper.Map<CouponDto>(coupon);
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Coupon not found";
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _logger.LogError(ex, "Internal error system. CouponCode: {code}", code);
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDto Delete(int id)
        {
            try
            {
                var coupon = _db.Coupons.Find(id);
                _db.Remove(coupon);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _logger.LogError(ex, "Internal error system. CouponId {id}", id);
            }
            return _response;
        }

        [HttpPost]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDto);
                _db.Add(coupon);
                _db.SaveChanges();

                _response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _logger.LogError(ex, "Internal error system");
            }
            return _response;
        }

        [HttpPut]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDto);
                _db.Update(coupon);
                _db.SaveChanges();

                _response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _logger.LogError(ex, "Internal error system");
            }
            return _response;
        }
    }
}
