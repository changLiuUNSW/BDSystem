using AutoMapper;
namespace DateAccess.Services.Mappers
{
    public class DtoToDatabaseMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DtoToDatabaseMappingProfile"; }
        }

        protected override void Configure()
        {
        

//            //TODO: need to add more
//            Mapper.CreateMap<ContactDTO, Contact>()
//                .IgnoreAllUnmapped()
//                .ForMember(d => d.Data_Updat, s=> s.MapFrom(x => x.DaToCheck))
//                .ForMember(d=>d.ContactPersonId,s=>s.MapFrom(x=>x.ContactPersonId))
//                .ForMember(d => d.Info_Updat, s => s.MapFrom(x => x.DaToCheckInfo));

        }

    }
    // Ignore all mapping and disable convention mapping
//    public static class MappingExpressionExtensions
//    {
//        public static IMappingExpression<TSource, TDest> IgnoreAllUnmapped<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
//        {
//            expression.ForAllMembers(opt => opt.Ignore());
//            return expression;
//        }
//    }
}
