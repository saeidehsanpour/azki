using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Views.MenuItems;
using System.Security.Claims;

namespace PerformanceManagementSystem.ViewComponents;

public class UserMenuItemsViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public UserMenuItemsViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (HttpContext.Request.Cookies["PerformanceManagementCookie"] == null)
            return await Task.Run(() => View(new List<UserMenuItemResponseDto>()));

        var currentUrl = HttpContext.Request.Path.Value ?? string.Empty;

        var res = new List<UserMenuItemResponseDto>
        {
            new()
            {
                Title = "خود ارزیابی وظایف",
                AriaCurrent = false,
                AspArea ="PerformanceManagement",
                AspPage = "/SelfAssessments/Index",
                Class = currentUrl.Contains("SelfAssessments") ? "list-group-item list-group-item-action active": "list-group-item list-group-item-action",
                DisplayOrder = 1
            },
            new()
            {
                Title = "خود ارزیابی کلی",
                AriaCurrent = false,
                AspArea ="PerformanceManagement",
                AspPage = "/SelfAssessmentOveralls/Index",
                Class = currentUrl.Contains("SelfAssessmentOveralls") ? "list-group-item list-group-item-action active": "list-group-item list-group-item-action",
                DisplayOrder = 2
            },new()
            {
                Title = "ارزیابی مدیر مستقیم توسط شما",
                AriaCurrent = false,
                AspArea ="PerformanceManagement",
                AspPage = "/ManagerEvaluations/Index",
                Class = currentUrl.Contains("ManagerEvaluations") ? "list-group-item list-group-item-action active": "list-group-item list-group-item-action",
                DisplayOrder = 3
            },new()
            {
                Title = "ارزیابی همکار",
                AriaCurrent = false,
                AspArea ="PerformanceManagement",
                AspPage = "/PeerAssessments/Index",
                Class = currentUrl.Contains("PeerAssessments") ? "list-group-item list-group-item-action active": "list-group-item list-group-item-action",
                DisplayOrder = 4
            },new()
            {
                Title = "ارزیابی مدیر مستقیم",
                AriaCurrent = false,
                AspArea ="PerformanceManagement",
                AspPage = "/ManagerAssessments/Index",
                Class = currentUrl.Contains("ManagerAssessments") ? "list-group-item list-group-item-action active": "list-group-item list-group-item-action",
                DisplayOrder = 5
            },new()
            {
                Title = "نتایج ارزیابی",
                AriaCurrent = false,
                AspArea ="PerformanceManagement",
                AspPage = "/SelfReports/Index",
                Class = currentUrl.Contains("SelfReports") ? "list-group-item list-group-item-action active": "list-group-item list-group-item-action",
                DisplayOrder = 6
            },new()
            {
                Title = "نتایج نهایی (ارزیابی توسط مدیر مستقیم)",
                AriaCurrent = false,
                AspArea ="PerformanceManagement",
                AspPage = "/ManagerReports/Index",
                Class = currentUrl.Contains("ManagerReports") ? "list-group-item list-group-item-action active": "list-group-item list-group-item-action",
                DisplayOrder = 7
            },new()
            {
                Title = "انتظارات دوره بعد",
                AriaCurrent = false,
                AspArea ="PerformanceManagement",
                AspPage = "/Exceptions/Index",
                Class = currentUrl.Contains("Exceptions") ? "list-group-item list-group-item-action active": "list-group-item list-group-item-action",
                DisplayOrder = 8
            }
        };
        var userId = HttpContext.User.UserId();
        var now = DateTimeOffset.UtcNow.Date;
        var result = await _context.PerformanceManagementPeriodUserMappings
            .Include(a => a.PerformanceManagementPeriod)
            .Where(a => a.UserId == userId && a.PerformanceManagementPeriod.Active)
            .Select(a => new
            {
                Id = a.PerformanceManagementPeriodId,
                a.PerformanceManagementPeriod.SelfScoreEndDate,
                a.PerformanceManagementPeriod.SelfScoreStartDate,
                a.PerformanceManagementPeriod.OtherScoreStartDate,
                a.PerformanceManagementPeriod.OtherScoreEndDate,
                a.PerformanceManagementPeriod.ManagerScoreStartDate,
                a.PerformanceManagementPeriod.ManagerScoreEndDate,
                a.PerformanceManagementPeriod.ReportStartDate,
                a.PerformanceManagementPeriod.ReportEndDate,
                a.PerformanceManagementPeriod.ExceptionScoreStartDate,
                a.PerformanceManagementPeriod.ExceptionScoreEndDate
            }).FirstOrDefaultAsync();

        if (result == null)
            return await Task.Run(() => View(new List<UserMenuItemResponseDto>()));

        if(result.ManagerScoreStartDate.Date < now)
        {
            var isManager = await _context.UserManagerMappings.AnyAsync(a => a.ManagerId == userId);

            if (!isManager)
            {
                var shouldDisabled = res.FirstOrDefault(a => a.DisplayOrder == 5);
                if (shouldDisabled != null)
                {
                    shouldDisabled.Class = "list-group-item list-group-item-action disabled";
                }
            }
        }

        foreach (var item in res)
        {
            if (result.SelfScoreStartDate.Date > now)
            {
                if (item.DisplayOrder is 1 or 2 or 3)
                {
                    item.Class = "list-group-item list-group-item-action disabled";
                }
            }
            if (result.OtherScoreStartDate.Date > now)
            {
                if (item.DisplayOrder is 4)
                {
                    item.Class = "list-group-item list-group-item-action disabled";
                }
            }
            if (result.ManagerScoreStartDate.Date > now)
            {
                if (item.DisplayOrder is 5)
                {
                    item.Class = "list-group-item list-group-item-action disabled";
                }
            }
            if (result.ReportStartDate.Date > now)
            {
                if (item.DisplayOrder is 6 or 7)
                {
                    item.Class = "list-group-item list-group-item-action disabled";
                }
            }
            if (result.ExceptionScoreStartDate.Date > now)
            {
                if (item.DisplayOrder is 8)
                {
                    item.Class = "list-group-item list-group-item-action disabled";
                }
            }
        }
        return await Task.Run(() => View(res));
    }
}