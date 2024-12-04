//Smaller verison of Assembled GlmSharp, Version=0.9.8.0 for CLI and C++ use

#region Assembly GlmSharp, Version=0.9.8.0, Culture=neutral, PublicKeyToken=null
// C:\Users\dotha\.nuget\packages\glmsharp\0.9.8\lib\Net45\GlmSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GlmSharp;

//
// Summary:
//     A matrix of type float with 4 columns and 4 rows.
[Serializable]
[DataContract(Namespace = "mat")]
public struct mat4 : IReadOnlyList<float>, IReadOnlyCollection<float>, IEnumerable<float>, IEnumerable, IEquatable<mat4>
{
    //
    // Summary:
    //     Column 0, Rows 0
    [DataMember]
    public float m00;

    //
    // Summary:
    //     Column 0, Rows 1
    [DataMember]
    public float m01;

    //
    // Summary:
    //     Column 0, Rows 2
    [DataMember]
    public float m02;

    //
    // Summary:
    //     Column 0, Rows 3
    [DataMember]
    public float m03;

    //
    // Summary:
    //     Column 1, Rows 0
    [DataMember]
    public float m10;

    //
    // Summary:
    //     Column 1, Rows 1
    [DataMember]
    public float m11;

    //
    // Summary:
    //     Column 1, Rows 2
    [DataMember]
    public float m12;

    //
    // Summary:
    //     Column 1, Rows 3
    [DataMember]
    public float m13;

    //
    // Summary:
    //     Column 2, Rows 0
    [DataMember]
    public float m20;

    //
    // Summary:
    //     Column 2, Rows 1
    [DataMember]
    public float m21;

    //
    // Summary:
    //     Column 2, Rows 2
    [DataMember]
    public float m22;

    //
    // Summary:
    //     Column 2, Rows 3
    [DataMember]
    public float m23;

    //
    // Summary:
    //     Column 3, Rows 0
    [DataMember]
    public float m30;

    //
    // Summary:
    //     Column 3, Rows 1
    [DataMember]
    public float m31;

    //
    // Summary:
    //     Column 3, Rows 2
    [DataMember]
    public float m32;

    //
    // Summary:
    //     Column 3, Rows 3
    [DataMember]
    public float m33;

    //
    // Summary:
    //     Creates a 2D array with all values (address: Values[x, y])
    public float[,] Values => new float[4, 4]
    {
        { m00, m01, m02, m03 },
        { m10, m11, m12, m13 },
        { m20, m21, m22, m23 },
        { m30, m31, m32, m33 }
    };

    //
    // Summary:
    //     Creates a 1D array with all values (internal order)
    public float[] Values1D => new float[16]
    {
        m00, m01, m02, m03, m10, m11, m12, m13, m20, m21,
        m22, m23, m30, m31, m32, m33
    };

    //
    // Summary:
    //     Gets or sets the column nr 0
    public vec4 Column0
    {
        get
        {
            return new vec4(m00, m01, m02, m03);
        }
        set
        {
            m00 = value.x;
            m01 = value.y;
            m02 = value.z;
            m03 = value.w;
        }
    }

    //
    // Summary:
    //     Gets or sets the column nr 1
    public vec4 Column1
    {
        get
        {
            return new vec4(m10, m11, m12, m13);
        }
        set
        {
            m10 = value.x;
            m11 = value.y;
            m12 = value.z;
            m13 = value.w;
        }
    }

    //
    // Summary:
    //     Gets or sets the column nr 2
    public vec4 Column2
    {
        get
        {
            return new vec4(m20, m21, m22, m23);
        }
        set
        {
            m20 = value.x;
            m21 = value.y;
            m22 = value.z;
            m23 = value.w;
        }
    }

    //
    // Summary:
    //     Gets or sets the column nr 3
    public vec4 Column3
    {
        get
        {
            return new vec4(m30, m31, m32, m33);
        }
        set
        {
            m30 = value.x;
            m31 = value.y;
            m32 = value.z;
            m33 = value.w;
        }
    }

    //
    // Summary:
    //     Gets or sets the row nr 0
    public vec4 Row0
    {
        get
        {
            return new vec4(m00, m10, m20, m30);
        }
        set
        {
            m00 = value.x;
            m10 = value.y;
            m20 = value.z;
            m30 = value.w;
        }
    }

    //
    // Summary:
    //     Gets or sets the row nr 1
    public vec4 Row1
    {
        get
        {
            return new vec4(m01, m11, m21, m31);
        }
        set
        {
            m01 = value.x;
            m11 = value.y;
            m21 = value.z;
            m31 = value.w;
        }
    }

    //
    // Summary:
    //     Gets or sets the row nr 2
    public vec4 Row2
    {
        get
        {
            return new vec4(m02, m12, m22, m32);
        }
        set
        {
            m02 = value.x;
            m12 = value.y;
            m22 = value.z;
            m32 = value.w;
        }
    }

    //
    // Summary:
    //     Gets or sets the row nr 3
    public vec4 Row3
    {
        get
        {
            return new vec4(m03, m13, m23, m33);
        }
        set
        {
            m03 = value.x;
            m13 = value.y;
            m23 = value.z;
            m33 = value.w;
        }
    }

    //
    // Summary:
    //     Predefined all-zero matrix
    public static mat4 Zero { get; } = new mat4(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f);


    //
    // Summary:
    //     Predefined all-ones matrix
    public static mat4 Ones { get; } = new mat4(1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f);


    //
    // Summary:
    //     Predefined identity matrix
    public static mat4 Identity { get; } = new mat4(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);


    //
    // Summary:
    //     Predefined all-MaxValue matrix
    public static mat4 AllMaxValue { get; } = new mat4(float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue);


    //
    // Summary:
    //     Predefined diagonal-MaxValue matrix
    public static mat4 DiagonalMaxValue { get; } = new mat4(float.MaxValue, 0f, 0f, 0f, 0f, float.MaxValue, 0f, 0f, 0f, 0f, float.MaxValue, 0f, 0f, 0f, 0f, float.MaxValue);


    //
    // Summary:
    //     Predefined all-MinValue matrix
    public static mat4 AllMinValue { get; } = new mat4(float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue);


    //
    // Summary:
    //     Predefined diagonal-MinValue matrix
    public static mat4 DiagonalMinValue { get; } = new mat4(float.MinValue, 0f, 0f, 0f, 0f, float.MinValue, 0f, 0f, 0f, 0f, float.MinValue, 0f, 0f, 0f, 0f, float.MinValue);


