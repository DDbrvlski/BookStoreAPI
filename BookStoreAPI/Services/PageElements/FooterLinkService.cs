using BookStoreAPI.Helpers;
using BookStoreData.Data;
using BookStoreData.Models.PageContent;
using BookStoreDto.Dtos.PageContent.FooterLinks;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.PageElements
{
    public interface IFooterLinkService
    {
        Task DeactivateFooterLinkAsync(int footerLinkId);
        Task EditFooterLinkAsync(int footerLinkId, FooterLinks footerLinkModel);
        Task<IEnumerable<FooterLinkDto>> GetAllFooterLinksAsync();
        Task<FooterLinkDto> GetFooterLinkByIdAsync(int id);
        Task<FooterColumnDetailsDto> GetFooterLinksInColumnByColumnIdAsync(int columnId);
        Task<IEnumerable<FooterColumnDetailsDto>> GetFooterLinksInColumnsAsync();
        Task CreateFooterLinkAsync(FooterLinks footerLinkModel);
    }

    public class FooterLinkService(BookStoreContext context) : IFooterLinkService
    {
        public async Task<FooterLinkDto> GetFooterLinkByIdAsync(int id)
        {
            return await context.FooterLinks
               .Where(x => x.Id == id && x.IsActive)
               .Select(element => new FooterLinkDto()
               {
                   Id = id,
                   ColumnId = element.FooterColumn.Id,
                   ColumnName = element.FooterColumn.Name,
                   ColumnPosition = element.FooterColumn.Position,
                   HTMLObject = element.FooterColumn.HTMLObject,
                   Name = element.Name,
                   Path = element.Path,
                   Position = element.Position,
                   URL = element.URL,
               })
               .FirstAsync();
        }
        public async Task<IEnumerable<FooterLinkDto>> GetAllFooterLinksAsync()
        {
            return await context.FooterLinks
               .Where(x => x.IsActive)
               .OrderBy(x => x.Position)
               .Select(element => new FooterLinkDto()
               {
                   Id = element.Id,
                   ColumnId = element.FooterColumn.Id,
                   ColumnName = element.FooterColumn.Name,
                   ColumnPosition = element.FooterColumn.Position,
                   HTMLObject = element.FooterColumn.HTMLObject,
                   Name = element.Name,
                   Path = element.Path,
                   Position = element.Position,
                   URL = element.URL,
               })
               .ToListAsync();
        }
        public async Task<FooterColumnDetailsDto> GetFooterLinksInColumnByColumnIdAsync(int columnId)
        {
            var footerLinks = await context.FooterLinks
                .Where(x => x.IsActive && x.FooterColumnID == columnId)
                .OrderBy(x => x.Position)
                .Select(x => new FooterLinkDto()
                {
                    Id = x.Id,
                    ColumnId = x.FooterColumn.Id,
                    ColumnName = x.FooterColumn.Name,
                    ColumnPosition = x.FooterColumn.Position,
                    HTMLObject = x.FooterColumn.HTMLObject,
                    Name = x.Name,
                    Path = x.Path,
                    Position = x.Position,
                    URL = x.URL,
                })
            .ToListAsync();

            return footerLinks
            .GroupBy(x => x.ColumnPosition)
            .OrderBy(group => group.Key)
            .Select(group => new FooterColumnDetailsDto
            {
                ColumnId = group.First().ColumnId,
                ColumnName = group.First().ColumnName,
                ColumnPosition = group.Key,
                HTMLObject = group.First().HTMLObject,
                FooterLinksList = group
                    .OrderBy(item => item.Position)
                    .Select(item => new FooterLinkListDetailsDto
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Path = item.Path,
                        URL = item.URL,
                        Position = item.Position
                    })
                    .ToList()
            })
            .First();
        }
        public async Task<IEnumerable<FooterColumnDetailsDto>> GetFooterLinksInColumnsAsync()
        {
            var footerLinks = await context.FooterLinks
                .Include(x => x.FooterColumn)
                .Where(x => x.IsActive == true)
                .Select(x => new FooterLinkDto()
                {
                    Id = x.Id,
                    ColumnId = x.FooterColumn.Id,
                    ColumnName = x.FooterColumn.Name,
                    ColumnPosition = x.FooterColumn.Position,
                    HTMLObject = x.FooterColumn.HTMLObject,
                    Name = x.Name,
                    Path = x.Path,
                    Position = x.Position,
                    URL = x.URL,
                })
            .ToListAsync();

            return footerLinks
            .GroupBy(x => x.ColumnPosition)
            .OrderBy(group => group.Key)
            .Select(group => new FooterColumnDetailsDto
            {
                ColumnId = group.First().ColumnId,
                ColumnName = group.First().ColumnName,
                ColumnPosition = group.Key,
                HTMLObject = group.First().HTMLObject,
                FooterLinksList = group
                    .OrderBy(item => item.Position)
                    .Select(item => new FooterLinkListDetailsDto
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Path = item.Path,
                        URL = item.URL,
                        Position = item.Position
                    })
                    .ToList()
            })
            .ToList();
        }
        public async Task CreateFooterLinkAsync(FooterLinks footerLinkModel)
        {
            await context.FooterLinks.AddAsync(footerLinkModel);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task EditFooterLinkAsync(int footerLinkId, FooterLinks footerLinkModel)
        {
            var footerLink = await context.FooterLinks.FirstAsync(x => x.IsActive && x.Id == footerLinkId);
            footerLink.CopyProperties(footerLinkModel);
            footerLink.ModifiedDate = DateTime.UtcNow;

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task DeactivateFooterLinkAsync(int footerLinkId)
        {
            var footerLink = await context.FooterLinks.FirstAsync(x => x.IsActive && x.Id == footerLinkId);
            footerLink.IsActive = false;
            footerLink.ModifiedDate = DateTime.UtcNow;

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
    }
}
