using AutoMapper;
using TodoApp.API.Entities.Commons.Operations;
using TodoApp.API.Mapping.Commons.Converters;

namespace TodoApp.API.Mapping
{
    public class PagedListMappingProfile : Profile
    {
        public PagedListMappingProfile()
        {
            CreateMap(typeof(PagedList<>), typeof(PagedList<>)).ConvertUsing(typeof(PagedListConverter<,>));
        }
    }
}
