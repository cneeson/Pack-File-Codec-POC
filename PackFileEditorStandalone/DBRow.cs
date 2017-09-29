using System;
using System.Collections.Generic;

namespace PackFileEditorStandalone
{
    public class DBRow : List<FieldInstance> {
        private TypeInfo info;
        
        public DBRow (TypeInfo i, List<FieldInstance> val) : base(val) {
            info = i;
        }
        public DBRow (TypeInfo i) : this(i, CreateRow(i)) { }
        
        public TypeInfo Info {
            get {
                return info;
            }
        }

        public FieldInstance this[string fieldName] {
            get {
                return this[IndexOfField(fieldName)];
            }
            set {
                this[IndexOfField(fieldName)] = value;
            }
        }
        private int IndexOfField(string fieldName) {
            for(int i = 0; i < info.Fields.Count; i++) {
                if (info.Fields[i].Name.Equals(fieldName)) {
                    return i;
                }
            }
            throw new IndexOutOfRangeException(string.Format("Field name {0} not valid for type {1}", fieldName, info.Name));
        }
        
        public static List<FieldInstance> CreateRow(TypeInfo info) {
            List<FieldInstance> result = new List<FieldInstance>(info.Fields.Count);
            info.Fields.ForEach(f => result.Add(f.CreateInstance()));
            return result;
        }
    }
}

