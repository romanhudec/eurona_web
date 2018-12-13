using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace SHP.Entities {
    [Serializable]
    public class Attribute : Entity {
        public class ReadById {
            public int AttributeId { get; set; }
        }

        /// <summary>
        /// Načíta atribúty danej kategórie
        /// </summary>
        public class ReadByCategoryId {
            public int CategoryId { get; set; }
        }

        /// <summary>
        /// Načíta všetky atribúty danej kategórie, 
        /// teda aj zdedené atribúty z rodičovských kategórií
        /// </summary>
        public class ReadAllInherits4Category {
            public int CategoryId { get; set; }
        }

        public int InstanceId { get; set; }
        public string Name { get; set; }
        public string Locale { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public AttributeType.Type Type { get; set; }
        public string TypeLimit { get; set; }
        public string TypeName {
            get { return AttributeType.GetLocalizedType(this.Type); }
        }
    }

    public class AttributeType {
        public enum Type : int {
            None = 0,
            String = 1,
            Double = 2,
            Int = 3,
            Boolean = 4,
            Choice = 5,
            MultiChoice = 6,
            Picture = 7,
            Attachment = 8
        }

        public AttributeType(string name, Type value) {
            this.Name = name;
            this.Id = (int)value;
        }
        public string Name { get; set; }
        public int Id { get; set; }

        #region Static methods
        public static string LimitFromToSeparator { get { return ";"; } }
        public static string LimitChoiceSeparator { get { return "|"; } }

        public static string GetLocalizedType(Type type) {
            switch (type) {
                case Type.None:
                    return string.Empty;
                case Type.String:
                    return Resources.AttributeType.String;
                case Type.Double:
                    return Resources.AttributeType.Double;
                case Type.Int:
                    return Resources.AttributeType.Int;
                case Type.Boolean:
                    return Resources.AttributeType.Boolean;
                case Type.Choice:
                    return Resources.AttributeType.Choice;
                case Type.MultiChoice:
                    return Resources.AttributeType.MultiChoice;
                case Type.Picture:
                    return Resources.AttributeType.Picture;
                case Type.Attachment:
                    return Resources.AttributeType.Attachment;
                default:
                    return string.Empty;
            }
        }
        public static List<AttributeType> LoadLocalizedTypes() {
            List<AttributeType> list = new List<AttributeType>();
            list.Add(new AttributeType(GetLocalizedType(Type.None), Type.None));
            list.Add(new AttributeType(GetLocalizedType(Type.String), Type.String));
            list.Add(new AttributeType(GetLocalizedType(Type.Int), Type.Int));
            list.Add(new AttributeType(GetLocalizedType(Type.Boolean), Type.Boolean));
            list.Add(new AttributeType(GetLocalizedType(Type.Double), Type.Double));
            list.Add(new AttributeType(GetLocalizedType(Type.Choice), Type.Choice));
            list.Add(new AttributeType(GetLocalizedType(Type.MultiChoice), Type.MultiChoice));
            list.Add(new AttributeType(GetLocalizedType(Type.Picture), Type.Picture));
            list.Add(new AttributeType(GetLocalizedType(Type.Attachment), Type.Attachment));

            return list;
        }
        #endregion

    }
}
