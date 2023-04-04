using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.CompetencyCategoryPositionMappings;

public class CompetencyCategoryPositionMappingRequestDto
{
    public CompetencyCategoryPositionMappingRequestDto()
    {
        CompetencyCategoryIds = new List<Guid>();
    }

    public Guid PositionId { get; set; }
    [DisplayName("دسته بندی شایستگی ها")]
    public List<Guid> CompetencyCategoryIds { get; set; }
}