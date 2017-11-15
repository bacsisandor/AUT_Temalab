using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockly
{
    public abstract class VariableType : IEquatable<VariableType>
    {
        public static readonly VariableType INT = new ElementalType("int");
        public static readonly VariableType BOOLEAN = new ElementalType("bool");
        public static readonly VariableType STRING = new ElementalType("string");
        public static readonly VariableType NULL = new ElementalType("null");
        public static readonly VariableType VOID = new ElementalType("void");

        public abstract string Name { get; }

        public abstract bool IsArray { get; }

        public abstract VariableType ElementType { get; }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(VariableType other)
        {
            return Name == other.Name && IsArray == other.IsArray;
        }

        public override bool Equals(object obj)
        {
            if (obj is VariableType)
            {
                return Equals((VariableType)obj);
            }
            return false;
        }

        public static bool operator ==(VariableType type1, VariableType type2)
        {
            return type1.Equals(type2);
        }

        public static bool operator !=(VariableType type1, VariableType type2)
        {
            return !type1.Equals(type2);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ IsArray.GetHashCode();
        }

        public static VariableType FromString(string name, bool array = false)
        {
            if (name == "integer")
            {
                name = "int";
            }
            else if (name == "boolean")
            {
                name = "bool";
            }
            ElementalType type = new ElementalType(name);
            if (array)
            {
                return new ArrayType(type);
            }
            return type;
        }
    }

    public class ElementalType : VariableType
    {
        private string name;

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override bool IsArray
        {
            get
            {
                return false;
            }
        }

        public override VariableType ElementType
        {
            get
            {
                return this;
            }
        }

        public ElementalType(string name)
        {
            this.name = name;
        }
    }

    public class ArrayType : VariableType
    {
        private VariableType elementType;
        private int size;

        public override string Name
        {
            get
            {
                return elementType.Name + "[]";
            }
        }

        public override bool IsArray
        {
            get
            {
                return true;
            }
        }

        public override VariableType ElementType
        {
            get
            {
                return elementType;
            }
        }

        public ArrayType(VariableType elementType)
        {
            this.elementType = elementType;
        }
    }
}
