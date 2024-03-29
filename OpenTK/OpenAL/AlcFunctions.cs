﻿#region --- OpenTK.OpenAL License ---
/* AlcFunctions.cs
 * C header: \OpenAL 1.1 SDK\include\Alc.h
 * Spec: http://www.openal.org/openal_webstf/specs/OpenAL11Specification.pdf
 * Copyright (c) 2008 Christoph Brandtner and Stefanos Apostolopoulos
 * See license.txt for license details
 * http://www.OpenTK.net */
#endregion

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

/* Type Mapping
// 8-bit boolean 
typedef char ALCboolean;
 * byte
// character 
typedef char ALCchar;
 * byte
// signed 8-bit 2's complement integer 
typedef char ALCbyte;
 * byte

// unsigned 8-bit integer 
typedef unsigned char ALCubyte;
 * ubyte

// signed 16-bit 2's complement integer 
typedef short ALCshort;
 * short

// unsigned 16-bit integer 
typedef unsigned short ALCushort;
 * ushort

// unsigned 32-bit integer 
typedef unsigned int ALCuint;
 * uint
  
// signed 32-bit 2's complement integer 
typedef int ALCint;
 * int
// non-negative 32-bit binary integer size
typedef int ALCsizei;
 * int
// enumerated 32-bit value
typedef int ALCenum;
 * int

// 32-bit IEEE754 floating-point
typedef float ALCfloat;
 * float

// 64-bit IEEE754 floating-point
typedef double ALCdouble;
 * double
 
// void type (for opaque pointers only)
typedef void ALCvoid;
 * void
 
 * ALCdevice
 * ALCcontext *context
 * IntPtr
*/

namespace OpenTK.OpenAL
{

    /// <summary>Alc = Audio Library Context</summary>
    public static class Alc
    {
        #region Constants

        private const string Lib = AL.Lib;
        private const CallingConvention Style = CallingConvention.Cdecl;

        #endregion Constants

        #region Context Management

        /// <summary>This function creates a context using a specified device.</summary>
        /// <param name="device">a pointer to a device</param>
        /// <param name="attrlist">a pointer to a set of attributes: ALC_FREQUENCY, ALC_MONO_SOURCES, ALC_REFRESH, ALC_STEREO_SOURCES, ALC_SYNC</param>
        /// <returns>Returns a pointer to the new context (NULL on failure). The attribute list can be NULL, or a zero terminated list of integer pairs composed of valid ALC attribute tokens and requested values.</returns>
        [DllImport(Alc.Lib, EntryPoint = "alcCreateContext", ExactSpelling = true, CallingConvention = Alc.Style), SuppressUnmanagedCodeSecurity()]
        public static extern IntPtr CreateContext([In] IntPtr device, [In] IntPtr attrlist);
        // ALC_API ALCcontext *    ALC_APIENTRY alcCreateContext( ALCdevice *device, const ALCint* attrlist );

        /// <summary>This function makes a specified context the current context.</summary>
        /// <param name="context">A pointer to the new context.</param>
        /// <returns>Returns True on success, or False on failure.</returns>
        [DllImport(Alc.Lib, EntryPoint = "alcMakeContextCurrent", ExactSpelling = true, CallingConvention = Alc.Style), SuppressUnmanagedCodeSecurity()]
        public static extern AL.Bool MakeContextCurrent([In] IntPtr context);
        // ALC_API ALCboolean      ALC_APIENTRY alcMakeContextCurrent( ALCcontext *context );

        /// <summary>This function tells a context to begin processing. When a context is suspended, changes in OpenAL state will be accepted but will not be processed. alcSuspendContext can be used to suspend a context, and then all the OpenAL state changes can be applied at once, followed by a call to alcProcessContext to apply all the state changes immediately. In some cases, this procedure may be more efficient than application of properties in a non-suspended state. In some implementations, process and suspend calls are each a NOP.</summary>
        /// <param name="context">a pointer to the new context</param>
        [DllImport(Alc.Lib, EntryPoint = "alcProcessContext", ExactSpelling = true, CallingConvention = Alc.Style), SuppressUnmanagedCodeSecurity()]
        public static extern void ProcessContext([In] IntPtr context);
        // ALC_API void            ALC_APIENTRY alcProcessContext( ALCcontext *context );

        /// <summary>This function suspends processing on a specified context. When a context is suspended, changes in OpenAL state will be accepted but will not be processed. A typical use of alcSuspendContext would be to suspend a context, apply all the OpenAL state changes at once, and then call alcProcessContext to apply all the state changes at once. In some cases, this procedure may be more efficient than application of properties in a non-suspended state. In some implementations, process and suspend calls are each a NOP.</summary>
        /// <param name="context">a pointer to the context to be suspended.</param>
        [DllImport(Alc.Lib, EntryPoint = "alcSuspendContext", ExactSpelling = true, CallingConvention = Alc.Style), SuppressUnmanagedCodeSecurity()]
        public static extern void SuspendContext([In] IntPtr context);
        // ALC_API void            ALC_APIENTRY alcSuspendContext( ALCcontext *context );

