using AutoMapper;
using MasPatas.Application.DTOs;
using MasPatas.Domain.Entities;

namespace MasPatas.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateProductRequest, Product>();
        CreateMap<Product, ProductDto>();

        CreateMap<CreateCustomerRequest, Customer>();
        CreateMap<Customer, CustomerDto>();

        CreateMap<Inventory, InventoryDto>();
        CreateMap<InventoryMovement, InventoryMovementDto>();

        CreateMap<SaleItem, SaleItemDto>();
        CreateMap<Sale, SaleDto>();

        CreateMap<User, UserDto>();
    }
}
