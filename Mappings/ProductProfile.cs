using AutoMapper;
using IMS.DTOs;
using IMS.DTOs.Product;
using IMS.Models;

namespace IMS.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductReadDto>();

            CreateMap<ProductCreateDTO, Product>();

            CreateMap<ProductUpdateDTO, Product>();
        }
    }
}