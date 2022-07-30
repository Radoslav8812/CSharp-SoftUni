using AutoMapper;
using ProductShop.DTOs.User;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //Map from ImportUserDto(source) to User(destination)
            CreateMap<ImportUserDto, User>();
        }
    }
}
