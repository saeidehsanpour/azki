using AutoMapper;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views;
using PerformanceManagementSystem.Data.Views.Competencies;
using PerformanceManagementSystem.Data.Views.CompetencyCategories;
using PerformanceManagementSystem.Data.Views.CompetencyLevels;
using PerformanceManagementSystem.Data.Views.Organizations;
using PerformanceManagementSystem.Data.Views.Positions;

namespace PerformanceManagementSystem;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Users
        CreateMap<AppUser, UserResponseDto>()
            .ForMember(dest => dest.Name
                , opt
                    => opt.MapFrom(src => $"{src.Firstname} {src.Lastname}"));


        // Organizations
        CreateMap<OrganizationRequestDto, Organization>();
        CreateMap<Organization, OrganizationResponseDto>()
            .ForMember(dest => dest.AdminUserName
            , opt
                => opt.MapFrom(src => $"{src.AdminUser.Firstname} {src.AdminUser.Lastname}"));


        // CompetencyCategories
        CreateMap<CompetencyCategoryRequestDto, CompetencyCategory>();
        CreateMap<CompetencyCategory, CompetencyCategoryResponseDto>();


        // Competencies
        CreateMap<CompetencyRequestDto, Competency>();
        CreateMap<Competency, CompetencyResponseDto>();


        // CompetencyLevels
        CreateMap<CompetencyLevelRequestDto, CompetencyLevel>();
        CreateMap<CompetencyLevel, CompetencyLevelResponseDto>();


        // Positions
        CreateMap<PositionRequestDto, Position>();
        CreateMap<Position, PositionResponseDto>();
    }
}