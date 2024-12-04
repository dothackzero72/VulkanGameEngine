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
//     A vector of type float with 2 components.
[Serializable]
[DataContract(Namespace = "vec")]
public struct vec2 : IReadOnlyList<float>, IReadOnlyCollection<float>, IEnumerable<float>, IEnumerable, IEquatable<vec2>
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
    //     Gets/Sets a specific indexed component (a bit slower than direct access).
    public float this[int index]
    {
        get
        {
            return index switch
            {
                0 => x,
                1 => y,
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
    //     Returns an array with all values
    public float[] Values => new float[2] { x, y };

    //
    // Summary:
    //     Returns the number of components (2).
    public int Count => 2;

    //
    // Summary:
    //     Returns the minimal component of this vector.
    public float MinElement => Math.Min(x, y);

    //
    // Summary:
    //     Returns the maximal component of this vector.
    public float MaxElement => Math.Max(x, y);

    //
    // Summary:
    //     Returns the euclidean length of this vector.
    public float Length => (float)Math.Sqrt(x * x + y * y);

    //
    // Summary:
    //     Returns the squared euclidean length of this vector.
    public float LengthSqr => x * x + y * y;

    //
    // Summary:
    //     Returns the sum of all components.
    public float Sum => x + y;

    //
    // Summary:
    //     Returns the euclidean norm of this vector.
    public float Norm => (float)Math.Sqrt(x * x + y * y);

    //
    // Summary:
    //     Returns the one-norm of this vector.
    public float Norm1 => Math.Abs(x) + Math.Abs(y);

    //
    // Summary:
    //     Returns the two-norm (euclidean length) of this vector.
    public float Norm2 => (float)Math.Sqrt(x * x + y * y);

    //
    // Summary:
    //     Returns the max-norm of this vector.
    public float NormMax => Math.Max(Math.Abs(x), Math.Abs(y));

    //
    // Summary:
    //     Returns a copy of this vector with length one (undefined if this has zero length).
    public vec2 Normalized => this / Length;

    //
    // Summary:
    //     Returns a copy of this vector with length one (returns zero if length is zero).
    public vec2 NormalizedSafe
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
    //     Returns the vector angle (atan2(y, x)) in radians.
    public double Angle => Math.Atan2(y, x);

    //
    // Summary:
    //     Predefined all-zero vector
    public static vec2 Zero { get; } = new vec2(0f, 0f);


    //
    // Summary:
    //     Predefined all-ones vector
    public static vec2 Ones { get; } = new vec2(1f, 1f);


    //
    // Summary:
    //     Predefined unit-X vector
    public static vec2 UnitX { get; } = new vec2(1f, 0f);


    //
    // Summary:
    //     Predefined unit-Y vector
    public static vec2 UnitY { get; } = new vec2(0f, 1f);


    //
    // Summary:
    //     Predefined all-MaxValue vector
    public static vec2 MaxValue { get; } = new vec2(float.MaxValue, float.MaxValue);


    //
    // Summary:
    //     Predefined all-MinValue vector
    public static vec2 MinValue { get; } = new vec2(float.MinValue, float.MinValue);


    //
    // Summary:
    //     Predefined all-Epsilon vector
    public static vec2 Epsilon { get; } = new vec2(float.Epsilon, float.Epsilon);


    //
    // Summary:
    //     Predefined all-NaN vector
    public static vec2 NaN { get; } = new vec2(float.NaN, float.NaN);


    //
    // Summary:
    //     Predefined all-NegativeInfinity vector
    public static vec2 NegativeInfinity { get; } = new vec2(float.NegativeInfinity, float.NegativeInfinity);


    //
    // Summary:
    //     Predefined all-PositiveInfinity vector
    public static vec2 PositiveInfinity { get; } = new vec2(float.PositiveInfinity, float.PositiveInfinity);


    //
    // Summary:
    //     Component-wise constructor
    public vec2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    //
    // Summary:
    //     all-same-value constructor
    public vec2(float v)
    {
        x = v;
        y = v;
    }

    //
    // Summary:
    //     from-vector constructor
    public vec2(vec2 v)
    {
        x = v.x;
        y = v.y;
    }

    //
    // Summary:
    //     from-vector constructor (additional fields are truncated)
    public vec2(vec3 v)
    {
        x = v.x;
        y = v.y;
    }

    //
    // Summary:
    //     from-vector constructor (additional fields are truncated)
    public vec2(vec4 v)
    {
        x = v.x;
        y = v.y;
    }

    //
    // Summary:
    //     From-array/list constructor (superfluous values are ignored, missing values are
    //     zero-filled).
    public vec2(IReadOnlyList<float> v)
    {
        int count = v.Count;
        x = ((count < 0) ? 0f : v[0]);
        y = ((count < 1) ? 0f : v[1]);
    }

    //
    // Summary:
    //     Generic from-array constructor (superfluous values are ignored, missing values
    //     are zero-filled).
    public vec2(object[] v)
    {
        int num = v.Length;
        x = ((num < 0) ? 0f : ((float)v[0]));
        y = ((num < 1) ? 0f : ((float)v[1]));
    }

    //
    // Summary:
    //     From-array constructor (superfluous values are ignored, missing values are zero-filled).
    public vec2(float[] v)
    {
        int num = v.Length;
        x = ((num < 0) ? 0f : v[0]);
        y = ((num < 1) ? 0f : v[1]);
    }

    //
    // Summary:
    //     From-array constructor with base index (superfluous values are ignored, missing
    //     values are zero-filled).
    public vec2(float[] v, int startIndex)
    {
        int num = v.Length;
        x = ((num + startIndex < 0) ? 0f : v[startIndex]);
        y = ((num + startIndex < 1) ? 0f : v[1 + startIndex]);
    }

    //
    // Summary:
    //     From-IEnumerable constructor (superfluous values are ignored, missing values
    //     are zero-filled).
    public vec2(IEnumerable<float> v)
        : this(v.ToArray())
    {
    }

    //
    // Summary:
    //     Explicitly converts this to a ivec2.
    public static explicit operator ivec2(vec2 v)
    {
        return new ivec2((int)v.x, (int)v.y);
    }

    //
    // Summary:
    //     Explicitly converts this to a ivec3. (Higher components are zeroed)
    public static explicit operator ivec3(vec2 v)
    {
        return new ivec3((int)v.x, (int)v.y, 0);
    }

    //
    // Summary:
    //     Explicitly converts this to a ivec4. (Higher components are zeroed)
    public static explicit operator ivec4(vec2 v)
    {
        return new ivec4((int)v.x, (int)v.y, 0, 0);
    }

    //
    // Summary:
    //     Explicitly converts this to a vec4. (Higher components are zeroed)
    public static explicit operator vec4(vec2 v)
    {
        return new vec4(v.x, v.y, 0f, 0f);
    }

    //
    // Summary:
    //     Explicitly converts this to a float array.
    public static explicit operator float[](vec2 v)
    {
        return new float[2] { v.x, v.y };
    }

    //
    // Summary:
    //     Explicitly converts this to a generic object array.
    public static explicit operator object[](vec2 v)
    {
        return new object[2] { v.x, v.y };
    }

    //
    // Summary:
    //     Returns true iff this equals rhs component-wise.
    public static bool operator ==(vec2 lhs, vec2 rhs)
    {
        return lhs.Equals(rhs);
    }

    //
    // Summary:
    //     Returns true iff this does not equal rhs (component-wise).
    public static bool operator !=(vec2 lhs, vec2 rhs)
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
        return x + sep + y;
    }

    //
    // Summary:
    //     Returns a string representation of this vector using a provided seperator and
    //     a format provider for each component.
    public string ToString(string sep, IFormatProvider provider)
    {
        return x.ToString(provider) + sep + y.ToString(provider);
    }

    //
    // Summary:
    //     Returns a string representation of this vector using a provided seperator and
    //     a format for each component.
    public string ToString(string sep, string format)
    {
        return x.ToString(format) + sep + y.ToString(format);
    }

    //
    // Summary:
    //     Returns a string representation of this vector using a provided seperator and
    //     a format and format provider for each component.
    public string ToString(string sep, string format, IFormatProvider provider)
    {
        return x.ToString(format, provider) + sep + y.ToString(format, provider);
    }

    //
    // Summary:
    //     Returns true iff this equals rhs component-wise.
    public bool Equals(vec2 rhs)
    {
        if (x.Equals(rhs.x))
        {
            return y.Equals(rhs.y);
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

        if (obj is vec2)
        {
            return Equals((vec2)obj);
        }

        return false;
    }

    //
    // Summary:
    //     Returns a hash code for this instance.
    public override int GetHashCode()
    {
        return (x.GetHashCode() * 397) ^ y.GetHashCode();
    }

    //
    // Summary:
    //     Returns the p-norm of this vector.
    public double NormP(double p)
    {
        return Math.Pow(Math.Pow(Math.Abs(x), p) + Math.Pow(Math.Abs(y), p), 1.0 / p);
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using ', ' as a separator).
    public static vec2 Parse(string s)
    {
        return Parse(s, ", ");
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator).
    public static vec2 Parse(string s, string sep)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 2)
        {
            throw new FormatException("input has not exactly 2 parts");
        }

        return new vec2(float.Parse(array[0].Trim()), float.Parse(array[1].Trim()));
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator and a type provider).
    public static vec2 Parse(string s, string sep, IFormatProvider provider)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 2)
        {
            throw new FormatException("input has not exactly 2 parts");
        }

        return new vec2(float.Parse(array[0].Trim(), provider), float.Parse(array[1].Trim(), provider));
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator and a number style).
    public static vec2 Parse(string s, string sep, NumberStyles style)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 2)
        {
            throw new FormatException("input has not exactly 2 parts");
        }

        return new vec2(float.Parse(array[0].Trim(), style), float.Parse(array[1].Trim(), style));
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator and a number style and a format provider).
    public static vec2 Parse(string s, string sep, NumberStyles style, IFormatProvider provider)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 2)
        {
            throw new FormatException("input has not exactly 2 parts");
        }

        return new vec2(float.Parse(array[0].Trim(), style, provider), float.Parse(array[1].Trim(), style, provider));
    }

    //
    // Summary:
    //     Tries to convert the string representation of the vector into a vector representation
    //     (using ', ' as a separator), returns false if string was invalid.
    public static bool TryParse(string s, out vec2 result)
    {
        return TryParse(s, ", ", out result);
    }

    //
    // Summary:
    //     Tries to convert the string representation of the vector into a vector representation
    //     (using a designated separator), returns false if string was invalid.
    public static bool TryParse(string s, string sep, out vec2 result)
    {
        result = Zero;
        if (string.IsNullOrEmpty(s))
        {
            return false;
        }

        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 2)
        {
            return false;
        }

        float result2 = 0f;
        float result3 = 0f;
        bool flag = float.TryParse(array[0].Trim(), out result2) && float.TryParse(array[1].Trim(), out result3);
        result = (flag ? new vec2(result2, result3) : Zero);
        return flag;
    }

    //
    // Summary:
    //     Tries to convert the string representation of the vector into a vector representation
    //     (using a designated separator and a number style and a format provider), returns
    //     false if string was invalid.
    public static bool TryParse(string s, string sep, NumberStyles style, IFormatProvider provider, out vec2 result)
    {
        result = Zero;
        if (string.IsNullOrEmpty(s))
        {
            return false;
        }

        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 2)
        {
            return false;
        }

        float result2 = 0f;
        float result3 = 0f;
        bool flag = float.TryParse(array[0].Trim(), style, provider, out result2) && float.TryParse(array[1].Trim(), style, provider, out result3);
        result = (flag ? new vec2(result2, result3) : Zero);
        return flag;
    }

    //
    // Summary:
    //     Returns true iff distance between lhs and rhs is less than or equal to epsilon
    public static bool ApproxEqual(vec2 lhs, vec2 rhs, float eps = 0.1f)
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
    public static mat2 OuterProduct(vec2 c, vec2 r)
    {
        return new mat2(c.x * r.x, c.y * r.x, c.x * r.y, c.y * r.y);
    }

    //
    // Summary:
    //     Returns a unit 2D vector with a given angle in radians (CAUTION: result may be
    //     truncated for integer types).
    public static vec2 FromAngle(double angleInRad)
    {
        return new vec2((float)Math.Cos(angleInRad), (float)Math.Sin(angleInRad));
    }

    //
    // Summary:
    //     Returns the inner product (dot product, scalar product) of the two vectors.
    public static float Dot(vec2 lhs, vec2 rhs)
    {
        return lhs.x * rhs.x + lhs.y * rhs.y;
    }

    //
    // Summary:
    //     Returns the euclidean distance between the two vectors.
    public static float Distance(vec2 lhs, vec2 rhs)
    {
        return (lhs - rhs).Length;
    }

    //
    // Summary:
    //     Returns the squared euclidean distance between the two vectors.
    public static float DistanceSqr(vec2 lhs, vec2 rhs)
    {
        return (lhs - rhs).LengthSqr;
    }

    //
    // Summary:
    //     Calculate the reflection direction for an incident vector (N should be normalized
    //     in order to achieve the desired result).
    public static vec2 Reflect(vec2 I, vec2 N)
    {
        return I - 2f * Dot(N, I) * N;
    }

    //
    // Summary:
    //     Calculate the refraction direction for an incident vector (The input parameters
    //     I and N should be normalized in order to achieve the desired result).
    public static vec2 Refract(vec2 I, vec2 N, float eta)
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
    public static vec2 FaceForward(vec2 N, vec2 I, vec2 Nref)
    {
        if (!(Dot(Nref, I) < 0f))
        {
            return -N;
        }

        return N;
    }

    //
    // Summary:
    //     Returns the length of the outer product (cross product, vector product) of the
    //     two vectors.
    public static float Cross(vec2 l, vec2 r)
    {
        return l.x * r.y - l.y * r.x;
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed uniform values between
    //     0.0 and 1.0.
    public static vec2 Random(Random random)
    {
        return new vec2((float)random.NextDouble(), (float)random.NextDouble());
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed uniform values between
    //     -1.0 and 1.0.
    public static vec2 RandomSigned(Random random)
    {
        return new vec2((float)(random.NextDouble() * 2.0 - 1.0), (float)(random.NextDouble() * 2.0 - 1.0));
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed values according
    //     to a normal distribution (zero mean, unit variance).
    public static vec2 RandomNormal(Random random)
    {
        return new vec2((float)(Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))), (float)(Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Abs (Math.Abs(v)).
    public static vec2 Abs(vec2 v)
    {
        return new vec2(Math.Abs(v.x), Math.Abs(v.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Abs (Math.Abs(v)).
    public static vec2 Abs(float v)
    {
        return new vec2(Math.Abs(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of HermiteInterpolationOrder3
    //     ((3 - 2 * v) * v * v).
    public static vec2 HermiteInterpolationOrder3(vec2 v)
    {
        return new vec2((3f - 2f * v.x) * v.x * v.x, (3f - 2f * v.y) * v.y * v.y);
    }

    //
    // Summary:
    //     Returns a vec from the application of HermiteInterpolationOrder3 ((3 - 2 * v)
    //     * v * v).
    public static vec2 HermiteInterpolationOrder3(float v)
    {
        return new vec2((3f - 2f * v) * v * v);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of HermiteInterpolationOrder5
    //     (((6 * v - 15) * v + 10) * v * v * v).
    public static vec2 HermiteInterpolationOrder5(vec2 v)
    {
        return new vec2(((6f * v.x - 15f) * v.x + 10f) * v.x * v.x * v.x, ((6f * v.y - 15f) * v.y + 10f) * v.y * v.y * v.y);
    }

    //
    // Summary:
    //     Returns a vec from the application of HermiteInterpolationOrder5 (((6 * v - 15)
    //     * v + 10) * v * v * v).
    public static vec2 HermiteInterpolationOrder5(float v)
    {
        return new vec2(((6f * v - 15f) * v + 10f) * v * v * v);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Sqr (v * v).
    public static vec2 Sqr(vec2 v)
    {
        return new vec2(v.x * v.x, v.y * v.y);
    }

    //
    // Summary:
    //     Returns a vec from the application of Sqr (v * v).
    public static vec2 Sqr(float v)
    {
        return new vec2(v * v);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Pow2 (v * v).
    public static vec2 Pow2(vec2 v)
    {
        return new vec2(v.x * v.x, v.y * v.y);
    }

    //
    // Summary:
    //     Returns a vec from the application of Pow2 (v * v).
    public static vec2 Pow2(float v)
    {
        return new vec2(v * v);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Pow3 (v * v * v).
    public static vec2 Pow3(vec2 v)
    {
        return new vec2(v.x * v.x * v.x, v.y * v.y * v.y);
    }

    //
    // Summary:
    //     Returns a vec from the application of Pow3 (v * v * v).
    public static vec2 Pow3(float v)
    {
        return new vec2(v * v * v);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Step (v >= 0f ? 1f : 0f).
    public static vec2 Step(vec2 v)
    {
        return new vec2((v.x >= 0f) ? 1f : 0f, (v.y >= 0f) ? 1f : 0f);
    }

    //
    // Summary:
    //     Returns a vec from the application of Step (v >= 0f ? 1f : 0f).
    public static vec2 Step(float v)
    {
        return new vec2((v >= 0f) ? 1f : 0f);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Sqrt ((float)Math.Sqrt((double)v)).
    public static vec2 Sqrt(vec2 v)
    {
        return new vec2((float)Math.Sqrt(v.x), (float)Math.Sqrt(v.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Sqrt ((float)Math.Sqrt((double)v)).
    public static vec2 Sqrt(float v)
    {
        return new vec2((float)Math.Sqrt(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of InverseSqrt ((float)(1.0 /
    //     Math.Sqrt((double)v))).
    public static vec2 InverseSqrt(vec2 v)
    {
        return new vec2((float)(1.0 / Math.Sqrt(v.x)), (float)(1.0 / Math.Sqrt(v.y)));
    }

    //
    // Summary:
    //     Returns a vec from the application of InverseSqrt ((float)(1.0 / Math.Sqrt((double)v))).
    public static vec2 InverseSqrt(float v)
    {
        return new vec2((float)(1.0 / Math.Sqrt(v)));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Sign (Math.Sign(v)).
    public static ivec2 Sign(vec2 v)
    {
        return new ivec2(Math.Sign(v.x), Math.Sign(v.y));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Sign (Math.Sign(v)).
    public static ivec2 Sign(float v)
    {
        return new ivec2(Math.Sign(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Max (Math.Max(lhs, rhs)).
    public static vec2 Max(vec2 lhs, vec2 rhs)
    {
        return new vec2(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Max (Math.Max(lhs, rhs)).
    public static vec2 Max(vec2 lhs, float rhs)
    {
        return new vec2(Math.Max(lhs.x, rhs), Math.Max(lhs.y, rhs));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Max (Math.Max(lhs, rhs)).
    public static vec2 Max(float lhs, vec2 rhs)
    {
        return new vec2(Math.Max(lhs, rhs.x), Math.Max(lhs, rhs.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Max (Math.Max(lhs, rhs)).
    public static vec2 Max(float lhs, float rhs)
    {
        return new vec2(Math.Max(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Min (Math.Min(lhs, rhs)).
    public static vec2 Min(vec2 lhs, vec2 rhs)
    {
        return new vec2(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Min (Math.Min(lhs, rhs)).
    public static vec2 Min(vec2 lhs, float rhs)
    {
        return new vec2(Math.Min(lhs.x, rhs), Math.Min(lhs.y, rhs));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Min (Math.Min(lhs, rhs)).
    public static vec2 Min(float lhs, vec2 rhs)
    {
        return new vec2(Math.Min(lhs, rhs.x), Math.Min(lhs, rhs.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Min (Math.Min(lhs, rhs)).
    public static vec2 Min(float lhs, float rhs)
    {
        return new vec2(Math.Min(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Pow ((float)Math.Pow((double)lhs,
    //     (double)rhs)).
    public static vec2 Pow(vec2 lhs, vec2 rhs)
    {
        return new vec2((float)Math.Pow(lhs.x, rhs.x), (float)Math.Pow(lhs.y, rhs.y));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Pow ((float)Math.Pow((double)lhs,
    //     (double)rhs)).
    public static vec2 Pow(vec2 lhs, float rhs)
    {
        return new vec2((float)Math.Pow(lhs.x, rhs), (float)Math.Pow(lhs.y, rhs));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Pow ((float)Math.Pow((double)lhs,
    //     (double)rhs)).
    public static vec2 Pow(float lhs, vec2 rhs)
    {
        return new vec2((float)Math.Pow(lhs, rhs.x), (float)Math.Pow(lhs, rhs.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Pow ((float)Math.Pow((double)lhs, (double)rhs)).
    public static vec2 Pow(float lhs, float rhs)
    {
        return new vec2((float)Math.Pow(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Log ((float)Math.Log((double)lhs,
    //     (double)rhs)).
    public static vec2 Log(vec2 lhs, vec2 rhs)
    {
        return new vec2((float)Math.Log(lhs.x, rhs.x), (float)Math.Log(lhs.y, rhs.y));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Log ((float)Math.Log((double)lhs,
    //     (double)rhs)).
    public static vec2 Log(vec2 lhs, float rhs)
    {
        return new vec2((float)Math.Log(lhs.x, rhs), (float)Math.Log(lhs.y, rhs));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Log ((float)Math.Log((double)lhs,
    //     (double)rhs)).
    public static vec2 Log(float lhs, vec2 rhs)
    {
        return new vec2((float)Math.Log(lhs, rhs.x), (float)Math.Log(lhs, rhs.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Log ((float)Math.Log((double)lhs, (double)rhs)).
    public static vec2 Log(float lhs, float rhs)
    {
        return new vec2((float)Math.Log(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec2 Clamp(vec2 v, vec2 min, vec2 max)
    {
        return new vec2(Math.Min(Math.Max(v.x, min.x), max.x), Math.Min(Math.Max(v.y, min.y), max.y));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec2 Clamp(vec2 v, vec2 min, float max)
    {
        return new vec2(Math.Min(Math.Max(v.x, min.x), max), Math.Min(Math.Max(v.y, min.y), max));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec2 Clamp(vec2 v, float min, vec2 max)
    {
        return new vec2(Math.Min(Math.Max(v.x, min), max.x), Math.Min(Math.Max(v.y, min), max.y));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec2 Clamp(vec2 v, float min, float max)
    {
        return new vec2(Math.Min(Math.Max(v.x, min), max), Math.Min(Math.Max(v.y, min), max));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec2 Clamp(float v, vec2 min, vec2 max)
    {
        return new vec2(Math.Min(Math.Max(v, min.x), max.x), Math.Min(Math.Max(v, min.y), max.y));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec2 Clamp(float v, vec2 min, float max)
    {
        return new vec2(Math.Min(Math.Max(v, min.x), max), Math.Min(Math.Max(v, min.y), max));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static vec2 Clamp(float v, float min, vec2 max)
    {
        return new vec2(Math.Min(Math.Max(v, min), max.x), Math.Min(Math.Max(v, min), max.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Clamp (Math.Min(Math.Max(v, min), max)).
    public static vec2 Clamp(float v, float min, float max)
    {
        return new vec2(Math.Min(Math.Max(v, min), max));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec2 Mix(vec2 min, vec2 max, vec2 a)
    {
        return new vec2(min.x * (1f - a.x) + max.x * a.x, min.y * (1f - a.y) + max.y * a.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec2 Mix(vec2 min, vec2 max, float a)
    {
        return new vec2(min.x * (1f - a) + max.x * a, min.y * (1f - a) + max.y * a);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec2 Mix(vec2 min, float max, vec2 a)
    {
        return new vec2(min.x * (1f - a.x) + max * a.x, min.y * (1f - a.y) + max * a.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec2 Mix(vec2 min, float max, float a)
    {
        return new vec2(min.x * (1f - a) + max * a, min.y * (1f - a) + max * a);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec2 Mix(float min, vec2 max, vec2 a)
    {
        return new vec2(min * (1f - a.x) + max.x * a.x, min * (1f - a.y) + max.y * a.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec2 Mix(float min, vec2 max, float a)
    {
        return new vec2(min * (1f - a) + max.x * a, min * (1f - a) + max.y * a);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Mix (min * (1-a) + max * a).
    public static vec2 Mix(float min, float max, vec2 a)
    {
        return new vec2(min * (1f - a.x) + max * a.x, min * (1f - a.y) + max * a.y);
    }

    //
    // Summary:
    //     Returns a vec from the application of Mix (min * (1-a) + max * a).
    public static vec2 Mix(float min, float max, float a)
    {
        return new vec2(min * (1f - a) + max * a);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec2 Lerp(vec2 min, vec2 max, vec2 a)
    {
        return new vec2(min.x * (1f - a.x) + max.x * a.x, min.y * (1f - a.y) + max.y * a.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec2 Lerp(vec2 min, vec2 max, float a)
    {
        return new vec2(min.x * (1f - a) + max.x * a, min.y * (1f - a) + max.y * a);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec2 Lerp(vec2 min, float max, vec2 a)
    {
        return new vec2(min.x * (1f - a.x) + max * a.x, min.y * (1f - a.y) + max * a.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec2 Lerp(vec2 min, float max, float a)
    {
        return new vec2(min.x * (1f - a) + max * a, min.y * (1f - a) + max * a);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec2 Lerp(float min, vec2 max, vec2 a)
    {
        return new vec2(min * (1f - a.x) + max.x * a.x, min * (1f - a.y) + max.y * a.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec2 Lerp(float min, vec2 max, float a)
    {
        return new vec2(min * (1f - a) + max.x * a, min * (1f - a) + max.y * a);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Lerp (min * (1-a) + max * a).
    public static vec2 Lerp(float min, float max, vec2 a)
    {
        return new vec2(min * (1f - a.x) + max * a.x, min * (1f - a.y) + max * a.y);
    }

    //
    // Summary:
    //     Returns a vec from the application of Lerp (min * (1-a) + max * a).
    public static vec2 Lerp(float min, float max, float a)
    {
        return new vec2(min * (1f - a) + max * a);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Fma (a * b + c).
    public static vec2 Fma(vec2 a, vec2 b, vec2 c)
    {
        return new vec2(a.x * b.x + c.x, a.y * b.y + c.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Fma (a * b + c).
    public static vec2 Fma(vec2 a, vec2 b, float c)
    {
        return new vec2(a.x * b.x + c, a.y * b.y + c);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Fma (a * b + c).
    public static vec2 Fma(vec2 a, float b, vec2 c)
    {
        return new vec2(a.x * b + c.x, a.y * b + c.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Fma (a * b + c).
    public static vec2 Fma(vec2 a, float b, float c)
    {
        return new vec2(a.x * b + c, a.y * b + c);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Fma (a * b + c).
    public static vec2 Fma(float a, vec2 b, vec2 c)
    {
        return new vec2(a * b.x + c.x, a * b.y + c.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Fma (a * b + c).
    public static vec2 Fma(float a, vec2 b, float c)
    {
        return new vec2(a * b.x + c, a * b.y + c);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Fma (a * b + c).
    public static vec2 Fma(float a, float b, vec2 c)
    {
        return new vec2(a * b + c.x, a * b + c.y);
    }

    //
    // Summary:
    //     Returns a vec from the application of Fma (a * b + c).
    public static vec2 Fma(float a, float b, float c)
    {
        return new vec2(a * b + c);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Add (lhs + rhs).
    public static vec2 Add(vec2 lhs, vec2 rhs)
    {
        return new vec2(lhs.x + rhs.x, lhs.y + rhs.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Add (lhs + rhs).
    public static vec2 Add(vec2 lhs, float rhs)
    {
        return new vec2(lhs.x + rhs, lhs.y + rhs);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Add (lhs + rhs).
    public static vec2 Add(float lhs, vec2 rhs)
    {
        return new vec2(lhs + rhs.x, lhs + rhs.y);
    }

    //
    // Summary:
    //     Returns a vec from the application of Add (lhs + rhs).
    public static vec2 Add(float lhs, float rhs)
    {
        return new vec2(lhs + rhs);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Sub (lhs - rhs).
    public static vec2 Sub(vec2 lhs, vec2 rhs)
    {
        return new vec2(lhs.x - rhs.x, lhs.y - rhs.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Sub (lhs - rhs).
    public static vec2 Sub(vec2 lhs, float rhs)
    {
        return new vec2(lhs.x - rhs, lhs.y - rhs);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Sub (lhs - rhs).
    public static vec2 Sub(float lhs, vec2 rhs)
    {
        return new vec2(lhs - rhs.x, lhs - rhs.y);
    }

    //
    // Summary:
    //     Returns a vec from the application of Sub (lhs - rhs).
    public static vec2 Sub(float lhs, float rhs)
    {
        return new vec2(lhs - rhs);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Mul (lhs * rhs).
    public static vec2 Mul(vec2 lhs, vec2 rhs)
    {
        return new vec2(lhs.x * rhs.x, lhs.y * rhs.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Mul (lhs * rhs).
    public static vec2 Mul(vec2 lhs, float rhs)
    {
        return new vec2(lhs.x * rhs, lhs.y * rhs);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Mul (lhs * rhs).
    public static vec2 Mul(float lhs, vec2 rhs)
    {
        return new vec2(lhs * rhs.x, lhs * rhs.y);
    }

    //
    // Summary:
    //     Returns a vec from the application of Mul (lhs * rhs).
    public static vec2 Mul(float lhs, float rhs)
    {
        return new vec2(lhs * rhs);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Div (lhs / rhs).
    public static vec2 Div(vec2 lhs, vec2 rhs)
    {
        return new vec2(lhs.x / rhs.x, lhs.y / rhs.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Div (lhs / rhs).
    public static vec2 Div(vec2 lhs, float rhs)
    {
        return new vec2(lhs.x / rhs, lhs.y / rhs);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Div (lhs / rhs).
    public static vec2 Div(float lhs, vec2 rhs)
    {
        return new vec2(lhs / rhs.x, lhs / rhs.y);
    }

    //
    // Summary:
    //     Returns a vec from the application of Div (lhs / rhs).
    public static vec2 Div(float lhs, float rhs)
    {
        return new vec2(lhs / rhs);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Modulo (lhs % rhs).
    public static vec2 Modulo(vec2 lhs, vec2 rhs)
    {
        return new vec2(lhs.x % rhs.x, lhs.y % rhs.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Modulo (lhs % rhs).
    public static vec2 Modulo(vec2 lhs, float rhs)
    {
        return new vec2(lhs.x % rhs, lhs.y % rhs);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Modulo (lhs % rhs).
    public static vec2 Modulo(float lhs, vec2 rhs)
    {
        return new vec2(lhs % rhs.x, lhs % rhs.y);
    }

    //
    // Summary:
    //     Returns a vec from the application of Modulo (lhs % rhs).
    public static vec2 Modulo(float lhs, float rhs)
    {
        return new vec2(lhs % rhs);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Degrees (Radians-To-Degrees
    //     Conversion).
    public static vec2 Degrees(vec2 v)
    {
        return new vec2(v.x * 57.29578f, v.y * 57.29578f);
    }

    //
    // Summary:
    //     Returns a vec from the application of Degrees (Radians-To-Degrees Conversion).
    public static vec2 Degrees(float v)
    {
        return new vec2(v * 57.29578f);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Radians (Degrees-To-Radians
    //     Conversion).
    public static vec2 Radians(vec2 v)
    {
        return new vec2(v.x * ((float)Math.PI / 180f), v.y * ((float)Math.PI / 180f));
    }

    //
    // Summary:
    //     Returns a vec from the application of Radians (Degrees-To-Radians Conversion).
    public static vec2 Radians(float v)
    {
        return new vec2(v * ((float)Math.PI / 180f));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Acos ((float)Math.Acos((double)v)).
    public static vec2 Acos(vec2 v)
    {
        return new vec2((float)Math.Acos(v.x), (float)Math.Acos(v.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Acos ((float)Math.Acos((double)v)).
    public static vec2 Acos(float v)
    {
        return new vec2((float)Math.Acos(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Asin ((float)Math.Asin((double)v)).
    public static vec2 Asin(vec2 v)
    {
        return new vec2((float)Math.Asin(v.x), (float)Math.Asin(v.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Asin ((float)Math.Asin((double)v)).
    public static vec2 Asin(float v)
    {
        return new vec2((float)Math.Asin(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Atan ((float)Math.Atan((double)v)).
    public static vec2 Atan(vec2 v)
    {
        return new vec2((float)Math.Atan(v.x), (float)Math.Atan(v.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Atan ((float)Math.Atan((double)v)).
    public static vec2 Atan(float v)
    {
        return new vec2((float)Math.Atan(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Cos ((float)Math.Cos((double)v)).
    public static vec2 Cos(vec2 v)
    {
        return new vec2((float)Math.Cos(v.x), (float)Math.Cos(v.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Cos ((float)Math.Cos((double)v)).
    public static vec2 Cos(float v)
    {
        return new vec2((float)Math.Cos(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Cosh ((float)Math.Cosh((double)v)).
    public static vec2 Cosh(vec2 v)
    {
        return new vec2((float)Math.Cosh(v.x), (float)Math.Cosh(v.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Cosh ((float)Math.Cosh((double)v)).
    public static vec2 Cosh(float v)
    {
        return new vec2((float)Math.Cosh(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Exp ((float)Math.Exp((double)v)).
    public static vec2 Exp(vec2 v)
    {
        return new vec2((float)Math.Exp(v.x), (float)Math.Exp(v.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Exp ((float)Math.Exp((double)v)).
    public static vec2 Exp(float v)
    {
        return new vec2((float)Math.Exp(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Log ((float)Math.Log((double)v)).
    public static vec2 Log(vec2 v)
    {
        return new vec2((float)Math.Log(v.x), (float)Math.Log(v.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Log ((float)Math.Log((double)v)).
    public static vec2 Log(float v)
    {
        return new vec2((float)Math.Log(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Log2 ((float)Math.Log((double)v,
    //     2)).
    public static vec2 Log2(vec2 v)
    {
        return new vec2((float)Math.Log(v.x, 2.0), (float)Math.Log(v.y, 2.0));
    }

    //
    // Summary:
    //     Returns a vec from the application of Log2 ((float)Math.Log((double)v, 2)).
    public static vec2 Log2(float v)
    {
        return new vec2((float)Math.Log(v, 2.0));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Log10 ((float)Math.Log10((double)v)).
    public static vec2 Log10(vec2 v)
    {
        return new vec2((float)Math.Log10(v.x), (float)Math.Log10(v.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Log10 ((float)Math.Log10((double)v)).
    public static vec2 Log10(float v)
    {
        return new vec2((float)Math.Log10(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Floor ((float)Math.Floor(v)).
    public static vec2 Floor(vec2 v)
    {
        return new vec2((float)Math.Floor(v.x), (float)Math.Floor(v.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Floor ((float)Math.Floor(v)).
    public static vec2 Floor(float v)
    {
        return new vec2((float)Math.Floor(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Ceiling ((float)Math.Ceiling(v)).
    public static vec2 Ceiling(vec2 v)
    {
        return new vec2((float)Math.Ceiling(v.x), (float)Math.Ceiling(v.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Ceiling ((float)Math.Ceiling(v)).
    public static vec2 Ceiling(float v)
    {
        return new vec2((float)Math.Ceiling(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Round ((float)Math.Round(v)).
    public static vec2 Round(vec2 v)
    {
        return new vec2((float)Math.Round(v.x), (float)Math.Round(v.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Round ((float)Math.Round(v)).
    public static vec2 Round(float v)
    {
        return new vec2((float)Math.Round(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Sin ((float)Math.Sin((double)v)).
    public static vec2 Sin(vec2 v)
    {
        return new vec2((float)Math.Sin(v.x), (float)Math.Sin(v.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Sin ((float)Math.Sin((double)v)).
    public static vec2 Sin(float v)
    {
        return new vec2((float)Math.Sin(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Sinh ((float)Math.Sinh((double)v)).
    public static vec2 Sinh(vec2 v)
    {
        return new vec2((float)Math.Sinh(v.x), (float)Math.Sinh(v.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Sinh ((float)Math.Sinh((double)v)).
    public static vec2 Sinh(float v)
    {
        return new vec2((float)Math.Sinh(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Tan ((float)Math.Tan((double)v)).
    public static vec2 Tan(vec2 v)
    {
        return new vec2((float)Math.Tan(v.x), (float)Math.Tan(v.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Tan ((float)Math.Tan((double)v)).
    public static vec2 Tan(float v)
    {
        return new vec2((float)Math.Tan(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Tanh ((float)Math.Tanh((double)v)).
    public static vec2 Tanh(vec2 v)
    {
        return new vec2((float)Math.Tanh(v.x), (float)Math.Tanh(v.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Tanh ((float)Math.Tanh((double)v)).
    public static vec2 Tanh(float v)
    {
        return new vec2((float)Math.Tanh(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Truncate ((float)Math.Truncate((double)v)).
    public static vec2 Truncate(vec2 v)
    {
        return new vec2((float)Math.Truncate(v.x), (float)Math.Truncate(v.y));
    }

    //
    // Summary:
    //     Returns a vec from the application of Truncate ((float)Math.Truncate((double)v)).
    public static vec2 Truncate(float v)
    {
        return new vec2((float)Math.Truncate(v));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Fract ((float)(v - Math.Floor(v))).
    public static vec2 Fract(vec2 v)
    {
        return new vec2((float)((double)v.x - Math.Floor(v.x)), (float)((double)v.y - Math.Floor(v.y)));
    }

    //
    // Summary:
    //     Returns a vec from the application of Fract ((float)(v - Math.Floor(v))).
    public static vec2 Fract(float v)
    {
        return new vec2((float)((double)v - Math.Floor(v)));
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of Trunc ((long)(v)).
    public static vec2 Trunc(vec2 v)
    {
        return new vec2((long)v.x, (long)v.y);
    }

    //
    // Summary:
    //     Returns a vec from the application of Trunc ((long)(v)).
    public static vec2 Trunc(float v)
    {
        return new vec2((long)v);
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec2 Random(Random random, vec2 minValue, vec2 maxValue)
    {
        return new vec2((float)random.NextDouble() * (maxValue.x - minValue.x) + minValue.x, (float)random.NextDouble() * (maxValue.y - minValue.y) + minValue.y);
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec2 Random(Random random, vec2 minValue, float maxValue)
    {
        return new vec2((float)random.NextDouble() * (maxValue - minValue.x) + minValue.x, (float)random.NextDouble() * (maxValue - minValue.y) + minValue.y);
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec2 Random(Random random, float minValue, vec2 maxValue)
    {
        return new vec2((float)random.NextDouble() * (maxValue.x - minValue) + minValue, (float)random.NextDouble() * (maxValue.y - minValue) + minValue);
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec2 Random(Random random, float minValue, float maxValue)
    {
        return new vec2((float)random.NextDouble() * (maxValue - minValue) + minValue);
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec2 RandomUniform(Random random, vec2 minValue, vec2 maxValue)
    {
        return new vec2((float)random.NextDouble() * (maxValue.x - minValue.x) + minValue.x, (float)random.NextDouble() * (maxValue.y - minValue.y) + minValue.y);
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec2 RandomUniform(Random random, vec2 minValue, float maxValue)
    {
        return new vec2((float)random.NextDouble() * (maxValue - minValue.x) + minValue.x, (float)random.NextDouble() * (maxValue - minValue.y) + minValue.y);
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec2 RandomUniform(Random random, float minValue, vec2 maxValue)
    {
        return new vec2((float)random.NextDouble() * (maxValue.x - minValue) + minValue, (float)random.NextDouble() * (maxValue.y - minValue) + minValue);
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed uniform values between
    //     'minValue' and 'maxValue'.
    public static vec2 RandomUniform(Random random, float minValue, float maxValue)
    {
        return new vec2((float)random.NextDouble() * (maxValue - minValue) + minValue);
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec2 RandomNormal(Random random, vec2 mean, vec2 variance)
    {
        return new vec2((float)(Math.Sqrt(variance.x) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.x, (float)(Math.Sqrt(variance.y) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.y);
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec2 RandomNormal(Random random, vec2 mean, float variance)
    {
        return new vec2((float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.x, (float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.y);
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec2 RandomNormal(Random random, float mean, vec2 variance)
    {
        return new vec2((float)(Math.Sqrt(variance.x) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean, (float)(Math.Sqrt(variance.y) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean);
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec2 RandomNormal(Random random, float mean, float variance)
    {
        return new vec2((float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean);
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec2 RandomGaussian(Random random, vec2 mean, vec2 variance)
    {
        return new vec2((float)(Math.Sqrt(variance.x) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.x, (float)(Math.Sqrt(variance.y) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.y);
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec2 RandomGaussian(Random random, vec2 mean, float variance)
    {
        return new vec2((float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.x, (float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.y);
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec2 RandomGaussian(Random random, float mean, vec2 variance)
    {
        return new vec2((float)(Math.Sqrt(variance.x) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean, (float)(Math.Sqrt(variance.y) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean);
    }

    //
    // Summary:
    //     Returns a vec2 with independent and identically distributed values according
    //     to a normal/Gaussian distribution with specified mean and variance.
    public static vec2 RandomGaussian(Random random, float mean, float variance)
    {
        return new vec2((float)(Math.Sqrt(variance) * Math.Cos(Math.PI * 2.0 * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of operator+ (lhs + rhs).
    public static vec2 operator +(vec2 lhs, vec2 rhs)
    {
        return new vec2(lhs.x + rhs.x, lhs.y + rhs.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of operator+ (lhs + rhs).
    public static vec2 operator +(vec2 lhs, float rhs)
    {
        return new vec2(lhs.x + rhs, lhs.y + rhs);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of operator+ (lhs + rhs).
    public static vec2 operator +(float lhs, vec2 rhs)
    {
        return new vec2(lhs + rhs.x, lhs + rhs.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of operator- (lhs - rhs).
    public static vec2 operator -(vec2 lhs, vec2 rhs)
    {
        return new vec2(lhs.x - rhs.x, lhs.y - rhs.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of operator- (lhs - rhs).
    public static vec2 operator -(vec2 lhs, float rhs)
    {
        return new vec2(lhs.x - rhs, lhs.y - rhs);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of operator- (lhs - rhs).
    public static vec2 operator -(float lhs, vec2 rhs)
    {
        return new vec2(lhs - rhs.x, lhs - rhs.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of operator* (lhs * rhs).
    public static vec2 operator *(vec2 lhs, vec2 rhs)
    {
        return new vec2(lhs.x * rhs.x, lhs.y * rhs.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of operator* (lhs * rhs).
    public static vec2 operator *(vec2 lhs, float rhs)
    {
        return new vec2(lhs.x * rhs, lhs.y * rhs);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of operator* (lhs * rhs).
    public static vec2 operator *(float lhs, vec2 rhs)
    {
        return new vec2(lhs * rhs.x, lhs * rhs.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of operator/ (lhs / rhs).
    public static vec2 operator /(vec2 lhs, vec2 rhs)
    {
        return new vec2(lhs.x / rhs.x, lhs.y / rhs.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of operator/ (lhs / rhs).
    public static vec2 operator /(vec2 lhs, float rhs)
    {
        return new vec2(lhs.x / rhs, lhs.y / rhs);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of operator/ (lhs / rhs).
    public static vec2 operator /(float lhs, vec2 rhs)
    {
        return new vec2(lhs / rhs.x, lhs / rhs.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of operator+ (identity).
    public static vec2 operator +(vec2 v)
    {
        return v;
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of operator- (-v).
    public static vec2 operator -(vec2 v)
    {
        return new vec2(0f - v.x, 0f - v.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of operator% (lhs % rhs).
    public static vec2 operator %(vec2 lhs, vec2 rhs)
    {
        return new vec2(lhs.x % rhs.x, lhs.y % rhs.y);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of operator% (lhs % rhs).
    public static vec2 operator %(vec2 lhs, float rhs)
    {
        return new vec2(lhs.x % rhs, lhs.y % rhs);
    }

    //
    // Summary:
    //     Returns a vec2 from component-wise application of operator% (lhs % rhs).
    public static vec2 operator %(float lhs, vec2 rhs)
    {
        return new vec2(lhs % rhs.x, lhs % rhs.y);
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
