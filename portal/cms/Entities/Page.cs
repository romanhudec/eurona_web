using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities {
    [Serializable]
    public class Page : Entity, IUrlAliasEntity {
        public Page() {
            this.Alias = string.Empty;
        }

        public class ReadById {
            public int PageId { get; set; }
        }

        public class ReadByParent {
            public int? ParentId { get; set; }
        }

        public class ReadForCurrentAccount {
            public string Name { get; set; }
            public string Locale { get; set; }
        }

        public class ReadByName {
            public string Name { get; set; }
            public string Locale { get; set; }
        }

        public class ReadContentPages { }

        public int InstanceId { get; set; }
        public int MasterPageId { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string Locale { get; set; }
        public string Content { get; set; }
        public string ContentKeywords { get; set; }

        //FK Role
        public int? RoleId { get; set; }
        private Role role = null;
        public Role Role {
            get {
                if (!this.RoleId.HasValue) return null;
                if (role != null) return role;
                role = Storage<Role>.ReadFirst(new Role.ReadById { RoleId = this.RoleId.Value });
                return role;
            }
        }

        public string RoleName {
            get { return Role != null ? role.Name : String.Empty; }
        }

        //FK MasterPage
        private MasterPage masterPage;
        public MasterPage MasterPage {
            get {
                if (masterPage != null) return masterPage;
                masterPage = Storage<MasterPage>.ReadFirst(new MasterPage.ReadById { MasterPageId = this.MasterPageId });
                return masterPage;
            }
        }

        //FK Childs
        private List<Page> childPages;
        public List<Page> ChildPages {
            get {
                if (childPages != null) return childPages;
                childPages = Storage<Page>.Read(new Page.ReadByParent { ParentId = this.Id });
                return childPages;
            }
        }

        #region IUrlAliasEntity Members
        public int? UrlAliasId { get; set; }
        public string Alias { get; set; }
        #endregion

    }
}
