﻿using WebFrontend.Models;
using WebFrontend.Service.IService;
using static WebFrontend.Utility.SD;

namespace WebFrontend.Service
{
    public class CouponService : ICouponService
    {
        private readonly string apiVersion = "v1";
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Data = couponDto,
                Url = CouponAPIBase + $"/{apiVersion}/api/coupon"
            });
        }
        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.DELETE,
                Url = CouponAPIBase + $"/{apiVersion}/api/coupon/{id}"
            });
        }
        public async Task<ResponseDto?> GetAllCouponAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = CouponAPIBase + $"/{apiVersion}/api/coupon"
            });
        }
        public async Task<ResponseDto?> GetCouponAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = CouponAPIBase + $"/{apiVersion}/api/coupon/GetByCode/{couponCode}"
            });
        }
        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = CouponAPIBase + $"/{apiVersion}/api/coupon/{id}"
            });
        }
        public async Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.PUT,
                Data = couponDto,
                Url = CouponAPIBase + $"/{apiVersion}/api/coupon"
            });
        }
    }
}
