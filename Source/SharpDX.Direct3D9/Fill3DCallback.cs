// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Runtime.InteropServices;

namespace SharpDX.Direct3D9
{
    /// <summary>
    /// A callback function used to fill 2D texture.
    /// </summary>
    /// <param name="coordinate">Texture coordinate being sampled.</param>
    /// <param name="texelSize">Dimensions of the texel.</param>
    /// <returns>The desired color of the specified texel.</returns>
    /// <unmanaged>typedef VOID (WINAPI *LPD3DXFILL2D)(D3DXVECTOR4 *pOut, CONST D3DXVECTOR2 *pTexCoord, CONST D3DXVECTOR2 *pTexelSize, LPVOID pData);</unmanaged>
    public delegate Color4 Fill2DCallback(Vector2 coordinate, Vector2 texelSize);

    /// <summary>
    /// A callback function used to fill 3D texture.
    /// </summary>
    /// <param name="coordinate">Texture coordinate being sampled.</param>
    /// <param name="texelSize">Dimensions of the texel.</param>
    /// <returns>The desired color of the specified texel.</returns>
    /// <unmanaged>typedef VOID (WINAPI *LPD3DXFILL2D)(D3DXVECTOR4 *pOut, CONST D3DXVECTOR2 *pTexCoord, CONST D3DXVECTOR2 *pTexelSize, LPVOID pData);</unmanaged>
    public delegate Color4 Fill3DCallback(Vector3 coordinate, Vector3 texelSize);

    /// <summary>
    /// Fill callback helper class.
    /// </summary>
    internal static class FillCallbackHelper
    {
        private static unsafe Delegate native2DCallback = new Fill2DCallbackDelegate(Fill2DCallbackImpl);
        private static unsafe Delegate native3DCallback = new Fill3DCallbackDelegate(Fill3DCallbackImpl);

        /// <summary>
        /// Pointer to the native callback for 2D function
        /// </summary>
        public static IntPtr Native2DCallbackPtr;

        /// <summary>
        /// Pointer to the native callback for 3D function
        /// </summary>
        public static IntPtr Native3DCallbackPtr;

        static FillCallbackHelper()
        {
            Native2DCallbackPtr = Marshal.GetFunctionPointerForDelegate(native2DCallback);
            Native3DCallbackPtr = Marshal.GetFunctionPointerForDelegate(native3DCallback);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate Result Fill2DCallbackDelegate(Color4* outVector, Vector2* textCoord, Vector2* textelSize, IntPtr data);
        private static unsafe Result Fill2DCallbackImpl(Color4* outVector, Vector2* textCoord, Vector2* textelSize, IntPtr data)
        {
            try
            {
                var handle = GCHandle.FromIntPtr(data);
                *outVector = ((Fill2DCallback)handle.Target)(*textCoord, *textelSize);
            }
            catch (SharpDXException exception)
            {
                return exception.ResultCode.Code;
            }
            catch (Exception)
            {
                return Result.Fail.Code;
            }
            return Result.Ok.Code;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate Result Fill3DCallbackDelegate(Color4* outVector, Vector3* textCoord, Vector3* textelSize, IntPtr data);
        private static unsafe Result Fill3DCallbackImpl(Color4* outVector, Vector3* textCoord, Vector3* textelSize, IntPtr data)
        {
            try
            {
                var handle = GCHandle.FromIntPtr(data);
                *outVector = ((Fill3DCallback)handle.Target)(*textCoord, *textelSize);
            }
            catch (SharpDXException exception)
            {
                return exception.ResultCode.Code;
            }
            catch (Exception)
            {
                return Result.Fail.Code;
            }
            return Result.Ok.Code;
        }
    }
}