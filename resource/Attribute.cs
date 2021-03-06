using System;

namespace Hello {
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ExcludeAttribute : Attribute {
        public Type BaseType { set; get; }
        public string[] Properties { set; get; }

        public ExcludeAttribute(Type baseType, params string[] props) {
            BaseType = baseType;
            Properties = prop;
        }
    }

    public class Student {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public int Age { set; get; }
    }

    [Exclude(typeof(Student), "FirstName", "Age")]
    public class Student2 {

    }
}