        /// <summary>This function destroys a context.</summary>
        /// <param name="context">a pointer to the new context.</param>
        [DllImport(Alc.Lib, EntryPoint = "alcDestroyContext", ExactSpelling = true, CallingConvention = Alc.Style), SuppressUnmanagedCodeSecurity()]
        public static extern void DestroyContext([In] IntPtr context);
        // ALC_API void            ALC_APIENTRY alcDestroyContext( ALCcontext *context );

        /// <summary>This function retrieves the current context.</summary>
        /// <returns>Returns a pointer to the current context.</returns>
        [DllImport(Alc.Lib, EntryPoint = "alcGetCurrentContext", ExactSpelling = true, CallingConvention = Alc.Style), SuppressUnmanagedCodeSecurity()]
        public static extern IntPtr GetCurrentContext();
        // ALC_API ALCcontext *    ALC_APIENTRY alcGetCurrentContext( void );

        /// <summary>This function retrieves a context's device pointer.</summary>
        /// <param name="context">a pointer to a context.</param>
        /// <returns>Returns a pointer to the specified context's device.</returns>
        [DllImport(Alc.Lib, EntryPoint = "alcGetContextsDevice", ExactSpelling = true, CallingConvention = Alc.Style), SuppressUnmanagedCodeSecurity()]
        public static extern IntPtr GetContextsDevice([In] IntPtr context);
        // ALC_API ALCdevice*      ALC_APIENTRY alcGetContextsDevice( ALCcontext *context );

        #endregion Context Management

        #region Device Management

