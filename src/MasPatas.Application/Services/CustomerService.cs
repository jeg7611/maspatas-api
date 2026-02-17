using AutoMapper;
using MasPatas.Application.DTOs;
using MasPatas.Application.Interfaces;
using MasPatas.Domain.Entities;

namespace MasPatas.Application.Services;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var customer = _mapper.Map<Customer>(request);
        await _customerRepository.CreateAsync(customer, cancellationToken);
        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<List<CustomerDto>> GetCustomersAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _customerRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<CustomerDto>>(customers);
    }
}
