using AutoMapper;
using CashManiaAPI.Data.Models.Entities;
using CashManiaAPI.DTOs;

namespace CashManiaAPI.Automapper;

public class DTOToDomainMappingProfile : Profile
{
    public DTOToDomainMappingProfile()
    {
        CreateMap<TransactionDto, Transaction>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.UserId, opt => opt.Ignore())
            .ForMember(x => x.User, opt => opt.Ignore());
    }
}