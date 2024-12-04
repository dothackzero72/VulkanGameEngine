//Smaller verison of Assembled GlmSharp, Version=0.9.8.0 for CLI and C++ use

#region Assembly GlmSharp, Version=0.9.8.0, Culture=neutral, PublicKeyToken=null
// C:\Users\dotha\.nuget\packages\glmsharp\0.9.8\lib\Net45\GlmSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace GlmSharp;

//
// Summary:
//     A vector of type int with 4 components.
[Serializable]
[DataContract(Namespace = "vec")]
public struct ivec4 : IReadOnlyList<int>, IReadOnlyCollection<int>, IEnumerable<int>, IEnumerable, IEquatable<ivec4>
{
    //
    // Summary:
    //     x-component
    [DataMember]
    public int x;

    //
    // Summary:
    //     y-component
    [DataMember]
    public int y;

    //
    // Summary:
    //     z-component
    [DataMember]
    public int z;

    //
    // Summary:
    //     w-component
    [DataMember]
    public int w;

    //
    // Summary:
    //     Gets/Sets a specific indexed component (a bit slower than direct access).
    public int this[int index]
    {
        get
        {
            return index switch
            {
                0 => x,
                1 => y,
                2 => z,
                3 => w,
                _ => throw new ArgumentOutOfRangeException("index"),
            };
        }
        set
        {
            switch (index)
            {
                case 0:
                    x = value;
                    break;
                case 1:
                    y = value;
                    break;
                case 2:
                    z = value;
                    break;
                case 3:
                    w = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("index");
            }
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec2 xy
    {
        get
        {
            return new ivec2(x, y);
        }
        set
        {
            x = value.x;
            y = value.y;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec2 xz
    {
        get
        {
            return new ivec2(x, z);
        }
        set
        {
            x = value.x;
            z = value.y;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec2 yz
    {
        get
        {
            return new ivec2(y, z);
        }
        set
        {
            y = value.x;
            z = value.y;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec3 xyz
    {
        get
        {
            return new ivec3(x, y, z);
        }
        set
        {
            x = value.x;
            y = value.y;
            z = value.z;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec2 xw
    {
        get
        {
            return new ivec2(x, w);
        }
        set
        {
            x = value.x;
            w = value.y;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec2 yw
    {
        get
        {
            return new ivec2(y, w);
        }
        set
        {
            y = value.x;
            w = value.y;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec3 xyw
    {
        get
        {
            return new ivec3(x, y, w);
        }
        set
        {
            x = value.x;
            y = value.y;
            w = value.z;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec2 zw
    {
        get
        {
            return new ivec2(z, w);
        }
        set
        {
            z = value.x;
            w = value.y;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec3 xzw
    {
        get
        {
            return new ivec3(x, z, w);
        }
        set
        {
            x = value.x;
            z = value.y;
            w = value.z;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec3 yzw
    {
        get
        {
            return new ivec3(y, z, w);
        }
        set
        {
            y = value.x;
            z = value.y;
            w = value.z;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec4 xyzw
    {
        get
        {
            return new ivec4(x, y, z, w);
        }
        set
        {
            x = value.x;
            y = value.y;
            z = value.z;
            w = value.w;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec2 rg
    {
        get
        {
            return new ivec2(x, y);
        }
        set
        {
            x = value.x;
            y = value.y;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec2 rb
    {
        get
        {
            return new ivec2(x, z);
        }
        set
        {
            x = value.x;
            z = value.y;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec2 gb
    {
        get
        {
            return new ivec2(y, z);
        }
        set
        {
            y = value.x;
            z = value.y;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec3 rgb
    {
        get
        {
            return new ivec3(x, y, z);
        }
        set
        {
            x = value.x;
            y = value.y;
            z = value.z;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec2 ra
    {
        get
        {
            return new ivec2(x, w);
        }
        set
        {
            x = value.x;
            w = value.y;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec2 ga
    {
        get
        {
            return new ivec2(y, w);
        }
        set
        {
            y = value.x;
            w = value.y;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec3 rga
    {
        get
        {
            return new ivec3(x, y, w);
        }
        set
        {
            x = value.x;
            y = value.y;
            w = value.z;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec2 ba
    {
        get
        {
            return new ivec2(z, w);
        }
        set
        {
            z = value.x;
            w = value.y;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec3 rba
    {
        get
        {
            return new ivec3(x, z, w);
        }
        set
        {
            x = value.x;
            z = value.y;
            w = value.z;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec3 gba
    {
        get
        {
            return new ivec3(y, z, w);
        }
        set
        {
            y = value.x;
            z = value.y;
            w = value.z;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified subset of components. For more advanced (read-only)
    //     swizzling, use the .swizzle property.
    public ivec4 rgba
    {
        get
        {
            return new ivec4(x, y, z, w);
        }
        set
        {
            x = value.x;
            y = value.y;
            z = value.z;
            w = value.w;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified RGBA component. For more advanced (read-only) swizzling,
    //     use the .swizzle property.
    public int r
    {
        get
        {
            return x;
        }
        set
        {
            x = value;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified RGBA component. For more advanced (read-only) swizzling,
    //     use the .swizzle property.
    public int g
    {
        get
        {
            return y;
        }
        set
        {
            y = value;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified RGBA component. For more advanced (read-only) swizzling,
    //     use the .swizzle property.
    public int b
    {
        get
        {
            return z;
        }
        set
        {
            z = value;
        }
    }

    //
    // Summary:
    //     Gets or sets the specified RGBA component. For more advanced (read-only) swizzling,
    //     use the .swizzle property.
    public int a
    {
        get
        {
            return w;
        }
        set
        {
            w = value;
        }
    }

    //
    // Summary:
    //     Returns an array with all values
    public int[] Values => new int[4] { x, y, z, w };

    //
    // Summary:
    //     Returns the number of components (4).
    public int Count => 4;

    //
    // Summary:
    //     Returns the minimal component of this vector.
    public int MinElement => Math.Min(Math.Min(x, y), Math.Min(z, w));

    //
    // Summary:
    //     Returns the maximal component of this vector.
    public int MaxElement => Math.Max(Math.Max(x, y), Math.Max(z, w));

    //
    // Summary:
    //     Returns the euclidean length of this vector.
    public float Length => (float)Math.Sqrt(x * x + y * y + (z * z + w * w));

    //
    // Summary:
    //     Returns the squared euclidean length of this vector.
    public float LengthSqr => x * x + y * y + (z * z + w * w);

    //
    // Summary:
    //     Returns the sum of all components.
    public int Sum => x + y + (z + w);

    //
    // Summary:
    //     Returns the euclidean norm of this vector.
    public float Norm => (float)Math.Sqrt(x * x + y * y + (z * z + w * w));

    //
    // Summary:
    //     Returns the one-norm of this vector.
    public float Norm1 => Math.Abs(x) + Math.Abs(y) + (Math.Abs(z) + Math.Abs(w));

    //
    // Summary:
    //     Returns the two-norm (euclidean length) of this vector.
    public float Norm2 => (float)Math.Sqrt(x * x + y * y + (z * z + w * w));

    //
    // Summary:
    //     Returns the max-norm of this vector.
    public float NormMax => Math.Max(Math.Max(Math.Abs(x), Math.Abs(y)), Math.Max(Math.Abs(z), Math.Abs(w)));

    //
    // Summary:
    //     Predefined all-zero vector
    public static ivec4 Zero { get; } = new ivec4(0, 0, 0, 0);


    //
    // Summary:
    //     Predefined all-ones vector
    public static ivec4 Ones { get; } = new ivec4(1, 1, 1, 1);


    //
    // Summary:
    //     Predefined unit-X vector
    public static ivec4 UnitX { get; } = new ivec4(1, 0, 0, 0);


    //
    // Summary:
    //     Predefined unit-Y vector
    public static ivec4 UnitY { get; } = new ivec4(0, 1, 0, 0);


    //
    // Summary:
    //     Predefined unit-Z vector
    public static ivec4 UnitZ { get; } = new ivec4(0, 0, 1, 0);


    //
    // Summary:
    //     Predefined unit-W vector
    public static ivec4 UnitW { get; } = new ivec4(0, 0, 0, 1);


    //
    // Summary:
    //     Predefined all-MaxValue vector
    public static ivec4 MaxValue { get; } = new ivec4(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);


    //
    // Summary:
    //     Predefined all-MinValue vector
    public static ivec4 MinValue { get; } = new ivec4(int.MinValue, int.MinValue, int.MinValue, int.MinValue);


    //
    // Summary:
    //     Component-wise constructor
    public ivec4(int x, int y, int z, int w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    //
    // Summary:
    //     all-same-value constructor
    public ivec4(int v)
    {
        x = v;
        y = v;
        z = v;
        w = v;
    }

    //
    // Summary:
    //     from-vector constructor (empty fields are zero/false)
    public ivec4(ivec2 v)
    {
        x = v.x;
        y = v.y;
        z = 0;
        w = 0;
    }

    //
    // Summary:
    //     from-vector-and-value constructor (empty fields are zero/false)
    public ivec4(ivec2 v, int z)
    {
        x = v.x;
        y = v.y;
        this.z = z;
        w = 0;
    }

    //
    // Summary:
    //     from-vector-and-value constructor
    public ivec4(ivec2 v, int z, int w)
    {
        x = v.x;
        y = v.y;
        this.z = z;
        this.w = w;
    }

    //
    // Summary:
    //     from-vector constructor (empty fields are zero/false)
    public ivec4(ivec3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        w = 0;
    }

    //
    // Summary:
    //     from-vector-and-value constructor
    public ivec4(ivec3 v, int w)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        this.w = w;
    }

    //
    // Summary:
    //     from-vector constructor
    public ivec4(ivec4 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        w = v.w;
    }

    //
    // Summary:
    //     From-array/list constructor (superfluous values are ignored, missing values are
    //     zero-filled).
    public ivec4(IReadOnlyList<int> v)
    {
        int count = v.Count;
        x = ((count >= 0) ? v[0] : 0);
        y = ((count >= 1) ? v[1] : 0);
        z = ((count >= 2) ? v[2] : 0);
        w = ((count >= 3) ? v[3] : 0);
    }

    //
    // Summary:
    //     Generic from-array constructor (superfluous values are ignored, missing values
    //     are zero-filled).
    public ivec4(object[] v)
    {
        int num = v.Length;
        x = ((num >= 0) ? ((int)v[0]) : 0);
        y = ((num >= 1) ? ((int)v[1]) : 0);
        z = ((num >= 2) ? ((int)v[2]) : 0);
        w = ((num >= 3) ? ((int)v[3]) : 0);
    }

    //
    // Summary:
    //     From-array constructor (superfluous values are ignored, missing values are zero-filled).
    public ivec4(int[] v)
    {
        int num = v.Length;
        x = ((num >= 0) ? v[0] : 0);
        y = ((num >= 1) ? v[1] : 0);
        z = ((num >= 2) ? v[2] : 0);
        w = ((num >= 3) ? v[3] : 0);
    }

    //
    // Summary:
    //     From-array constructor with base index (superfluous values are ignored, missing
    //     values are zero-filled).
    public ivec4(int[] v, int startIndex)
    {
        int num = v.Length;
        x = ((num + startIndex >= 0) ? v[startIndex] : 0);
        y = ((num + startIndex >= 1) ? v[1 + startIndex] : 0);
        z = ((num + startIndex >= 2) ? v[2 + startIndex] : 0);
        w = ((num + startIndex >= 3) ? v[3 + startIndex] : 0);
    }

    //
    // Summary:
    //     From-IEnumerable constructor (superfluous values are ignored, missing values
    //     are zero-filled).
    public ivec4(IEnumerable<int> v)
        : this(v.ToArray())
    {
    }

    //
    // Summary:
    //     Implicitly converts this to a vec4.
    public static implicit operator vec4(ivec4 v)
    {
        return new vec4(v.x, v.y, v.z, v.w);
    }

    //
    // Summary:
    //     Explicitly converts this to a ivec2.
    public static explicit operator ivec2(ivec4 v)
    {
        return new ivec2(v.x, v.y);
    }

    //
    // Summary:
    //     Explicitly converts this to a ivec3.
    public static explicit operator ivec3(ivec4 v)
    {
        return new ivec3(v.x, v.y, v.z);
    }

    //
    // Summary:
    //     Explicitly converts this to a vec2.
    public static explicit operator vec2(ivec4 v)
    {
        return new vec2(v.x, v.y);
    }

    //
    // Summary:
    //     Explicitly converts this to a vec3.
    public static explicit operator vec3(ivec4 v)
    {
        return new vec3(v.x, v.y, v.z);
    }

    //
    // Summary:
    //     Explicitly converts this to a int array.
    public static explicit operator int[](ivec4 v)
    {
        return new int[4] { v.x, v.y, v.z, v.w };
    }

    //
    // Summary:
    //     Explicitly converts this to a generic object array.
    public static explicit operator object[](ivec4 v)
    {
        return new object[4] { v.x, v.y, v.z, v.w };
    }

    //
    // Summary:
    //     Returns true iff this equals rhs component-wise.
    public static bool operator ==(ivec4 lhs, ivec4 rhs)
    {
        return lhs.Equals(rhs);
    }

    //
    // Summary:
    //     Returns true iff this does not equal rhs (component-wise).
    public static bool operator !=(ivec4 lhs, ivec4 rhs)
    {
        return !lhs.Equals(rhs);
    }

    //
    // Summary:
    //     Returns an enumerator that iterates through all components.
    public IEnumerator<int> GetEnumerator()
    {
        yield return x;
        yield return y;
        yield return z;
        yield return w;
    }

    //
    // Summary:
    //     Returns an enumerator that iterates through all components.
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    //
    // Summary:
    //     Returns a string representation of this vector using ', ' as a seperator.
    public override string ToString()
    {
        return ToString(", ");
    }

    //
    // Summary:
    //     Returns a string representation of this vector using a provided seperator.
    public string ToString(string sep)
    {
        return x + sep + y + sep + z + sep + w;
    }

    //
    // Summary:
    //     Returns a string representation of this vector using a provided seperator and
    //     a format provider for each component.
    public string ToString(string sep, IFormatProvider provider)
    {
        return x.ToString(provider) + sep + y.ToString(provider) + sep + z.ToString(provider) + sep + w.ToString(provider);
    }

    //
    // Summary:
    //     Returns a string representation of this vector using a provided seperator and
    //     a format for each component.
    public string ToString(string sep, string format)
    {
        return x.ToString(format) + sep + y.ToString(format) + sep + z.ToString(format) + sep + w.ToString(format);
    }

    //
    // Summary:
    //     Returns a string representation of this vector using a provided seperator and
    //     a format and format provider for each component.
    public string ToString(string sep, string format, IFormatProvider provider)
    {
        return x.ToString(format, provider) + sep + y.ToString(format, provider) + sep + z.ToString(format, provider) + sep + w.ToString(format, provider);
    }

    //
    // Summary:
    //     Returns true iff this equals rhs component-wise.
    public bool Equals(ivec4 rhs)
    {
        if (x.Equals(rhs.x) && y.Equals(rhs.y))
        {
            if (z.Equals(rhs.z))
            {
                return w.Equals(rhs.w);
            }

            return false;
        }

        return false;
    }

    //
    // Summary:
    //     Returns true iff this equals rhs type- and component-wise.
    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is ivec4)
        {
            return Equals((ivec4)obj);
        }

        return false;
    }

    //
    // Summary:
    //     Returns a hash code for this instance.
    public override int GetHashCode()
    {
        return (((((x.GetHashCode() * 397) ^ y.GetHashCode()) * 397) ^ z.GetHashCode()) * 397) ^ w.GetHashCode();
    }

    //
    // Summary:
    //     Returns the p-norm of this vector.
    public double NormP(double p)
    {
        return Math.Pow(Math.Pow(Math.Abs(x), p) + Math.Pow(Math.Abs(y), p) + (Math.Pow(Math.Abs(z), p) + Math.Pow(Math.Abs(w), p)), 1.0 / p);
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using ', ' as a separator).
    public static ivec4 Parse(string s)
    {
        return Parse(s, ", ");
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator).
    public static ivec4 Parse(string s, string sep)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 4)
        {
            throw new FormatException("input has not exactly 4 parts");
        }

        return new ivec4(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()), int.Parse(array[2].Trim()), int.Parse(array[3].Trim()));
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator and a type provider).
    public static ivec4 Parse(string s, string sep, IFormatProvider provider)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 4)
        {
            throw new FormatException("input has not exactly 4 parts");
        }

        return new ivec4(int.Parse(array[0].Trim(), provider), int.Parse(array[1].Trim(), provider), int.Parse(array[2].Trim(), provider), int.Parse(array[3].Trim(), provider));
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator and a number style).
    public static ivec4 Parse(string s, string sep, NumberStyles style)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 4)
        {
            throw new FormatException("input has not exactly 4 parts");
        }

        return new ivec4(int.Parse(array[0].Trim(), style), int.Parse(array[1].Trim(), style), int.Parse(array[2].Trim(), style), int.Parse(array[3].Trim(), style));
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator and a number style and a format provider).
    public static ivec4 Parse(string s, string sep, NumberStyles style, IFormatProvider provider)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 4)
        {
            throw new FormatException("input has not exactly 4 parts");
        }

        return new ivec4(int.Parse(array[0].Trim(), style, provider), int.Parse(array[1].Trim(), style, provider), int.Parse(array[2].Trim(), style, provider), int.Parse(array[3].Trim(), style, provider));
    }

    //
    // Summary:
    //     Tries to convert the string representation of the vector into a vector representation
    //     (using ', ' as a separator), returns false if string was invalid.
    public static bool TryParse(string s, out ivec4 result)
    {
        return TryParse(s, ", ", out result);
    }

    //
    // Summary:
    //     Tries to convert the string representation of the vector into a vector representation
    //     (using a designated separator), returns false if string was invalid.
    public static bool TryParse(string s, string sep, out ivec4 result)
    {
        result = Zero;
        if (string.IsNullOrEmpty(s))
        {
            return false;
        }

        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 4)
        {
            return false;
        }

        int result2 = 0;
        int result3 = 0;
        int result4 = 0;
        int result5 = 0;
        bool flag = int.TryParse(array[0].Trim(), out result2) && int.TryParse(array[1].Trim(), out result3) && int.TryParse(array[2].Trim(), out result4) && int.TryParse(array[3].Trim(), out result5);
        result = (flag ? new ivec4(result2, result3, result4, result5) : Zero);
        return flag;
    }

    //
    // Summary:
    //     Tries to convert the string representation of the vector into a vector representation
    //     (using a designated separator and a number style and a format provider), returns
    //     false if string was invalid.
    public static bool TryParse(string s, string sep, NumberStyles style, IFormatProvider provider, out ivec4 result)
    {
        result = Zero;
        if (string.IsNullOrEmpty(s))
        {
            return false;
        }

        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 4)
        {
            return false;
        }

        int result2 = 0;
        int result3 = 0;
        int result4 = 0;
        int result5 = 0;
        bool flag = int.TryParse(array[0].Trim(), style, provider, out result2) && int.TryParse(array[1].Trim(), style, provider, out result3) && int.TryParse(array[2].Trim(), style, provider, out result4) && int.TryParse(array[3].Trim(), style, provider, out result5);
        result = (flag ? new ivec4(result2, result3, result4, result5) : Zero);
        return flag;
    }

    //
    // Summary:
    //     Returns the inner product (dot product, scalar product) of the two vectors.
    public static int Dot(ivec4 lhs, ivec4 rhs)
    {
        return lhs.x * rhs.x + lhs.y * rhs.y + (lhs.z * rhs.z + lhs.w * rhs.w);
    }

    //
    // Summary:
    //     Returns the euclidean distance between the two vectors.
    public static float Distance(ivec4 lhs, ivec4 rhs)
    {
        return (lhs - rhs).Length;
    }

    //
    // Summary:
    //     Returns the squared euclidean distance between the two vectors.
    public static float DistanceSqr(ivec4 lhs, ivec4 rhs)
    {
        return (lhs - rhs).LengthSqr;
    }

    //
    // Summary:
    //     Calculate the reflection direction for an incident vector (N should be normalized
    //     in order to achieve the desired result).
    public static ivec4 Reflect(ivec4 I, ivec4 N)
    {
        return I - 2 * Dot(N, I) * N;
    }

    //
    // Summary:
    //     Calculate the refraction direction for an incident vector (The input parameters
    //     I and N should be normalized in order to achieve the desired result).
    public static ivec4 Refract(ivec4 I, ivec4 N, int eta)
    {
        int num = Dot(N, I);
        int num2 = 1 - eta * eta * (1 - num * num);
        if (num2 < 0)
        {
            return Zero;
        }

        return eta * I - (eta * num + (int)Math.Sqrt(num2)) * N;
    }

    //
    // Summary:
    //     Returns a vector pointing in the same direction as another (faceforward orients
    //     a vector to point away from a surface as defined by its normal. If dot(Nref,
    //     I) is negative faceforward returns N, otherwise it returns -N).
    public static ivec4 FaceForward(ivec4 N, ivec4 I, ivec4 Nref)
    {
        if (Dot(Nref, I) >= 0)
        {
            return -N;
        }

        return N;
    }

    //
    // Summary:
    //     Returns a ivec4 with independent and identically distributed uniform integer
    //     values between 0 (inclusive) and int.MaxValue (exclusive).
    public static ivec4 Random(Random random)
    {
        return new ivec4(random.Next(), random.Next(), random.Next(), random.Next());
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Abs (Math.Abs(v)).
    public static ivec4 Abs(ivec4 v)
    {
        return new ivec4(Math.Abs(v.x), Math.Abs(v.y), Math.Abs(v.z), Math.Abs(v.w));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Abs (Math.Abs(v)).
    public static ivec4 Abs(int v)
    {
        return new ivec4(Math.Abs(v));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of HermiteInterpolationOrder3
    //     ((3 - 2 * v) * v * v).
    public static ivec4 HermiteInterpolationOrder3(ivec4 v)
    {
        return new ivec4((3 - 2 * v.x) * v.x * v.x, (3 - 2 * v.y) * v.y * v.y, (3 - 2 * v.z) * v.z * v.z, (3 - 2 * v.w) * v.w * v.w);
    }

    //
    // Summary:
    //     Returns a ivec from the application of HermiteInterpolationOrder3 ((3 - 2 * v)
    //     * v * v).
    public static ivec4 HermiteInterpolationOrder3(int v)
    {
        return new ivec4((3 - 2 * v) * v * v);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of HermiteInterpolationOrder5
    //     (((6 * v - 15) * v + 10) * v * v * v).
    public static ivec4 HermiteInterpolationOrder5(ivec4 v)
    {
        return new ivec4(((6 * v.x - 15) * v.x + 10) * v.x * v.x * v.x, ((6 * v.y - 15) * v.y + 10) * v.y * v.y * v.y, ((6 * v.z - 15) * v.z + 10) * v.z * v.z * v.z, ((6 * v.w - 15) * v.w + 10) * v.w * v.w * v.w);
    }

    //
    // Summary:
    //     Returns a ivec from the application of HermiteInterpolationOrder5 (((6 * v -
    //     15) * v + 10) * v * v * v).
    public static ivec4 HermiteInterpolationOrder5(int v)
    {
        return new ivec4(((6 * v - 15) * v + 10) * v * v * v);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Sqr (v * v).
    public static ivec4 Sqr(ivec4 v)
    {
        return new ivec4(v.x * v.x, v.y * v.y, v.z * v.z, v.w * v.w);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Sqr (v * v).
    public static ivec4 Sqr(int v)
    {
        return new ivec4(v * v);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Pow2 (v * v).
    public static ivec4 Pow2(ivec4 v)
    {
        return new ivec4(v.x * v.x, v.y * v.y, v.z * v.z, v.w * v.w);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Pow2 (v * v).
    public static ivec4 Pow2(int v)
    {
        return new ivec4(v * v);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Pow3 (v * v * v).
    public static ivec4 Pow3(ivec4 v)
    {
        return new ivec4(v.x * v.x * v.x, v.y * v.y * v.y, v.z * v.z * v.z, v.w * v.w * v.w);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Pow3 (v * v * v).
    public static ivec4 Pow3(int v)
    {
        return new ivec4(v * v * v);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Step (v >= 0 ? 1 : 0).
    public static ivec4 Step(ivec4 v)
    {
        return new ivec4((v.x >= 0) ? 1 : 0, (v.y >= 0) ? 1 : 0, (v.z >= 0) ? 1 : 0, (v.w >= 0) ? 1 : 0);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Step (v >= 0 ? 1 : 0).
    public static ivec4 Step(int v)
    {
        return new ivec4((v >= 0) ? 1 : 0);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Sqrt ((int)Math.Sqrt((double)v)).
    public static ivec4 Sqrt(ivec4 v)
    {
        return new ivec4((int)Math.Sqrt(v.x), (int)Math.Sqrt(v.y), (int)Math.Sqrt(v.z), (int)Math.Sqrt(v.w));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Sqrt ((int)Math.Sqrt((double)v)).
    public static ivec4 Sqrt(int v)
    {
        return new ivec4((int)Math.Sqrt(v));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of InverseSqrt ((int)(1.0 / Math.Sqrt((double)v))).
    public static ivec4 InverseSqrt(ivec4 v)
    {
        return new ivec4((int)(1.0 / Math.Sqrt(v.x)), (int)(1.0 / Math.Sqrt(v.y)), (int)(1.0 / Math.Sqrt(v.z)), (int)(1.0 / Math.Sqrt(v.w)));
    }

    //
    // Summary:
    //     Returns a ivec from the application of InverseSqrt ((int)(1.0 / Math.Sqrt((double)v))).
    public static ivec4 InverseSqrt(int v)
    {
        return new ivec4((int)(1.0 / Math.Sqrt(v)));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Sign (Math.Sign(v)).
    public static ivec4 Sign(ivec4 v)
    {
        return new ivec4(Math.Sign(v.x), Math.Sign(v.y), Math.Sign(v.z), Math.Sign(v.w));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Sign (Math.Sign(v)).
    public static ivec4 Sign(int v)
    {
        return new ivec4(Math.Sign(v));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Max (Math.Max(lhs, rhs)).
    public static ivec4 Max(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z), Math.Max(lhs.w, rhs.w));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Max (Math.Max(lhs, rhs)).
    public static ivec4 Max(ivec4 lhs, int rhs)
    {
        return new ivec4(Math.Max(lhs.x, rhs), Math.Max(lhs.y, rhs), Math.Max(lhs.z, rhs), Math.Max(lhs.w, rhs));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Max (Math.Max(lhs, rhs)).
    public static ivec4 Max(int lhs, ivec4 rhs)
    {
        return new ivec4(Math.Max(lhs, rhs.x), Math.Max(lhs, rhs.y), Math.Max(lhs, rhs.z), Math.Max(lhs, rhs.w));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Max (Math.Max(lhs, rhs)).
    public static ivec4 Max(int lhs, int rhs)
    {
        return new ivec4(Math.Max(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Min (Math.Min(lhs, rhs)).
    public static ivec4 Min(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z), Math.Min(lhs.w, rhs.w));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Min (Math.Min(lhs, rhs)).
    public static ivec4 Min(ivec4 lhs, int rhs)
    {
        return new ivec4(Math.Min(lhs.x, rhs), Math.Min(lhs.y, rhs), Math.Min(lhs.z, rhs), Math.Min(lhs.w, rhs));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Min (Math.Min(lhs, rhs)).
    public static ivec4 Min(int lhs, ivec4 rhs)
    {
        return new ivec4(Math.Min(lhs, rhs.x), Math.Min(lhs, rhs.y), Math.Min(lhs, rhs.z), Math.Min(lhs, rhs.w));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Min (Math.Min(lhs, rhs)).
    public static ivec4 Min(int lhs, int rhs)
    {
        return new ivec4(Math.Min(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Pow ((int)Math.Pow((double)lhs,
    //     (double)rhs)).
    public static ivec4 Pow(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4((int)Math.Pow(lhs.x, rhs.x), (int)Math.Pow(lhs.y, rhs.y), (int)Math.Pow(lhs.z, rhs.z), (int)Math.Pow(lhs.w, rhs.w));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Pow ((int)Math.Pow((double)lhs,
    //     (double)rhs)).
    public static ivec4 Pow(ivec4 lhs, int rhs)
    {
        return new ivec4((int)Math.Pow(lhs.x, rhs), (int)Math.Pow(lhs.y, rhs), (int)Math.Pow(lhs.z, rhs), (int)Math.Pow(lhs.w, rhs));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Pow ((int)Math.Pow((double)lhs,
    //     (double)rhs)).
    public static ivec4 Pow(int lhs, ivec4 rhs)
    {
        return new ivec4((int)Math.Pow(lhs, rhs.x), (int)Math.Pow(lhs, rhs.y), (int)Math.Pow(lhs, rhs.z), (int)Math.Pow(lhs, rhs.w));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Pow ((int)Math.Pow((double)lhs, (double)rhs)).
    public static ivec4 Pow(int lhs, int rhs)
    {
        return new ivec4((int)Math.Pow(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Log ((int)Math.Log((double)lhs,
    //     (double)rhs)).
    public static ivec4 Log(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4((int)Math.Log(lhs.x, rhs.x), (int)Math.Log(lhs.y, rhs.y), (int)Math.Log(lhs.z, rhs.z), (int)Math.Log(lhs.w, rhs.w));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Log ((int)Math.Log((double)lhs,
    //     (double)rhs)).
    public static ivec4 Log(ivec4 lhs, int rhs)
    {
        return new ivec4((int)Math.Log(lhs.x, rhs), (int)Math.Log(lhs.y, rhs), (int)Math.Log(lhs.z, rhs), (int)Math.Log(lhs.w, rhs));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Log ((int)Math.Log((double)lhs,
    //     (double)rhs)).
    public static ivec4 Log(int lhs, ivec4 rhs)
    {
        return new ivec4((int)Math.Log(lhs, rhs.x), (int)Math.Log(lhs, rhs.y), (int)Math.Log(lhs, rhs.z), (int)Math.Log(lhs, rhs.w));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Log ((int)Math.Log((double)lhs, (double)rhs)).
    public static ivec4 Log(int lhs, int rhs)
    {
        return new ivec4((int)Math.Log(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static ivec4 Clamp(ivec4 v, ivec4 min, ivec4 max)
    {
        return new ivec4(Math.Min(Math.Max(v.x, min.x), max.x), Math.Min(Math.Max(v.y, min.y), max.y), Math.Min(Math.Max(v.z, min.z), max.z), Math.Min(Math.Max(v.w, min.w), max.w));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static ivec4 Clamp(ivec4 v, ivec4 min, int max)
    {
        return new ivec4(Math.Min(Math.Max(v.x, min.x), max), Math.Min(Math.Max(v.y, min.y), max), Math.Min(Math.Max(v.z, min.z), max), Math.Min(Math.Max(v.w, min.w), max));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static ivec4 Clamp(ivec4 v, int min, ivec4 max)
    {
        return new ivec4(Math.Min(Math.Max(v.x, min), max.x), Math.Min(Math.Max(v.y, min), max.y), Math.Min(Math.Max(v.z, min), max.z), Math.Min(Math.Max(v.w, min), max.w));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static ivec4 Clamp(ivec4 v, int min, int max)
    {
        return new ivec4(Math.Min(Math.Max(v.x, min), max), Math.Min(Math.Max(v.y, min), max), Math.Min(Math.Max(v.z, min), max), Math.Min(Math.Max(v.w, min), max));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static ivec4 Clamp(int v, ivec4 min, ivec4 max)
    {
        return new ivec4(Math.Min(Math.Max(v, min.x), max.x), Math.Min(Math.Max(v, min.y), max.y), Math.Min(Math.Max(v, min.z), max.z), Math.Min(Math.Max(v, min.w), max.w));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static ivec4 Clamp(int v, ivec4 min, int max)
    {
        return new ivec4(Math.Min(Math.Max(v, min.x), max), Math.Min(Math.Max(v, min.y), max), Math.Min(Math.Max(v, min.z), max), Math.Min(Math.Max(v, min.w), max));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static ivec4 Clamp(int v, int min, ivec4 max)
    {
        return new ivec4(Math.Min(Math.Max(v, min), max.x), Math.Min(Math.Max(v, min), max.y), Math.Min(Math.Max(v, min), max.z), Math.Min(Math.Max(v, min), max.w));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Clamp (Math.Min(Math.Max(v, min), max)).
    public static ivec4 Clamp(int v, int min, int max)
    {
        return new ivec4(Math.Min(Math.Max(v, min), max));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Mix (min * (1-a) + max * a).
    public static ivec4 Mix(ivec4 min, ivec4 max, ivec4 a)
    {
        return new ivec4(min.x * (1 - a.x) + max.x * a.x, min.y * (1 - a.y) + max.y * a.y, min.z * (1 - a.z) + max.z * a.z, min.w * (1 - a.w) + max.w * a.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Mix (min * (1-a) + max * a).
    public static ivec4 Mix(ivec4 min, ivec4 max, int a)
    {
        return new ivec4(min.x * (1 - a) + max.x * a, min.y * (1 - a) + max.y * a, min.z * (1 - a) + max.z * a, min.w * (1 - a) + max.w * a);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Mix (min * (1-a) + max * a).
    public static ivec4 Mix(ivec4 min, int max, ivec4 a)
    {
        return new ivec4(min.x * (1 - a.x) + max * a.x, min.y * (1 - a.y) + max * a.y, min.z * (1 - a.z) + max * a.z, min.w * (1 - a.w) + max * a.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Mix (min * (1-a) + max * a).
    public static ivec4 Mix(ivec4 min, int max, int a)
    {
        return new ivec4(min.x * (1 - a) + max * a, min.y * (1 - a) + max * a, min.z * (1 - a) + max * a, min.w * (1 - a) + max * a);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Mix (min * (1-a) + max * a).
    public static ivec4 Mix(int min, ivec4 max, ivec4 a)
    {
        return new ivec4(min * (1 - a.x) + max.x * a.x, min * (1 - a.y) + max.y * a.y, min * (1 - a.z) + max.z * a.z, min * (1 - a.w) + max.w * a.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Mix (min * (1-a) + max * a).
    public static ivec4 Mix(int min, ivec4 max, int a)
    {
        return new ivec4(min * (1 - a) + max.x * a, min * (1 - a) + max.y * a, min * (1 - a) + max.z * a, min * (1 - a) + max.w * a);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Mix (min * (1-a) + max * a).
    public static ivec4 Mix(int min, int max, ivec4 a)
    {
        return new ivec4(min * (1 - a.x) + max * a.x, min * (1 - a.y) + max * a.y, min * (1 - a.z) + max * a.z, min * (1 - a.w) + max * a.w);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Mix (min * (1-a) + max * a).
    public static ivec4 Mix(int min, int max, int a)
    {
        return new ivec4(min * (1 - a) + max * a);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Lerp (min * (1-a) + max *
    //     a).
    public static ivec4 Lerp(ivec4 min, ivec4 max, ivec4 a)
    {
        return new ivec4(min.x * (1 - a.x) + max.x * a.x, min.y * (1 - a.y) + max.y * a.y, min.z * (1 - a.z) + max.z * a.z, min.w * (1 - a.w) + max.w * a.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Lerp (min * (1-a) + max *
    //     a).
    public static ivec4 Lerp(ivec4 min, ivec4 max, int a)
    {
        return new ivec4(min.x * (1 - a) + max.x * a, min.y * (1 - a) + max.y * a, min.z * (1 - a) + max.z * a, min.w * (1 - a) + max.w * a);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Lerp (min * (1-a) + max *
    //     a).
    public static ivec4 Lerp(ivec4 min, int max, ivec4 a)
    {
        return new ivec4(min.x * (1 - a.x) + max * a.x, min.y * (1 - a.y) + max * a.y, min.z * (1 - a.z) + max * a.z, min.w * (1 - a.w) + max * a.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Lerp (min * (1-a) + max *
    //     a).
    public static ivec4 Lerp(ivec4 min, int max, int a)
    {
        return new ivec4(min.x * (1 - a) + max * a, min.y * (1 - a) + max * a, min.z * (1 - a) + max * a, min.w * (1 - a) + max * a);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Lerp (min * (1-a) + max *
    //     a).
    public static ivec4 Lerp(int min, ivec4 max, ivec4 a)
    {
        return new ivec4(min * (1 - a.x) + max.x * a.x, min * (1 - a.y) + max.y * a.y, min * (1 - a.z) + max.z * a.z, min * (1 - a.w) + max.w * a.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Lerp (min * (1-a) + max *
    //     a).
    public static ivec4 Lerp(int min, ivec4 max, int a)
    {
        return new ivec4(min * (1 - a) + max.x * a, min * (1 - a) + max.y * a, min * (1 - a) + max.z * a, min * (1 - a) + max.w * a);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Lerp (min * (1-a) + max *
    //     a).
    public static ivec4 Lerp(int min, int max, ivec4 a)
    {
        return new ivec4(min * (1 - a.x) + max * a.x, min * (1 - a.y) + max * a.y, min * (1 - a.z) + max * a.z, min * (1 - a.w) + max * a.w);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Lerp (min * (1-a) + max * a).
    public static ivec4 Lerp(int min, int max, int a)
    {
        return new ivec4(min * (1 - a) + max * a);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Fma (a * b + c).
    public static ivec4 Fma(ivec4 a, ivec4 b, ivec4 c)
    {
        return new ivec4(a.x * b.x + c.x, a.y * b.y + c.y, a.z * b.z + c.z, a.w * b.w + c.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Fma (a * b + c).
    public static ivec4 Fma(ivec4 a, ivec4 b, int c)
    {
        return new ivec4(a.x * b.x + c, a.y * b.y + c, a.z * b.z + c, a.w * b.w + c);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Fma (a * b + c).
    public static ivec4 Fma(ivec4 a, int b, ivec4 c)
    {
        return new ivec4(a.x * b + c.x, a.y * b + c.y, a.z * b + c.z, a.w * b + c.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Fma (a * b + c).
    public static ivec4 Fma(ivec4 a, int b, int c)
    {
        return new ivec4(a.x * b + c, a.y * b + c, a.z * b + c, a.w * b + c);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Fma (a * b + c).
    public static ivec4 Fma(int a, ivec4 b, ivec4 c)
    {
        return new ivec4(a * b.x + c.x, a * b.y + c.y, a * b.z + c.z, a * b.w + c.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Fma (a * b + c).
    public static ivec4 Fma(int a, ivec4 b, int c)
    {
        return new ivec4(a * b.x + c, a * b.y + c, a * b.z + c, a * b.w + c);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Fma (a * b + c).
    public static ivec4 Fma(int a, int b, ivec4 c)
    {
        return new ivec4(a * b + c.x, a * b + c.y, a * b + c.z, a * b + c.w);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Fma (a * b + c).
    public static ivec4 Fma(int a, int b, int c)
    {
        return new ivec4(a * b + c);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Add (lhs + rhs).
    public static ivec4 Add(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Add (lhs + rhs).
    public static ivec4 Add(ivec4 lhs, int rhs)
    {
        return new ivec4(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs, lhs.w + rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Add (lhs + rhs).
    public static ivec4 Add(int lhs, ivec4 rhs)
    {
        return new ivec4(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z, lhs + rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Add (lhs + rhs).
    public static ivec4 Add(int lhs, int rhs)
    {
        return new ivec4(lhs + rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Sub (lhs - rhs).
    public static ivec4 Sub(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Sub (lhs - rhs).
    public static ivec4 Sub(ivec4 lhs, int rhs)
    {
        return new ivec4(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs, lhs.w - rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Sub (lhs - rhs).
    public static ivec4 Sub(int lhs, ivec4 rhs)
    {
        return new ivec4(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z, lhs - rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Sub (lhs - rhs).
    public static ivec4 Sub(int lhs, int rhs)
    {
        return new ivec4(lhs - rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Mul (lhs * rhs).
    public static ivec4 Mul(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z, lhs.w * rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Mul (lhs * rhs).
    public static ivec4 Mul(ivec4 lhs, int rhs)
    {
        return new ivec4(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs, lhs.w * rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Mul (lhs * rhs).
    public static ivec4 Mul(int lhs, ivec4 rhs)
    {
        return new ivec4(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z, lhs * rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Mul (lhs * rhs).
    public static ivec4 Mul(int lhs, int rhs)
    {
        return new ivec4(lhs * rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Div (lhs / rhs).
    public static ivec4 Div(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z, lhs.w / rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Div (lhs / rhs).
    public static ivec4 Div(ivec4 lhs, int rhs)
    {
        return new ivec4(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs, lhs.w / rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Div (lhs / rhs).
    public static ivec4 Div(int lhs, ivec4 rhs)
    {
        return new ivec4(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z, lhs / rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Div (lhs / rhs).
    public static ivec4 Div(int lhs, int rhs)
    {
        return new ivec4(lhs / rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Xor (lhs ^ rhs).
    public static ivec4 Xor(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4(lhs.x ^ rhs.x, lhs.y ^ rhs.y, lhs.z ^ rhs.z, lhs.w ^ rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Xor (lhs ^ rhs).
    public static ivec4 Xor(ivec4 lhs, int rhs)
    {
        return new ivec4(lhs.x ^ rhs, lhs.y ^ rhs, lhs.z ^ rhs, lhs.w ^ rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Xor (lhs ^ rhs).
    public static ivec4 Xor(int lhs, ivec4 rhs)
    {
        return new ivec4(lhs ^ rhs.x, lhs ^ rhs.y, lhs ^ rhs.z, lhs ^ rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Xor (lhs ^ rhs).
    public static ivec4 Xor(int lhs, int rhs)
    {
        return new ivec4(lhs ^ rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of BitwiseOr (lhs | rhs).
    public static ivec4 BitwiseOr(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4(lhs.x | rhs.x, lhs.y | rhs.y, lhs.z | rhs.z, lhs.w | rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of BitwiseOr (lhs | rhs).
    public static ivec4 BitwiseOr(ivec4 lhs, int rhs)
    {
        return new ivec4(lhs.x | rhs, lhs.y | rhs, lhs.z | rhs, lhs.w | rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of BitwiseOr (lhs | rhs).
    public static ivec4 BitwiseOr(int lhs, ivec4 rhs)
    {
        return new ivec4(lhs | rhs.x, lhs | rhs.y, lhs | rhs.z, lhs | rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec from the application of BitwiseOr (lhs | rhs).
    public static ivec4 BitwiseOr(int lhs, int rhs)
    {
        return new ivec4(lhs | rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of BitwiseAnd (lhs & rhs).
    public static ivec4 BitwiseAnd(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4(lhs.x & rhs.x, lhs.y & rhs.y, lhs.z & rhs.z, lhs.w & rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of BitwiseAnd (lhs & rhs).
    public static ivec4 BitwiseAnd(ivec4 lhs, int rhs)
    {
        return new ivec4(lhs.x & rhs, lhs.y & rhs, lhs.z & rhs, lhs.w & rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of BitwiseAnd (lhs & rhs).
    public static ivec4 BitwiseAnd(int lhs, ivec4 rhs)
    {
        return new ivec4(lhs & rhs.x, lhs & rhs.y, lhs & rhs.z, lhs & rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec from the application of BitwiseAnd (lhs & rhs).
    public static ivec4 BitwiseAnd(int lhs, int rhs)
    {
        return new ivec4(lhs & rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of LeftShift (lhs << rhs).
    public static ivec4 LeftShift(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4(lhs.x << rhs.x, lhs.y << rhs.y, lhs.z << rhs.z, lhs.w << rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of LeftShift (lhs << rhs).
    public static ivec4 LeftShift(ivec4 lhs, int rhs)
    {
        return new ivec4(lhs.x << rhs, lhs.y << rhs, lhs.z << rhs, lhs.w << rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of LeftShift (lhs << rhs).
    public static ivec4 LeftShift(int lhs, ivec4 rhs)
    {
        return new ivec4(lhs << rhs.x, lhs << rhs.y, lhs << rhs.z, lhs << rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec from the application of LeftShift (lhs << rhs).
    public static ivec4 LeftShift(int lhs, int rhs)
    {
        return new ivec4(lhs << rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of RightShift (lhs >> rhs).
    public static ivec4 RightShift(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4(lhs.x >> rhs.x, lhs.y >> rhs.y, lhs.z >> rhs.z, lhs.w >> rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of RightShift (lhs >> rhs).
    public static ivec4 RightShift(ivec4 lhs, int rhs)
    {
        return new ivec4(lhs.x >> rhs, lhs.y >> rhs, lhs.z >> rhs, lhs.w >> rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of RightShift (lhs >> rhs).
    public static ivec4 RightShift(int lhs, ivec4 rhs)
    {
        return new ivec4(lhs >> rhs.x, lhs >> rhs.y, lhs >> rhs.z, lhs >> rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec from the application of RightShift (lhs >> rhs).
    public static ivec4 RightShift(int lhs, int rhs)
    {
        return new ivec4(lhs >> rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 with independent and identically distributed uniform integer
    //     values between 0 (inclusive) and maxValue (exclusive). (A maxValue of 0 is allowed
    //     and returns 0.)
    public static ivec4 Random(Random random, ivec4 maxValue)
    {
        return new ivec4(random.Next(maxValue.x), random.Next(maxValue.y), random.Next(maxValue.z), random.Next(maxValue.w));
    }

    //
    // Summary:
    //     Returns a ivec4 with independent and identically distributed uniform integer
    //     values between 0 (inclusive) and maxValue (exclusive). (A maxValue of 0 is allowed
    //     and returns 0.)
    public static ivec4 Random(Random random, int maxValue)
    {
        return new ivec4(random.Next(maxValue));
    }

    //
    // Summary:
    //     Returns a ivec4 with independent and identically distributed uniform integer
    //     values between minValue (inclusive) and maxValue (exclusive). (minValue == maxValue
    //     is allowed and returns minValue. Negative values are allowed.)
    public static ivec4 Random(Random random, ivec4 minValue, ivec4 maxValue)
    {
        return new ivec4(random.Next(minValue.x, maxValue.x), random.Next(minValue.y, maxValue.y), random.Next(minValue.z, maxValue.z), random.Next(minValue.w, maxValue.w));
    }

    //
    // Summary:
    //     Returns a ivec4 with independent and identically distributed uniform integer
    //     values between minValue (inclusive) and maxValue (exclusive). (minValue == maxValue
    //     is allowed and returns minValue. Negative values are allowed.)
    public static ivec4 Random(Random random, ivec4 minValue, int maxValue)
    {
        return new ivec4(random.Next(minValue.x, maxValue), random.Next(minValue.y, maxValue), random.Next(minValue.z, maxValue), random.Next(minValue.w, maxValue));
    }

    //
    // Summary:
    //     Returns a ivec4 with independent and identically distributed uniform integer
    //     values between minValue (inclusive) and maxValue (exclusive). (minValue == maxValue
    //     is allowed and returns minValue. Negative values are allowed.)
    public static ivec4 Random(Random random, int minValue, ivec4 maxValue)
    {
        return new ivec4(random.Next(minValue, maxValue.x), random.Next(minValue, maxValue.y), random.Next(minValue, maxValue.z), random.Next(minValue, maxValue.w));
    }

    //
    // Summary:
    //     Returns a ivec4 with independent and identically distributed uniform integer
    //     values between minValue (inclusive) and maxValue (exclusive). (minValue == maxValue
    //     is allowed and returns minValue. Negative values are allowed.)
    public static ivec4 Random(Random random, int minValue, int maxValue)
    {
        return new ivec4(random.Next(minValue, maxValue));
    }

    //
    // Summary:
    //     Returns a ivec4 with independent and identically distributed uniform integer
    //     values between minValue (inclusive) and maxValue (exclusive). (minValue == maxValue
    //     is allowed and returns minValue. Negative values are allowed.)
    public static ivec4 RandomUniform(Random random, ivec4 minValue, ivec4 maxValue)
    {
        return new ivec4(random.Next(minValue.x, maxValue.x), random.Next(minValue.y, maxValue.y), random.Next(minValue.z, maxValue.z), random.Next(minValue.w, maxValue.w));
    }

    //
    // Summary:
    //     Returns a ivec4 with independent and identically distributed uniform integer
    //     values between minValue (inclusive) and maxValue (exclusive). (minValue == maxValue
    //     is allowed and returns minValue. Negative values are allowed.)
    public static ivec4 RandomUniform(Random random, ivec4 minValue, int maxValue)
    {
        return new ivec4(random.Next(minValue.x, maxValue), random.Next(minValue.y, maxValue), random.Next(minValue.z, maxValue), random.Next(minValue.w, maxValue));
    }

    //
    // Summary:
    //     Returns a ivec4 with independent and identically distributed uniform integer
    //     values between minValue (inclusive) and maxValue (exclusive). (minValue == maxValue
    //     is allowed and returns minValue. Negative values are allowed.)
    public static ivec4 RandomUniform(Random random, int minValue, ivec4 maxValue)
    {
        return new ivec4(random.Next(minValue, maxValue.x), random.Next(minValue, maxValue.y), random.Next(minValue, maxValue.z), random.Next(minValue, maxValue.w));
    }

    //
    // Summary:
    //     Returns a ivec4 with independent and identically distributed uniform integer
    //     values between minValue (inclusive) and maxValue (exclusive). (minValue == maxValue
    //     is allowed and returns minValue. Negative values are allowed.)
    public static ivec4 RandomUniform(Random random, int minValue, int maxValue)
    {
        return new ivec4(random.Next(minValue, maxValue));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator+ (lhs + rhs).
    public static ivec4 operator +(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator+ (lhs + rhs).
    public static ivec4 operator +(ivec4 lhs, int rhs)
    {
        return new ivec4(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs, lhs.w + rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator+ (lhs + rhs).
    public static ivec4 operator +(int lhs, ivec4 rhs)
    {
        return new ivec4(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z, lhs + rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator- (lhs - rhs).
    public static ivec4 operator -(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator- (lhs - rhs).
    public static ivec4 operator -(ivec4 lhs, int rhs)
    {
        return new ivec4(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs, lhs.w - rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator- (lhs - rhs).
    public static ivec4 operator -(int lhs, ivec4 rhs)
    {
        return new ivec4(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z, lhs - rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator* (lhs * rhs).
    public static ivec4 operator *(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z, lhs.w * rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator* (lhs * rhs).
    public static ivec4 operator *(ivec4 lhs, int rhs)
    {
        return new ivec4(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs, lhs.w * rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator* (lhs * rhs).
    public static ivec4 operator *(int lhs, ivec4 rhs)
    {
        return new ivec4(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z, lhs * rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator/ (lhs / rhs).
    public static ivec4 operator /(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z, lhs.w / rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator/ (lhs / rhs).
    public static ivec4 operator /(ivec4 lhs, int rhs)
    {
        return new ivec4(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs, lhs.w / rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator/ (lhs / rhs).
    public static ivec4 operator /(int lhs, ivec4 rhs)
    {
        return new ivec4(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z, lhs / rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator+ (identity).
    public static ivec4 operator +(ivec4 v)
    {
        return v;
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator- (-v).
    public static ivec4 operator -(ivec4 v)
    {
        return new ivec4(-v.x, -v.y, -v.z, -v.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator~ (~v).
    public static ivec4 operator ~(ivec4 v)
    {
        return new ivec4(~v.x, ~v.y, ~v.z, ~v.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator^ (lhs ^ rhs).
    public static ivec4 operator ^(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4(lhs.x ^ rhs.x, lhs.y ^ rhs.y, lhs.z ^ rhs.z, lhs.w ^ rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator^ (lhs ^ rhs).
    public static ivec4 operator ^(ivec4 lhs, int rhs)
    {
        return new ivec4(lhs.x ^ rhs, lhs.y ^ rhs, lhs.z ^ rhs, lhs.w ^ rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator^ (lhs ^ rhs).
    public static ivec4 operator ^(int lhs, ivec4 rhs)
    {
        return new ivec4(lhs ^ rhs.x, lhs ^ rhs.y, lhs ^ rhs.z, lhs ^ rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator| (lhs | rhs).
    public static ivec4 operator |(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4(lhs.x | rhs.x, lhs.y | rhs.y, lhs.z | rhs.z, lhs.w | rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator| (lhs | rhs).
    public static ivec4 operator |(ivec4 lhs, int rhs)
    {
        return new ivec4(lhs.x | rhs, lhs.y | rhs, lhs.z | rhs, lhs.w | rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator| (lhs | rhs).
    public static ivec4 operator |(int lhs, ivec4 rhs)
    {
        return new ivec4(lhs | rhs.x, lhs | rhs.y, lhs | rhs.z, lhs | rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator& (lhs & rhs).
    public static ivec4 operator &(ivec4 lhs, ivec4 rhs)
    {
        return new ivec4(lhs.x & rhs.x, lhs.y & rhs.y, lhs.z & rhs.z, lhs.w & rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator& (lhs & rhs).
    public static ivec4 operator &(ivec4 lhs, int rhs)
    {
        return new ivec4(lhs.x & rhs, lhs.y & rhs, lhs.z & rhs, lhs.w & rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator& (lhs & rhs).
    public static ivec4 operator &(int lhs, ivec4 rhs)
    {
        return new ivec4(lhs & rhs.x, lhs & rhs.y, lhs & rhs.z, lhs & rhs.w);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator<< (lhs << rhs).
    public static ivec4 operator <<(ivec4 lhs, int rhs)
    {
        return new ivec4(lhs.x << rhs, lhs.y << rhs, lhs.z << rhs, lhs.w << rhs);
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of operator>> (lhs >> rhs).
    public static ivec4 operator >>(ivec4 lhs, int rhs)
    {
        return new ivec4(lhs.x >> rhs, lhs.y >> rhs, lhs.z >> rhs, lhs.w >> rhs);
    }
}
#if false // Decompilation log
'169' items in cache
------------------
Resolve: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\mscorlib.dll'
------------------
Resolve: 'System.Numerics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Numerics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Numerics.dll'
------------------
Resolve: 'System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Runtime.Serialization.dll'
------------------
Resolve: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Core.dll'
------------------
Resolve: 'Microsoft.Win32.Registry, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'Microsoft.Win32.Registry, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\Microsoft.Win32.Registry.dll'
------------------
Resolve: 'System.Runtime, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Runtime.dll'
------------------
Resolve: 'System.Security.Principal.Windows, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Principal.Windows, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Security.Principal.Windows.dll'
------------------
Resolve: 'System.Security.Permissions, Version=0.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Could not find by name: 'System.Security.Permissions, Version=0.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
------------------
Resolve: 'System.Collections, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Collections, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Collections.dll'
------------------
Resolve: 'System.Collections.NonGeneric, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Collections.NonGeneric, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Collections.NonGeneric.dll'
------------------
Resolve: 'System.Collections.Concurrent, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Collections.Concurrent, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Collections.Concurrent.dll'
------------------
Resolve: 'System.ObjectModel, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ObjectModel, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.ObjectModel.dll'
------------------
Resolve: 'System.Console, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Console, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Console.dll'
------------------
Resolve: 'System.Runtime.InteropServices, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.InteropServices, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Runtime.InteropServices.dll'
------------------
Resolve: 'System.Diagnostics.Contracts, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Diagnostics.Contracts, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Diagnostics.Contracts.dll'
------------------
Resolve: 'System.Diagnostics.StackTrace, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Diagnostics.StackTrace, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Diagnostics.StackTrace.dll'
------------------
Resolve: 'System.Diagnostics.Tracing, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Diagnostics.Tracing, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Diagnostics.Tracing.dll'
------------------
Resolve: 'System.IO.FileSystem.DriveInfo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.IO.FileSystem.DriveInfo, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.IO.FileSystem.DriveInfo.dll'
------------------
Resolve: 'System.IO.IsolatedStorage, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.IO.IsolatedStorage, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.IO.IsolatedStorage.dll'
------------------
Resolve: 'System.ComponentModel, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.ComponentModel.dll'
------------------
Resolve: 'System.Threading.Thread, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading.Thread, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Threading.Thread.dll'
------------------
Resolve: 'System.Reflection.Emit, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Reflection.Emit, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Reflection.Emit.dll'
------------------
Resolve: 'System.Reflection.Emit.ILGeneration, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Reflection.Emit.ILGeneration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Reflection.Emit.ILGeneration.dll'
------------------
Resolve: 'System.Reflection.Emit.Lightweight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Reflection.Emit.Lightweight, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Reflection.Emit.Lightweight.dll'
------------------
Resolve: 'System.Reflection.Primitives, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Reflection.Primitives, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Reflection.Primitives.dll'
------------------
Resolve: 'System.Resources.Writer, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Resources.Writer, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Resources.Writer.dll'
------------------
Resolve: 'System.Runtime.CompilerServices.VisualC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.CompilerServices.VisualC, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Runtime.CompilerServices.VisualC.dll'
------------------
Resolve: 'System.Runtime.Serialization.Formatters, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.Serialization.Formatters, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Runtime.Serialization.Formatters.dll'
------------------
Resolve: 'System.Security.AccessControl, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.AccessControl, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Security.AccessControl.dll'
------------------
Resolve: 'System.IO.FileSystem.AccessControl, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.IO.FileSystem.AccessControl, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.IO.FileSystem.AccessControl.dll'
------------------
Resolve: 'System.Threading.AccessControl, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Could not find by name: 'System.Threading.AccessControl, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
------------------
Resolve: 'System.Security.Claims, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Claims, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Security.Claims.dll'
------------------
Resolve: 'System.Security.Cryptography, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Cryptography, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Security.Cryptography.dll'
------------------
Resolve: 'System.Text.Encoding.Extensions, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Text.Encoding.Extensions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Text.Encoding.Extensions.dll'
------------------
Resolve: 'System.Threading, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Threading.dll'
------------------
Resolve: 'System.Threading.Overlapped, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading.Overlapped, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Threading.Overlapped.dll'
------------------
Resolve: 'System.Threading.ThreadPool, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading.ThreadPool, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Threading.ThreadPool.dll'
------------------
Resolve: 'System.Threading.Tasks.Parallel, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading.Tasks.Parallel, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Threading.Tasks.Parallel.dll'
------------------
Resolve: 'System.Runtime.Numerics, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.Numerics, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Runtime.Numerics.dll'
------------------
Resolve: 'System.Numerics.Vectors, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Numerics.Vectors, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Numerics.Vectors.dll'
------------------
Resolve: 'System.Runtime.Serialization.Primitives, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.Serialization.Primitives, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Runtime.Serialization.Primitives.dll'
------------------
Resolve: 'System.Runtime.Serialization.Xml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.Serialization.Xml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Runtime.Serialization.Xml.dll'
------------------
Resolve: 'System.Runtime.Serialization.Json, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.Serialization.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Runtime.Serialization.Json.dll'
------------------
Resolve: 'System.Runtime.Serialization.Schema, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Could not find by name: 'System.Runtime.Serialization.Schema, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
------------------
Resolve: 'System.IO.MemoryMappedFiles, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.IO.MemoryMappedFiles, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.IO.MemoryMappedFiles.dll'
------------------
Resolve: 'System.IO.Pipes, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.IO.Pipes, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.IO.Pipes.dll'
------------------
Resolve: 'System.Diagnostics.EventLog, Version=0.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Could not find by name: 'System.Diagnostics.EventLog, Version=0.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
------------------
Resolve: 'System.Diagnostics.PerformanceCounter, Version=0.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Could not find by name: 'System.Diagnostics.PerformanceCounter, Version=0.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
------------------
Resolve: 'System.Linq.Expressions, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Linq.Expressions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Linq.Expressions.dll'
------------------
Resolve: 'System.IO.Pipes.AccessControl, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.IO.Pipes.AccessControl, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.IO.Pipes.AccessControl.dll'
------------------
Resolve: 'System.Linq, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Linq, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Linq.dll'
------------------
Resolve: 'System.Linq.Queryable, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Linq.Queryable, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Linq.Queryable.dll'
------------------
Resolve: 'System.Linq.Parallel, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Linq.Parallel, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '0.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Linq.Parallel.dll'
------------------
Resolve: 'System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Runtime.dll'
------------------
Resolve: 'System.Runtime.Serialization.Primitives, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.Serialization.Primitives, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.11\ref\net8.0\System.Runtime.Serialization.Primitives.dll'
#endif
