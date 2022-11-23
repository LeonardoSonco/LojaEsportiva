using AutoMapper;
using InventoryService.Data.Inventory;
using InventoryService.Models; 

namespace InventoryService.Profiles
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<CreateProductDto, Stock>(); // faz a conversão da classe productDto, para a classe stock ( que contem o id)
            CreateMap<Stock, ReadProductDto>(); 
            CreateMap<UpdateProductDto, Stock>();

        }
    }
}
