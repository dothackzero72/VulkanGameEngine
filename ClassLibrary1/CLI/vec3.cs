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
using System.Numerics;
using System.Runtime.Serialization;

namespace GlmSharp;

//
// Summary:
//     A vector of type float with 3 components.
[Serializable]
[DataContract(Namespace = "vec")]
public struct vec3 : IReadOnlyList<float>, IReadOnlyCollection<float>, IEnumerable<float>, IEnumerable, IEquatable<vec3>
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
    //     Returns an array with all values
    public float[] Values => new float[3] { x, y, z };

    //
    // Summary:
    //     Returns the number of components (3).
    public int Count => 3;

    //
    // Summary:
    //     Returns the minimal component of this vector.
    public float MinElement => Math.Min(Math.Min(x, y), z);

    //
    // Summary:
    //     Returns the maximal component of this vector.
    public float MaxElement => Math.Max(Math.Max(x, y), z);

    //
    // Summary:
    //     Returns the euclidean length of this vector.
    public float Length => (float)Math.Sqrt(x * x + y * y + z * z);

    //
    // Summary:
    //     Returns the squared euclidean length of this vector.
    public float LengthSqr => x * x + y * y + z * z;

    //
    // Summary:
    //     Returns the sum of all components.
    public float Sum => x + y + z;

    //
    // Summary:
    //     Returns the euclidean norm of this vector.
    public float Norm => (float)Math.Sqrt(x * x + y * y + z * z);

    //
    // Summary:
    //     Returns the one-norm of this vector.
    public float Norm1 => Math.Abs(x) + Math.Abs(y) + Math.Abs(z);

    //
    // Summary:
    //     Returns the two-norm (euclidean length) of this vector.
    public float Norm2 => (float)Math.Sqrt(x * x + y * y + z * z);

    //
    // Summary:
    //     Returns the max-norm of this vector.
    public float NormMax => Math.Max(Math.Max(Math.Abs(x), Math.Abs(y)), Math.Abs(z));

    //
    // Summary:
    //     Returns a copy of this vector with length one (undefined if this has zero length).
    public vec3 Normalized => this / Length;

    //
    // Summary:
    //     Returns a copy of this vector with length one (returns zero if length is zero).
    public vec3 NormalizedSafe
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
    public static vec3 Zero { get; } = new vec3(0f, 0f, 0f);


    //
    // Summary:
    //     Predefined all-ones vector
    public static vec3 Ones { get; } = new vec3(1f, 1f, 1f);


    //
    // Summary:
    //     Predefined unit-X vector
    public static vec3 UnitX { get; } = new vec3(1f, 0f, 0f);


    //
    // Summary:
    //     Predefined unit-Y vector
    public static vec3 UnitY { get; } = new vec3(0f, 1f, 0f);


    //
    // Summary:
    //     Predefined unit-Z vector
    public static vec3 UnitZ { get; } = new vec3(0f, 0f, 1f);


    //
    // Summary:
    //     Predefined all-MaxValue vector
    public static vec3 MaxValue { get; } = new vec3(float.MaxValue, float.MaxValue, float.MaxValue);


    //
    // Summary:
    //     Predefined all-MinValue vector
    public static vec3 MinValue { get; } = new vec3(float.MinValue, float.MinValue, float.MinValue);


    //
    // Summary:
    //     Predefined all-Epsilon vector
    public static vec3 Epsilon { get; } = new vec3(float.Epsilon, float.Epsilon, float.Epsilon);


    //
    // Summary:
    //     Predefined all-NaN vector
    public static vec3 NaN { get; } = new vec3(float.NaN, float.NaN, float.NaN);


    //
    // Summary:
    //     Predefined all-NegativeInfinity vector
    public static vec3 NegativeInfinity { get; } = new vec3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);


    //
    // Summary:
    //     Predefined all-PositiveInfinity vector
    public static vec3 PositiveInfinity { get; } = new vec3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);


    //
    // Summary:
    //     Component-wise constructor
    public vec3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    //
    // Summary:
    //     all-same-value constructor
    public vec3(float v)
    {
        x = v;
        y = v;
        z = v;
    }

    //
    // Summary:
    //     from-vector constructor (empty fields are zero/false)
    public vec3(vec2 v)
    {
        x = v.x;
        y = v.y;
        z = 0f;
    }

    //
    // Summary:
    //     from-vector-and-value constructor
    public vec3(vec2 v, float z)
    {
        x = v.x;
        y = v.y;
        this.z = z;
    }

    //
    // Summary:
    //     from-vector constructor
    public vec3(vec3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    //
    // Summary:
    //     from-vector constructor (additional fields are truncated)
    public vec3(vec4 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    //
    // Summary:
    //     From-array/list constructor (superfluous values are ignored, missing values are
    //     zero-filled).
    public vec3(IReadOnlyList<float> v)
    {
        int count = v.Count;
        x = ((count < 0) ? 0f : v[0]);
        y = ((count < 1) ? 0f : v[1]);
        z = ((count < 2) ? 0f : v[2]);
    }

    //
    // Summary:
    //     Generic from-array constructor (superfluous values are ignored, missing values
    //     are zero-filled).
    public vec3(object[] v)
    {
        int num = v.Length;
        x = ((num < 0) ? 0f : ((float)v[0]));
        y = ((num < 1) ? 0f : ((float)v[1]));
        z = ((num < 2) ? 0f : ((float)v[2]));
    }

    //
    // Summary:
    //     From-array constructor (superfluous values are ignored, missing values are zero-filled).
    public vec3(float[] v)
    {
        int num = v.Length;
        x = ((num < 0) ? 0f : v[0]);
        y = ((num < 1) ? 0f : v[1]);
        z = ((num < 2) ? 0f : v[2]);
    }

    //
    // Summary:
    //     From-array constructor with base index (superfluous values are ignored, missing
    //     values are zero-filled).
    public vec3(float[] v, int startIndex)
    {
        int num = v.Length;
        x = ((num + startIndex < 0) ? 0f : v[startIndex]);
        y = ((num + startIndex < 1) ? 0f : v[1 + startIndex]);
        z = ((num + startIndex < 2) ? 0f : v[2 + startIndex]);
    }

    //
    // Summary:
    //     From-IEnumerable constructor (superfluous values are ignored, missing values
    //     are zero-filled).
    public vec3(IEnumerable<float> v)
        : this(v.ToArray())
    {
    }

    //
    // Summary:
    //     Explicitly converts this to a ivec2.
    public static explicit operator ivec2(vec3 v)
    {
        return new ivec2((int)v.x, (int)v.y);
    }

    //
    // Summary:
    //     Explicitly converts this to a ivec3.
    public static explicit operator ivec3(vec3 v)
    {
        return new ivec3((int)v.x, (int)v.y, (int)v.z);
    }

    //
    // Summary:
    //     Explicitly converts this to a ivec4. (Higher components are zeroed)
    public static explicit operator ivec4(vec3 v)
    {
        return new ivec4((int)v.x, (int)v.y, (int)v.z, 0);
    }

    //
    // Summary:
    //     Explicitly converts this to a vec2.
    public static explicit operator vec2(vec3 v)
    {
        return new vec2(v.x, v.y);
    }

    //
    // Summary:
    //     Explicitly converts this to a vec4. (Higher components are zeroed)
    public static explicit operator vec4(vec3 v)
    {
        return new vec4(v.x, v.y, v.z, 0f);
    }

    //
    // Summary:
    //     Explicitly converts this to a float array.
    public static explicit operator float[](vec3 v)
    {
        return new float[3] { v.x, v.y, v.z };
    }

    //
    // Summary:
    //     Explicitly converts this to a generic object array.
    public static explicit operator object[](vec3 v)
    {
        return new object[3] { v.x, v.y, v.z };
    }

    //
    // Summary:
    //     Returns true iff this equals rhs component-wise.
    public static bool operator ==(vec3 lhs, vec3 rhs)
    {
        return lhs.Equals(rhs);
    }

    //
    // Summary:
    //     Returns true iff this does not equal rhs (component-wise).
    public static bool operator !=(vec3 lhs, vec3 rhs)
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
        return x + sep + y + sep + z;
    }

    //
    // Summary:
    //     Returns a string representation of this vector using a provided seperator and
    //     a format provider for each component.
    public string ToString(string sep, IFormatProvider provider)
    {
        return x.ToString(provider) + sep + y.ToString(provider) + sep + z.ToString(provider);
    }

    //
    // Summary:
    //     Returns a string representation of this vector using a provided seperator and
    //     a format for each component.
    public string ToString(string sep, string format)
    {
        return x.ToString(format) + sep + y.ToString(format) + sep + z.ToString(format);
    }

    //
    // Summary:
    //     Returns a string representation of this vector using a provided seperator and
    //     a format and format provider for each component.
    public string ToString(string sep, string format, IFormatProvider provider)
    {
        return x.ToString(format, provider) + sep + y.ToString(format, provider) + sep + z.ToString(format, provider);
    }

    //
    // Summary:
    //     Returns true iff this equals rhs component-wise.
    public bool Equals(vec3 rhs)
    {
        if (x.Equals(rhs.x) && y.Equals(rhs.y))
        {
            return z.Equals(rhs.z);
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

        if (obj is vec3)
        {
            return Equals((vec3)obj);
        }

        return false;
    }

    //
    // Summary:
    //     Returns a hash code for this instance.
    public override int GetHashCode()
    {
        return (((x.GetHashCode() * 397) ^ y.GetHashCode()) * 397) ^ z.GetHashCode();
    }

    //
    // Summary:
    //     Returns the p-norm of this vector.
    public double NormP(double p)
    {
        return Math.Pow(Math.Pow(Math.Abs(x), p) + Math.Pow(Math.Abs(y), p) + Math.Pow(Math.Abs(z), p), 1.0 / p);
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using ', ' as a separator).
    public static vec3 Parse(string s)
    {
        return Parse(s, ", ");
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator).
    public static vec3 Parse(string s, string sep)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 3)
        {
            throw new FormatException("input has not exactly 3 parts");
        }

        return new vec3(float.Parse(array[0].Trim()), float.Parse(array[1].Trim()), float.Parse(array[2].Trim()));
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator and a type provider).
    public static vec3 Parse(string s, string sep, IFormatProvider provider)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 3)
        {
            throw new FormatException("input has not exactly 3 parts");
        }

        return new vec3(float.Parse(array[0].Trim(), provider), float.Parse(array[1].Trim(), provider), float.Parse(array[2].Trim(), provider));
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator and a number style).
    public static vec3 Parse(string s, string sep, NumberStyles style)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 3)
        {
            throw new FormatException("input has not exactly 3 parts");
        }

        return new vec3(float.Parse(array[0].Trim(), style), float.Parse(array[1].Trim(), style), float.Parse(array[2].Trim(), style));
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator and a number style and a format provider).
    public static vec3 Parse(string s, string sep, NumberStyles style, IFormatProvider provider)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 3)
        {
            throw new FormatException("input has not exactly 3 parts");
        }

        return new vec3(float.Parse(array[0].Trim(), style, provider), float.Parse(array[1].Trim(), style, provider), float.Parse(array[2].Trim(), style, provider));
    }

    //
    // Summary:
    //     Tries to convert the string representation of the vector into a vector representation
    //     (using ', ' as a separator), returns false if string was invalid.
    public static bool TryParse(string s, out vec3 result)
    {
        return TryParse(s, ", ", out result);
    }

    //
    // Summary:
    //     Tries to convert the string representation of the vector into a vector representation
    //     (using a designated separator), returns false if string was invalid.
    public static bool TryParse(string s, string sep, out vec3 result)
    {
        result = Zero;
        if (string.IsNullOrEmpty(s))
        {
            return false;
        }

        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 3)
        {
            return false;
        }

        float result2 = 0f;
        float result3 = 0f;
        float result4 = 0f;
        bool flag = float.TryParse(array[0].Trim(), out result2) && float.TryParse(array[1].Trim(), out result3) && float.TryParse(array[2].Trim(), out result4);
        result = (flag ? new vec3(result2, result3, result4) : Zero);
        return flag;
    }

    //
    // Summary:
    //     Tries to convert the string representation of the vector into a vector representation
    //     (using a designated separator and a number style and a format provider), returns
    //     false if string was invalid.
    public static bool TryParse(string s, string sep, NumberStyles style, IFormatProvider provider, out vec3 result)
    {
        result = Zero;
        if (string.IsNullOrEmpty(s))
        {
            return false;
        }

        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 3)
        {
            return false;
        }

        float result2 = 0f;
        float result3 = 0f;
        float result4 = 0f;
        bool flag = float.TryParse(array[0].Trim(), style, provider, out result2) && float.TryParse(array[1].Trim(), style, provider, out result3) && float.TryParse(array[2].Trim(), style, provider, out result4);
        result = (flag ? new vec3(result2, result3, result4) : Zero);
        return flag;
    }

    //
    // Summary:
    //     Returns true iff distance between lhs and rhs is less than or equal to epsilon
    public static bool ApproxEqual(vec3 lhs, vec3 rhs, float eps = 0.1f)
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
    public static mat3 OuterProduct(vec3 c, vec3 r)
    {
        return new mat3(c.x * r.x, c.y * r.x, c.z * r.x, c.x * r.y, c.y * r.y, c.z * r.y, c.x * r.z, c.y * r.z, c.z * r.z);
    }

    //
    // Summary:
    //     Returns the inner product (dot product, scalar product) of the two vectors.
    public static float Dot(vec3 lhs, vec3 rhs)
    {
        return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
    }

    //
    // Summary:
    //     Returns the euclidean distance between the two vectors.
    public static float Distance(vec3 lhs, vec3 rhs)
    {
        return (lhs - rhs).Length;
    }

    //
    // Summary:
    //     Returns the squared euclidean distance between the two vectors.
    public static float DistanceSqr(vec3 lhs, vec3 rhs)
    {
        return (lhs - rhs).LengthSqr;
    }

    //
    // Summary:
    //     Calculate the reflection direction for an incident vector (N should be normalized
    //     in order to achieve the desired result).
    public static vec3 Reflect(vec3 I, vec3 N)
    {
        return I - 2f * Dot(N, I) * N;
    }

    //
    // Summary:
    //     Calculate the refraction direction for an incident vector (The input parameters
    //     I and N should be normalized in order to achieve the desired result).
    public static vec3 Refract(vec3 I, vec3 N, float eta)
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
    public static vec3 FaceForward(vec3 N, vec3 I, vec3 Nref)
    {
        if (!(Dot(Nref, I) < 0f))
        {
            return -N;
        }

        return N;
    }

    //
    // Summary:
    //     Returns the outer product (cross product, vector product) of the two vectors.
    public static vec3 Cross(vec3 l, vec3 r)
    {
        return new vec3(l.y * r.z - l.z * r.y, l.z * r.x - l.x * r.z, l.x * r.y - l.y * r.x);
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed uniform values between
    //     0.0 and 1.0.
    public static vec3 Random(Random random)
    {
        return new vec3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed uniform values between
    //     -1.0 and 1.0.
    public static vec3 RandomSigned(Random random)
    {
        return new vec3((float)(random.NextDouble() * 2.0 - 1.0), (float)(random.NextDouble() * 2.0 - 1.0), (float)(random.NextDouble() * 2.0 - 1.0));
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed values according
    //     to a normal distribution (zero mean, unit variance).
    public static vec3 RandomNormal(Random random)
    {
        return new vec3((float)(Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))), (float)(Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))), (float)(Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Abs (Math.Abs(v)).
    public static vec3 Abs(vec3 v)
    {
        return new vec3(Math.Abs(v.x), Math.Abs(v.y), Math.Abs(v.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Abs (Math.Abs(v)).
    public static vec3 Abs(float v)
    {
        return new vec3(Math.Abs(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of HermiteInterpolationOrder3
    //     ((3 - 2 * v) * v * v).
    public static vec3 HermiteInterpolationOrder3(vec3 v)
    {
        return new vec3((3f - 2f * v.x) * v.x * v.x, (3f - 2f * v.y) * v.y * v.y, (3f - 2f * v.z) * v.z * v.z);
    }

    //
    // Summary:
    //     Returns a vec from the application of HermiteInterpolationOrder3 ((3 - 2 * v)
    //     * v * v).
    public static vec3 HermiteInterpolationOrder3(float v)
    {
        return new vec3((3f - 2f * v) * v * v);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of HermiteInterpolationOrder5
    //     (((6 * v - 15) * v + 10) * v * v * v).
    public static vec3 HermiteInterpolationOrder5(vec3 v)
    {
        return new vec3(((6f * v.x - 15f) * v.x + 10f) * v.x * v.x * v.x, ((6f * v.y - 15f) * v.y + 10f) * v.y * v.y * v.y, ((6f * v.z - 15f) * v.z + 10f) * v.z * v.z * v.z);
    }

    //
    // Summary:
    //     Returns a vec from the application of HermiteInterpolationOrder5 (((6 * v - 15)
    //     * v + 10) * v * v * v).
    public static vec3 HermiteInterpolationOrder5(float v)
    {
        return new vec3(((6f * v - 15f) * v + 10f) * v * v * v);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Sqr (v * v).
    public static vec3 Sqr(vec3 v)
    {
        return new vec3(v.x * v.x, v.y * v.y, v.z * v.z);
    }

    //
    // Summary:
    //     Returns a vec from the application of Sqr (v * v).
    public static vec3 Sqr(float v)
    {
        return new vec3(v * v);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Pow2 (v * v).
    public static vec3 Pow2(vec3 v)
    {
        return new vec3(v.x * v.x, v.y * v.y, v.z * v.z);
    }

    //
    // Summary:
    //     Returns a vec from the application of Pow2 (v * v).
    public static vec3 Pow2(float v)
    {
        return new vec3(v * v);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Pow3 (v * v * v).
    public static vec3 Pow3(vec3 v)
    {
        return new vec3(v.x * v.x * v.x, v.y * v.y * v.y, v.z * v.z * v.z);
    }

    //
    // Summary:
    //     Returns a vec from the application of Pow3 (v * v * v).
    public static vec3 Pow3(float v)
    {
        return new vec3(v * v * v);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Step (v >= 0f ? 1f : 0f).
    public static vec3 Step(vec3 v)
    {
        return new vec3((v.x >= 0f) ? 1f : 0f, (v.y >= 0f) ? 1f : 0f, (v.z >= 0f) ? 1f : 0f);
    }

    //
    // Summary:
    //     Returns a vec from the application of Step (v >= 0f ? 1f : 0f).
    public static vec3 Step(float v)
    {
        return new vec3((v >= 0f) ? 1f : 0f);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Sqrt ((float)Math.Sqrt((double)v)).
    public static vec3 Sqrt(vec3 v)
    {
        return new vec3((float)Math.Sqrt(v.x), (float)Math.Sqrt(v.y), (float)Math.Sqrt(v.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Sqrt ((float)Math.Sqrt((double)v)).
    public static vec3 Sqrt(float v)
    {
        return new vec3((float)Math.Sqrt(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of InverseSqrt ((float)(1.0 /
    //     Math.Sqrt((double)v))).
    public static vec3 InverseSqrt(vec3 v)
    {
        return new vec3((float)(1.0 / Math.Sqrt(v.x)), (float)(1.0 / Math.Sqrt(v.y)), (float)(1.0 / Math.Sqrt(v.z)));
    }

    //
    // Summary:
    //     Returns a vec from the application of InverseSqrt ((float)(1.0 / Math.Sqrt((double)v))).
    public static vec3 InverseSqrt(float v)
    {
        return new vec3((float)(1.0 / Math.Sqrt(v)));
    }

    //
    // Summary:
    //     Returns a ivec3 from component-wise application of Sign (Math.Sign(v)).
    public static ivec3 Sign(vec3 v)
    {
        return new ivec3(Math.Sign(v.x), Math.Sign(v.y), Math.Sign(v.z));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Sign (Math.Sign(v)).
    public static ivec3 Sign(float v)
    {
        return new ivec3(Math.Sign(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Max (Math.Max(lhs, rhs)).
    public static vec3 Max(vec3 lhs, vec3 rhs)
    {
        return new vec3(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Max (Math.Max(lhs, rhs)).
    public static vec3 Max(vec3 lhs, float rhs)
    {
        return new vec3(Math.Max(lhs.x, rhs), Math.Max(lhs.y, rhs), Math.Max(lhs.z, rhs));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Max (Math.Max(lhs, rhs)).
    public static vec3 Max(float lhs, vec3 rhs)
    {
        return new vec3(Math.Max(lhs, rhs.x), Math.Max(lhs, rhs.y), Math.Max(lhs, rhs.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Max (Math.Max(lhs, rhs)).
    public static vec3 Max(float lhs, float rhs)
    {
        return new vec3(Math.Max(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Min (Math.Min(lhs, rhs)).
    public static vec3 Min(vec3 lhs, vec3 rhs)
    {
        return new vec3(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Min (Math.Min(lhs, rhs)).
    public static vec3 Min(vec3 lhs, float rhs)
    {
        return new vec3(Math.Min(lhs.x, rhs), Math.Min(lhs.y, rhs), Math.Min(lhs.z, rhs));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Min (Math.Min(lhs, rhs)).
    public static vec3 Min(float lhs, vec3 rhs)
    {
        return new vec3(Math.Min(lhs, rhs.x), Math.Min(lhs, rhs.y), Math.Min(lhs, rhs.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Min (Math.Min(lhs, rhs)).
    public static vec3 Min(float lhs, float rhs)
    {
        return new vec3(Math.Min(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Pow ((float)Math.Pow((double)lhs,
    //     (double)rhs)).
    public static vec3 Pow(vec3 lhs, vec3 rhs)
    {
        return new vec3((float)Math.Pow(lhs.x, rhs.x), (float)Math.Pow(lhs.y, rhs.y), (float)Math.Pow(lhs.z, rhs.z));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Pow ((float)Math.Pow((double)lhs,
    //     (double)rhs)).
    public static vec3 Pow(vec3 lhs, float rhs)
    {
        return new vec3((float)Math.Pow(lhs.x, rhs), (float)Math.Pow(lhs.y, rhs), (float)Math.Pow(lhs.z, rhs));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Pow ((float)Math.Pow((double)lhs,
    //     (double)rhs)).
    public static vec3 Pow(float lhs, vec3 rhs)
    {
        return new vec3((float)Math.Pow(lhs, rhs.x), (float)Math.Pow(lhs, rhs.y), (float)Math.Pow(lhs, rhs.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Pow ((float)Math.Pow((double)lhs, (double)rhs)).
    public static vec3 Pow(float lhs, float rhs)
    {
        return new vec3((float)Math.Pow(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Log ((float)Math.Log((double)lhs,
    //     (double)rhs)).
    public static vec3 Log(vec3 lhs, vec3 rhs)
    {
        return new vec3((float)Math.Log(lhs.x, rhs.x), (float)Math.Log(lhs.y, rhs.y), (float)Math.Log(lhs.z, rhs.z));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Log ((float)Math.Log((double)lhs,
    //     (double)rhs)).
    public static vec3 Log(vec3 lhs, float rhs)
    {
        return new vec3((float)Math.Log(lhs.x, rhs), (float)Math.Log(lhs.y, rhs), (float)Math.Log(lhs.z, rhs));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Log ((float)Math.Log((double)lhs,
    //     (double)rhs)).
    public static vec3 Log(float lhs, vec3 rhs)
    {
        return new vec3((float)Math.Log(lhs, rhs.x), (float)Math.Log(lhs, rhs.y), (float)Math.Log(lhs, rhs.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Log ((float)Math.Log((double)lhs, (double)rhs)).
    public static vec3 Log(float lhs, float rhs)
    {
        return new vec3((float)Math.Log(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec3 Clamp(vec3 v, vec3 min, vec3 max)
    {
        return new vec3(Math.Min(Math.Max(v.x, min.x), max.x), Math.Min(Math.Max(v.y, min.y), max.y), Math.Min(Math.Max(v.z, min.z), max.z));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec3 Clamp(vec3 v, vec3 min, float max)
    {
        return new vec3(Math.Min(Math.Max(v.x, min.x), max), Math.Min(Math.Max(v.y, min.y), max), Math.Min(Math.Max(v.z, min.z), max));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec3 Clamp(vec3 v, float min, vec3 max)
    {
        return new vec3(Math.Min(Math.Max(v.x, min), max.x), Math.Min(Math.Max(v.y, min), max.y), Math.Min(Math.Max(v.z, min), max.z));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec3 Clamp(vec3 v, float min, float max)
    {
        return new vec3(Math.Min(Math.Max(v.x, min), max), Math.Min(Math.Max(v.y, min), max), Math.Min(Math.Max(v.z, min), max));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec3 Clamp(float v, vec3 min, vec3 max)
    {
        return new vec3(Math.Min(Math.Max(v, min.x), max.x), Math.Min(Math.Max(v, min.y), max.y), Math.Min(Math.Max(v, min.z), max.z));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec3 Clamp(float v, vec3 min, float max)
    {
        return new vec3(Math.Min(Math.Max(v, min.x), max), Math.Min(Math.Max(v, min.y), max), Math.Min(Math.Max(v, min.z), max));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec3 Clamp(float v, float min, vec3 max)
    {
        return new vec3(Math.Min(Math.Max(v, min), max.x), Math.Min(Math.Max(v, min), max.y), Math.Min(Math.Max(v, min), max.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Clamp (Math.Min(Math.Max(v, min), max)).
    public static vec3 Clamp(float v, float min, float max)
    {
        return new vec3(Math.Min(Math.Max(v, min), max));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec3 Mix(vec3 min, vec3 max, vec3 a)
    {
        return new vec3(min.x * (1f - a.x) + max.x * a.x, min.y * (1f - a.y) + max.y * a.y, min.z * (1f - a.z) + max.z * a.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec3 Mix(vec3 min, vec3 max, float a)
    {
        return new vec3(min.x * (1f - a) + max.x * a, min.y * (1f - a) + max.y * a, min.z * (1f - a) + max.z * a);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec3 Mix(vec3 min, float max, vec3 a)
    {
        return new vec3(min.x * (1f - a.x) + max * a.x, min.y * (1f - a.y) + max * a.y, min.z * (1f - a.z) + max * a.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec3 Mix(vec3 min, float max, float a)
    {
        return new vec3(min.x * (1f - a) + max * a, min.y * (1f - a) + max * a, min.z * (1f - a) + max * a);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec3 Mix(float min, vec3 max, vec3 a)
    {
        return new vec3(min * (1f - a.x) + max.x * a.x, min * (1f - a.y) + max.y * a.y, min * (1f - a.z) + max.z * a.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec3 Mix(float min, vec3 max, float a)
    {
        return new vec3(min * (1f - a) + max.x * a, min * (1f - a) + max.y * a, min * (1f - a) + max.z * a);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec3 Mix(float min, float max, vec3 a)
    {
        return new vec3(min * (1f - a.x) + max * a.x, min * (1f - a.y) + max * a.y, min * (1f - a.z) + max * a.z);
    }

    //
    // Summary:
    //     Returns a vec from the application of Mix (min * (1-a) + max * a).
    public static vec3 Mix(float min, float max, float a)
    {
        return new vec3(min * (1f - a) + max * a);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec3 Lerp(vec3 min, vec3 max, vec3 a)
    {
        return new vec3(min.x * (1f - a.x) + max.x * a.x, min.y * (1f - a.y) + max.y * a.y, min.z * (1f - a.z) + max.z * a.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec3 Lerp(vec3 min, vec3 max, float a)
    {
        return new vec3(min.x * (1f - a) + max.x * a, min.y * (1f - a) + max.y * a, min.z * (1f - a) + max.z * a);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec3 Lerp(vec3 min, float max, vec3 a)
    {
        return new vec3(min.x * (1f - a.x) + max * a.x, min.y * (1f - a.y) + max * a.y, min.z * (1f - a.z) + max * a.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec3 Lerp(vec3 min, float max, float a)
    {
        return new vec3(min.x * (1f - a) + max * a, min.y * (1f - a) + max * a, min.z * (1f - a) + max * a);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec3 Lerp(float min, vec3 max, vec3 a)
    {
        return new vec3(min * (1f - a.x) + max.x * a.x, min * (1f - a.y) + max.y * a.y, min * (1f - a.z) + max.z * a.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec3 Lerp(float min, vec3 max, float a)
    {
        return new vec3(min * (1f - a) + max.x * a, min * (1f - a) + max.y * a, min * (1f - a) + max.z * a);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec3 Lerp(float min, float max, vec3 a)
    {
        return new vec3(min * (1f - a.x) + max * a.x, min * (1f - a.y) + max * a.y, min * (1f - a.z) + max * a.z);
    }

    //
    // Summary:
    //     Returns a vec from the application of Lerp (min * (1-a) + max * a).
    public static vec3 Lerp(float min, float max, float a)
    {
        return new vec3(min * (1f - a) + max * a);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Fma (a * b + c).
    public static vec3 Fma(vec3 a, vec3 b, vec3 c)
    {
        return new vec3(a.x * b.x + c.x, a.y * b.y + c.y, a.z * b.z + c.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Fma (a * b + c).
    public static vec3 Fma(vec3 a, vec3 b, float c)
    {
        return new vec3(a.x * b.x + c, a.y * b.y + c, a.z * b.z + c);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Fma (a * b + c).
    public static vec3 Fma(vec3 a, float b, vec3 c)
    {
        return new vec3(a.x * b + c.x, a.y * b + c.y, a.z * b + c.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Fma (a * b + c).
    public static vec3 Fma(vec3 a, float b, float c)
    {
        return new vec3(a.x * b + c, a.y * b + c, a.z * b + c);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Fma (a * b + c).
    public static vec3 Fma(float a, vec3 b, vec3 c)
    {
        return new vec3(a * b.x + c.x, a * b.y + c.y, a * b.z + c.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Fma (a * b + c).
    public static vec3 Fma(float a, vec3 b, float c)
    {
        return new vec3(a * b.x + c, a * b.y + c, a * b.z + c);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Fma (a * b + c).
    public static vec3 Fma(float a, float b, vec3 c)
    {
        return new vec3(a * b + c.x, a * b + c.y, a * b + c.z);
    }

    //
    // Summary:
    //     Returns a vec from the application of Fma (a * b + c).
    public static vec3 Fma(float a, float b, float c)
    {
        return new vec3(a * b + c);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Add (lhs + rhs).
    public static vec3 Add(vec3 lhs, vec3 rhs)
    {
        return new vec3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Add (lhs + rhs).
    public static vec3 Add(vec3 lhs, float rhs)
    {
        return new vec3(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Add (lhs + rhs).
    public static vec3 Add(float lhs, vec3 rhs)
    {
        return new vec3(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z);
    }

    //
    // Summary:
    //     Returns a vec from the application of Add (lhs + rhs).
    public static vec3 Add(float lhs, float rhs)
    {
        return new vec3(lhs + rhs);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Sub (lhs - rhs).
    public static vec3 Sub(vec3 lhs, vec3 rhs)
    {
        return new vec3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Sub (lhs - rhs).
    public static vec3 Sub(vec3 lhs, float rhs)
    {
        return new vec3(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Sub (lhs - rhs).
    public static vec3 Sub(float lhs, vec3 rhs)
    {
        return new vec3(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z);
    }

    //
    // Summary:
    //     Returns a vec from the application of Sub (lhs - rhs).
    public static vec3 Sub(float lhs, float rhs)
    {
        return new vec3(lhs - rhs);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Mul (lhs * rhs).
    public static vec3 Mul(vec3 lhs, vec3 rhs)
    {
        return new vec3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Mul (lhs * rhs).
    public static vec3 Mul(vec3 lhs, float rhs)
    {
        return new vec3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Mul (lhs * rhs).
    public static vec3 Mul(float lhs, vec3 rhs)
    {
        return new vec3(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
    }

    //
    // Summary:
    //     Returns a vec from the application of Mul (lhs * rhs).
    public static vec3 Mul(float lhs, float rhs)
    {
        return new vec3(lhs * rhs);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Div (lhs / rhs).
    public static vec3 Div(vec3 lhs, vec3 rhs)
    {
        return new vec3(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Div (lhs / rhs).
    public static vec3 Div(vec3 lhs, float rhs)
    {
        return new vec3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Div (lhs / rhs).
    public static vec3 Div(float lhs, vec3 rhs)
    {
        return new vec3(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z);
    }

    //
    // Summary:
    //     Returns a vec from the application of Div (lhs / rhs).
    public static vec3 Div(float lhs, float rhs)
    {
        return new vec3(lhs / rhs);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Modulo (lhs % rhs).
    public static vec3 Modulo(vec3 lhs, vec3 rhs)
    {
        return new vec3(lhs.x % rhs.x, lhs.y % rhs.y, lhs.z % rhs.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Modulo (lhs % rhs).
    public static vec3 Modulo(vec3 lhs, float rhs)
    {
        return new vec3(lhs.x % rhs, lhs.y % rhs, lhs.z % rhs);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Modulo (lhs % rhs).
    public static vec3 Modulo(float lhs, vec3 rhs)
    {
        return new vec3(lhs % rhs.x, lhs % rhs.y, lhs % rhs.z);
    }

    //
    // Summary:
    //     Returns a vec from the application of Modulo (lhs % rhs).
    public static vec3 Modulo(float lhs, float rhs)
    {
        return new vec3(lhs % rhs);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Degrees (Radians-To-Degrees
    //     Conversion).
    public static vec3 Degrees(vec3 v)
    {
        return new vec3(v.x * 57.29578f, v.y * 57.29578f, v.z * 57.29578f);
    }

    //
    // Summary:
    //     Returns a vec from the application of Degrees (Radians-To-Degrees Conversion).
    public static vec3 Degrees(float v)
    {
        return new vec3(v * 57.29578f);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Radians (Degrees-To-Radians
    //     Conversion).
    public static vec3 Radians(vec3 v)
    {
        return new vec3(v.x * ((float)Math.PI / 180f), v.y * ((float)Math.PI / 180f), v.z * ((float)Math.PI / 180f));
    }

    //
    // Summary:
    //     Returns a vec from the application of Radians (Degrees-To-Radians Conversion).
    public static vec3 Radians(float v)
    {
        return new vec3(v * ((float)Math.PI / 180f));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Acos ((float)Math.Acos((double)v)).
    public static vec3 Acos(vec3 v)
    {
        return new vec3((float)Math.Acos(v.x), (float)Math.Acos(v.y), (float)Math.Acos(v.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Acos ((float)Math.Acos((double)v)).
    public static vec3 Acos(float v)
    {
        return new vec3((float)Math.Acos(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Asin ((float)Math.Asin((double)v)).
    public static vec3 Asin(vec3 v)
    {
        return new vec3((float)Math.Asin(v.x), (float)Math.Asin(v.y), (float)Math.Asin(v.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Asin ((float)Math.Asin((double)v)).
    public static vec3 Asin(float v)
    {
        return new vec3((float)Math.Asin(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Atan ((float)Math.Atan((double)v)).
    public static vec3 Atan(vec3 v)
    {
        return new vec3((float)Math.Atan(v.x), (float)Math.Atan(v.y), (float)Math.Atan(v.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Atan ((float)Math.Atan((double)v)).
    public static vec3 Atan(float v)
    {
        return new vec3((float)Math.Atan(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Cos ((float)Math.Cos((double)v)).
    public static vec3 Cos(vec3 v)
    {
        return new vec3((float)Math.Cos(v.x), (float)Math.Cos(v.y), (float)Math.Cos(v.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Cos ((float)Math.Cos((double)v)).
    public static vec3 Cos(float v)
    {
        return new vec3((float)Math.Cos(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Cosh ((float)Math.Cosh((double)v)).
    public static vec3 Cosh(vec3 v)
    {
        return new vec3((float)Math.Cosh(v.x), (float)Math.Cosh(v.y), (float)Math.Cosh(v.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Cosh ((float)Math.Cosh((double)v)).
    public static vec3 Cosh(float v)
    {
        return new vec3((float)Math.Cosh(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Exp ((float)Math.Exp((double)v)).
    public static vec3 Exp(vec3 v)
    {
        return new vec3((float)Math.Exp(v.x), (float)Math.Exp(v.y), (float)Math.Exp(v.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Exp ((float)Math.Exp((double)v)).
    public static vec3 Exp(float v)
    {
        return new vec3((float)Math.Exp(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Log ((float)Math.Log((double)v)).
    public static vec3 Log(vec3 v)
    {
        return new vec3((float)Math.Log(v.x), (float)Math.Log(v.y), (float)Math.Log(v.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Log ((float)Math.Log((double)v)).
    public static vec3 Log(float v)
    {
        return new vec3((float)Math.Log(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Log2 ((float)Math.Log((double)v,
    //     2)).
    public static vec3 Log2(vec3 v)
    {
        return new vec3((float)Math.Log(v.x, 2.0), (float)Math.Log(v.y, 2.0), (float)Math.Log(v.z, 2.0));
    }

    //
    // Summary:
    //     Returns a vec from the application of Log2 ((float)Math.Log((double)v, 2)).
    public static vec3 Log2(float v)
    {
        return new vec3((float)Math.Log(v, 2.0));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Log10 ((float)Math.Log10((double)v)).
    public static vec3 Log10(vec3 v)
    {
        return new vec3((float)Math.Log10(v.x), (float)Math.Log10(v.y), (float)Math.Log10(v.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Log10 ((float)Math.Log10((double)v)).
    public static vec3 Log10(float v)
    {
        return new vec3((float)Math.Log10(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Floor ((float)Math.Floor(v)).
    public static vec3 Floor(vec3 v)
    {
        return new vec3((float)Math.Floor(v.x), (float)Math.Floor(v.y), (float)Math.Floor(v.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Floor ((float)Math.Floor(v)).
    public static vec3 Floor(float v)
    {
        return new vec3((float)Math.Floor(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Ceiling ((float)Math.Ceiling(v)).
    public static vec3 Ceiling(vec3 v)
    {
        return new vec3((float)Math.Ceiling(v.x), (float)Math.Ceiling(v.y), (float)Math.Ceiling(v.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Ceiling ((float)Math.Ceiling(v)).
    public static vec3 Ceiling(float v)
    {
        return new vec3((float)Math.Ceiling(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Round ((float)Math.Round(v)).
    public static vec3 Round(vec3 v)
    {
        return new vec3((float)Math.Round(v.x), (float)Math.Round(v.y), (float)Math.Round(v.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Round ((float)Math.Round(v)).
    public static vec3 Round(float v)
    {
        return new vec3((float)Math.Round(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Sin ((float)Math.Sin((double)v)).
    public static vec3 Sin(vec3 v)
    {
        return new vec3((float)Math.Sin(v.x), (float)Math.Sin(v.y), (float)Math.Sin(v.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Sin ((float)Math.Sin((double)v)).
    public static vec3 Sin(float v)
    {
        return new vec3((float)Math.Sin(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Sinh ((float)Math.Sinh((double)v)).
    public static vec3 Sinh(vec3 v)
    {
        return new vec3((float)Math.Sinh(v.x), (float)Math.Sinh(v.y), (float)Math.Sinh(v.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Sinh ((float)Math.Sinh((double)v)).
    public static vec3 Sinh(float v)
    {
        return new vec3((float)Math.Sinh(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Tan ((float)Math.Tan((double)v)).
    public static vec3 Tan(vec3 v)
    {
        return new vec3((float)Math.Tan(v.x), (float)Math.Tan(v.y), (float)Math.Tan(v.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Tan ((float)Math.Tan((double)v)).
    public static vec3 Tan(float v)
    {
        return new vec3((float)Math.Tan(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Tanh ((float)Math.Tanh((double)v)).
    public static vec3 Tanh(vec3 v)
    {
        return new vec3((float)Math.Tanh(v.x), (float)Math.Tanh(v.y), (float)Math.Tanh(v.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Tanh ((float)Math.Tanh((double)v)).
    public static vec3 Tanh(float v)
    {
        return new vec3((float)Math.Tanh(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Truncate ((float)Math.Truncate((double)v)).
    public static vec3 Truncate(vec3 v)
    {
        return new vec3((float)Math.Truncate(v.x), (float)Math.Truncate(v.y), (float)Math.Truncate(v.z));
    }

    //
    // Summary:
    //     Returns a vec from the application of Truncate ((float)Math.Truncate((double)v)).
    public static vec3 Truncate(float v)
    {
        return new vec3((float)Math.Truncate(v));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Fract ((float)(v - Math.Floor(v))).
    public static vec3 Fract(vec3 v)
    {
        return new vec3((float)((double)v.x - Math.Floor(v.x)), (float)((double)v.y - Math.Floor(v.y)), (float)((double)v.z - Math.Floor(v.z)));
    }

    //
    // Summary:
    //     Returns a vec from the application of Fract ((float)(v - Math.Floor(v))).
    public static vec3 Fract(float v)
    {
        return new vec3((float)((double)v - Math.Floor(v)));
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of Trunc ((long)(v)).
    public static vec3 Trunc(vec3 v)
    {
        return new vec3((long)v.x, (long)v.y, (long)v.z);
    }

    //
    // Summary:
    //     Returns a vec from the application of Trunc ((long)(v)).
    public static vec3 Trunc(float v)
    {
        return new vec3((long)v);
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec3 Random(Random random, vec3 minValue, vec3 maxValue)
    {
        return new vec3((float)random.NextDouble() * (maxValue.x - minValue.x) + minValue.x, (float)random.NextDouble() * (maxValue.y - minValue.y) + minValue.y, (float)random.NextDouble() * (maxValue.z - minValue.z) + minValue.z);
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec3 Random(Random random, vec3 minValue, float maxValue)
    {
        return new vec3((float)random.NextDouble() * (maxValue - minValue.x) + minValue.x, (float)random.NextDouble() * (maxValue - minValue.y) + minValue.y, (float)random.NextDouble() * (maxValue - minValue.z) + minValue.z);
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec3 Random(Random random, float minValue, vec3 maxValue)
    {
        return new vec3((float)random.NextDouble() * (maxValue.x - minValue) + minValue, (float)random.NextDouble() * (maxValue.y - minValue) + minValue, (float)random.NextDouble() * (maxValue.z - minValue) + minValue);
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec3 Random(Random random, float minValue, float maxValue)
    {
        return new vec3((float)random.NextDouble() * (maxValue - minValue) + minValue);
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec3 RandomUniform(Random random, vec3 minValue, vec3 maxValue)
    {
        return new vec3((float)random.NextDouble() * (maxValue.x - minValue.x) + minValue.x, (float)random.NextDouble() * (maxValue.y - minValue.y) + minValue.y, (float)random.NextDouble() * (maxValue.z - minValue.z) + minValue.z);
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec3 RandomUniform(Random random, vec3 minValue, float maxValue)
    {
        return new vec3((float)random.NextDouble() * (maxValue - minValue.x) + minValue.x, (float)random.NextDouble() * (maxValue - minValue.y) + minValue.y, (float)random.NextDouble() * (maxValue - minValue.z) + minValue.z);
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec3 RandomUniform(Random random, float minValue, vec3 maxValue)
    {
        return new vec3((float)random.NextDouble() * (maxValue.x - minValue) + minValue, (float)random.NextDouble() * (maxValue.y - minValue) + minValue, (float)random.NextDouble() * (maxValue.z - minValue) + minValue);
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec3 RandomUniform(Random random, float minValue, float maxValue)
    {
        return new vec3((float)random.NextDouble() * (maxValue - minValue) + minValue);
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec3 RandomNormal(Random random, vec3 mean, vec3 variance)
    {
        return new vec3((float)(Math.Sqrt(variance.x) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.x, (float)(Math.Sqrt(variance.y) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.y, (float)(Math.Sqrt(variance.z) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.z);
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec3 RandomNormal(Random random, vec3 mean, float variance)
    {
        return new vec3((float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.x, (float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.y, (float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.z);
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec3 RandomNormal(Random random, float mean, vec3 variance)
    {
        return new vec3((float)(Math.Sqrt(variance.x) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean, (float)(Math.Sqrt(variance.y) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean, (float)(Math.Sqrt(variance.z) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean);
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec3 RandomNormal(Random random, float mean, float variance)
    {
        return new vec3((float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean);
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec3 RandomGaussian(Random random, vec3 mean, vec3 variance)
    {
        return new vec3((float)(Math.Sqrt(variance.x) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.x, (float)(Math.Sqrt(variance.y) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.y, (float)(Math.Sqrt(variance.z) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.z);
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec3 RandomGaussian(Random random, vec3 mean, float variance)
    {
        return new vec3((float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.x, (float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.y, (float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.z);
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec3 RandomGaussian(Random random, float mean, vec3 variance)
    {
        return new vec3((float)(Math.Sqrt(variance.x) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean, (float)(Math.Sqrt(variance.y) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean, (float)(Math.Sqrt(variance.z) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean);
    }

    //
    // Summary:
    //     Returns a vec3 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec3 RandomGaussian(Random random, float mean, float variance)
    {
        return new vec3((float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of operator+ (lhs + rhs).
    public static vec3 operator +(vec3 lhs, vec3 rhs)
    {
        return new vec3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of operator+ (lhs + rhs).
    public static vec3 operator +(vec3 lhs, float rhs)
    {
        return new vec3(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of operator+ (lhs + rhs).
    public static vec3 operator +(float lhs, vec3 rhs)
    {
        return new vec3(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of operator- (lhs - rhs).
    public static vec3 operator -(vec3 lhs, vec3 rhs)
    {
        return new vec3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of operator- (lhs - rhs).
    public static vec3 operator -(vec3 lhs, float rhs)
    {
        return new vec3(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of operator- (lhs - rhs).
    public static vec3 operator -(float lhs, vec3 rhs)
    {
        return new vec3(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of operator* (lhs * rhs).
    public static vec3 operator *(vec3 lhs, vec3 rhs)
    {
        return new vec3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of operator* (lhs * rhs).
    public static vec3 operator *(vec3 lhs, float rhs)
    {
        return new vec3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of operator* (lhs * rhs).
    public static vec3 operator *(float lhs, vec3 rhs)
    {
        return new vec3(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of operator/ (lhs / rhs).
    public static vec3 operator /(vec3 lhs, vec3 rhs)
    {
        return new vec3(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of operator/ (lhs / rhs).
    public static vec3 operator /(vec3 lhs, float rhs)
    {
        return new vec3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of operator/ (lhs / rhs).
    public static vec3 operator /(float lhs, vec3 rhs)
    {
        return new vec3(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of operator+ (identity).
    public static vec3 operator +(vec3 v)
    {
        return v;
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of operator- (-v).
    public static vec3 operator -(vec3 v)
    {
        return new vec3(0f - v.x, 0f - v.y, 0f - v.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of operator% (lhs % rhs).
    public static vec3 operator %(vec3 lhs, vec3 rhs)
    {
        return new vec3(lhs.x % rhs.x, lhs.y % rhs.y, lhs.z % rhs.z);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of operator% (lhs % rhs).
    public static vec3 operator %(vec3 lhs, float rhs)
    {
        return new vec3(lhs.x % rhs, lhs.y % rhs, lhs.z % rhs);
    }

    //
    // Summary:
    //     Returns a vec3 from component-wise application of operator% (lhs % rhs).
    public static vec3 operator %(float lhs, vec3 rhs)
    {
        return new vec3(lhs % rhs.x, lhs % rhs.y, lhs % rhs.z);
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
