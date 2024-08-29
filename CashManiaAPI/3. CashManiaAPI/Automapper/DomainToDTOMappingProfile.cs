using AutoMapper;
using CashManiaAPI.Data.Models.Entities;
using CashManiaAPI.DTOs;

namespace CashManiaAPI.Automapper;

public class DomainToDTOMappingProfile : Profile
{
    public DomainToDTOMappingProfile()
    {
        CreateMap<Transaction, TransactionDto>();

        CreateMap<Category, CategoryDto>();
    }
}