    //
    // Summary:
    //     Predefined all-Epsilon matrix
    public static mat4 AllEpsilon { get; } = new mat4(float.Epsilon, float.Epsilon, float.Epsilon, float.Epsilon, float.Epsilon, float.Epsilon, float.Epsilon, float.Epsilon, float.Epsilon, float.Epsilon, float.Epsilon, float.Epsilon, float.Epsilon, float.Epsilon, float.Epsilon, float.Epsilon);


    //
    // Summary:
    //     Predefined diagonal-Epsilon matrix
    public static mat4 DiagonalEpsilon { get; } = new mat4(float.Epsilon, 0f, 0f, 0f, 0f, float.Epsilon, 0f, 0f, 0f, 0f, float.Epsilon, 0f, 0f, 0f, 0f, float.Epsilon);


    //
    // Summary:
    //     Predefined all-NaN matrix
    public static mat4 AllNaN { get; } = new mat4(float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN);


    //
    // Summary:
    //     Predefined diagonal-NaN matrix
    public static mat4 DiagonalNaN { get; } = new mat4(float.NaN, 0f, 0f, 0f, 0f, float.NaN, 0f, 0f, 0f, 0f, float.NaN, 0f, 0f, 0f, 0f, float.NaN);


    //
    // Summary:
    //     Predefined all-NegativeInfinity matrix
    public static mat4 AllNegativeInfinity { get; } = new mat4(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);


    //
    // Summary:
    //     Predefined diagonal-NegativeInfinity matrix
    public static mat4 DiagonalNegativeInfinity { get; } = new mat4(float.NegativeInfinity, 0f, 0f, 0f, 0f, float.NegativeInfinity, 0f, 0f, 0f, 0f, float.NegativeInfinity, 0f, 0f, 0f, 0f, float.NegativeInfinity);


    //
    // Summary:
    //     Predefined all-PositiveInfinity matrix
    public static mat4 AllPositiveInfinity { get; } = new mat4(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);


    //
    // Summary:
    //     Predefined diagonal-PositiveInfinity matrix
    public static mat4 DiagonalPositiveInfinity { get; } = new mat4(float.PositiveInfinity, 0f, 0f, 0f, 0f, float.PositiveInfinity, 0f, 0f, 0f, 0f, float.PositiveInfinity, 0f, 0f, 0f, 0f, float.PositiveInfinity);


    //
    // Summary:
    //     Returns the number of Fields (4 x 4 = 16).
    public int Count => 16;

    //
    // Summary:
    //     Gets/Sets a specific indexed component (a bit slower than direct access).
    public float this[int fieldIndex]
    {
        get
        {
            return fieldIndex switch
            {
                0 => m00,
                1 => m01,
                2 => m02,
                3 => m03,
                4 => m10,
                5 => m11,
                6 => m12,
                7 => m13,
                8 => m20,
                9 => m21,
                10 => m22,
                11 => m23,
                12 => m30,
                13 => m31,
                14 => m32,
                15 => m33,
                _ => throw new ArgumentOutOfRangeException("fieldIndex"),
            };
        }
        set
        {
            switch (fieldIndex)
            {
                case 0:
                    m00 = value;
                    break;
                case 1:
                    m01 = value;
                    break;
                case 2:
                    m02 = value;
                    break;
                case 3:
                    m03 = value;
                    break;
                case 4:
                    m10 = value;
                    break;
                case 5:
                    m11 = value;
                    break;
                case 6:
                    m12 = value;
                    break;
                case 7:
                    m13 = value;
                    break;
                case 8:
                    m20 = value;
                    break;
                case 9:
                    m21 = value;
                    break;
                case 10:
                    m22 = value;
                    break;
                case 11:
                    m23 = value;
                    break;
                case 12:
                    m30 = value;
                    break;
                case 13:
                    m31 = value;
                    break;
                case 14:
                    m32 = value;
                    break;
                case 15:
                    m33 = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("fieldIndex");
            }
        }
    }

    //
    // Summary:
    //     Gets/Sets a specific 2D-indexed component (a bit slower than direct access).
    public float this[int col, int row]
    {
        get
        {
            return this[col * 4 + row];
        }
        set
        {
            this[col * 4 + row] = value;
        }
    }

    //
    // Summary:
    //     Returns a transposed version of this matrix.
    public mat4 Transposed => new mat4(m00, m10, m20, m30, m01, m11, m21, m31, m02, m12, m22, m32, m03, m13, m23, m33);

    //
    // Summary:
    //     Returns the minimal component of this matrix.
    public float MinElement => Math.Min(Math.Min(Math.Min(Math.Min(Math.Min(Math.Min(Math.Min(Math.Min(Math.Min(Math.Min(Math.Min(Math.Min(Math.Min(Math.Min(Math.Min(m00, m01), m02), m03), m10), m11), m12), m13), m20), m21), m22), m23), m30), m31), m32), m33);