        /// <summary>This function opens a device by name.</summary>
        /// <param name="devicename">a null-terminated string describing a device.</param>
        /// <returns>Returns a pointer to the opened device. The return value will be NULL if there is an error.</returns>
        [DllImport(Alc.Lib, EntryPoint = "alcOpenDevice", ExactSpelling = true, CallingConvention = Alc.Style, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity()]
        public static extern IntPtr OpenDevice([In] string devicename);
        // ALC_API ALCdevice *     ALC_APIENTRY alcOpenDevice( const ALCchar *devicename );

        /// <summary>This function closes a device by name.</summary>
        /// <param name="device">a pointer to an opened device</param>
        /// <returns>True will be returned on success or False on failure. Closing a device will fail if the device contains any contexts or buffers.</returns>
        [DllImport(Alc.Lib, EntryPoint = "alcCloseDevice", ExactSpelling = true, CallingConvention = Alc.Style), SuppressUnmanagedCodeSecurity()]
        public static extern AL.Bool CloseDevice([In] IntPtr device);
        // ALC_API ALCboolean      ALC_APIENTRY alcCloseDevice( ALCdevice *device );

        #endregion Device Management

        #region Error support.

        /// <summary>This function retrieves the current context error state.</summary>
        /// <param name="device">a pointer to the device to retrieve the error state from</param>
        /// <returns>Errorcode Int32.</returns>
        [DllImport(Alc.Lib, EntryPoint = "alcGetError", ExactSpelling = true, CallingConvention = Alc.Style), SuppressUnmanagedCodeSecurity()]
        public static extern Enums.AlcError GetError([In] IntPtr device);
        // ALC_API ALCenum         ALC_APIENTRY alcGetError( ALCdevice *device );

        #endregion Error support.

        #region Extension support.

        /// <summary>This function queries if a specified context extension is available.</summary>
        /// <param name="device">a pointer to the device to be queried for an extension.</param>
        /// <param name="extname">a null-terminated string describing the extension.</param>
        /// <returns>Returns True if the extension is available, False if the extension is not available.</returns>
        [DllImport(Alc.Lib, EntryPoint = "alcIsExtensionPresent", ExactSpelling = true, CallingConvention = Alc.Style, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity()]
        public static extern AL.Bool IsExtensionPresent([In] IntPtr device, [In] string extname);
        // ALC_API ALCboolean      ALC_APIENTRY alcIsExtensionPresent( ALCdevice *device, const ALCchar *extname );

        /// <summary>This function retrieves the address of a specified context extension function.</summary>
        /// <param name="device">a pointer to the device to be queried for the function.</param>
        /// <param name="funcname">a null-terminated string describing the function.</param>
        /// <returns>Returns the address of the function, or NULL if it is not found.</returns>
        [DllImport(Alc.Lib, EntryPoint = "alcGetProcAddress", ExactSpelling = true, CallingConvention = Alc.Style, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity()]
        public static extern IntPtr GetProcAddress([In] IntPtr device, [In] string funcname);
        // ALC_API void  *         ALC_APIENTRY alcGetProcAddress( ALCdevice *device, const ALCchar *funcname );

        /// <summary>This function retrieves the enum value for a specified enumeration name.</summary>
        /// <param name="device">a pointer to the device to be queried.</param>
        /// <param name="enumname">a null terminated string describing the enum value.</param>
        /// <returns>Returns the enum value described by the enumName string. This is most often used for querying an enum value for an ALC extension.</returns>
        [DllImport(Alc.Lib, EntryPoint = "alcGetEnumValue", ExactSpelling = true, CallingConvention = Alc.Style, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity()]
        public static extern int GetEnumValue([In] IntPtr device, [In] string enumname);
        // ALC_API ALCenum         ALC_APIENTRY alcGetEnumValue( ALCdevice *device, const ALCchar *enumname );

        #endregion Extension support.

        #region Query functions

        [DllImport(Alc.Lib, EntryPoint = "alcGetString", ExactSpelling = true, CallingConvention = Alc.Style, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity()]
        private static extern IntPtr GetStringPrivate([In] IntPtr device, Enums.AlcGetString param);
        // ALC_API const ALCchar * ALC_APIENTRY alcGetString( ALCdevice *device, ALCenum param );

        /// <summary>This function returns pointers to strings related to the context.</summary>
        /// <remarks>
        /// ALC_DEFAULT_DEVICE_SPECIFIER will return the name of the default output device.
        /// ALC_CAPTURE_DEFAULT_DEVICE_SPECIFIER will return the name of the default capture device.
        /// ALC_DEVICE_SPECIFIER will return the name of the specified output device if a pointer is supplied, or will return a list of all available devices if a NULL device pointer is supplied. A list is a pointer to a series of strings separated by NULL characters, with the list terminated by two NULL characters. See Enumeration Extension for more details.
        /// ALC_CAPTURE_DEVICE_SPECIFIER will return the name of the specified capture device if a pointer is supplied, or will return a list of all available devices if a NULL device pointer is supplied.
        /// ALC_EXTENSIONS returns a list of available context extensions, with each extension separated by a space and the list terminated by a NULL character.
        /// </remarks>
        /// <param name="device">a pointer to the device to be queried.</param>
        /// <param name="param">an attribute to be retrieved: ALC_DEFAULT_DEVICE_SPECIFIER, ALC_CAPTURE_DEFAULT_DEVICE_SPECIFIER, ALC_DEVICE_SPECIFIER, ALC_CAPTURE_DEVICE_SPECIFIER, ALC_EXTENSIONS</param>
        /// <returns>A string containing the name of the Device.</returns>
        public static string GetString(IntPtr device, Enums.AlcGetString param)
        {
            return Marshal.PtrToStringAnsi(GetStringPrivate(device, param));
        }

        /// <summary>This function returns a List of strings related to the context.</summary>
        /// <remarks>
        /// ALC_DEVICE_SPECIFIER will return the name of the specified output device if a pointer is supplied, or will return a list of all available devices if a NULL device pointer is supplied. A list is a pointer to a series of strings separated by NULL characters, with the list terminated by two NULL characters. See Enumeration Extension for more details.
        /// ALC_CAPTURE_DEVICE_SPECIFIER will return the name of the specified capture device if a pointer is supplied, or will return a list of all available devices if a NULL device pointer is supplied.
        /// ALC_EXTENSIONS returns a list of available context extensions, with each extension separated by a space and the list terminated by a NULL character.
        /// </remarks>
        /// <param name="device">a pointer to the device to be queried.</param>
        /// <param name="param">an attribute to be retrieved: ALC_DEVICE_SPECIFIER, ALC_CAPTURE_DEVICE_SPECIFIER, ALC_ALL_DEVICES_SPECIFIER</param>
        /// <returns>A List of strings containing the names of the Devices.</returns>
        public static List<string> GetString(IntPtr device, Enums.AlcGetStringList param)
        {
            List<string> result = new List<string>();
            IntPtr t = GetStringPrivate(AL.Null, (Enums.AlcGetString)Enums.AlcGetStringList.DeviceSpecifier);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            byte b;
            int offset = 0;
            do
            {
                b = Marshal.ReadByte(t, offset++);
                if (b != 0)
                    sb.Append((char)b);
                if (b == 0)
                {
                    result.Add(sb.ToString());
                    if (Marshal.ReadByte(t, offset) == 0) // offset already properly increased through ++
                        break; // 2x null
                    else 
                        sb.Remove(0, sb.Length); // 1x null
                }
            } while (true);

            return result;
        }

        /// <summary>This function returns integers related to the context.</summary>
        /// <param name="device">a pointer to the device to be queried.</param>
        /// <param name="param">an attribute to be retrieved: ALC_MAJOR_VERSION, ALC_MINOR_VERSION, ALC_ATTRIBUTES_SIZE, ALC_ALL_ATTRIBUTES</param>
        /// <param name="size">the size of the destination buffer provided. In bytes.</param>
        /// <param name="data">a pointer to the data to be returned</param>
        [DllImport(Alc.Lib, EntryPoint = "alcGetIntegerv", ExactSpelling = true, CallingConvention = Alc.Style, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity()]
        public static extern void GetInteger([In] IntPtr device, Enums.AlcGetInteger param, int size, [Out] out int data);
        // ALC_API void            ALC_APIENTRY alcGetIntegerv( ALCdevice *device, ALCenum param, ALCsizei size, ALCint *data );

        #endregion Query functions

        #region Capture functions

        /// <summary>This function opens a capture device by name. </summary>
        /// <param name="devicename">a pointer to a device name string</param>
        /// <param name="frequency">the frequency that the data should be captured at</param>
        /// <param name="format">the requested capture buffer format</param>
        /// <param name="buffersize">the size of the capture buffer in bytes</param>
        /// <returns>Returns the capture device pointer, or NULL on failure.</returns>
        [CLSCompliant(false), DllImport(Alc.Lib, EntryPoint = "alcCaptureOpenDevice", ExactSpelling = true, CallingConvention = Alc.Style, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity()]
        public static extern IntPtr CaptureOpenDevice(string devicename, uint frequency, Enums.ALFormat format, int buffersize);
        // ALC_API ALCdevice*      ALC_APIENTRY alcCaptureOpenDevice( const ALCchar *devicename, ALCuint frequency, ALCenum format, ALCsizei buffersize );

        /// <summary>This function closes the specified capture device.</summary>
        /// <param name="device">a pointer to a capture device.</param>
        /// <returns>Returns True if the close operation was successful, False on failure.</returns>
        [DllImport(Alc.Lib, EntryPoint = "alcCaptureCloseDevice", ExactSpelling = true, CallingConvention = Alc.Style), SuppressUnmanagedCodeSecurity()]
        public static extern AL.Bool CaptureCloseDevice([In] IntPtr device);
        // ALC_API ALCboolean      ALC_APIENTRY alcCaptureCloseDevice( ALCdevice *device );

        /// <summary>This function begins a capture operation.</summary>
        /// <remarks>alcCaptureStart will begin recording to an internal ring buffer of the size specified when opening the capture device. The application can then retrieve the number of samples currently available using the ALC_CAPTURE_SAPMPLES token with alcGetIntegerv. When the application determines that enough samples are available for processing, then it can obtain them with a call to alcCaptureSamples.</remarks>
        /// <param name="device">a pointer to a capture device.</param>
        [DllImport(Alc.Lib, EntryPoint = "alcCaptureStart", ExactSpelling = true, CallingConvention = Alc.Style), SuppressUnmanagedCodeSecurity()]
        public static extern void CaptureStart([In] IntPtr device);
        // ALC_API void            ALC_APIENTRY alcCaptureStart( ALCdevice *device );

        /// <summary>This function stops a capture operation.</summary>
        /// <param name="device">a pointer to a capture device.</param>
        [DllImport(Alc.Lib, EntryPoint = "alcCaptureStop", ExactSpelling = true, CallingConvention = Alc.Style), SuppressUnmanagedCodeSecurity()]
        public static extern void CaptureStop([In] IntPtr device);
        // ALC_API void            ALC_APIENTRY alcCaptureStop( ALCdevice *device );

        /// <summary>This function completes a capture operation, and does not block.</summary>
        /// <param name="device">a pointer to a capture device.</param>
        /// <param name="buffer">a pointer to a data buffer, which must be large enough to accommodate samples number of samples.</param>
        /// <param name="samples">the number of samples to be retrieved.</param>
        [DllImport(Alc.Lib, EntryPoint = "alcCaptureSamples", ExactSpelling = true, CallingConvention = Alc.Style), SuppressUnmanagedCodeSecurity()]
        public static extern void CaptureSamples([In] IntPtr device, [Out] out IntPtr buffer, [Out] out int samples);
        // ALC_API void            ALC_APIENTRY alcCaptureSamples( ALCdevice *device, ALCvoid *buffer, ALCsizei samples );

        #endregion Capture functions

    }
}