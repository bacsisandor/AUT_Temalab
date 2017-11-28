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

        public virtual bool Equals(VariableType other)
        {
            return Name == other.Name;
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
            return Name.GetHashCode();
        }

        public static VariableType FromString(string name)
        {
            if (name == "integer")
            {
                name = "int";
            }
            else if (name == "boolean")
            {
                name = "bool";
            }
            return new ElementalType(name);
        }

        public static VariableType ArrayFromString(string name, int size)
        {
            return new ArrayType(FromString(name), size);
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

        public int Size { get; private set; }

        public ArrayType(VariableType elementType, int size)
        {
            this.elementType = elementType;
            Size = size;
        }

        public override bool Equals(VariableType other)
        {
            return base.Equals(other) && Size == ((ArrayType)other).Size;
        }
    }
}
