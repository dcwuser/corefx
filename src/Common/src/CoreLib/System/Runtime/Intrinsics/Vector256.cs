// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Internal.Runtime.CompilerServices;

namespace System.Runtime.Intrinsics
{
    [Intrinsic]
    [DebuggerDisplay("{DisplayString,nq}")]
    [DebuggerTypeProxy(typeof(Vector256DebugView<>))]
    [StructLayout(LayoutKind.Sequential, Size = 32)]
    public struct Vector256<T> where T : struct
    {
        // These fields exist to ensure the alignment is 8, rather than 1.
        // This also allows the debug view to work https://github.com/dotnet/coreclr/issues/15694)
        private ulong _00;
        private ulong _01;
        private ulong _02;
        private ulong _03;

        private unsafe string DisplayString
        {
            get
            {
                // The IsPrimitive check ends up working for `bool`, `char`, `IntPtr`, and `UIntPtr`
                // which are not actually supported by any current architecture. This shouldn't be
                // an issue however and greatly simplifies the check

                if (typeof(T).IsPrimitive)
                {
                    var items = new T[32 / Unsafe.SizeOf<T>()];
                    Unsafe.WriteUnaligned(ref Unsafe.As<T, byte>(ref items[0]), this);
                    return $"({string.Join(", ", items)})";
                }
                else
                {
                    return SR.NotSupported_Type;
                }
            }
        }
    }
}
