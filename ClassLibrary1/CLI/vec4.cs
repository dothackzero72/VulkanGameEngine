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
//     A vector of type float with 4 components.
[Serializable]
[DataContract(Namespace = "vec")]
public struct vec4 : IReadOnlyList<float>, IReadOnlyCollection<float>, IEnumerable<float>, IEnumerable, IEquatable<vec4>
{
    //
    // Summary:
    //     x-component
    [DataMember]
    public float x;

    //
    // Summary:
    //     y-component
    [DataMember]
    public float y;

    //
    // Summary:
    //     z-component
    [DataMember]
    public float z;

    //
    // Summary:
    //     w-component
    [DataMember]
    public float w;

    //
    // Summary:
    //     Gets/Sets a specific indexed component (a bit slower than direct access).
    public float this[int index]
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
    public vec2 xy
    {
        get
        {
            return new vec2(x, y);
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
    public vec2 xz
    {
        get
        {
            return new vec2(x, z);
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
    public vec2 yz
    {
        get
        {
            return new vec2(y, z);
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
    public vec3 xyz
    {
        get
        {
            return new vec3(x, y, z);
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
    public vec2 xw
    {
        get
        {
            return new vec2(x, w);
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
    public vec2 yw
    {
        get
        {
            return new vec2(y, w);
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
    public vec3 xyw
    {
        get
        {
            return new vec3(x, y, w);
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
    public vec2 zw
    {
        get
        {
            return new vec2(z, w);
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
    public vec3 xzw
    {
        get
        {
            return new vec3(x, z, w);
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
    public vec3 yzw
    {
        get
        {
            return new vec3(y, z, w);
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
    public vec4 xyzw
    {
        get
        {
            return new vec4(x, y, z, w);
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
    public vec2 rg
    {
        get
        {
            return new vec2(x, y);
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
    public vec2 rb
    {
        get
        {
            return new vec2(x, z);
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
    public vec2 gb
    {
        get
        {
            return new vec2(y, z);
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
    public vec3 rgb
    {
        get
        {
            return new vec3(x, y, z);
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
    public vec2 ra
    {
        get
        {
            return new vec2(x, w);
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
    public vec2 ga
    {
        get
        {
            return new vec2(y, w);
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
    public vec3 rga
    {
        get
        {
            return new vec3(x, y, w);
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
    public vec2 ba
    {
        get
        {
            return new vec2(z, w);
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
    public vec3 rba
    {
        get
        {
            return new vec3(x, z, w);
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
    public vec3 gba
    {
        get
        {
            return new vec3(y, z, w);
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
    public vec4 rgba
    {
        get
        {
            return new vec4(x, y, z, w);
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
    public float r
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
    public float g
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
    public float b
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
    public float a
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
    public float[] Values => new float[4] { x, y, z, w };

    //
    // Summary:
    //     Returns the number of components (4).
    public int Count => 4;

    //
    // Summary:
    //     Returns the minimal component of this vector.
    public float MinElement => Math.Min(Math.Min(x, y), Math.Min(z, w));

    //
    // Summary:
    //     Returns the maximal component of this vector.
    public float MaxElement => Math.Max(Math.Max(x, y), Math.Max(z, w));

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
    public float Sum => x + y + (z + w);

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
    //     Returns a copy of this vector with length one (undefined if this has zero length).
    public vec4 Normalized => this / Length;

    //
    // Summary:
    //     Returns a copy of this vector with length one (returns zero if length is zero).
    public vec4 NormalizedSafe
    {
        get
        {
            if (!(this == Zero))
            {
                return this / Length;
            }

            return Zero;
        }
    }

    //
    // Summary:
    //     Predefined all-zero vector
    public static vec4 Zero { get; } = new vec4(0f, 0f, 0f, 0f);


    //
    // Summary:
    //     Predefined all-ones vector
    public static vec4 Ones { get; } = new vec4(1f, 1f, 1f, 1f);


    //
    // Summary:
    //     Predefined unit-X vector
    public static vec4 UnitX { get; } = new vec4(1f, 0f, 0f, 0f);


    //
    // Summary:
    //     Predefined unit-Y vector
    public static vec4 UnitY { get; } = new vec4(0f, 1f, 0f, 0f);


    //
    // Summary:
    //     Predefined unit-Z vector
    public static vec4 UnitZ { get; } = new vec4(0f, 0f, 1f, 0f);


    //
    // Summary:
    //     Predefined unit-W vector
    public static vec4 UnitW { get; } = new vec4(0f, 0f, 0f, 1f);


    //
    // Summary:
    //     Predefined all-MaxValue vector
    public static vec4 MaxValue { get; } = new vec4(float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue);


    //
    // Summary:
    //     Predefined all-MinValue vector
    public static vec4 MinValue { get; } = new vec4(float.MinValue, float.MinValue, float.MinValue, float.MinValue);


    //
    // Summary:
    //     Predefined all-Epsilon vector
    public static vec4 Epsilon { get; } = new vec4(float.Epsilon, float.Epsilon, float.Epsilon, float.Epsilon);


    //
    // Summary:
    //     Predefined all-NaN vector
    public static vec4 NaN { get; } = new vec4(float.NaN, float.NaN, float.NaN, float.NaN);


    //
    // Summary:
    //     Predefined all-NegativeInfinity vector
    public static vec4 NegativeInfinity { get; } = new vec4(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);


    //
    // Summary:
    //     Predefined all-PositiveInfinity vector
    public static vec4 PositiveInfinity { get; } = new vec4(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);


    //
    // Summary:
    //     Component-wise constructor
    public vec4(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    //
    // Summary:
    //     all-same-value constructor
    public vec4(float v)
    {
        x = v;
        y = v;
        z = v;
        w = v;
    }

    //
    // Summary:
    //     from-vector constructor (empty fields are zero/false)
    public vec4(vec2 v)
    {
        x = v.x;
        y = v.y;
        z = 0f;
        w = 0f;
    }

    //
    // Summary:
    //     from-vector-and-value constructor (empty fields are zero/false)
    public vec4(vec2 v, float z)
    {
        x = v.x;
        y = v.y;
        this.z = z;
        w = 0f;
    }

    //
    // Summary:
    //     from-vector-and-value constructor
    public vec4(vec2 v, float z, float w)
    {
        x = v.x;
        y = v.y;
        this.z = z;
        this.w = w;
    }

    //
    // Summary:
    //     from-vector constructor (empty fields are zero/false)
    public vec4(vec3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        w = 0f;
    }

    //
    // Summary:
    //     from-vector-and-value constructor
    public vec4(vec3 v, float w)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        this.w = w;
    }

    //
    // Summary:
    //     from-vector constructor
    public vec4(vec4 v)
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
    public vec4(IReadOnlyList<float> v)
    {
        int count = v.Count;
        x = ((count < 0) ? 0f : v[0]);
        y = ((count < 1) ? 0f : v[1]);
        z = ((count < 2) ? 0f : v[2]);
        w = ((count < 3) ? 0f : v[3]);
    }

    //
    // Summary:
    //     Generic from-array constructor (superfluous values are ignored, missing values
    //     are zero-filled).
    public vec4(object[] v)
    {
        int num = v.Length;
        x = ((num < 0) ? 0f : ((float)v[0]));
        y = ((num < 1) ? 0f : ((float)v[1]));
        z = ((num < 2) ? 0f : ((float)v[2]));
        w = ((num < 3) ? 0f : ((float)v[3]));
    }

    //
    // Summary:
    //     From-array constructor (superfluous values are ignored, missing values are zero-filled).
    public vec4(float[] v)
    {
        int num = v.Length;
        x = ((num < 0) ? 0f : v[0]);
        y = ((num < 1) ? 0f : v[1]);
        z = ((num < 2) ? 0f : v[2]);
        w = ((num < 3) ? 0f : v[3]);
    }

    //
    // Summary:
    //     From-array constructor with base index (superfluous values are ignored, missing
    //     values are zero-filled).
    public vec4(float[] v, int startIndex)
    {
        int num = v.Length;
        x = ((num + startIndex < 0) ? 0f : v[startIndex]);
        y = ((num + startIndex < 1) ? 0f : v[1 + startIndex]);
        z = ((num + startIndex < 2) ? 0f : v[2 + startIndex]);
        w = ((num + startIndex < 3) ? 0f : v[3 + startIndex]);
    }

    //
    // Summary:
    //     From-IEnumerable constructor (superfluous values are ignored, missing values
    //     are zero-filled).
    public vec4(IEnumerable<float> v)
        : this(v.ToArray())
    {
    }

    //
    // Summary:
    //     Explicitly converts this to a ivec2.
    public static explicit operator ivec2(vec4 v)
    {
        return new ivec2((int)v.x, (int)v.y);
    }

    //
    // Summary:
    //     Explicitly converts this to a ivec3.
    public static explicit operator ivec3(vec4 v)
    {
        return new ivec3((int)v.x, (int)v.y, (int)v.z);
    }

    //
    // Summary:
    //     Explicitly converts this to a ivec4.
    public static explicit operator ivec4(vec4 v)
    {
        return new ivec4((int)v.x, (int)v.y, (int)v.z, (int)v.w);
    }

    //
    // Summary:
    //     Explicitly converts this to a vec2.
    public static explicit operator vec2(vec4 v)
    {
        return new vec2(v.x, v.y);
    }

    //
    // Summary:
    //     Explicitly converts this to a vec3.
    public static explicit operator vec3(vec4 v)
    {
        return new vec3(v.x, v.y, v.z);
    }

    //
    // Summary:
    //     Explicitly converts this to a float array.
    public static explicit operator float[](vec4 v)
    {
        return new float[4] { v.x, v.y, v.z, v.w };
    }

    //
    // Summary:
    //     Explicitly converts this to a generic object array.
    public static explicit operator object[](vec4 v)
    {
        return new object[4] { v.x, v.y, v.z, v.w };
    }

    //
    // Summary:
    //     Returns true iff this equals rhs component-wise.
    public static bool operator ==(vec4 lhs, vec4 rhs)
    {
        return lhs.Equals(rhs);
    }

    //
    // Summary:
    //     Returns true iff this does not equal rhs (component-wise).
    public static bool operator !=(vec4 lhs, vec4 rhs)
    {
        return !lhs.Equals(rhs);
    }

    //
    // Summary:
    //     Returns an enumerator that iterates through all components.
    public IEnumerator<float> GetEnumerator()
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
    public bool Equals(vec4 rhs)
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

        if (obj is vec4)
        {
            return Equals((vec4)obj);
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
    public static vec4 Parse(string s)
    {
        return Parse(s, ", ");
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator).
    public static vec4 Parse(string s, string sep)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 4)
        {
            throw new FormatException("input has not exactly 4 parts");
        }

        return new vec4(float.Parse(array[0].Trim()), float.Parse(array[1].Trim()), float.Parse(array[2].Trim()), float.Parse(array[3].Trim()));
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator and a type provider).
    public static vec4 Parse(string s, string sep, IFormatProvider provider)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 4)
        {
            throw new FormatException("input has not exactly 4 parts");
        }

        return new vec4(float.Parse(array[0].Trim(), provider), float.Parse(array[1].Trim(), provider), float.Parse(array[2].Trim(), provider), float.Parse(array[3].Trim(), provider));
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator and a number style).
    public static vec4 Parse(string s, string sep, NumberStyles style)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 4)
        {
            throw new FormatException("input has not exactly 4 parts");
        }

        return new vec4(float.Parse(array[0].Trim(), style), float.Parse(array[1].Trim(), style), float.Parse(array[2].Trim(), style), float.Parse(array[3].Trim(), style));
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator and a number style and a format provider).
    public static vec4 Parse(string s, string sep, NumberStyles style, IFormatProvider provider)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 4)
        {
            throw new FormatException("input has not exactly 4 parts");
        }

        return new vec4(float.Parse(array[0].Trim(), style, provider), float.Parse(array[1].Trim(), style, provider), float.Parse(array[2].Trim(), style, provider), float.Parse(array[3].Trim(), style, provider));
    }

    //
    // Summary:
    //     Tries to convert the string representation of the vector into a vector representation
    //     (using ', ' as a separator), returns false if string was invalid.
    public static bool TryParse(string s, out vec4 result)
    {
        return TryParse(s, ", ", out result);
    }

    //
    // Summary:
    //     Tries to convert the string representation of the vector into a vector representation
    //     (using a designated separator), returns false if string was invalid.
    public static bool TryParse(string s, string sep, out vec4 result)
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

        float result2 = 0f;
        float result3 = 0f;
        float result4 = 0f;
        float result5 = 0f;
        bool flag = float.TryParse(array[0].Trim(), out result2) && float.TryParse(array[1].Trim(), out result3) && float.TryParse(array[2].Trim(), out result4) && float.TryParse(array[3].Trim(), out result5);
        result = (flag ? new vec4(result2, result3, result4, result5) : Zero);
        return flag;
    }

    //
    // Summary:
    //     Tries to convert the string representation of the vector into a vector representation
    //     (using a designated separator and a number style and a format provider), returns
    //     false if string was invalid.
    public static bool TryParse(string s, string sep, NumberStyles style, IFormatProvider provider, out vec4 result)
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

        float result2 = 0f;
        float result3 = 0f;
        float result4 = 0f;
        float result5 = 0f;
        bool flag = float.TryParse(array[0].Trim(), style, provider, out result2) && float.TryParse(array[1].Trim(), style, provider, out result3) && float.TryParse(array[2].Trim(), style, provider, out result4) && float.TryParse(array[3].Trim(), style, provider, out result5);
        result = (flag ? new vec4(result2, result3, result4, result5) : Zero);
        return flag;
    }

    //
    // Summary:
    //     Returns true iff distance between lhs and rhs is less than or equal to epsilon
    public static bool ApproxEqual(vec4 lhs, vec4 rhs, float eps = 0.1f)
    {
        return Distance(lhs, rhs) <= eps;
    }

    //
    // Summary:
    //     OuterProduct treats the first parameter c as a column vector (matrix with one
    //     column) and the second parameter r as a row vector (matrix with one row) and
    //     does a linear algebraic matrix multiply c * r, yielding a matrix whose number
    //     of rows is the number of components in c and whose number of columns is the number
    //     of components in r.
    public static mat4 OuterProduct(vec4 c, vec4 r)
    {
        return new mat4(c.x * r.x, c.y * r.x, c.z * r.x, c.w * r.x, c.x * r.y, c.y * r.y, c.z * r.y, c.w * r.y, c.x * r.z, c.y * r.z, c.z * r.z, c.w * r.z, c.x * r.w, c.y * r.w, c.z * r.w, c.w * r.w);
    }

    //
    // Summary:
    //     Returns the inner product (dot product, scalar product) of the two vectors.
    public static float Dot(vec4 lhs, vec4 rhs)
    {
        return lhs.x * rhs.x + lhs.y * rhs.y + (lhs.z * rhs.z + lhs.w * rhs.w);
    }

    //
    // Summary:
    //     Returns the euclidean distance between the two vectors.
    public static float Distance(vec4 lhs, vec4 rhs)
    {
        return (lhs - rhs).Length;
    }

    //
    // Summary:
    //     Returns the squared euclidean distance between the two vectors.
    public static float DistanceSqr(vec4 lhs, vec4 rhs)
    {
        return (lhs - rhs).LengthSqr;
    }

    //
    // Summary:
    //     Calculate the reflection direction for an incident vector (N should be normalized
    //     in order to achieve the desired result).
    public static vec4 Reflect(vec4 I, vec4 N)
    {
        return I - 2f * Dot(N, I) * N;
    }

    //
    // Summary:
    //     Calculate the refraction direction for an incident vector (The input parameters
    //     I and N should be normalized in order to achieve the desired result).
    public static vec4 Refract(vec4 I, vec4 N, float eta)
    {
        float num = Dot(N, I);
        float num2 = 1f - eta * eta * (1f - num * num);
        if (num2 < 0f)
        {
            return Zero;
        }

        return eta * I - (eta * num + (float)Math.Sqrt(num2)) * N;
    }

    //
    // Summary:
    //     Returns a vector pointing in the same direction as another (faceforward orients
    //     a vector to point away from a surface as defined by its normal. If dot(Nref,
    //     I) is negative faceforward returns N, otherwise it returns -N).
    public static vec4 FaceForward(vec4 N, vec4 I, vec4 Nref)
    {
        if (!(Dot(Nref, I) < 0f))
        {
            return -N;
        }

        return N;
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed uniform values between
    //     0.0 and 1.0.
    public static vec4 Random(Random random)
    {
        return new vec4((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed uniform values between
    //     -1.0 and 1.0.
    public static vec4 RandomSigned(Random random)
    {
        return new vec4((float)(random.NextDouble() * 2.0 - 1.0), (float)(random.NextDouble() * 2.0 - 1.0), (float)(random.NextDouble() * 2.0 - 1.0), (float)(random.NextDouble() * 2.0 - 1.0));
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed values according
    //     to a normal distribution (zero mean, unit variance).
    public static vec4 RandomNormal(Random random)
    {
        return new vec4((float)(Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))), (float)(Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))), (float)(Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))), (float)(Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Abs (Math.Abs(v)).
    public static vec4 Abs(vec4 v)
    {
        return new vec4(Math.Abs(v.x), Math.Abs(v.y), Math.Abs(v.z), Math.Abs(v.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Abs (Math.Abs(v)).
    public static vec4 Abs(float v)
    {
        return new vec4(Math.Abs(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of HermiteInterpolationOrder3
    //     ((3 - 2 * v) * v * v).
    public static vec4 HermiteInterpolationOrder3(vec4 v)
    {
        return new vec4((3f - 2f * v.x) * v.x * v.x, (3f - 2f * v.y) * v.y * v.y, (3f - 2f * v.z) * v.z * v.z, (3f - 2f * v.w) * v.w * v.w);
    }

    //
    // Summary:
    //     Returns a vec from the application of HermiteInterpolationOrder3 ((3 - 2 * v)
    //     * v * v).
    public static vec4 HermiteInterpolationOrder3(float v)
    {
        return new vec4((3f - 2f * v) * v * v);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of HermiteInterpolationOrder5
    //     (((6 * v - 15) * v + 10) * v * v * v).
    public static vec4 HermiteInterpolationOrder5(vec4 v)
    {
        return new vec4(((6f * v.x - 15f) * v.x + 10f) * v.x * v.x * v.x, ((6f * v.y - 15f) * v.y + 10f) * v.y * v.y * v.y, ((6f * v.z - 15f) * v.z + 10f) * v.z * v.z * v.z, ((6f * v.w - 15f) * v.w + 10f) * v.w * v.w * v.w);
    }

    //
    // Summary:
    //     Returns a vec from the application of HermiteInterpolationOrder5 (((6 * v - 15)
    //     * v + 10) * v * v * v).
    public static vec4 HermiteInterpolationOrder5(float v)
    {
        return new vec4(((6f * v - 15f) * v + 10f) * v * v * v);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Sqr (v * v).
    public static vec4 Sqr(vec4 v)
    {
        return new vec4(v.x * v.x, v.y * v.y, v.z * v.z, v.w * v.w);
    }

    //
    // Summary:
    //     Returns a vec from the application of Sqr (v * v).
    public static vec4 Sqr(float v)
    {
        return new vec4(v * v);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Pow2 (v * v).
    public static vec4 Pow2(vec4 v)
    {
        return new vec4(v.x * v.x, v.y * v.y, v.z * v.z, v.w * v.w);
    }

    //
    // Summary:
    //     Returns a vec from the application of Pow2 (v * v).
    public static vec4 Pow2(float v)
    {
        return new vec4(v * v);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Pow3 (v * v * v).
    public static vec4 Pow3(vec4 v)
    {
        return new vec4(v.x * v.x * v.x, v.y * v.y * v.y, v.z * v.z * v.z, v.w * v.w * v.w);
    }

    //
    // Summary:
    //     Returns a vec from the application of Pow3 (v * v * v).
    public static vec4 Pow3(float v)
    {
        return new vec4(v * v * v);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Step (v >= 0f ? 1f : 0f).
    public static vec4 Step(vec4 v)
    {
        return new vec4((v.x >= 0f) ? 1f : 0f, (v.y >= 0f) ? 1f : 0f, (v.z >= 0f) ? 1f : 0f, (v.w >= 0f) ? 1f : 0f);
    }

    //
    // Summary:
    //     Returns a vec from the application of Step (v >= 0f ? 1f : 0f).
    public static vec4 Step(float v)
    {
        return new vec4((v >= 0f) ? 1f : 0f);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Sqrt ((float)Math.Sqrt((double)v)).
    public static vec4 Sqrt(vec4 v)
    {
        return new vec4((float)Math.Sqrt(v.x), (float)Math.Sqrt(v.y), (float)Math.Sqrt(v.z), (float)Math.Sqrt(v.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Sqrt ((float)Math.Sqrt((double)v)).
    public static vec4 Sqrt(float v)
    {
        return new vec4((float)Math.Sqrt(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of InverseSqrt ((float)(1.0 /
    //     Math.Sqrt((double)v))).
    public static vec4 InverseSqrt(vec4 v)
    {
        return new vec4((float)(1.0 / Math.Sqrt(v.x)), (float)(1.0 / Math.Sqrt(v.y)), (float)(1.0 / Math.Sqrt(v.z)), (float)(1.0 / Math.Sqrt(v.w)));
    }

    //
    // Summary:
    //     Returns a vec from the application of InverseSqrt ((float)(1.0 / Math.Sqrt((double)v))).
    public static vec4 InverseSqrt(float v)
    {
        return new vec4((float)(1.0 / Math.Sqrt(v)));
    }

    //
    // Summary:
    //     Returns a ivec4 from component-wise application of Sign (Math.Sign(v)).
    public static ivec4 Sign(vec4 v)
    {
        return new ivec4(Math.Sign(v.x), Math.Sign(v.y), Math.Sign(v.z), Math.Sign(v.w));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Sign (Math.Sign(v)).
    public static ivec4 Sign(float v)
    {
        return new ivec4(Math.Sign(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Max (Math.Max(lhs, rhs)).
    public static vec4 Max(vec4 lhs, vec4 rhs)
    {
        return new vec4(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z), Math.Max(lhs.w, rhs.w));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Max (Math.Max(lhs, rhs)).
    public static vec4 Max(vec4 lhs, float rhs)
    {
        return new vec4(Math.Max(lhs.x, rhs), Math.Max(lhs.y, rhs), Math.Max(lhs.z, rhs), Math.Max(lhs.w, rhs));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Max (Math.Max(lhs, rhs)).
    public static vec4 Max(float lhs, vec4 rhs)
    {
        return new vec4(Math.Max(lhs, rhs.x), Math.Max(lhs, rhs.y), Math.Max(lhs, rhs.z), Math.Max(lhs, rhs.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Max (Math.Max(lhs, rhs)).
    public static vec4 Max(float lhs, float rhs)
    {
        return new vec4(Math.Max(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Min (Math.Min(lhs, rhs)).
    public static vec4 Min(vec4 lhs, vec4 rhs)
    {
        return new vec4(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z), Math.Min(lhs.w, rhs.w));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Min (Math.Min(lhs, rhs)).
    public static vec4 Min(vec4 lhs, float rhs)
    {
        return new vec4(Math.Min(lhs.x, rhs), Math.Min(lhs.y, rhs), Math.Min(lhs.z, rhs), Math.Min(lhs.w, rhs));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Min (Math.Min(lhs, rhs)).
    public static vec4 Min(float lhs, vec4 rhs)
    {
        return new vec4(Math.Min(lhs, rhs.x), Math.Min(lhs, rhs.y), Math.Min(lhs, rhs.z), Math.Min(lhs, rhs.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Min (Math.Min(lhs, rhs)).
    public static vec4 Min(float lhs, float rhs)
    {
        return new vec4(Math.Min(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Pow ((float)Math.Pow((double)lhs,
    //     (double)rhs)).
    public static vec4 Pow(vec4 lhs, vec4 rhs)
    {
        return new vec4((float)Math.Pow(lhs.x, rhs.x), (float)Math.Pow(lhs.y, rhs.y), (float)Math.Pow(lhs.z, rhs.z), (float)Math.Pow(lhs.w, rhs.w));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Pow ((float)Math.Pow((double)lhs,
    //     (double)rhs)).
    public static vec4 Pow(vec4 lhs, float rhs)
    {
        return new vec4((float)Math.Pow(lhs.x, rhs), (float)Math.Pow(lhs.y, rhs), (float)Math.Pow(lhs.z, rhs), (float)Math.Pow(lhs.w, rhs));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Pow ((float)Math.Pow((double)lhs,
    //     (double)rhs)).
    public static vec4 Pow(float lhs, vec4 rhs)
    {
        return new vec4((float)Math.Pow(lhs, rhs.x), (float)Math.Pow(lhs, rhs.y), (float)Math.Pow(lhs, rhs.z), (float)Math.Pow(lhs, rhs.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Pow ((float)Math.Pow((double)lhs, (double)rhs)).
    public static vec4 Pow(float lhs, float rhs)
    {
        return new vec4((float)Math.Pow(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Log ((float)Math.Log((double)lhs,
    //     (double)rhs)).
    public static vec4 Log(vec4 lhs, vec4 rhs)
    {
        return new vec4((float)Math.Log(lhs.x, rhs.x), (float)Math.Log(lhs.y, rhs.y), (float)Math.Log(lhs.z, rhs.z), (float)Math.Log(lhs.w, rhs.w));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Log ((float)Math.Log((double)lhs,
    //     (double)rhs)).
    public static vec4 Log(vec4 lhs, float rhs)
    {
        return new vec4((float)Math.Log(lhs.x, rhs), (float)Math.Log(lhs.y, rhs), (float)Math.Log(lhs.z, rhs), (float)Math.Log(lhs.w, rhs));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Log ((float)Math.Log((double)lhs,
    //     (double)rhs)).
    public static vec4 Log(float lhs, vec4 rhs)
    {
        return new vec4((float)Math.Log(lhs, rhs.x), (float)Math.Log(lhs, rhs.y), (float)Math.Log(lhs, rhs.z), (float)Math.Log(lhs, rhs.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Log ((float)Math.Log((double)lhs, (double)rhs)).
    public static vec4 Log(float lhs, float rhs)
    {
        return new vec4((float)Math.Log(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec4 Clamp(vec4 v, vec4 min, vec4 max)
    {
        return new vec4(Math.Min(Math.Max(v.x, min.x), max.x), Math.Min(Math.Max(v.y, min.y), max.y), Math.Min(Math.Max(v.z, min.z), max.z), Math.Min(Math.Max(v.w, min.w), max.w));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec4 Clamp(vec4 v, vec4 min, float max)
    {
        return new vec4(Math.Min(Math.Max(v.x, min.x), max), Math.Min(Math.Max(v.y, min.y), max), Math.Min(Math.Max(v.z, min.z), max), Math.Min(Math.Max(v.w, min.w), max));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec4 Clamp(vec4 v, float min, vec4 max)
    {
        return new vec4(Math.Min(Math.Max(v.x, min), max.x), Math.Min(Math.Max(v.y, min), max.y), Math.Min(Math.Max(v.z, min), max.z), Math.Min(Math.Max(v.w, min), max.w));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec4 Clamp(vec4 v, float min, float max)
    {
        return new vec4(Math.Min(Math.Max(v.x, min), max), Math.Min(Math.Max(v.y, min), max), Math.Min(Math.Max(v.z, min), max), Math.Min(Math.Max(v.w, min), max));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec4 Clamp(float v, vec4 min, vec4 max)
    {
        return new vec4(Math.Min(Math.Max(v, min.x), max.x), Math.Min(Math.Max(v, min.y), max.y), Math.Min(Math.Max(v, min.z), max.z), Math.Min(Math.Max(v, min.w), max.w));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec4 Clamp(float v, vec4 min, float max)
    {
        return new vec4(Math.Min(Math.Max(v, min.x), max), Math.Min(Math.Max(v, min.y), max), Math.Min(Math.Max(v, min.z), max), Math.Min(Math.Max(v, min.w), max));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec4 Clamp(float v, float min, vec4 max)
    {
        return new vec4(Math.Min(Math.Max(v, min), max.x), Math.Min(Math.Max(v, min), max.y), Math.Min(Math.Max(v, min), max.z), Math.Min(Math.Max(v, min), max.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Clamp (Math.Min(Math.Max(v, min), max)).
    public static vec4 Clamp(float v, float min, float max)
    {
        return new vec4(Math.Min(Math.Max(v, min), max));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec4 Mix(vec4 min, vec4 max, vec4 a)
    {
        return new vec4(min.x * (1f - a.x) + max.x * a.x, min.y * (1f - a.y) + max.y * a.y, min.z * (1f - a.z) + max.z * a.z, min.w * (1f - a.w) + max.w * a.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec4 Mix(vec4 min, vec4 max, float a)
    {
        return new vec4(min.x * (1f - a) + max.x * a, min.y * (1f - a) + max.y * a, min.z * (1f - a) + max.z * a, min.w * (1f - a) + max.w * a);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec4 Mix(vec4 min, float max, vec4 a)
    {
        return new vec4(min.x * (1f - a.x) + max * a.x, min.y * (1f - a.y) + max * a.y, min.z * (1f - a.z) + max * a.z, min.w * (1f - a.w) + max * a.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec4 Mix(vec4 min, float max, float a)
    {
        return new vec4(min.x * (1f - a) + max * a, min.y * (1f - a) + max * a, min.z * (1f - a) + max * a, min.w * (1f - a) + max * a);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec4 Mix(float min, vec4 max, vec4 a)
    {
        return new vec4(min * (1f - a.x) + max.x * a.x, min * (1f - a.y) + max.y * a.y, min * (1f - a.z) + max.z * a.z, min * (1f - a.w) + max.w * a.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec4 Mix(float min, vec4 max, float a)
    {
        return new vec4(min * (1f - a) + max.x * a, min * (1f - a) + max.y * a, min * (1f - a) + max.z * a, min * (1f - a) + max.w * a);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec4 Mix(float min, float max, vec4 a)
    {
        return new vec4(min * (1f - a.x) + max * a.x, min * (1f - a.y) + max * a.y, min * (1f - a.z) + max * a.z, min * (1f - a.w) + max * a.w);
    }

    //
    // Summary:
    //     Returns a vec from the application of Mix (min * (1-a) + max * a).
    public static vec4 Mix(float min, float max, float a)
    {
        return new vec4(min * (1f - a) + max * a);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec4 Lerp(vec4 min, vec4 max, vec4 a)
    {
        return new vec4(min.x * (1f - a.x) + max.x * a.x, min.y * (1f - a.y) + max.y * a.y, min.z * (1f - a.z) + max.z * a.z, min.w * (1f - a.w) + max.w * a.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec4 Lerp(vec4 min, vec4 max, float a)
    {
        return new vec4(min.x * (1f - a) + max.x * a, min.y * (1f - a) + max.y * a, min.z * (1f - a) + max.z * a, min.w * (1f - a) + max.w * a);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec4 Lerp(vec4 min, float max, vec4 a)
    {
        return new vec4(min.x * (1f - a.x) + max * a.x, min.y * (1f - a.y) + max * a.y, min.z * (1f - a.z) + max * a.z, min.w * (1f - a.w) + max * a.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec4 Lerp(vec4 min, float max, float a)
    {
        return new vec4(min.x * (1f - a) + max * a, min.y * (1f - a) + max * a, min.z * (1f - a) + max * a, min.w * (1f - a) + max * a);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec4 Lerp(float min, vec4 max, vec4 a)
    {
        return new vec4(min * (1f - a.x) + max.x * a.x, min * (1f - a.y) + max.y * a.y, min * (1f - a.z) + max.z * a.z, min * (1f - a.w) + max.w * a.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec4 Lerp(float min, vec4 max, float a)
    {
        return new vec4(min * (1f - a) + max.x * a, min * (1f - a) + max.y * a, min * (1f - a) + max.z * a, min * (1f - a) + max.w * a);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec4 Lerp(float min, float max, vec4 a)
    {
        return new vec4(min * (1f - a.x) + max * a.x, min * (1f - a.y) + max * a.y, min * (1f - a.z) + max * a.z, min * (1f - a.w) + max * a.w);
    }

    //
    // Summary:
    //     Returns a vec from the application of Lerp (min * (1-a) + max * a).
    public static vec4 Lerp(float min, float max, float a)
    {
        return new vec4(min * (1f - a) + max * a);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Fma (a * b + c).
    public static vec4 Fma(vec4 a, vec4 b, vec4 c)
    {
        return new vec4(a.x * b.x + c.x, a.y * b.y + c.y, a.z * b.z + c.z, a.w * b.w + c.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Fma (a * b + c).
    public static vec4 Fma(vec4 a, vec4 b, float c)
    {
        return new vec4(a.x * b.x + c, a.y * b.y + c, a.z * b.z + c, a.w * b.w + c);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Fma (a * b + c).
    public static vec4 Fma(vec4 a, float b, vec4 c)
    {
        return new vec4(a.x * b + c.x, a.y * b + c.y, a.z * b + c.z, a.w * b + c.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Fma (a * b + c).
    public static vec4 Fma(vec4 a, float b, float c)
    {
        return new vec4(a.x * b + c, a.y * b + c, a.z * b + c, a.w * b + c);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Fma (a * b + c).
    public static vec4 Fma(float a, vec4 b, vec4 c)
    {
        return new vec4(a * b.x + c.x, a * b.y + c.y, a * b.z + c.z, a * b.w + c.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Fma (a * b + c).
    public static vec4 Fma(float a, vec4 b, float c)
    {
        return new vec4(a * b.x + c, a * b.y + c, a * b.z + c, a * b.w + c);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Fma (a * b + c).
    public static vec4 Fma(float a, float b, vec4 c)
    {
        return new vec4(a * b + c.x, a * b + c.y, a * b + c.z, a * b + c.w);
    }

    //
    // Summary:
    //     Returns a vec from the application of Fma (a * b + c).
    public static vec4 Fma(float a, float b, float c)
    {
        return new vec4(a * b + c);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Add (lhs + rhs).
    public static vec4 Add(vec4 lhs, vec4 rhs)
    {
        return new vec4(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Add (lhs + rhs).
    public static vec4 Add(vec4 lhs, float rhs)
    {
        return new vec4(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs, lhs.w + rhs);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Add (lhs + rhs).
    public static vec4 Add(float lhs, vec4 rhs)
    {
        return new vec4(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z, lhs + rhs.w);
    }

    //
    // Summary:
    //     Returns a vec from the application of Add (lhs + rhs).
    public static vec4 Add(float lhs, float rhs)
    {
        return new vec4(lhs + rhs);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Sub (lhs - rhs).
    public static vec4 Sub(vec4 lhs, vec4 rhs)
    {
        return new vec4(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Sub (lhs - rhs).
    public static vec4 Sub(vec4 lhs, float rhs)
    {
        return new vec4(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs, lhs.w - rhs);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Sub (lhs - rhs).
    public static vec4 Sub(float lhs, vec4 rhs)
    {
        return new vec4(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z, lhs - rhs.w);
    }

    //
    // Summary:
    //     Returns a vec from the application of Sub (lhs - rhs).
    public static vec4 Sub(float lhs, float rhs)
    {
        return new vec4(lhs - rhs);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Mul (lhs * rhs).
    public static vec4 Mul(vec4 lhs, vec4 rhs)
    {
        return new vec4(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z, lhs.w * rhs.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Mul (lhs * rhs).
    public static vec4 Mul(vec4 lhs, float rhs)
    {
        return new vec4(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs, lhs.w * rhs);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Mul (lhs * rhs).
    public static vec4 Mul(float lhs, vec4 rhs)
    {
        return new vec4(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z, lhs * rhs.w);
    }

    //
    // Summary:
    //     Returns a vec from the application of Mul (lhs * rhs).
    public static vec4 Mul(float lhs, float rhs)
    {
        return new vec4(lhs * rhs);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Div (lhs / rhs).
    public static vec4 Div(vec4 lhs, vec4 rhs)
    {
        return new vec4(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z, lhs.w / rhs.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Div (lhs / rhs).
    public static vec4 Div(vec4 lhs, float rhs)
    {
        return new vec4(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs, lhs.w / rhs);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Div (lhs / rhs).
    public static vec4 Div(float lhs, vec4 rhs)
    {
        return new vec4(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z, lhs / rhs.w);
    }

    //
    // Summary:
    //     Returns a vec from the application of Div (lhs / rhs).
    public static vec4 Div(float lhs, float rhs)
    {
        return new vec4(lhs / rhs);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Modulo (lhs % rhs).
    public static vec4 Modulo(vec4 lhs, vec4 rhs)
    {
        return new vec4(lhs.x % rhs.x, lhs.y % rhs.y, lhs.z % rhs.z, lhs.w % rhs.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Modulo (lhs % rhs).
    public static vec4 Modulo(vec4 lhs, float rhs)
    {
        return new vec4(lhs.x % rhs, lhs.y % rhs, lhs.z % rhs, lhs.w % rhs);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Modulo (lhs % rhs).
    public static vec4 Modulo(float lhs, vec4 rhs)
    {
        return new vec4(lhs % rhs.x, lhs % rhs.y, lhs % rhs.z, lhs % rhs.w);
    }

    //
    // Summary:
    //     Returns a vec from the application of Modulo (lhs % rhs).
    public static vec4 Modulo(float lhs, float rhs)
    {
        return new vec4(lhs % rhs);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Degrees (Radians-To-Degrees
    //     Conversion).
    public static vec4 Degrees(vec4 v)
    {
        return new vec4(v.x * 57.29578f, v.y * 57.29578f, v.z * 57.29578f, v.w * 57.29578f);
    }

    //
    // Summary:
    //     Returns a vec from the application of Degrees (Radians-To-Degrees Conversion).
    public static vec4 Degrees(float v)
    {
        return new vec4(v * 57.29578f);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Radians (Degrees-To-Radians
    //     Conversion).
    public static vec4 Radians(vec4 v)
    {
        return new vec4(v.x * ((float)Math.PI / 180f), v.y * ((float)Math.PI / 180f), v.z * ((float)Math.PI / 180f), v.w * ((float)Math.PI / 180f));
    }

    //
    // Summary:
    //     Returns a vec from the application of Radians (Degrees-To-Radians Conversion).
    public static vec4 Radians(float v)
    {
        return new vec4(v * ((float)Math.PI / 180f));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Acos ((float)Math.Acos((double)v)).
    public static vec4 Acos(vec4 v)
    {
        return new vec4((float)Math.Acos(v.x), (float)Math.Acos(v.y), (float)Math.Acos(v.z), (float)Math.Acos(v.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Acos ((float)Math.Acos((double)v)).
    public static vec4 Acos(float v)
    {
        return new vec4((float)Math.Acos(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Asin ((float)Math.Asin((double)v)).
    public static vec4 Asin(vec4 v)
    {
        return new vec4((float)Math.Asin(v.x), (float)Math.Asin(v.y), (float)Math.Asin(v.z), (float)Math.Asin(v.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Asin ((float)Math.Asin((double)v)).
    public static vec4 Asin(float v)
    {
        return new vec4((float)Math.Asin(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Atan ((float)Math.Atan((double)v)).
    public static vec4 Atan(vec4 v)
    {
        return new vec4((float)Math.Atan(v.x), (float)Math.Atan(v.y), (float)Math.Atan(v.z), (float)Math.Atan(v.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Atan ((float)Math.Atan((double)v)).
    public static vec4 Atan(float v)
    {
        return new vec4((float)Math.Atan(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Cos ((float)Math.Cos((double)v)).
    public static vec4 Cos(vec4 v)
    {
        return new vec4((float)Math.Cos(v.x), (float)Math.Cos(v.y), (float)Math.Cos(v.z), (float)Math.Cos(v.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Cos ((float)Math.Cos((double)v)).
    public static vec4 Cos(float v)
    {
        return new vec4((float)Math.Cos(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Cosh ((float)Math.Cosh((double)v)).
    public static vec4 Cosh(vec4 v)
    {
        return new vec4((float)Math.Cosh(v.x), (float)Math.Cosh(v.y), (float)Math.Cosh(v.z), (float)Math.Cosh(v.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Cosh ((float)Math.Cosh((double)v)).
    public static vec4 Cosh(float v)
    {
        return new vec4((float)Math.Cosh(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Exp ((float)Math.Exp((double)v)).
    public static vec4 Exp(vec4 v)
    {
        return new vec4((float)Math.Exp(v.x), (float)Math.Exp(v.y), (float)Math.Exp(v.z), (float)Math.Exp(v.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Exp ((float)Math.Exp((double)v)).
    public static vec4 Exp(float v)
    {
        return new vec4((float)Math.Exp(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Log ((float)Math.Log((double)v)).
    public static vec4 Log(vec4 v)
    {
        return new vec4((float)Math.Log(v.x), (float)Math.Log(v.y), (float)Math.Log(v.z), (float)Math.Log(v.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Log ((float)Math.Log((double)v)).
    public static vec4 Log(float v)
    {
        return new vec4((float)Math.Log(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Log2 ((float)Math.Log((double)v,
    //     2)).
    public static vec4 Log2(vec4 v)
    {
        return new vec4((float)Math.Log(v.x, 2.0), (float)Math.Log(v.y, 2.0), (float)Math.Log(v.z, 2.0), (float)Math.Log(v.w, 2.0));
    }

    //
    // Summary:
    //     Returns a vec from the application of Log2 ((float)Math.Log((double)v, 2)).
    public static vec4 Log2(float v)
    {
        return new vec4((float)Math.Log(v, 2.0));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Log10 ((float)Math.Log10((double)v)).
    public static vec4 Log10(vec4 v)
    {
        return new vec4((float)Math.Log10(v.x), (float)Math.Log10(v.y), (float)Math.Log10(v.z), (float)Math.Log10(v.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Log10 ((float)Math.Log10((double)v)).
    public static vec4 Log10(float v)
    {
        return new vec4((float)Math.Log10(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Floor ((float)Math.Floor(v)).
    public static vec4 Floor(vec4 v)
    {
        return new vec4((float)Math.Floor(v.x), (float)Math.Floor(v.y), (float)Math.Floor(v.z), (float)Math.Floor(v.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Floor ((float)Math.Floor(v)).
    public static vec4 Floor(float v)
    {
        return new vec4((float)Math.Floor(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Ceiling ((float)Math.Ceiling(v)).
    public static vec4 Ceiling(vec4 v)
    {
        return new vec4((float)Math.Ceiling(v.x), (float)Math.Ceiling(v.y), (float)Math.Ceiling(v.z), (float)Math.Ceiling(v.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Ceiling ((float)Math.Ceiling(v)).
    public static vec4 Ceiling(float v)
    {
        return new vec4((float)Math.Ceiling(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Round ((float)Math.Round(v)).
    public static vec4 Round(vec4 v)
    {
        return new vec4((float)Math.Round(v.x), (float)Math.Round(v.y), (float)Math.Round(v.z), (float)Math.Round(v.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Round ((float)Math.Round(v)).
    public static vec4 Round(float v)
    {
        return new vec4((float)Math.Round(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Sin ((float)Math.Sin((double)v)).
    public static vec4 Sin(vec4 v)
    {
        return new vec4((float)Math.Sin(v.x), (float)Math.Sin(v.y), (float)Math.Sin(v.z), (float)Math.Sin(v.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Sin ((float)Math.Sin((double)v)).
    public static vec4 Sin(float v)
    {
        return new vec4((float)Math.Sin(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Sinh ((float)Math.Sinh((double)v)).
    public static vec4 Sinh(vec4 v)
    {
        return new vec4((float)Math.Sinh(v.x), (float)Math.Sinh(v.y), (float)Math.Sinh(v.z), (float)Math.Sinh(v.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Sinh ((float)Math.Sinh((double)v)).
    public static vec4 Sinh(float v)
    {
        return new vec4((float)Math.Sinh(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Tan ((float)Math.Tan((double)v)).
    public static vec4 Tan(vec4 v)
    {
        return new vec4((float)Math.Tan(v.x), (float)Math.Tan(v.y), (float)Math.Tan(v.z), (float)Math.Tan(v.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Tan ((float)Math.Tan((double)v)).
    public static vec4 Tan(float v)
    {
        return new vec4((float)Math.Tan(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Tanh ((float)Math.Tanh((double)v)).
    public static vec4 Tanh(vec4 v)
    {
        return new vec4((float)Math.Tanh(v.x), (float)Math.Tanh(v.y), (float)Math.Tanh(v.z), (float)Math.Tanh(v.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Tanh ((float)Math.Tanh((double)v)).
    public static vec4 Tanh(float v)
    {
        return new vec4((float)Math.Tanh(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Truncate ((float)Math.Truncate((double)v)).
    public static vec4 Truncate(vec4 v)
    {
        return new vec4((float)Math.Truncate(v.x), (float)Math.Truncate(v.y), (float)Math.Truncate(v.z), (float)Math.Truncate(v.w));
    }

    //
    // Summary:
    //     Returns a vec from the application of Truncate ((float)Math.Truncate((double)v)).
    public static vec4 Truncate(float v)
    {
        return new vec4((float)Math.Truncate(v));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Fract ((float)(v - Math.Floor(v))).
    public static vec4 Fract(vec4 v)
    {
        return new vec4((float)((double)v.x - Math.Floor(v.x)), (float)((double)v.y - Math.Floor(v.y)), (float)((double)v.z - Math.Floor(v.z)), (float)((double)v.w - Math.Floor(v.w)));
    }

    //
    // Summary:
    //     Returns a vec from the application of Fract ((float)(v - Math.Floor(v))).
    public static vec4 Fract(float v)
    {
        return new vec4((float)((double)v - Math.Floor(v)));
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of Trunc ((long)(v)).
    public static vec4 Trunc(vec4 v)
    {
        return new vec4((long)v.x, (long)v.y, (long)v.z, (long)v.w);
    }

    //
    // Summary:
    //     Returns a vec from the application of Trunc ((long)(v)).
    public static vec4 Trunc(float v)
    {
        return new vec4((long)v);
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec4 Random(Random random, vec4 minValue, vec4 maxValue)
    {
        return new vec4((float)random.NextDouble() * (maxValue.x - minValue.x) + minValue.x, (float)random.NextDouble() * (maxValue.y - minValue.y) + minValue.y, (float)random.NextDouble() * (maxValue.z - minValue.z) + minValue.z, (float)random.NextDouble() * (maxValue.w - minValue.w) + minValue.w);
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec4 Random(Random random, vec4 minValue, float maxValue)
    {
        return new vec4((float)random.NextDouble() * (maxValue - minValue.x) + minValue.x, (float)random.NextDouble() * (maxValue - minValue.y) + minValue.y, (float)random.NextDouble() * (maxValue - minValue.z) + minValue.z, (float)random.NextDouble() * (maxValue - minValue.w) + minValue.w);
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec4 Random(Random random, float minValue, vec4 maxValue)
    {
        return new vec4((float)random.NextDouble() * (maxValue.x - minValue) + minValue, (float)random.NextDouble() * (maxValue.y - minValue) + minValue, (float)random.NextDouble() * (maxValue.z - minValue) + minValue, (float)random.NextDouble() * (maxValue.w - minValue) + minValue);
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec4 Random(Random random, float minValue, float maxValue)
    {
        return new vec4((float)random.NextDouble() * (maxValue - minValue) + minValue);
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec4 RandomUniform(Random random, vec4 minValue, vec4 maxValue)
    {
        return new vec4((float)random.NextDouble() * (maxValue.x - minValue.x) + minValue.x, (float)random.NextDouble() * (maxValue.y - minValue.y) + minValue.y, (float)random.NextDouble() * (maxValue.z - minValue.z) + minValue.z, (float)random.NextDouble() * (maxValue.w - minValue.w) + minValue.w);
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec4 RandomUniform(Random random, vec4 minValue, float maxValue)
    {
        return new vec4((float)random.NextDouble() * (maxValue - minValue.x) + minValue.x, (float)random.NextDouble() * (maxValue - minValue.y) + minValue.y, (float)random.NextDouble() * (maxValue - minValue.z) + minValue.z, (float)random.NextDouble() * (maxValue - minValue.w) + minValue.w);
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec4 RandomUniform(Random random, float minValue, vec4 maxValue)
    {
        return new vec4((float)random.NextDouble() * (maxValue.x - minValue) + minValue, (float)random.NextDouble() * (maxValue.y - minValue) + minValue, (float)random.NextDouble() * (maxValue.z - minValue) + minValue, (float)random.NextDouble() * (maxValue.w - minValue) + minValue);
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec4 RandomUniform(Random random, float minValue, float maxValue)
    {
        return new vec4((float)random.NextDouble() * (maxValue - minValue) + minValue);
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec4 RandomNormal(Random random, vec4 mean, vec4 variance)
    {
        return new vec4((float)(Math.Sqrt(variance.x) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.x, (float)(Math.Sqrt(variance.y) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.y, (float)(Math.Sqrt(variance.z) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.z, (float)(Math.Sqrt(variance.w) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.w);
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec4 RandomNormal(Random random, vec4 mean, float variance)
    {
        return new vec4((float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.x, (float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.y, (float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.z, (float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.w);
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec4 RandomNormal(Random random, float mean, vec4 variance)
    {
        return new vec4((float)(Math.Sqrt(variance.x) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean, (float)(Math.Sqrt(variance.y) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean, (float)(Math.Sqrt(variance.z) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean, (float)(Math.Sqrt(variance.w) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean);
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec4 RandomNormal(Random random, float mean, float variance)
    {
        return new vec4((float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean);
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec4 RandomGaussian(Random random, vec4 mean, vec4 variance)
    {
        return new vec4((float)(Math.Sqrt(variance.x) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.x, (float)(Math.Sqrt(variance.y) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.y, (float)(Math.Sqrt(variance.z) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.z, (float)(Math.Sqrt(variance.w) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.w);
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec4 RandomGaussian(Random random, vec4 mean, float variance)
    {
        return new vec4((float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.x, (float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.y, (float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.z, (float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.w);
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec4 RandomGaussian(Random random, float mean, vec4 variance)
    {
        return new vec4((float)(Math.Sqrt(variance.x) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean, (float)(Math.Sqrt(variance.y) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean, (float)(Math.Sqrt(variance.z) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean, (float)(Math.Sqrt(variance.w) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean);
    }

    //
    // Summary:
    //     Returns a vec4 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec4 RandomGaussian(Random random, float mean, float variance)
    {
        return new vec4((float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of operator+ (lhs + rhs).
    public static vec4 operator +(vec4 lhs, vec4 rhs)
    {
        return new vec4(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of operator+ (lhs + rhs).
    public static vec4 operator +(vec4 lhs, float rhs)
    {
        return new vec4(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs, lhs.w + rhs);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of operator+ (lhs + rhs).
    public static vec4 operator +(float lhs, vec4 rhs)
    {
        return new vec4(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z, lhs + rhs.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of operator- (lhs - rhs).
    public static vec4 operator -(vec4 lhs, vec4 rhs)
    {
        return new vec4(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of operator- (lhs - rhs).
    public static vec4 operator -(vec4 lhs, float rhs)
    {
        return new vec4(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs, lhs.w - rhs);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of operator- (lhs - rhs).
    public static vec4 operator -(float lhs, vec4 rhs)
    {
        return new vec4(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z, lhs - rhs.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of operator* (lhs * rhs).
    public static vec4 operator *(vec4 lhs, vec4 rhs)
    {
        return new vec4(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z, lhs.w * rhs.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of operator* (lhs * rhs).
    public static vec4 operator *(vec4 lhs, float rhs)
    {
        return new vec4(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs, lhs.w * rhs);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of operator* (lhs * rhs).
    public static vec4 operator *(float lhs, vec4 rhs)
    {
        return new vec4(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z, lhs * rhs.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of operator/ (lhs / rhs).
    public static vec4 operator /(vec4 lhs, vec4 rhs)
    {
        return new vec4(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z, lhs.w / rhs.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of operator/ (lhs / rhs).
    public static vec4 operator /(vec4 lhs, float rhs)
    {
        return new vec4(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs, lhs.w / rhs);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of operator/ (lhs / rhs).
    public static vec4 operator /(float lhs, vec4 rhs)
    {
        return new vec4(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z, lhs / rhs.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of operator+ (identity).
    public static vec4 operator +(vec4 v)
    {
        return v;
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of operator- (-v).
    public static vec4 operator -(vec4 v)
    {
        return new vec4(0f - v.x, 0f - v.y, 0f - v.z, 0f - v.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of operator% (lhs % rhs).
    public static vec4 operator %(vec4 lhs, vec4 rhs)
    {
        return new vec4(lhs.x % rhs.x, lhs.y % rhs.y, lhs.z % rhs.z, lhs.w % rhs.w);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of operator% (lhs % rhs).
    public static vec4 operator %(vec4 lhs, float rhs)
    {
        return new vec4(lhs.x % rhs, lhs.y % rhs, lhs.z % rhs, lhs.w % rhs);
    }

    //
    // Summary:
    //     Returns a vec4 from component-wise application of operator% (lhs % rhs).
    public static vec4 operator %(float lhs, vec4 rhs)
    {
        return new vec4(lhs % rhs.x, lhs % rhs.y, lhs % rhs.z, lhs % rhs.w);
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