    //
    // Summary:
    //     Returns the maximal component of this matrix.
    public float MaxElement => Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(m00, m01), m02), m03), m10), m11), m12), m13), m20), m21), m22), m23), m30), m31), m32), m33);

    //
    // Summary:
    //     Returns the euclidean length of this matrix.
    public float Length => (float)Math.Sqrt(m00 * m00 + m01 * m01 + (m02 * m02 + m03 * m03) + (m10 * m10 + m11 * m11 + (m12 * m12 + m13 * m13)) + (m20 * m20 + m21 * m21 + (m22 * m22 + m23 * m23) + (m30 * m30 + m31 * m31 + (m32 * m32 + m33 * m33))));

    //
    // Summary:
    //     Returns the squared euclidean length of this matrix.
    public float LengthSqr => m00 * m00 + m01 * m01 + (m02 * m02 + m03 * m03) + (m10 * m10 + m11 * m11 + (m12 * m12 + m13 * m13)) + (m20 * m20 + m21 * m21 + (m22 * m22 + m23 * m23) + (m30 * m30 + m31 * m31 + (m32 * m32 + m33 * m33)));

    //
    // Summary:
    //     Returns the sum of all fields.
    public float Sum => m00 + m01 + (m02 + m03) + (m10 + m11 + (m12 + m13)) + (m20 + m21 + (m22 + m23) + (m30 + m31 + (m32 + m33)));

    //
    // Summary:
    //     Returns the euclidean norm of this matrix.
    public float Norm => (float)Math.Sqrt(m00 * m00 + m01 * m01 + (m02 * m02 + m03 * m03) + (m10 * m10 + m11 * m11 + (m12 * m12 + m13 * m13)) + (m20 * m20 + m21 * m21 + (m22 * m22 + m23 * m23) + (m30 * m30 + m31 * m31 + (m32 * m32 + m33 * m33))));

    //
    // Summary:
    //     Returns the one-norm of this matrix.
    public float Norm1 => Math.Abs(m00) + Math.Abs(m01) + (Math.Abs(m02) + Math.Abs(m03)) + (Math.Abs(m10) + Math.Abs(m11) + (Math.Abs(m12) + Math.Abs(m13))) + (Math.Abs(m20) + Math.Abs(m21) + (Math.Abs(m22) + Math.Abs(m23)) + (Math.Abs(m30) + Math.Abs(m31) + (Math.Abs(m32) + Math.Abs(m33))));

    //
    // Summary:
    //     Returns the two-norm of this matrix.
    public float Norm2 => (float)Math.Sqrt(m00 * m00 + m01 * m01 + (m02 * m02 + m03 * m03) + (m10 * m10 + m11 * m11 + (m12 * m12 + m13 * m13)) + (m20 * m20 + m21 * m21 + (m22 * m22 + m23 * m23) + (m30 * m30 + m31 * m31 + (m32 * m32 + m33 * m33))));

    //
    // Summary:
    //     Returns the max-norm of this matrix.
    public float NormMax => Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Abs(m00), Math.Abs(m01)), Math.Abs(m02)), Math.Abs(m03)), Math.Abs(m10)), Math.Abs(m11)), Math.Abs(m12)), Math.Abs(m13)), Math.Abs(m20)), Math.Abs(m21)), Math.Abs(m22)), Math.Abs(m23)), Math.Abs(m30)), Math.Abs(m31)), Math.Abs(m32)), Math.Abs(m33));

    //
    // Summary:
    //     Returns determinant of this matrix.
    public float Determinant => m00 * (m11 * (m22 * m33 - m32 * m23) - m21 * (m12 * m33 - m32 * m13) + m31 * (m12 * m23 - m22 * m13)) - m10 * (m01 * (m22 * m33 - m32 * m23) - m21 * (m02 * m33 - m32 * m03) + m31 * (m02 * m23 - m22 * m03)) + m20 * (m01 * (m12 * m33 - m32 * m13) - m11 * (m02 * m33 - m32 * m03) + m31 * (m02 * m13 - m12 * m03)) - m30 * (m01 * (m12 * m23 - m22 * m13) - m11 * (m02 * m23 - m22 * m03) + m21 * (m02 * m13 - m12 * m03));

    //
    // Summary:
    //     Returns the adjunct of this matrix.
    public mat4 Adjugate => new mat4(m11 * (m22 * m33 - m32 * m23) - m21 * (m12 * m33 - m32 * m13) + m31 * (m12 * m23 - m22 * m13), (0f - m01) * (m22 * m33 - m32 * m23) + m21 * (m02 * m33 - m32 * m03) - m31 * (m02 * m23 - m22 * m03), m01 * (m12 * m33 - m32 * m13) - m11 * (m02 * m33 - m32 * m03) + m31 * (m02 * m13 - m12 * m03), (0f - m01) * (m12 * m23 - m22 * m13) + m11 * (m02 * m23 - m22 * m03) - m21 * (m02 * m13 - m12 * m03), (0f - m10) * (m22 * m33 - m32 * m23) + m20 * (m12 * m33 - m32 * m13) - m30 * (m12 * m23 - m22 * m13), m00 * (m22 * m33 - m32 * m23) - m20 * (m02 * m33 - m32 * m03) + m30 * (m02 * m23 - m22 * m03), (0f - m00) * (m12 * m33 - m32 * m13) + m10 * (m02 * m33 - m32 * m03) - m30 * (m02 * m13 - m12 * m03), m00 * (m12 * m23 - m22 * m13) - m10 * (m02 * m23 - m22 * m03) + m20 * (m02 * m13 - m12 * m03), m10 * (m21 * m33 - m31 * m23) - m20 * (m11 * m33 - m31 * m13) + m30 * (m11 * m23 - m21 * m13), (0f - m00) * (m21 * m33 - m31 * m23) + m20 * (m01 * m33 - m31 * m03) - m30 * (m01 * m23 - m21 * m03), m00 * (m11 * m33 - m31 * m13) - m10 * (m01 * m33 - m31 * m03) + m30 * (m01 * m13 - m11 * m03), (0f - m00) * (m11 * m23 - m21 * m13) + m10 * (m01 * m23 - m21 * m03) - m20 * (m01 * m13 - m11 * m03), (0f - m10) * (m21 * m32 - m31 * m22) + m20 * (m11 * m32 - m31 * m12) - m30 * (m11 * m22 - m21 * m12), m00 * (m21 * m32 - m31 * m22) - m20 * (m01 * m32 - m31 * m02) + m30 * (m01 * m22 - m21 * m02), (0f - m00) * (m11 * m32 - m31 * m12) + m10 * (m01 * m32 - m31 * m02) - m30 * (m01 * m12 - m11 * m02), m00 * (m11 * m22 - m21 * m12) - m10 * (m01 * m22 - m21 * m02) + m20 * (m01 * m12 - m11 * m02));

    //
    // Summary:
    //     Returns the inverse of this matrix (use with caution).
    public mat4 Inverse => Adjugate / Determinant;

    //
    // Summary:
    //     Component-wise constructor
    public mat4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)
    {
        this.m00 = m00;
        this.m01 = m01;
        this.m02 = m02;
        this.m03 = m03;
        this.m10 = m10;
        this.m11 = m11;
        this.m12 = m12;
        this.m13 = m13;
        this.m20 = m20;
        this.m21 = m21;
        this.m22 = m22;
        this.m23 = m23;
        this.m30 = m30;
        this.m31 = m31;
        this.m32 = m32;
        this.m33 = m33;
    }

    //
    // Summary:
    //     Constructs this matrix from a mat2. Non-overwritten fields are from an Identity
    //     matrix.
    public mat4(mat2 m)
    {
        m00 = m.m00;
        m01 = m.m01;
        m02 = 0f;
        m03 = 0f;
        m10 = m.m10;
        m11 = m.m11;
        m12 = 0f;
        m13 = 0f;
        m20 = 0f;
        m21 = 0f;
        m22 = 1f;
        m23 = 0f;
        m30 = 0f;
        m31 = 0f;
        m32 = 0f;
        m33 = 1f;
    }

    //
    // Summary:
    //     Constructs this matrix from a mat3. Non-overwritten fields are from an Identity
    //     matrix.
    public mat4(mat3 m)
    {
        m00 = m.m00;
        m01 = m.m01;
        m02 = m.m02;
        m03 = 0f;
        m10 = m.m10;
        m11 = m.m11;
        m12 = m.m12;
        m13 = 0f;
        m20 = m.m20;
        m21 = m.m21;
        m22 = m.m22;
        m23 = 0f;
        m30 = 0f;
        m31 = 0f;
        m32 = 0f;
        m33 = 1f;
    }

    //
    // Summary:
    //     Constructs this matrix from a mat4. Non-overwritten fields are from an Identity
    //     matrix.
    public mat4(mat4 m)
    {
        m00 = m.m00;
        m01 = m.m01;
        m02 = m.m02;
        m03 = m.m03;
        m10 = m.m10;
        m11 = m.m11;
        m12 = m.m12;
        m13 = m.m13;
        m20 = m.m20;
        m21 = m.m21;
        m22 = m.m22;
        m23 = m.m23;
        m30 = m.m30;
        m31 = m.m31;
        m32 = m.m32;
        m33 = m.m33;
    }

    //
    // Summary:
    //     Constructs this matrix from a series of column vectors. Non-overwritten fields
    //     are from an Identity matrix.
    public mat4(vec2 c0, vec2 c1)
    {
        m00 = c0.x;
        m01 = c0.y;
        m02 = 0f;
        m03 = 0f;
        m10 = c1.x;
        m11 = c1.y;
        m12 = 0f;
        m13 = 0f;
        m20 = 0f;
        m21 = 0f;
        m22 = 1f;
        m23 = 0f;
        m30 = 0f;
        m31 = 0f;
        m32 = 0f;
        m33 = 1f;
    }

    //
    // Summary:
    //     Constructs this matrix from a series of column vectors. Non-overwritten fields
    //     are from an Identity matrix.
    public mat4(vec2 c0, vec2 c1, vec2 c2)
    {
        m00 = c0.x;
        m01 = c0.y;
        m02 = 0f;
        m03 = 0f;
        m10 = c1.x;
        m11 = c1.y;
        m12 = 0f;
        m13 = 0f;
        m20 = c2.x;
        m21 = c2.y;
        m22 = 1f;
        m23 = 0f;
        m30 = 0f;
        m31 = 0f;
        m32 = 0f;
        m33 = 1f;
    }

    //
    // Summary:
    //     Constructs this matrix from a series of column vectors. Non-overwritten fields
    //     are from an Identity matrix.
    public mat4(vec2 c0, vec2 c1, vec2 c2, vec2 c3)
    {
        m00 = c0.x;
        m01 = c0.y;
        m02 = 0f;
        m03 = 0f;
        m10 = c1.x;
        m11 = c1.y;
        m12 = 0f;
        m13 = 0f;
        m20 = c2.x;
        m21 = c2.y;
        m22 = 1f;
        m23 = 0f;
        m30 = c3.x;
        m31 = c3.y;
        m32 = 0f;
        m33 = 1f;
    }

    //
    // Summary:
    //     Constructs this matrix from a series of column vectors. Non-overwritten fields
    //     are from an Identity matrix.
    public mat4(vec3 c0, vec3 c1)
    {
        m00 = c0.x;
        m01 = c0.y;
        m02 = c0.z;
        m03 = 0f;
        m10 = c1.x;
        m11 = c1.y;
        m12 = c1.z;
        m13 = 0f;
        m20 = 0f;
        m21 = 0f;
        m22 = 1f;
        m23 = 0f;
        m30 = 0f;
        m31 = 0f;
        m32 = 0f;
        m33 = 1f;
    }

    //
    // Summary:
    //     Constructs this matrix from a series of column vectors. Non-overwritten fields
    //     are from an Identity matrix.
    public mat4(vec3 c0, vec3 c1, vec3 c2)
    {
        m00 = c0.x;
        m01 = c0.y;
        m02 = c0.z;
        m03 = 0f;
        m10 = c1.x;
        m11 = c1.y;
        m12 = c1.z;
        m13 = 0f;
        m20 = c2.x;
        m21 = c2.y;
        m22 = c2.z;
        m23 = 0f;
        m30 = 0f;
        m31 = 0f;
        m32 = 0f;
        m33 = 1f;
    }

    //
    // Summary:
    //     Constructs this matrix from a series of column vectors. Non-overwritten fields
    //     are from an Identity matrix.
    public mat4(vec3 c0, vec3 c1, vec3 c2, vec3 c3)
    {
        m00 = c0.x;
        m01 = c0.y;
        m02 = c0.z;
        m03 = 0f;
        m10 = c1.x;
        m11 = c1.y;
        m12 = c1.z;
        m13 = 0f;
        m20 = c2.x;
        m21 = c2.y;
        m22 = c2.z;
        m23 = 0f;
        m30 = c3.x;
        m31 = c3.y;
        m32 = c3.z;
        m33 = 1f;
    }

    //
    // Summary:
    //     Constructs this matrix from a series of column vectors. Non-overwritten fields
    //     are from an Identity matrix.
    public mat4(vec4 c0, vec4 c1)
    {
        m00 = c0.x;
        m01 = c0.y;
        m02 = c0.z;
        m03 = c0.w;
        m10 = c1.x;
        m11 = c1.y;
        m12 = c1.z;
        m13 = c1.w;
        m20 = 0f;
        m21 = 0f;
        m22 = 1f;
        m23 = 0f;
        m30 = 0f;
        m31 = 0f;
        m32 = 0f;
        m33 = 1f;
    }

    //
    // Summary:
    //     Constructs this matrix from a series of column vectors. Non-overwritten fields
    //     are from an Identity matrix.
    public mat4(vec4 c0, vec4 c1, vec4 c2)
    {
        m00 = c0.x;
        m01 = c0.y;
        m02 = c0.z;
        m03 = c0.w;
        m10 = c1.x;
        m11 = c1.y;
        m12 = c1.z;
        m13 = c1.w;
        m20 = c2.x;
        m21 = c2.y;
        m22 = c2.z;
        m23 = c2.w;
        m30 = 0f;
        m31 = 0f;
        m32 = 0f;
        m33 = 1f;
    }

    //
    // Summary:
    //     Constructs this matrix from a series of column vectors. Non-overwritten fields
    //     are from an Identity matrix.
    public mat4(vec4 c0, vec4 c1, vec4 c2, vec4 c3)
    {
        m00 = c0.x;
        m01 = c0.y;
        m02 = c0.z;
        m03 = c0.w;
        m10 = c1.x;
        m11 = c1.y;
        m12 = c1.z;
        m13 = c1.w;
        m20 = c2.x;
        m21 = c2.y;
        m22 = c2.z;
        m23 = c2.w;
        m30 = c3.x;
        m31 = c3.y;
        m32 = c3.z;
        m33 = c3.w;
    }

    //
    // Summary:
    //     Returns an enumerator that iterates through all fields.
    public IEnumerator<float> GetEnumerator()
    {
        yield return m00;
        yield return m01;
        yield return m02;
        yield return m03;
        yield return m10;
        yield return m11;
        yield return m12;
        yield return m13;
        yield return m20;
        yield return m21;
        yield return m22;
        yield return m23;
        yield return m30;
        yield return m31;
        yield return m32;
        yield return m33;
    }

    //
    // Summary:
    //     Returns an enumerator that iterates through all fields.
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    //
    // Summary:
    //     Returns true iff this equals rhs component-wise.
    public bool Equals(mat4 rhs)
    {
        if (m00.Equals(rhs.m00) && m01.Equals(rhs.m01) && m02.Equals(rhs.m02) && m03.Equals(rhs.m03) && m10.Equals(rhs.m10) && m11.Equals(rhs.m11) && m12.Equals(rhs.m12) && m13.Equals(rhs.m13))
        {
            if (m20.Equals(rhs.m20) && m21.Equals(rhs.m21) && m22.Equals(rhs.m22) && m23.Equals(rhs.m23))
            {
                if (m30.Equals(rhs.m30) && m31.Equals(rhs.m31))
                {
                    if (m32.Equals(rhs.m32))
                    {
                        return m33.Equals(rhs.m33);
                    }

                    return false;
                }

                return false;
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

        if (obj is mat4)
        {
            return Equals((mat4)obj);
        }

        return false;
    }

    //
    // Summary:
    //     Returns true iff this equals rhs component-wise.
    public static bool operator ==(mat4 lhs, mat4 rhs)
    {
        return lhs.Equals(rhs);
    }

    //
    // Summary:
    //     Returns true iff this does not equal rhs (component-wise).
    public static bool operator !=(mat4 lhs, mat4 rhs)
    {
        return !lhs.Equals(rhs);
    }

    //
    // Summary:
    //     Returns a hash code for this instance.
    public override int GetHashCode()
    {
        return (((((((((((((((((((((((((((((m00.GetHashCode() * 397) ^ m01.GetHashCode()) * 397) ^ m02.GetHashCode()) * 397) ^ m03.GetHashCode()) * 397) ^ m10.GetHashCode()) * 397) ^ m11.GetHashCode()) * 397) ^ m12.GetHashCode()) * 397) ^ m13.GetHashCode()) * 397) ^ m20.GetHashCode()) * 397) ^ m21.GetHashCode()) * 397) ^ m22.GetHashCode()) * 397) ^ m23.GetHashCode()) * 397) ^ m30.GetHashCode()) * 397) ^ m31.GetHashCode()) * 397) ^ m32.GetHashCode()) * 397) ^ m33.GetHashCode();
    }

    //
    // Summary:
    //     Returns the p-norm of this matrix.
    public double NormP(double p)
    {
        return Math.Pow(Math.Pow(Math.Abs(m00), p) + Math.Pow(Math.Abs(m01), p) + (Math.Pow(Math.Abs(m02), p) + Math.Pow(Math.Abs(m03), p)) + (Math.Pow(Math.Abs(m10), p) + Math.Pow(Math.Abs(m11), p) + (Math.Pow(Math.Abs(m12), p) + Math.Pow(Math.Abs(m13), p))) + (Math.Pow(Math.Abs(m20), p) + Math.Pow(Math.Abs(m21), p) + (Math.Pow(Math.Abs(m22), p) + Math.Pow(Math.Abs(m23), p)) + (Math.Pow(Math.Abs(m30), p) + Math.Pow(Math.Abs(m31), p) + (Math.Pow(Math.Abs(m32), p) + Math.Pow(Math.Abs(m33), p)))), 1.0 / p);
    }

    //
    // Summary:
    //     Executes a matrix-matrix-multiplication mat4 * mat4 -> mat4.
    public static mat4 operator *(mat4 lhs, mat4 rhs)
    {
        return new mat4(lhs.m00 * rhs.m00 + lhs.m10 * rhs.m01 + (lhs.m20 * rhs.m02 + lhs.m30 * rhs.m03), lhs.m01 * rhs.m00 + lhs.m11 * rhs.m01 + (lhs.m21 * rhs.m02 + lhs.m31 * rhs.m03), lhs.m02 * rhs.m00 + lhs.m12 * rhs.m01 + (lhs.m22 * rhs.m02 + lhs.m32 * rhs.m03), lhs.m03 * rhs.m00 + lhs.m13 * rhs.m01 + (lhs.m23 * rhs.m02 + lhs.m33 * rhs.m03), lhs.m00 * rhs.m10 + lhs.m10 * rhs.m11 + (lhs.m20 * rhs.m12 + lhs.m30 * rhs.m13), lhs.m01 * rhs.m10 + lhs.m11 * rhs.m11 + (lhs.m21 * rhs.m12 + lhs.m31 * rhs.m13), lhs.m02 * rhs.m10 + lhs.m12 * rhs.m11 + (lhs.m22 * rhs.m12 + lhs.m32 * rhs.m13), lhs.m03 * rhs.m10 + lhs.m13 * rhs.m11 + (lhs.m23 * rhs.m12 + lhs.m33 * rhs.m13), lhs.m00 * rhs.m20 + lhs.m10 * rhs.m21 + (lhs.m20 * rhs.m22 + lhs.m30 * rhs.m23), lhs.m01 * rhs.m20 + lhs.m11 * rhs.m21 + (lhs.m21 * rhs.m22 + lhs.m31 * rhs.m23), lhs.m02 * rhs.m20 + lhs.m12 * rhs.m21 + (lhs.m22 * rhs.m22 + lhs.m32 * rhs.m23), lhs.m03 * rhs.m20 + lhs.m13 * rhs.m21 + (lhs.m23 * rhs.m22 + lhs.m33 * rhs.m23), lhs.m00 * rhs.m30 + lhs.m10 * rhs.m31 + (lhs.m20 * rhs.m32 + lhs.m30 * rhs.m33), lhs.m01 * rhs.m30 + lhs.m11 * rhs.m31 + (lhs.m21 * rhs.m32 + lhs.m31 * rhs.m33), lhs.m02 * rhs.m30 + lhs.m12 * rhs.m31 + (lhs.m22 * rhs.m32 + lhs.m32 * rhs.m33), lhs.m03 * rhs.m30 + lhs.m13 * rhs.m31 + (lhs.m23 * rhs.m32 + lhs.m33 * rhs.m33));
    }

    //
    // Summary:
    //     Executes a matrix-vector-multiplication.
    public static vec4 operator *(mat4 m, vec4 v)
    {
        return new vec4(m.m00 * v.x + m.m10 * v.y + (m.m20 * v.z + m.m30 * v.w), m.m01 * v.x + m.m11 * v.y + (m.m21 * v.z + m.m31 * v.w), m.m02 * v.x + m.m12 * v.y + (m.m22 * v.z + m.m32 * v.w), m.m03 * v.x + m.m13 * v.y + (m.m23 * v.z + m.m33 * v.w));
    }

    //
    // Summary:
    //     Executes a matrix-matrix-divison A / B == A * B^-1 (use with caution).
    public static mat4 operator /(mat4 A, mat4 B)
    {
        return A * B.Inverse;
    }

    //
    // Summary:
    //     Executes a component-wise * (multiply).
    public static mat4 CompMul(mat4 A, mat4 B)
    {
        return new mat4(A.m00 * B.m00, A.m01 * B.m01, A.m02 * B.m02, A.m03 * B.m03, A.m10 * B.m10, A.m11 * B.m11, A.m12 * B.m12, A.m13 * B.m13, A.m20 * B.m20, A.m21 * B.m21, A.m22 * B.m22, A.m23 * B.m23, A.m30 * B.m30, A.m31 * B.m31, A.m32 * B.m32, A.m33 * B.m33);
    }

    //
    // Summary:
    //     Executes a component-wise / (divide).
    public static mat4 CompDiv(mat4 A, mat4 B)
    {
        return new mat4(A.m00 / B.m00, A.m01 / B.m01, A.m02 / B.m02, A.m03 / B.m03, A.m10 / B.m10, A.m11 / B.m11, A.m12 / B.m12, A.m13 / B.m13, A.m20 / B.m20, A.m21 / B.m21, A.m22 / B.m22, A.m23 / B.m23, A.m30 / B.m30, A.m31 / B.m31, A.m32 / B.m32, A.m33 / B.m33);
    }

    //
    // Summary:
    //     Executes a component-wise + (add).
    public static mat4 CompAdd(mat4 A, mat4 B)
    {
        return new mat4(A.m00 + B.m00, A.m01 + B.m01, A.m02 + B.m02, A.m03 + B.m03, A.m10 + B.m10, A.m11 + B.m11, A.m12 + B.m12, A.m13 + B.m13, A.m20 + B.m20, A.m21 + B.m21, A.m22 + B.m22, A.m23 + B.m23, A.m30 + B.m30, A.m31 + B.m31, A.m32 + B.m32, A.m33 + B.m33);
    }

    //
    // Summary:
    //     Executes a component-wise - (subtract).
    public static mat4 CompSub(mat4 A, mat4 B)
    {
        return new mat4(A.m00 - B.m00, A.m01 - B.m01, A.m02 - B.m02, A.m03 - B.m03, A.m10 - B.m10, A.m11 - B.m11, A.m12 - B.m12, A.m13 - B.m13, A.m20 - B.m20, A.m21 - B.m21, A.m22 - B.m22, A.m23 - B.m23, A.m30 - B.m30, A.m31 - B.m31, A.m32 - B.m32, A.m33 - B.m33);
    }

    //
    // Summary:
    //     Executes a component-wise + (add).
    public static mat4 operator +(mat4 lhs, mat4 rhs)
    {
        return new mat4(lhs.m00 + rhs.m00, lhs.m01 + rhs.m01, lhs.m02 + rhs.m02, lhs.m03 + rhs.m03, lhs.m10 + rhs.m10, lhs.m11 + rhs.m11, lhs.m12 + rhs.m12, lhs.m13 + rhs.m13, lhs.m20 + rhs.m20, lhs.m21 + rhs.m21, lhs.m22 + rhs.m22, lhs.m23 + rhs.m23, lhs.m30 + rhs.m30, lhs.m31 + rhs.m31, lhs.m32 + rhs.m32, lhs.m33 + rhs.m33);
    }

    //
    // Summary:
    //     Executes a component-wise + (add) with a scalar.
    public static mat4 operator +(mat4 lhs, float rhs)
    {
        return new mat4(lhs.m00 + rhs, lhs.m01 + rhs, lhs.m02 + rhs, lhs.m03 + rhs, lhs.m10 + rhs, lhs.m11 + rhs, lhs.m12 + rhs, lhs.m13 + rhs, lhs.m20 + rhs, lhs.m21 + rhs, lhs.m22 + rhs, lhs.m23 + rhs, lhs.m30 + rhs, lhs.m31 + rhs, lhs.m32 + rhs, lhs.m33 + rhs);
    }

    //
    // Summary:
    //     Executes a component-wise + (add) with a scalar.
    public static mat4 operator +(float lhs, mat4 rhs)
    {
        return new mat4(lhs + rhs.m00, lhs + rhs.m01, lhs + rhs.m02, lhs + rhs.m03, lhs + rhs.m10, lhs + rhs.m11, lhs + rhs.m12, lhs + rhs.m13, lhs + rhs.m20, lhs + rhs.m21, lhs + rhs.m22, lhs + rhs.m23, lhs + rhs.m30, lhs + rhs.m31, lhs + rhs.m32, lhs + rhs.m33);
    }

    //
    // Summary:
    //     Executes a component-wise - (subtract).
    public static mat4 operator -(mat4 lhs, mat4 rhs)
    {
        return new mat4(lhs.m00 - rhs.m00, lhs.m01 - rhs.m01, lhs.m02 - rhs.m02, lhs.m03 - rhs.m03, lhs.m10 - rhs.m10, lhs.m11 - rhs.m11, lhs.m12 - rhs.m12, lhs.m13 - rhs.m13, lhs.m20 - rhs.m20, lhs.m21 - rhs.m21, lhs.m22 - rhs.m22, lhs.m23 - rhs.m23, lhs.m30 - rhs.m30, lhs.m31 - rhs.m31, lhs.m32 - rhs.m32, lhs.m33 - rhs.m33);
    }

    //
    // Summary:
    //     Executes a component-wise - (subtract) with a scalar.
    public static mat4 operator -(mat4 lhs, float rhs)
    {
        return new mat4(lhs.m00 - rhs, lhs.m01 - rhs, lhs.m02 - rhs, lhs.m03 - rhs, lhs.m10 - rhs, lhs.m11 - rhs, lhs.m12 - rhs, lhs.m13 - rhs, lhs.m20 - rhs, lhs.m21 - rhs, lhs.m22 - rhs, lhs.m23 - rhs, lhs.m30 - rhs, lhs.m31 - rhs, lhs.m32 - rhs, lhs.m33 - rhs);
    }

    //
    // Summary:
    //     Executes a component-wise - (subtract) with a scalar.
    public static mat4 operator -(float lhs, mat4 rhs)
    {
        return new mat4(lhs - rhs.m00, lhs - rhs.m01, lhs - rhs.m02, lhs - rhs.m03, lhs - rhs.m10, lhs - rhs.m11, lhs - rhs.m12, lhs - rhs.m13, lhs - rhs.m20, lhs - rhs.m21, lhs - rhs.m22, lhs - rhs.m23, lhs - rhs.m30, lhs - rhs.m31, lhs - rhs.m32, lhs - rhs.m33);
    }

    //
    // Summary:
    //     Executes a component-wise / (divide) with a scalar.
    public static mat4 operator /(mat4 lhs, float rhs)
    {
        return new mat4(lhs.m00 / rhs, lhs.m01 / rhs, lhs.m02 / rhs, lhs.m03 / rhs, lhs.m10 / rhs, lhs.m11 / rhs, lhs.m12 / rhs, lhs.m13 / rhs, lhs.m20 / rhs, lhs.m21 / rhs, lhs.m22 / rhs, lhs.m23 / rhs, lhs.m30 / rhs, lhs.m31 / rhs, lhs.m32 / rhs, lhs.m33 / rhs);
    }

    //
    // Summary:
    //     Executes a component-wise / (divide) with a scalar.
    public static mat4 operator /(float lhs, mat4 rhs)
    {
        return new mat4(lhs / rhs.m00, lhs / rhs.m01, lhs / rhs.m02, lhs / rhs.m03, lhs / rhs.m10, lhs / rhs.m11, lhs / rhs.m12, lhs / rhs.m13, lhs / rhs.m20, lhs / rhs.m21, lhs / rhs.m22, lhs / rhs.m23, lhs / rhs.m30, lhs / rhs.m31, lhs / rhs.m32, lhs / rhs.m33);
    }

    //
    // Summary:
    //     Executes a component-wise * (multiply) with a scalar.
    public static mat4 operator *(mat4 lhs, float rhs)
    {
        return new mat4(lhs.m00 * rhs, lhs.m01 * rhs, lhs.m02 * rhs, lhs.m03 * rhs, lhs.m10 * rhs, lhs.m11 * rhs, lhs.m12 * rhs, lhs.m13 * rhs, lhs.m20 * rhs, lhs.m21 * rhs, lhs.m22 * rhs, lhs.m23 * rhs, lhs.m30 * rhs, lhs.m31 * rhs, lhs.m32 * rhs, lhs.m33 * rhs);
    }

    //
    // Summary:
    //     Executes a component-wise * (multiply) with a scalar.
    public static mat4 operator *(float lhs, mat4 rhs)
    {
        return new mat4(lhs * rhs.m00, lhs * rhs.m01, lhs * rhs.m02, lhs * rhs.m03, lhs * rhs.m10, lhs * rhs.m11, lhs * rhs.m12, lhs * rhs.m13, lhs * rhs.m20, lhs * rhs.m21, lhs * rhs.m22, lhs * rhs.m23, lhs * rhs.m30, lhs * rhs.m31, lhs * rhs.m32, lhs * rhs.m33);
    }

    //
    // Summary:
    //     Creates a frustrum projection matrix.
    public static mat4 Frustum(float left, float right, float bottom, float top, float nearVal, float farVal)
    {
        mat4 identity = Identity;
        identity.m00 = 2f * nearVal / (right - left);
        identity.m11 = 2f * nearVal / (top - bottom);
        identity.m20 = (right + left) / (right - left);
        identity.m21 = (top + bottom) / (top - bottom);
        identity.m22 = (0f - (farVal + nearVal)) / (farVal - nearVal);
        identity.m23 = -1f;
        identity.m32 = (0f - 2f * farVal * nearVal) / (farVal - nearVal);
        return identity;
    }

    //
    // Summary:
    //     Creates a matrix for a symmetric perspective-view frustum with far plane at infinite.
    public static mat4 InfinitePerspective(float fovy, float aspect, float zNear)
    {
        double num = Math.Tan((double)fovy / 2.0) * (double)zNear;
        double num2 = (0.0 - num) * (double)aspect;
        double num3 = num * (double)aspect;
        double num4 = 0.0 - num;
        double num5 = num;
        mat4 identity = Identity;
        identity.m00 = (float)(2.0 * (double)zNear / (num3 - num2));
        identity.m11 = (float)(2.0 * (double)zNear / (num5 - num4));
        identity.m22 = -1f;
        identity.m23 = -1f;
        identity.m32 = (float)(-2.0 * (double)zNear);
        return identity;
    }

    //
    // Summary:
    //     Build a look at view matrix.
    public static mat4 LookAt(vec3 eye, vec3 center, vec3 up)
    {
        vec3 normalized = (center - eye).Normalized;
        vec3 normalized2 = vec3.Cross(normalized, up).Normalized;
        vec3 lhs = vec3.Cross(normalized2, normalized);
        mat4 identity = Identity;
        identity.m00 = normalized2.x;
        identity.m10 = normalized2.y;
        identity.m20 = normalized2.z;
        identity.m01 = lhs.x;
        identity.m11 = lhs.y;
        identity.m21 = lhs.z;
        identity.m02 = 0f - normalized.x;
        identity.m12 = 0f - normalized.y;
        identity.m22 = 0f - normalized.z;
        identity.m30 = 0f - vec3.Dot(normalized2, eye);
        identity.m31 = 0f - vec3.Dot(lhs, eye);
        identity.m32 = vec3.Dot(normalized, eye);
        return identity;
    }

    //
    // Summary:
    //     Creates a matrix for an orthographic parallel viewing volume.
    public static mat4 Ortho(float left, float right, float bottom, float top, float zNear, float zFar)
    {
        mat4 identity = Identity;
        identity.m00 = 2f / (right - left);
        identity.m11 = 2f / (top - bottom);
        identity.m22 = -2f / (zFar - zNear);
        identity.m30 = (0f - (right + left)) / (right - left);
        identity.m31 = (0f - (top + bottom)) / (top - bottom);
        identity.m32 = (0f - (zFar + zNear)) / (zFar - zNear);
        return identity;
    }

    //
    // Summary:
    //     Creates a matrix for projecting two-dimensional coordinates onto the screen.
    public static mat4 Ortho(float left, float right, float bottom, float top)
    {
        mat4 identity = Identity;
        identity.m00 = 2f / (right - left);
        identity.m11 = 2f / (top - bottom);
        identity.m22 = -1f;
        identity.m30 = (0f - (right + left)) / (right - left);
        identity.m31 = (0f - (top + bottom)) / (top - bottom);
        return identity;
    }

    //
    // Summary:
    //     Creates a perspective transformation matrix.
    public static mat4 Perspective(float fovy, float aspect, float zNear, float zFar)
    {
        double num = Math.Tan((double)fovy / 2.0);
        mat4 zero = Zero;
        zero.m00 = (float)(1.0 / ((double)aspect * num));
        zero.m11 = (float)(1.0 / num);
        zero.m22 = (0f - (zFar + zNear)) / (zFar - zNear);
        zero.m23 = -1f;
        zero.m32 = (0f - 2f * zFar * zNear) / (zFar - zNear);
        return zero;
    }

    //
    // Summary:
    //     Builds a perspective projection matrix based on a field of view.
    public static mat4 PerspectiveFov(float fov, float width, float height, float zNear, float zFar)
    {
        if (width <= 0f)
        {
            throw new ArgumentOutOfRangeException("width");
        }

        if (height <= 0f)
        {
            throw new ArgumentOutOfRangeException("height");
        }

        if (fov <= 0f)
        {
            throw new ArgumentOutOfRangeException("fov");
        }

        double num = Math.Cos((double)fov / 2.0) / Math.Sin((double)fov / 2.0);
        double num2 = num * (double)(height / width);
        mat4 zero = Zero;
        zero.m00 = (float)num2;
        zero.m11 = (float)num;
        zero.m22 = (0f - (zFar + zNear)) / (zFar - zNear);
        zero.m23 = -1f;
        zero.m32 = (0f - 2f * zFar * zNear) / (zFar - zNear);
        return zero;
    }

    //
    // Summary:
    //     Map the specified window coordinates (win.x, win.y, win.z) into object coordinates.
    public static vec3 UnProject(vec3 win, mat4 model, mat4 proj, vec4 viewport)
    {
        vec4 vec5 = new vec4(win, 1f);
        vec5.x = (vec5.x - viewport.x) / viewport.z;
        vec5.y = (vec5.y - viewport.y) / viewport.w;
        vec5 = vec5 * 2f - 1f;
        vec4 vec6 = proj.Inverse * vec5;
        vec6 /= vec6.w;
        vec4 vec7 = model.Inverse * vec6;
        return (vec3)vec7 / vec7.w;
    }

    //
    // Summary:
    //     Map the specified window coordinates (win.x, win.y, win.z) into object coordinates
    //     (faster but less precise).
    public static vec3 UnProjectFaster(vec3 win, mat4 model, mat4 proj, vec4 viewport)
    {
        vec4 vec5 = new vec4(win, 1f);
        vec5.x = (vec5.x - viewport.x) / viewport.z;
        vec5.y = (vec5.y - viewport.y) / viewport.w;
        vec5 = vec5 * 2f - 1f;
        vec4 vec6 = (proj * model).Inverse * vec5;
        return (vec3)vec6 / vec6.w;
    }

    //
    // Summary:
    //     Builds a rotation 4 * 4 matrix created from an axis vector and an angle in radians.
    public static mat4 Rotate(float angle, vec3 v)
    {
        float num = (float)Math.Cos(angle);
        float num2 = (float)Math.Sin(angle);
        vec3 normalized = v.Normalized;
        vec3 vec5 = (1f - num) * normalized;
        mat4 identity = Identity;
        identity.m00 = num + vec5.x * normalized.x;
        identity.m01 = 0f + vec5.x * normalized.y + num2 * normalized.z;
        identity.m02 = 0f + vec5.x * normalized.z - num2 * normalized.y;
        identity.m10 = 0f + vec5.y * normalized.x - num2 * normalized.z;
        identity.m11 = num + vec5.y * normalized.y;
        identity.m12 = 0f + vec5.y * normalized.z + num2 * normalized.x;
        identity.m20 = 0f + vec5.z * normalized.x + num2 * normalized.y;
        identity.m21 = 0f + vec5.z * normalized.y - num2 * normalized.x;
        identity.m22 = num + vec5.z * normalized.z;
        return identity;
    }

    //
    // Summary:
    //     Builds a rotation matrix around UnitX and an angle in radians.
    public static mat4 RotateX(float angle)
    {
        return Rotate(angle, vec3.UnitX);
    }

    //
    // Summary:
    //     Builds a rotation matrix around UnitY and an angle in radians.
    public static mat4 RotateY(float angle)
    {
        return Rotate(angle, vec3.UnitY);
    }

    //
    // Summary:
    //     Builds a rotation matrix around UnitZ and an angle in radians.
    public static mat4 RotateZ(float angle)
    {
        return Rotate(angle, vec3.UnitZ);
    }

    //
    // Summary:
    //     Builds a scale matrix by components x, y, z.
    public static mat4 Scale(float x, float y, float z)
    {
        mat4 identity = Identity;
        identity.m00 = x;
        identity.m11 = y;
        identity.m22 = z;
        return identity;
    }

    //
    // Summary:
    //     Builds a scale matrix by vector v.
    public static mat4 Scale(vec3 v)
    {
        return Scale(v.x, v.y, v.z);
    }

    //
    // Summary:
    //     Builds a scale matrix by uniform scaling s.
    public static mat4 Scale(float s)
    {
        return Scale(s, s, s);
    }

    //
    // Summary:
    //     Builds a translation matrix by components x, y, z.
    public static mat4 Translate(float x, float y, float z)
    {
        mat4 identity = Identity;
        identity.m30 = x;
        identity.m31 = y;
        identity.m32 = z;
        return identity;
    }

    //
    // Summary:
    //     Builds a translation matrix by vector v.
    public static mat4 Translate(vec3 v)
    {
        return Translate(v.x, v.y, v.z);
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
