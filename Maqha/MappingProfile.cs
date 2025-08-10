using AutoMapper;
using Maqha.Core.Entities;
using Maqha.Utilities.DTO;

namespace Maqha
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<CafeInfo, CreateCafeInfoDTO>().ReverseMap();
            CreateMap<CafeInfo, UpdateCafeInfoDTO>().ReverseMap();

            //Category mappings
            CreateMap<Category, ResultCategoryDTO>().ReverseMap();
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
            CreateMap<Category, ResultCategoryWithMenuItemDTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryDTO>().ReverseMap();
            // MenuItem mappings
            CreateMap<MenuItem, ResultMenuItemDTO>().ReverseMap();
            CreateMap<MenuItem, CreateMenuItmDTO>().ReverseMap();
            CreateMap<MenuItem,CategoriesMenuItemDTO>().ReverseMap();
            //OrderItem mappings
            CreateMap<OrderItem, CreateOrderItemDTO>().ReverseMap();
            //Order mapping
            CreateMap<Order, CreateOrderDTO>().ReverseMap();
            CreateMap<Order, ResultOrderDTO>().ReverseMap();
            CreateMap<Order, OrderCreatedDTO>().ReverseMap();
            //Table mappings 
            CreateMap<Table, CreateTableDTO>().ReverseMap();

            // Add your mapping configurations here
            // For example:
            // CreateMap<SourceEntity, DestinationDto>();
            // CreateMap<CreateCafeInfoDTO, CafeInfo>();
        }
    }
}
