using AutoMapper;
using MasPatas.Application.DTOs;
using MasPatas.Application.Interfaces;
using MasPatas.Domain.Entities;

namespace MasPatas.Application.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = _mapper.Map<Product>(request);
        await _productRepository.CreateAsync(product, cancellationToken);
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<List<ProductDto>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<ProductDto>>(products);
    }
}
