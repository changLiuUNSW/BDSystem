using AutoMapper;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DateAccess.Services.ViewModels;


namespace DateAccess.Services.Mappers
{
    public class DataBaseToDtoMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DataBaseToDtoMappingProfile"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<ContactPerson, ContactPersonHistory>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            Mapper.CreateMap<LeadPersonal, LeadPersonal>()
                .ForMember(dest => dest.LeadGroup, opt => opt.Ignore())
                .ForMember(dest => dest.Leads, opt => opt.Ignore());

        





            //Admin DTO Mapping
            Mapper.CreateMap<Site, SiteDTO>()
            .ForMember(dest => dest.GmZone, source => source.MapFrom(x => x.SalesBox.Zone))
            .ForMember(dest => dest.Region, source => source.MapFrom(x => x.SalesBox.Region));
            Mapper.CreateMap<SiteGroup, SiteGroupDTO>()
                .ForMember(dest=>dest.Count,source=>source.MapFrom(x=>x.Sites.Count));
            Mapper.CreateMap<SecurityContract, SecurityContractDTO>();
            Mapper.CreateMap<CleaningContract, CleaningContractDTO>();
            Mapper.CreateMap<Contact, ContactDTO>();
            Mapper.CreateMap<ContactPerson, ContactPersonDTO>()
                .ForMember(dest => dest.SiteName, source => source.MapFrom(x => x.Site.Name));
            Mapper.CreateMap<ContactPersonHistory, ContactPersonHistoryDTO>()
              .ForMember(dest => dest.SiteKey, source => source.MapFrom(x => x.ContactPerson.Site.Key))
              .ForMember(dest => dest.CurrentFirstName, source => source.MapFrom(x => x.ContactPerson.Firstname))
              .ForMember(dest => dest.CurrentLastName, source => source.MapFrom(x => x.ContactPerson.Lastname))
              .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id));

        }
    }
}