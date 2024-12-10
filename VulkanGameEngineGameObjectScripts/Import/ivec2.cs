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
//     A vector of type int with 2 components.
[Serializable]
[DataContract(Namespace = "vec")]
public struct ivec2 : IReadOnlyList<int>, IReadOnlyCollection<int>, IEnumerable<int>, IEnumerable, IEquatable<ivec2>
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
    //     Gets/Sets a specific indexed component (a bit slower than direct access).
    public int this[int index]
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
    //     Returns an array with all values
    public int[] Values => new int[2] { x, y };

    //
    // Summary:
    //     Returns the number of components (2).
    public int Count => 2;

    //
    // Summary:
    //     Returns the minimal component of this vector.
    public int MinElement => Math.Min(x, y);

    //
    // Summary:
    //     Returns the maximal component of this vector.
    public int MaxElement => Math.Max(x, y);

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
    public int Sum => x + y;

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
    //     Predefined all-zero vector
    public static ivec2 Zero { get; } = new ivec2(0, 0);


    //
    // Summary:
    //     Predefined all-ones vector
    public static ivec2 Ones { get; } = new ivec2(1, 1);


    //
    // Summary:
    //     Predefined unit-X vector
    public static ivec2 UnitX { get; } = new ivec2(1, 0);


    //
    // Summary:
    //     Predefined unit-Y vector
    public static ivec2 UnitY { get; } = new ivec2(0, 1);


    //
    // Summary:
    //     Predefined all-MaxValue vector
    public static ivec2 MaxValue { get; } = new ivec2(int.MaxValue, int.MaxValue);


    //
    // Summary:
    //     Predefined all-MinValue vector
    public static ivec2 MinValue { get; } = new ivec2(int.MinValue, int.MinValue);


    //
    // Summary:
    //     Component-wise constructor
    public ivec2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    //
    // Summary:
    //     all-same-value constructor
    public ivec2(int v)
    {
        x = v;
        y = v;
    }

    //
    // Summary:
    //     from-vector constructor
    public ivec2(ivec2 v)
    {
        x = v.x;
        y = v.y;
    }

    //
    // Summary:
    //     from-vector constructor (additional fields are truncated)
    public ivec2(ivec3 v)
    {
        x = v.x;
        y = v.y;
    }

    //
    // Summary:
    //     from-vector constructor (additional fields are truncated)
    public ivec2(ivec4 v)
    {
        x = v.x;
        y = v.y;
    }

    //
    // Summary:
    //     From-array/list constructor (superfluous values are ignored, missing values are
    //     zero-filled).
    public ivec2(IReadOnlyList<int> v)
    {
        int count = v.Count;
        x = ((count >= 0) ? v[0] : 0);
        y = ((count >= 1) ? v[1] : 0);
    }

    //
    // Summary:
    //     Generic from-array constructor (superfluous values are ignored, missing values
    //     are zero-filled).
    public ivec2(object[] v)
    {
        int num = v.Length;
        x = ((num >= 0) ? ((int)v[0]) : 0);
        y = ((num >= 1) ? ((int)v[1]) : 0);
    }

    //
    // Summary:
    //     From-array constructor (superfluous values are ignored, missing values are zero-filled).
    public ivec2(int[] v)
    {
        int num = v.Length;
        x = ((num >= 0) ? v[0] : 0);
        y = ((num >= 1) ? v[1] : 0);
    }

    //
    // Summary:
    //     From-array constructor with base index (superfluous values are ignored, missing
    //     values are zero-filled).
    public ivec2(int[] v, int startIndex)
    {
        int num = v.Length;
        x = ((num + startIndex >= 0) ? v[startIndex] : 0);
        y = ((num + startIndex >= 1) ? v[1 + startIndex] : 0);
    }

    //
    // Summary:
    //     From-IEnumerable constructor (superfluous values are ignored, missing values
    //     are zero-filled).
    public ivec2(IEnumerable<int> v)
        : this(v.ToArray())
    {
    }

    //
    // Summary:
    //     Explicitly converts this to a ivec3. (Higher components are zeroed)
    public static explicit operator ivec3(ivec2 v)
    {
        return new ivec3(v.x, v.y, 0);
    }

    //
    // Summary:
    //     Explicitly converts this to a ivec4. (Higher components are zeroed)
    public static explicit operator ivec4(ivec2 v)
    {
        return new ivec4(v.x, v.y, 0, 0);
    }

  
    //
    // Summary:
    //     Explicitly converts this to a vec3. (Higher components are zeroed)
    public static explicit operator vec3(ivec2 v)
    {
        return new vec3(v.x, v.y, 0f);
    }

    //
    // Summary:
    //     Explicitly converts this to a vec4. (Higher components are zeroed)
    public static explicit operator vec4(ivec2 v)
    {
        return new vec4(v.x, v.y, 0f, 0f);
    }

    //
    // Summary:
    //     Explicitly converts this to a int array.
    public static explicit operator int[](ivec2 v)
    {
        return new int[2] { v.x, v.y };
    }

    //
    // Summary:
    //     Explicitly converts this to a generic object array.
    public static explicit operator object[](ivec2 v)
    {
        return new object[2] { v.x, v.y };
    }

    //
    // Summary:
    //     Returns true iff this equals rhs component-wise.
    public static bool operator ==(ivec2 lhs, ivec2 rhs)
    {
        return lhs.Equals(rhs);
    }

    //
    // Summary:
    //     Returns true iff this does not equal rhs (component-wise).
    public static bool operator !=(ivec2 lhs, ivec2 rhs)
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
    public bool Equals(ivec2 rhs)
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

        if (obj is ivec2)
        {
            return Equals((ivec2)obj);
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
    public static ivec2 Parse(string s)
    {
        return Parse(s, ", ");
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator).
    public static ivec2 Parse(string s, string sep)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 2)
        {
            throw new FormatException("input has not exactly 2 parts");
        }

        return new ivec2(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()));
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator and a type provider).
    public static ivec2 Parse(string s, string sep, IFormatProvider provider)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 2)
        {
            throw new FormatException("input has not exactly 2 parts");
        }

        return new ivec2(int.Parse(array[0].Trim(), provider), int.Parse(array[1].Trim(), provider));
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator and a number style).
    public static ivec2 Parse(string s, string sep, NumberStyles style)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 2)
        {
            throw new FormatException("input has not exactly 2 parts");
        }

        return new ivec2(int.Parse(array[0].Trim(), style), int.Parse(array[1].Trim(), style));
    }

    //
    // Summary:
    //     Converts the string representation of the vector into a vector representation
    //     (using a designated separator and a number style and a format provider).
    public static ivec2 Parse(string s, string sep, NumberStyles style, IFormatProvider provider)
    {
        string[] array = s.Split(new string[1] { sep }, StringSplitOptions.None);
        if (array.Length != 2)
        {
            throw new FormatException("input has not exactly 2 parts");
        }

        return new ivec2(int.Parse(array[0].Trim(), style, provider), int.Parse(array[1].Trim(), style, provider));
    }

    //
    // Summary:
    //     Tries to convert the string representation of the vector into a vector representation
    //     (using ', ' as a separator), returns false if string was invalid.
    public static bool TryParse(string s, out ivec2 result)
    {
        return TryParse(s, ", ", out result);
    }

    //
    // Summary:
    //     Tries to convert the string representation of the vector into a vector representation
    //     (using a designated separator), returns false if string was invalid.
    public static bool TryParse(string s, string sep, out ivec2 result)
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

        int result2 = 0;
        int result3 = 0;
        bool flag = int.TryParse(array[0].Trim(), out result2) && int.TryParse(array[1].Trim(), out result3);
        result = (flag ? new ivec2(result2, result3) : Zero);
        return flag;
    }

    //
    // Summary:
    //     Tries to convert the string representation of the vector into a vector representation
    //     (using a designated separator and a number style and a format provider), returns
    //     false if string was invalid.
    public static bool TryParse(string s, string sep, NumberStyles style, IFormatProvider provider, out ivec2 result)
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

        int result2 = 0;
        int result3 = 0;
        bool flag = int.TryParse(array[0].Trim(), style, provider, out result2) && int.TryParse(array[1].Trim(), style, provider, out result3);
        result = (flag ? new ivec2(result2, result3) : Zero);
        return flag;
    }

    //
    // Summary:
    //     Returns the inner product (dot product, scalar product) of the two vectors.
    public static int Dot(ivec2 lhs, ivec2 rhs)
    {
        return lhs.x * rhs.x + lhs.y * rhs.y;
    }

    //
    // Summary:
    //     Returns the euclidean distance between the two vectors.
    public static float Distance(ivec2 lhs, ivec2 rhs)
    {
        return (lhs - rhs).Length;
    }

    //
    // Summary:
    //     Returns the squared euclidean distance between the two vectors.
    public static float DistanceSqr(ivec2 lhs, ivec2 rhs)
    {
        return (lhs - rhs).LengthSqr;
    }

    //
    // Summary:
    //     Calculate the reflection direction for an incident vector (N should be normalized
    //     in order to achieve the desired result).
    public static ivec2 Reflect(ivec2 I, ivec2 N)
    {
        return I - 2 * Dot(N, I) * N;
    }

    //
    // Summary:
    //     Calculate the refraction direction for an incident vector (The input parameters
    //     I and N should be normalized in order to achieve the desired result).
    public static ivec2 Refract(ivec2 I, ivec2 N, int eta)
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
    public static ivec2 FaceForward(ivec2 N, ivec2 I, ivec2 Nref)
    {
        if (Dot(Nref, I) >= 0)
        {
            return -N;
        }

        return N;
    }

    //
    // Summary:
    //     Returns the length of the outer product (cross product, vector product) of the
    //     two vectors.
    public static int Cross(ivec2 l, ivec2 r)
    {
        return l.x * r.y - l.y * r.x;
    }

    //
    // Summary:
    //     Returns a ivec2 with independent and identically distributed uniform integer
    //     values between 0 (inclusive) and int.MaxValue (exclusive).
    public static ivec2 Random(Random random)
    {
        return new ivec2(random.Next(), random.Next());
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Abs (Math.Abs(v)).
    public static ivec2 Abs(ivec2 v)
    {
        return new ivec2(Math.Abs(v.x), Math.Abs(v.y));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Abs (Math.Abs(v)).
    public static ivec2 Abs(int v)
    {
        return new ivec2(Math.Abs(v));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of HermiteInterpolationOrder3
    //     ((3 - 2 * v) * v * v).
    public static ivec2 HermiteInterpolationOrder3(ivec2 v)
    {
        return new ivec2((3 - 2 * v.x) * v.x * v.x, (3 - 2 * v.y) * v.y * v.y);
    }

    //
    // Summary:
    //     Returns a ivec from the application of HermiteInterpolationOrder3 ((3 - 2 * v)
    //     * v * v).
    public static ivec2 HermiteInterpolationOrder3(int v)
    {
        return new ivec2((3 - 2 * v) * v * v);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of HermiteInterpolationOrder5
    //     (((6 * v - 15) * v + 10) * v * v * v).
    public static ivec2 HermiteInterpolationOrder5(ivec2 v)
    {
        return new ivec2(((6 * v.x - 15) * v.x + 10) * v.x * v.x * v.x, ((6 * v.y - 15) * v.y + 10) * v.y * v.y * v.y);
    }

    //
    // Summary:
    //     Returns a ivec from the application of HermiteInterpolationOrder5 (((6 * v -
    //     15) * v + 10) * v * v * v).
    public static ivec2 HermiteInterpolationOrder5(int v)
    {
        return new ivec2(((6 * v - 15) * v + 10) * v * v * v);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Sqr (v * v).
    public static ivec2 Sqr(ivec2 v)
    {
        return new ivec2(v.x * v.x, v.y * v.y);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Sqr (v * v).
    public static ivec2 Sqr(int v)
    {
        return new ivec2(v * v);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Pow2 (v * v).
    public static ivec2 Pow2(ivec2 v)
    {
        return new ivec2(v.x * v.x, v.y * v.y);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Pow2 (v * v).
    public static ivec2 Pow2(int v)
    {
        return new ivec2(v * v);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Pow3 (v * v * v).
    public static ivec2 Pow3(ivec2 v)
    {
        return new ivec2(v.x * v.x * v.x, v.y * v.y * v.y);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Pow3 (v * v * v).
    public static ivec2 Pow3(int v)
    {
        return new ivec2(v * v * v);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Step (v >= 0 ? 1 : 0).
    public static ivec2 Step(ivec2 v)
    {
        return new ivec2((v.x >= 0) ? 1 : 0, (v.y >= 0) ? 1 : 0);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Step (v >= 0 ? 1 : 0).
    public static ivec2 Step(int v)
    {
        return new ivec2((v >= 0) ? 1 : 0);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Sqrt ((int)Math.Sqrt((double)v)).
    public static ivec2 Sqrt(ivec2 v)
    {
        return new ivec2((int)Math.Sqrt(v.x), (int)Math.Sqrt(v.y));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Sqrt ((int)Math.Sqrt((double)v)).
    public static ivec2 Sqrt(int v)
    {
        return new ivec2((int)Math.Sqrt(v));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of InverseSqrt ((int)(1.0 / Math.Sqrt((double)v))).
    public static ivec2 InverseSqrt(ivec2 v)
    {
        return new ivec2((int)(1.0 / Math.Sqrt(v.x)), (int)(1.0 / Math.Sqrt(v.y)));
    }

    //
    // Summary:
    //     Returns a ivec from the application of InverseSqrt ((int)(1.0 / Math.Sqrt((double)v))).
    public static ivec2 InverseSqrt(int v)
    {
        return new ivec2((int)(1.0 / Math.Sqrt(v)));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Sign (Math.Sign(v)).
    public static ivec2 Sign(ivec2 v)
    {
        return new ivec2(Math.Sign(v.x), Math.Sign(v.y));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Sign (Math.Sign(v)).
    public static ivec2 Sign(int v)
    {
        return new ivec2(Math.Sign(v));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Max (Math.Max(lhs, rhs)).
    public static ivec2 Max(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Max (Math.Max(lhs, rhs)).
    public static ivec2 Max(ivec2 lhs, int rhs)
    {
        return new ivec2(Math.Max(lhs.x, rhs), Math.Max(lhs.y, rhs));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Max (Math.Max(lhs, rhs)).
    public static ivec2 Max(int lhs, ivec2 rhs)
    {
        return new ivec2(Math.Max(lhs, rhs.x), Math.Max(lhs, rhs.y));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Max (Math.Max(lhs, rhs)).
    public static ivec2 Max(int lhs, int rhs)
    {
        return new ivec2(Math.Max(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Min (Math.Min(lhs, rhs)).
    public static ivec2 Min(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Min (Math.Min(lhs, rhs)).
    public static ivec2 Min(ivec2 lhs, int rhs)
    {
        return new ivec2(Math.Min(lhs.x, rhs), Math.Min(lhs.y, rhs));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Min (Math.Min(lhs, rhs)).
    public static ivec2 Min(int lhs, ivec2 rhs)
    {
        return new ivec2(Math.Min(lhs, rhs.x), Math.Min(lhs, rhs.y));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Min (Math.Min(lhs, rhs)).
    public static ivec2 Min(int lhs, int rhs)
    {
        return new ivec2(Math.Min(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Pow ((int)Math.Pow((double)lhs,
    //     (double)rhs)).
    public static ivec2 Pow(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2((int)Math.Pow(lhs.x, rhs.x), (int)Math.Pow(lhs.y, rhs.y));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Pow ((int)Math.Pow((double)lhs,
    //     (double)rhs)).
    public static ivec2 Pow(ivec2 lhs, int rhs)
    {
        return new ivec2((int)Math.Pow(lhs.x, rhs), (int)Math.Pow(lhs.y, rhs));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Pow ((int)Math.Pow((double)lhs,
    //     (double)rhs)).
    public static ivec2 Pow(int lhs, ivec2 rhs)
    {
        return new ivec2((int)Math.Pow(lhs, rhs.x), (int)Math.Pow(lhs, rhs.y));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Pow ((int)Math.Pow((double)lhs, (double)rhs)).
    public static ivec2 Pow(int lhs, int rhs)
    {
        return new ivec2((int)Math.Pow(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Log ((int)Math.Log((double)lhs,
    //     (double)rhs)).
    public static ivec2 Log(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2((int)Math.Log(lhs.x, rhs.x), (int)Math.Log(lhs.y, rhs.y));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Log ((int)Math.Log((double)lhs,
    //     (double)rhs)).
    public static ivec2 Log(ivec2 lhs, int rhs)
    {
        return new ivec2((int)Math.Log(lhs.x, rhs), (int)Math.Log(lhs.y, rhs));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Log ((int)Math.Log((double)lhs,
    //     (double)rhs)).
    public static ivec2 Log(int lhs, ivec2 rhs)
    {
        return new ivec2((int)Math.Log(lhs, rhs.x), (int)Math.Log(lhs, rhs.y));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Log ((int)Math.Log((double)lhs, (double)rhs)).
    public static ivec2 Log(int lhs, int rhs)
    {
        return new ivec2((int)Math.Log(lhs, rhs));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static ivec2 Clamp(ivec2 v, ivec2 min, ivec2 max)
    {
        return new ivec2(Math.Min(Math.Max(v.x, min.x), max.x), Math.Min(Math.Max(v.y, min.y), max.y));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static ivec2 Clamp(ivec2 v, ivec2 min, int max)
    {
        return new ivec2(Math.Min(Math.Max(v.x, min.x), max), Math.Min(Math.Max(v.y, min.y), max));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static ivec2 Clamp(ivec2 v, int min, ivec2 max)
    {
        return new ivec2(Math.Min(Math.Max(v.x, min), max.x), Math.Min(Math.Max(v.y, min), max.y));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static ivec2 Clamp(ivec2 v, int min, int max)
    {
        return new ivec2(Math.Min(Math.Max(v.x, min), max), Math.Min(Math.Max(v.y, min), max));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static ivec2 Clamp(int v, ivec2 min, ivec2 max)
    {
        return new ivec2(Math.Min(Math.Max(v, min.x), max.x), Math.Min(Math.Max(v, min.y), max.y));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static ivec2 Clamp(int v, ivec2 min, int max)
    {
        return new ivec2(Math.Min(Math.Max(v, min.x), max), Math.Min(Math.Max(v, min.y), max));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Clamp (Math.Min(Math.Max(v,
    //     min), max)).
    public static ivec2 Clamp(int v, int min, ivec2 max)
    {
        return new ivec2(Math.Min(Math.Max(v, min), max.x), Math.Min(Math.Max(v, min), max.y));
    }

    //
    // Summary:
    //     Returns a ivec from the application of Clamp (Math.Min(Math.Max(v, min), max)).
    public static ivec2 Clamp(int v, int min, int max)
    {
        return new ivec2(Math.Min(Math.Max(v, min), max));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Mix (min * (1-a) + max * a).
    public static ivec2 Mix(ivec2 min, ivec2 max, ivec2 a)
    {
        return new ivec2(min.x * (1 - a.x) + max.x * a.x, min.y * (1 - a.y) + max.y * a.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Mix (min * (1-a) + max * a).
    public static ivec2 Mix(ivec2 min, ivec2 max, int a)
    {
        return new ivec2(min.x * (1 - a) + max.x * a, min.y * (1 - a) + max.y * a);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Mix (min * (1-a) + max * a).
    public static ivec2 Mix(ivec2 min, int max, ivec2 a)
    {
        return new ivec2(min.x * (1 - a.x) + max * a.x, min.y * (1 - a.y) + max * a.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Mix (min * (1-a) + max * a).
    public static ivec2 Mix(ivec2 min, int max, int a)
    {
        return new ivec2(min.x * (1 - a) + max * a, min.y * (1 - a) + max * a);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Mix (min * (1-a) + max * a).
    public static ivec2 Mix(int min, ivec2 max, ivec2 a)
    {
        return new ivec2(min * (1 - a.x) + max.x * a.x, min * (1 - a.y) + max.y * a.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Mix (min * (1-a) + max * a).
    public static ivec2 Mix(int min, ivec2 max, int a)
    {
        return new ivec2(min * (1 - a) + max.x * a, min * (1 - a) + max.y * a);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Mix (min * (1-a) + max * a).
    public static ivec2 Mix(int min, int max, ivec2 a)
    {
        return new ivec2(min * (1 - a.x) + max * a.x, min * (1 - a.y) + max * a.y);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Mix (min * (1-a) + max * a).
    public static ivec2 Mix(int min, int max, int a)
    {
        return new ivec2(min * (1 - a) + max * a);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Lerp (min * (1-a) + max *
    //     a).
    public static ivec2 Lerp(ivec2 min, ivec2 max, ivec2 a)
    {
        return new ivec2(min.x * (1 - a.x) + max.x * a.x, min.y * (1 - a.y) + max.y * a.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Lerp (min * (1-a) + max *
    //     a).
    public static ivec2 Lerp(ivec2 min, ivec2 max, int a)
    {
        return new ivec2(min.x * (1 - a) + max.x * a, min.y * (1 - a) + max.y * a);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Lerp (min * (1-a) + max *
    //     a).
    public static ivec2 Lerp(ivec2 min, int max, ivec2 a)
    {
        return new ivec2(min.x * (1 - a.x) + max * a.x, min.y * (1 - a.y) + max * a.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Lerp (min * (1-a) + max *
    //     a).
    public static ivec2 Lerp(ivec2 min, int max, int a)
    {
        return new ivec2(min.x * (1 - a) + max * a, min.y * (1 - a) + max * a);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Lerp (min * (1-a) + max *
    //     a).
    public static ivec2 Lerp(int min, ivec2 max, ivec2 a)
    {
        return new ivec2(min * (1 - a.x) + max.x * a.x, min * (1 - a.y) + max.y * a.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Lerp (min * (1-a) + max *
    //     a).
    public static ivec2 Lerp(int min, ivec2 max, int a)
    {
        return new ivec2(min * (1 - a) + max.x * a, min * (1 - a) + max.y * a);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Lerp (min * (1-a) + max *
    //     a).
    public static ivec2 Lerp(int min, int max, ivec2 a)
    {
        return new ivec2(min * (1 - a.x) + max * a.x, min * (1 - a.y) + max * a.y);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Lerp (min * (1-a) + max * a).
    public static ivec2 Lerp(int min, int max, int a)
    {
        return new ivec2(min * (1 - a) + max * a);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Fma (a * b + c).
    public static ivec2 Fma(ivec2 a, ivec2 b, ivec2 c)
    {
        return new ivec2(a.x * b.x + c.x, a.y * b.y + c.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Fma (a * b + c).
    public static ivec2 Fma(ivec2 a, ivec2 b, int c)
    {
        return new ivec2(a.x * b.x + c, a.y * b.y + c);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Fma (a * b + c).
    public static ivec2 Fma(ivec2 a, int b, ivec2 c)
    {
        return new ivec2(a.x * b + c.x, a.y * b + c.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Fma (a * b + c).
    public static ivec2 Fma(ivec2 a, int b, int c)
    {
        return new ivec2(a.x * b + c, a.y * b + c);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Fma (a * b + c).
    public static ivec2 Fma(int a, ivec2 b, ivec2 c)
    {
        return new ivec2(a * b.x + c.x, a * b.y + c.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Fma (a * b + c).
    public static ivec2 Fma(int a, ivec2 b, int c)
    {
        return new ivec2(a * b.x + c, a * b.y + c);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Fma (a * b + c).
    public static ivec2 Fma(int a, int b, ivec2 c)
    {
        return new ivec2(a * b + c.x, a * b + c.y);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Fma (a * b + c).
    public static ivec2 Fma(int a, int b, int c)
    {
        return new ivec2(a * b + c);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Add (lhs + rhs).
    public static ivec2 Add(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2(lhs.x + rhs.x, lhs.y + rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Add (lhs + rhs).
    public static ivec2 Add(ivec2 lhs, int rhs)
    {
        return new ivec2(lhs.x + rhs, lhs.y + rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Add (lhs + rhs).
    public static ivec2 Add(int lhs, ivec2 rhs)
    {
        return new ivec2(lhs + rhs.x, lhs + rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Add (lhs + rhs).
    public static ivec2 Add(int lhs, int rhs)
    {
        return new ivec2(lhs + rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Sub (lhs - rhs).
    public static ivec2 Sub(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2(lhs.x - rhs.x, lhs.y - rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Sub (lhs - rhs).
    public static ivec2 Sub(ivec2 lhs, int rhs)
    {
        return new ivec2(lhs.x - rhs, lhs.y - rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Sub (lhs - rhs).
    public static ivec2 Sub(int lhs, ivec2 rhs)
    {
        return new ivec2(lhs - rhs.x, lhs - rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Sub (lhs - rhs).
    public static ivec2 Sub(int lhs, int rhs)
    {
        return new ivec2(lhs - rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Mul (lhs * rhs).
    public static ivec2 Mul(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2(lhs.x * rhs.x, lhs.y * rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Mul (lhs * rhs).
    public static ivec2 Mul(ivec2 lhs, int rhs)
    {
        return new ivec2(lhs.x * rhs, lhs.y * rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Mul (lhs * rhs).
    public static ivec2 Mul(int lhs, ivec2 rhs)
    {
        return new ivec2(lhs * rhs.x, lhs * rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Mul (lhs * rhs).
    public static ivec2 Mul(int lhs, int rhs)
    {
        return new ivec2(lhs * rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Div (lhs / rhs).
    public static ivec2 Div(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2(lhs.x / rhs.x, lhs.y / rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Div (lhs / rhs).
    public static ivec2 Div(ivec2 lhs, int rhs)
    {
        return new ivec2(lhs.x / rhs, lhs.y / rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Div (lhs / rhs).
    public static ivec2 Div(int lhs, ivec2 rhs)
    {
        return new ivec2(lhs / rhs.x, lhs / rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Div (lhs / rhs).
    public static ivec2 Div(int lhs, int rhs)
    {
        return new ivec2(lhs / rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Xor (lhs ^ rhs).
    public static ivec2 Xor(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2(lhs.x ^ rhs.x, lhs.y ^ rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Xor (lhs ^ rhs).
    public static ivec2 Xor(ivec2 lhs, int rhs)
    {
        return new ivec2(lhs.x ^ rhs, lhs.y ^ rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of Xor (lhs ^ rhs).
    public static ivec2 Xor(int lhs, ivec2 rhs)
    {
        return new ivec2(lhs ^ rhs.x, lhs ^ rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec from the application of Xor (lhs ^ rhs).
    public static ivec2 Xor(int lhs, int rhs)
    {
        return new ivec2(lhs ^ rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of BitwiseOr (lhs | rhs).
    public static ivec2 BitwiseOr(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2(lhs.x | rhs.x, lhs.y | rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of BitwiseOr (lhs | rhs).
    public static ivec2 BitwiseOr(ivec2 lhs, int rhs)
    {
        return new ivec2(lhs.x | rhs, lhs.y | rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of BitwiseOr (lhs | rhs).
    public static ivec2 BitwiseOr(int lhs, ivec2 rhs)
    {
        return new ivec2(lhs | rhs.x, lhs | rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec from the application of BitwiseOr (lhs | rhs).
    public static ivec2 BitwiseOr(int lhs, int rhs)
    {
        return new ivec2(lhs | rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of BitwiseAnd (lhs & rhs).
    public static ivec2 BitwiseAnd(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2(lhs.x & rhs.x, lhs.y & rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of BitwiseAnd (lhs & rhs).
    public static ivec2 BitwiseAnd(ivec2 lhs, int rhs)
    {
        return new ivec2(lhs.x & rhs, lhs.y & rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of BitwiseAnd (lhs & rhs).
    public static ivec2 BitwiseAnd(int lhs, ivec2 rhs)
    {
        return new ivec2(lhs & rhs.x, lhs & rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec from the application of BitwiseAnd (lhs & rhs).
    public static ivec2 BitwiseAnd(int lhs, int rhs)
    {
        return new ivec2(lhs & rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of LeftShift (lhs << rhs).
    public static ivec2 LeftShift(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2(lhs.x << rhs.x, lhs.y << rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of LeftShift (lhs << rhs).
    public static ivec2 LeftShift(ivec2 lhs, int rhs)
    {
        return new ivec2(lhs.x << rhs, lhs.y << rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of LeftShift (lhs << rhs).
    public static ivec2 LeftShift(int lhs, ivec2 rhs)
    {
        return new ivec2(lhs << rhs.x, lhs << rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec from the application of LeftShift (lhs << rhs).
    public static ivec2 LeftShift(int lhs, int rhs)
    {
        return new ivec2(lhs << rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of RightShift (lhs >> rhs).
    public static ivec2 RightShift(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2(lhs.x >> rhs.x, lhs.y >> rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of RightShift (lhs >> rhs).
    public static ivec2 RightShift(ivec2 lhs, int rhs)
    {
        return new ivec2(lhs.x >> rhs, lhs.y >> rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of RightShift (lhs >> rhs).
    public static ivec2 RightShift(int lhs, ivec2 rhs)
    {
        return new ivec2(lhs >> rhs.x, lhs >> rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec from the application of RightShift (lhs >> rhs).
    public static ivec2 RightShift(int lhs, int rhs)
    {
        return new ivec2(lhs >> rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 with independent and identically distributed uniform integer
    //     values between 0 (inclusive) and maxValue (exclusive). (A maxValue of 0 is allowed
    //     and returns 0.)
    public static ivec2 Random(Random random, ivec2 maxValue)
    {
        return new ivec2(random.Next(maxValue.x), random.Next(maxValue.y));
    }

    //
    // Summary:
    //     Returns a ivec2 with independent and identically distributed uniform integer
    //     values between 0 (inclusive) and maxValue (exclusive). (A maxValue of 0 is allowed
    //     and returns 0.)
    public static ivec2 Random(Random random, int maxValue)
    {
        return new ivec2(random.Next(maxValue));
    }

    //
    // Summary:
    //     Returns a ivec2 with independent and identically distributed uniform integer
    //     values between minValue (inclusive) and maxValue (exclusive). (minValue == maxValue
    //     is allowed and returns minValue. Negative values are allowed.)
    public static ivec2 Random(Random random, ivec2 minValue, ivec2 maxValue)
    {
        return new ivec2(random.Next(minValue.x, maxValue.x), random.Next(minValue.y, maxValue.y));
    }

    //
    // Summary:
    //     Returns a ivec2 with independent and identically distributed uniform integer
    //     values between minValue (inclusive) and maxValue (exclusive). (minValue == maxValue
    //     is allowed and returns minValue. Negative values are allowed.)
    public static ivec2 Random(Random random, ivec2 minValue, int maxValue)
    {
        return new ivec2(random.Next(minValue.x, maxValue), random.Next(minValue.y, maxValue));
    }

    //
    // Summary:
    //     Returns a ivec2 with independent and identically distributed uniform integer
    //     values between minValue (inclusive) and maxValue (exclusive). (minValue == maxValue
    //     is allowed and returns minValue. Negative values are allowed.)
    public static ivec2 Random(Random random, int minValue, ivec2 maxValue)
    {
        return new ivec2(random.Next(minValue, maxValue.x), random.Next(minValue, maxValue.y));
    }

    //
    // Summary:
    //     Returns a ivec2 with independent and identically distributed uniform integer
    //     values between minValue (inclusive) and maxValue (exclusive). (minValue == maxValue
    //     is allowed and returns minValue. Negative values are allowed.)
    public static ivec2 Random(Random random, int minValue, int maxValue)
    {
        return new ivec2(random.Next(minValue, maxValue));
    }

    //
    // Summary:
    //     Returns a ivec2 with independent and identically distributed uniform integer
    //     values between minValue (inclusive) and maxValue (exclusive). (minValue == maxValue
    //     is allowed and returns minValue. Negative values are allowed.)
    public static ivec2 RandomUniform(Random random, ivec2 minValue, ivec2 maxValue)
    {
        return new ivec2(random.Next(minValue.x, maxValue.x), random.Next(minValue.y, maxValue.y));
    }

    //
    // Summary:
    //     Returns a ivec2 with independent and identically distributed uniform integer
    //     values between minValue (inclusive) and maxValue (exclusive). (minValue == maxValue
    //     is allowed and returns minValue. Negative values are allowed.)
    public static ivec2 RandomUniform(Random random, ivec2 minValue, int maxValue)
    {
        return new ivec2(random.Next(minValue.x, maxValue), random.Next(minValue.y, maxValue));
    }

    //
    // Summary:
    //     Returns a ivec2 with independent and identically distributed uniform integer
    //     values between minValue (inclusive) and maxValue (exclusive). (minValue == maxValue
    //     is allowed and returns minValue. Negative values are allowed.)
    public static ivec2 RandomUniform(Random random, int minValue, ivec2 maxValue)
    {
        return new ivec2(random.Next(minValue, maxValue.x), random.Next(minValue, maxValue.y));
    }

    //
    // Summary:
    //     Returns a ivec2 with independent and identically distributed uniform integer
    //     values between minValue (inclusive) and maxValue (exclusive). (minValue == maxValue
    //     is allowed and returns minValue. Negative values are allowed.)
    public static ivec2 RandomUniform(Random random, int minValue, int maxValue)
    {
        return new ivec2(random.Next(minValue, maxValue));
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator+ (lhs + rhs).
    public static ivec2 operator +(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2(lhs.x + rhs.x, lhs.y + rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator+ (lhs + rhs).
    public static ivec2 operator +(ivec2 lhs, int rhs)
    {
        return new ivec2(lhs.x + rhs, lhs.y + rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator+ (lhs + rhs).
    public static ivec2 operator +(int lhs, ivec2 rhs)
    {
        return new ivec2(lhs + rhs.x, lhs + rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator- (lhs - rhs).
    public static ivec2 operator -(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2(lhs.x - rhs.x, lhs.y - rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator- (lhs - rhs).
    public static ivec2 operator -(ivec2 lhs, int rhs)
    {
        return new ivec2(lhs.x - rhs, lhs.y - rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator- (lhs - rhs).
    public static ivec2 operator -(int lhs, ivec2 rhs)
    {
        return new ivec2(lhs - rhs.x, lhs - rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator* (lhs * rhs).
    public static ivec2 operator *(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2(lhs.x * rhs.x, lhs.y * rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator* (lhs * rhs).
    public static ivec2 operator *(ivec2 lhs, int rhs)
    {
        return new ivec2(lhs.x * rhs, lhs.y * rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator* (lhs * rhs).
    public static ivec2 operator *(int lhs, ivec2 rhs)
    {
        return new ivec2(lhs * rhs.x, lhs * rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator/ (lhs / rhs).
    public static ivec2 operator /(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2(lhs.x / rhs.x, lhs.y / rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator/ (lhs / rhs).
    public static ivec2 operator /(ivec2 lhs, int rhs)
    {
        return new ivec2(lhs.x / rhs, lhs.y / rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator/ (lhs / rhs).
    public static ivec2 operator /(int lhs, ivec2 rhs)
    {
        return new ivec2(lhs / rhs.x, lhs / rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator+ (identity).
    public static ivec2 operator +(ivec2 v)
    {
        return v;
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator- (-v).
    public static ivec2 operator -(ivec2 v)
    {
        return new ivec2(-v.x, -v.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator~ (~v).
    public static ivec2 operator ~(ivec2 v)
    {
        return new ivec2(~v.x, ~v.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator^ (lhs ^ rhs).
    public static ivec2 operator ^(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2(lhs.x ^ rhs.x, lhs.y ^ rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator^ (lhs ^ rhs).
    public static ivec2 operator ^(ivec2 lhs, int rhs)
    {
        return new ivec2(lhs.x ^ rhs, lhs.y ^ rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator^ (lhs ^ rhs).
    public static ivec2 operator ^(int lhs, ivec2 rhs)
    {
        return new ivec2(lhs ^ rhs.x, lhs ^ rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator| (lhs | rhs).
    public static ivec2 operator |(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2(lhs.x | rhs.x, lhs.y | rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator| (lhs | rhs).
    public static ivec2 operator |(ivec2 lhs, int rhs)
    {
        return new ivec2(lhs.x | rhs, lhs.y | rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator| (lhs | rhs).
    public static ivec2 operator |(int lhs, ivec2 rhs)
    {
        return new ivec2(lhs | rhs.x, lhs | rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator& (lhs & rhs).
    public static ivec2 operator &(ivec2 lhs, ivec2 rhs)
    {
        return new ivec2(lhs.x & rhs.x, lhs.y & rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator& (lhs & rhs).
    public static ivec2 operator &(ivec2 lhs, int rhs)
    {
        return new ivec2(lhs.x & rhs, lhs.y & rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator& (lhs & rhs).
    public static ivec2 operator &(int lhs, ivec2 rhs)
    {
        return new ivec2(lhs & rhs.x, lhs & rhs.y);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator<< (lhs << rhs).
    public static ivec2 operator <<(ivec2 lhs, int rhs)
    {
        return new ivec2(lhs.x << rhs, lhs.y << rhs);
    }

    //
    // Summary:
    //     Returns a ivec2 from component-wise application of operator>> (lhs >> rhs).
    public static ivec2 operator >>(ivec2 lhs, int rhs)
    {
        return new ivec2(lhs.x >> rhs, lhs.y >> rhs);
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
