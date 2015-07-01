using System;
using System.Linq;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Models.BD.Telesale;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class UpdateGroup : ScriptAction
    {
        public UpdateGroup() : base("Update PM information", ScriptActionType.UpdateGroup) { }

        public SiteGroup Group { get; set; }

        public override void Update(Contact contact, LeadPersonal person, Telesale telesale)
        {
            if (Group.Id <= 0)
            {
                contact.Site.Groups.Add(Group);
            }
            else
            {
                var group = contact.Site.Groups.SingleOrDefault(x => x.Id == Group.Id);

                if (group == null)
                    throw new Exception("The group try to update does not belong to this site");

                group.GroupName = Group.GroupName;
                group.Description = Group.Description;
                group.Type = Group.Type;
                group.Code = Group.Code;
                group.Firstname = Group.Firstname;
                group.Lastname = Group.Lastname;
            }
        }
    }
}
