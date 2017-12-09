using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockly
{
    public abstract class VariableType : IEquatable<VariableType>
    {
        public static readonly VariableType INT = new PrimitiveType("int");
        public static readonly VariableType BOOLEAN = new PrimitiveType("bool");
        public static readonly VariableType STRING = new PrimitiveType("string");
        public static readonly VariableType VOID = new PrimitiveType("void");

        public abstract string Name { get; }

        public abstract bool IsArray { get; }

        public abstract VariableType ElementType { get; }

        public abstract int Size { get; }

        public virtual bool TryGetValue(out int value)
        {
            value = 0;
            return false;
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(VariableType other)
        {
            if (Name == other.Name)
            {
                return Size == -1 || other.Size == -1 || Size == other.Size;
            }
            return false;
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
            return new PrimitiveType(name);
        }

        public static VariableType ArrayFromString(string name, int size)
        {
            return new ArrayType(FromString(name), size);
        }
    }

    public class PrimitiveType : VariableType
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

        public override int Size
        {
            get
            {
                return 1;
            }
        }

        public PrimitiveType(string name)
        {
            this.name = name;
        }
    }

    public class Constant : PrimitiveType
    {
        private int value;

        public override bool TryGetValue(out int value)
        {
            value = this.value;
            return true;
        }

        public Constant(int value) : base(INT.Name)
        {
            this.value = value;
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

        public override int Size
        {
            get
            {
                return size;
            }
        }

        public ArrayType(VariableType elementType, int size)
        {
            this.elementType = elementType;
            this.size = size;
        }
    }
}
